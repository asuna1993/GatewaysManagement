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
using GatewaysManagement.Data.Entities.Enums;

namespace GatewaysManagement.Test.Services
{
    public class DeviceServiceTest : IServiceTest<DeviceService>
    {
        [Fact]
        public async void GetDeviceAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                DeviceService deviceService = InitService(context);
                Device device = await deviceService.GetDeviceAsync(Guid.Parse("75c3b965-c98a-4b9a-8d67-2cf9a3a98f1d"));
                Assert.NotNull(device);
                Assert.Equal(Guid.Parse("75c3b965-c98a-4b9a-8d67-2cf9a3a98f1d"), device.Id);
            }
        }


        [Fact]
        public async void AddDeviceAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();
                
                Device device = new Device
                {
                    UID = 5,
                    GatewayId = Guid.Parse("0c13787a-91a6-4f47-b06c-3912d91e0f5a"),
                    Vendor = "New Vendor",
                    Status = StatusEnum.OFFLINE                    
                };

                DeviceService deviceService = InitService(context);

                await deviceService.AddDeviceAsync(device);
                Device deviceSaved = await deviceService.GetDeviceAsync(device.Id);

                Assert.NotNull(deviceSaved);
                Assert.Equal(device, deviceSaved);
            }
        }

        [Fact]
        public async void UpdateDeviceAsync_Test()
        {
            var DeviceIdToGet = Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F");
            var vendorToChangeDevice = "New Vendor";
            
            using (CoreDbContext context = GetSampleData("SelltxtDbText"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();

                DeviceService deviceService = InitService(context);

                Device deviceToUpdate = await deviceService.GetDeviceAsync(DeviceIdToGet);

                deviceToUpdate.Vendor = vendorToChangeDevice;

                await deviceService.UpdateDeviceAsync(deviceToUpdate);
                Device deviceSaved = await deviceService.GetDeviceAsync(DeviceIdToGet);

                Assert.NotNull(deviceSaved);
                Assert.Equal(deviceSaved.Id, DeviceIdToGet);
                Assert.Equal(deviceSaved.Vendor, vendorToChangeDevice);
            }
        }

        [Fact]
        public async void DeleteDeviceAsync_Test()
        {
            var DeviceIdToDelete = Guid.Parse("08f7f9aa-6e41-49b0-82c1-1eba26ead6fc");

            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();

                DeviceService deviceService = InitService(context);

                Device deviceToDelete = await deviceService.GetDeviceAsync(DeviceIdToDelete);

                await deviceService.DeleteDeviceAsync(deviceToDelete);

                Assert.Null(await deviceService.GetDeviceAsync(DeviceIdToDelete));
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

            Gateway gateway = modelBuilder.GetMockGateway(Guid.Parse("0c13787a-91a6-4f47-b06c-3912d91e0f5a"));
            context.Add(gateway);
            context.SaveChanges();

            Device device = modelBuilder.GetMockDevice(Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F"));
            context.Add(device);

            Device device2 = modelBuilder.GetMockDevice(Guid.Parse("08f7f9aa-6e41-49b0-82c1-1eba26ead6fc"));
            context.Add(device2);

            Device device3 = modelBuilder.GetMockDevice(Guid.Parse("75c3b965-c98a-4b9a-8d67-2cf9a3a98f1d"));
            context.Add(device3);

            context.SaveChanges();
            return context;
        }

        public DeviceService InitService(CoreDbContext context)
        {
            UnitOfWork _uow = new UnitOfWork(context);
            IPropertyMappingService propertyMappingServiceMock = Substitute.For<IPropertyMappingService>();
            
            DeviceService deviceService = new DeviceService(_uow, propertyMappingServiceMock);
            return deviceService;
        }
    }
}