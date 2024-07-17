using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GatewaysManagement.API.Filters;
using GatewaysManagement.API;
using GatewaysManagement.Data.UoW;
using GatewaysManagement.Data.Repository;
using Newtonsoft.Json;
using GatewaysManagement.Services;
using GatewaysManagement.Services.Impls;
using GatewaysManagement.Services.Utils;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using GatewaysManagement.API.Utils;

namespace GatewaysManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureI18N();
            services.ConfigureCors();
            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(ApiValidationFilterAttribute));
                config.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.ConfigureHsts();
            services.AddControllers(setupAction => {}).AddNewtonsoftJson(setupAction =>
           {
               setupAction.SerializerSettings.ContractResolver =
                  new CamelCasePropertyNamesContractResolver();
           }).ConfigureApiBehaviorOptions(setupAction =>
           {
               setupAction.InvalidModelStateResponseFactory = context =>
               {
                    // create a problem details object
                    var problemDetailsFactory = context.HttpContext.RequestServices
                       .GetRequiredService<ProblemDetailsFactory>();
                   var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                           context.HttpContext,
                           context.ModelState);

                    // add additional info not added by default
                    problemDetails.Detail = "See the errors field for details.";
                   problemDetails.Instance = context.HttpContext.Request.Path;

                    // find out which status code to use
                    var actionExecutingContext =
                         context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // if there are modelstate errors & all keys were correctly
                    // found/parsed we're dealing with validation errors
                    if ((context.ModelState.ErrorCount > 0) &&
                       (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                   {
                       problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                       problemDetails.Title = "One or more validation errors occurred.";

                       return new UnprocessableEntityObjectResult(problemDetails)
                       {
                           ContentTypes = { "application/problem+json" }
                       };
                   }

                    // if one of the keys wasn't correctly found / couldn't be parsed
                    // we're dealing with null/unparsable input
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                   problemDetails.Title = "One or more errors on input occurred.";
                   return new BadRequestObjectResult(problemDetails)
                   {
                       ContentTypes = { "application/problem+json" }
                   };
               };
           });
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.ConfigureDbContext(Configuration);
            services.ConfigureSwagger();
            services.ConfigureTokenAuth(Configuration);
            services.ConfigurePerformance();

            services.ConfigureDetection();

      
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IGatewayService, GatewayService>();
            services.AddTransient<IDeviceService, DeviceService>();

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;//"explorer";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GatewaysManagement V1");
            });
        }
    }
}
