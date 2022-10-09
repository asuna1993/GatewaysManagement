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
using GatewaysManagement.Data.Entities.Enums;

namespace GatewaysManagement.Test.Services
{
    public class DeviceServiceTest : IServiceTest<DeviceService>
    {
        [Fact]
        public async Task GetDeviceAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                DeviceService deviceService = InitService(context);
                Device device = await deviceService.GetDeviceAsync(Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F"));
                Assert.NotNull(device);
                Assert.Equal(Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F"), device.Id);
            }
        }

        [Fact]
        public async Task GetDevicesAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                DeviceService deviceService = InitService(context);
                IEnumerable<Device> device = await deviceService.GetDevicesAsync(Guid.Parse("1E5DB10J-6HL8-4T5B-85B4-7B05C8DBH59G"));
                Assert.NotEmpty(device);
            }
        }

        [Fact]
        public async Task AddDeviceAsync_Test()
        {
            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();
                
                Device device = new Device
                {
                    UID = 5,
                    GatewayId = Guid.Parse("1E5DB10J-6HL8-4T5B-85B4-7B05C8DBH59G"),
                    Vendor = "New Vendor",
                    Status = StatusEnum.OFFLINE                    
                };

                DeviceService deviceService = InitService(context);

                await deviceService.AddDeviceAsync(device);
                Device deviceSaved = modelBuilder.GetMockDevice(device.Id);

                Assert.NotNull(deviceSaved);
                Assert.Equal(device, deviceSaved);
            }
        }

        [Fact]
        public async Task UpdateDeviceAsync_Test()
        {
            var DeviceIdToGet = Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F");
            var vendorToChangeDevice = "New Vendor";
            
            using (CoreDbContext context = GetSampleData("SelltxtDbText"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();

                DeviceService deviceService = InitService(context);

                Device deviceToUpdate = modelBuilder.GetMockDevice(DeviceIdToGet);

                deviceToUpdate.Vendor = vendorToChangeDevice;

                await deviceService.UpdateDeviceAsync(DeviceIdToGet, deviceToUpdate);
                Device deviceSaved = modelBuilder.GetMockDevice(DeviceIdToGet);

                Assert.NotNull(deviceSaved);
                Assert.Equal(deviceSaved.Id, DeviceIdToGet);
                Assert.Equal(deviceSaved.Vendor, vendorToChangeDevice);
            }
        }

        [Fact]
        public async Task DeleteDeviceAsync_Test()
        {
            var DeviceIdToDelete = Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F");

            using (CoreDbContext context = GetSampleData("CoreDbContext"))
            {
                MockModelBuilder modelBuilder = new MockModelBuilder();

                DeviceService deviceService = InitService(context);

                Device deviceToDelete = modelBuilder.GetMockDevice(DeviceIdToDelete);

                await deviceService.DeleteDeviceAsync(deviceToDelete);

                Assert.Null(modelBuilder.GetMockDevice(DeviceIdToDelete));
            }
        }

        public CoreDbContext GetSampleData(string db)
        {
            DbContextOptionsBuilder<CoreDbContext> builder = new DbContextOptionsBuilder<CoreDbContext>();
            builder.UseInMemoryDatabase(databaseName: db);
            CoreDbContext context = new CoreDbContext(builder.Options);
            MockModelBuilder modelBuilder = new MockModelBuilder();
            
            Gateway gateway = modelBuilder.GetMockGateway(Guid.Parse("1E5DB10J-6HL8-4T5B-85B4-7B05C8DBH59G"));
            context.Add(gateway);

            Device device = modelBuilder.GetMockDevice(Guid.Parse("9D2DD00A-6FE8-464B-85B4-7B05B8CBF59F"));
            context.Add(device);

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