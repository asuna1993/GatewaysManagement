using GatewaysManagement.Services.Impls;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using GatewaysManagement.Data;
using GatewaysManagement.Test.MockData;
using System;
using System.Collections.Generic;
using Xunit;
using GatewaysManagement.Data.Entities;
using GatewaysManagement.Data.UoW;
using GatewaysManagement.Services.Utils;

namespace GatewaysManagement.Test.Services
{
    public class GatewayServiceTest : IServiceTest<GatewayService>
    {
        [Fact]
        public async void GetGatewayAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                GatewayService gatewayService = InitService(context);
                Gateway gateway = await gatewayService.GetGatewayAsync(Guid.Parse("af32fea6-1a8b-428f-872c-6a1ce83d8028"));
                Assert.NotNull(gateway);
                Assert.Equal(Guid.Parse("af32fea6-1a8b-428f-872c-6a1ce83d8028"), gateway.Id);
            }
        }

        [Fact]
        public async void GetGatewaysAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                GatewayService gatewayService = InitService(context);
                IEnumerable<Gateway> gateway = await gatewayService.GetGatewaysAsync();
                Assert.NotEmpty(gateway);
            }
        }

        [Fact]
        public async void AddGatewayAsync_Test()
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
                Gateway gatewaySaved = await gatewayService.GetGatewayAsync(gateway.Id);

                Assert.NotNull(gatewaySaved);
                Assert.Equal(gateway, gatewaySaved);
            }
        }

        [Fact]
        public async void UpdateGatewayAsync_Test()
        {
            var GatewayIdToGet = Guid.Parse("50b572ec-7a15-4ff5-aca3-4659d599b527");
            var nameToChangeGateway = "New Name";
            
            using (CoreDbContext context = GetSampleData("SelltxtDbText"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();

                GatewayService gatewayService = InitService(context);

                Gateway gatewayToUpdate = await gatewayService.GetGatewayAsync(GatewayIdToGet);

                gatewayToUpdate.Name = nameToChangeGateway;

                await gatewayService.UpdateGatewayAsync(gatewayToUpdate);
                Gateway gatewaySaved = await gatewayService.GetGatewayAsync(GatewayIdToGet);


                Assert.NotNull(gatewaySaved);
                Assert.Equal(gatewaySaved.Id, GatewayIdToGet);
                Assert.Equal(gatewaySaved.Name, nameToChangeGateway);
            }
        }

        [Fact]
        public async void DeleteGatewayAsync_Test()
        {
            var GatewayIdToDelete = Guid.Parse("adea8952-f6e6-4c30-9356-6bcbb8fb2c46");

            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();

                GatewayService gatewayService = InitService(context);

                Gateway gatewayToDelete = await gatewayService.GetGatewayAsync(GatewayIdToDelete);

                await gatewayService.DeleteGatewayAsync(gatewayToDelete);

                Assert.Null(await gatewayService.GetGatewayAsync(GatewayIdToDelete));
            }
        }

        public CoreDbContext GetSampleData(string db)
        {
            DbContextOptionsBuilder<CoreDbContext> builder = new DbContextOptionsBuilder<CoreDbContext>();
            builder.UseInMemoryDatabase(databaseName: db);
            CoreDbContext context = new CoreDbContext(builder.Options);
            MockModelBuilder modelBuilder = new MockModelBuilder();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Gateway gateway = modelBuilder.GetMockGateway(Guid.Parse("50b572ec-7a15-4ff5-aca3-4659d599b527"));
            context.Add(gateway);

            Gateway gateway2 = modelBuilder.GetMockGateway(Guid.Parse("af32fea6-1a8b-428f-872c-6a1ce83d8028"));
            context.Add(gateway2);

            Gateway gateway3 = modelBuilder.GetMockGateway(Guid.Parse("adea8952-f6e6-4c30-9356-6bcbb8fb2c46"));
            context.Add(gateway3);

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