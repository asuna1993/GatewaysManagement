using GatewaysManagement.Services.Impls;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using GatewaysManagement.Data;
using GatewaysManagement.Services;
using GatewaysManagement.Test.MockData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using GatewaysManagement.Data.Entities;
using GatewaysManagement.Data.UoW;
using GatewaysManagement.Services.Utils;

namespace GatewaysManagement.Test.Services
{
    public class GatewayServiceTest : IServiceTest<GatewayService>
    {
        [Fact]
        public async Task GetGatewayAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                GatewayService gatewayService = InitService(context);
                Gateway gateway = await gatewayService.GetGatewayAsync(Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F"));
                Assert.NotNull(gateway);
                Assert.Equal(Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F"), gateway.Id);
            }
        }

        [Fact]
        public async Task GetGatewaysAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                GatewayService gatewayService = InitService(context);
                IEnumerable<Gateway> gateway = await gatewayService.GetGatewaysAsync();
                Assert.NotEmpty(gateway);
            }
        }

        [Fact]
        public async Task AddGatewayAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();
                
                Gateway gateway = new Gateway
                {
                    Name = "Test Name for Add",
                    IPAdress = "192.168.1.1",
                    SerialNumber = "12345678911"
                };

                GatewayService gatewayService = InitService(context);

                await gatewayService.AddGatewayAsync(gateway);
                Gateway gatewaySaved = modelBuilder.GetMockGateway(gateway.Id);

                Assert.NotNull(gatewaySaved);
                Assert.Equal(gateway, gatewaySaved);
            }
        }

        [Fact]
        public async Task UpdateGatewayAsync_Test()
        {
            var GatewayIdToGet = Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F");
            var nameToChangeGateway = "New Name";
            
            using (CoreDbContext context = GetSampleData("SelltxtDbText"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();

                GatewayService gatewayService = InitService(context);

                Gateway gatewayToUpdate = modelBuilder.GetMockGateway(GatewayIdToGet);

                gatewayToUpdate.Name = nameToChangeGateway;

                await gatewayService.UpdateGatewayAsync(GatewayIdToGet, gatewayToUpdate);
                Gateway gatewaySaved = modelBuilder.GetMockGateway(GatewayIdToGet);

                Assert.NotNull(gatewaySaved);
                Assert.Equal(gatewaySaved.Id, GatewayIdToGet);
                Assert.Equal(gatewaySaved.Name, nameToChangeGateway);
            }
        }

        [Fact]
        public async Task DeleteGatewayAsync_Test()
        {
            var GatewayIdToDelete = Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F");

            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();

                GatewayService gatewayService = InitService(context);

                Gateway gatewayToDelete = modelBuilder.GetMockGateway(GatewayIdToDelete);

                await gatewayService.DeleteGatewayAsync(gatewayToDelete);

                Assert.Null(modelBuilder.GetMockGateway(GatewayIdToDelete));
            }
        }

        public CoreDbContext GetSampleData(string db)
        {
            DbContextOptionsBuilder<CoreDbContext> builder = new DbContextOptionsBuilder<CoreDbContext>();
            builder.UseInMemoryDatabase(databaseName: db);
            CoreDbContext context = new CoreDbContext(builder.Options);
            MockModelBuilder modelBuilder = new MockModelBuilder();
            Gateway gateway = modelBuilder.GetMockGateway(Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F"));
            context.Add(gateway);
            context.SaveChanges();
            return context;
        }

        public GatewayService InitService(CoreDbContext context)
        {
            UnitOfWork _uow = new UnitOfWork(context);
            IPropertyMappingService propertyMappingServiceMock = Substitute.For<IPropertyMappingService>();
            
            GatewayService gatewayService = new GatewayService(_uow, propertyMappingServiceMock);
            return gatewayService;
        }
    }
}