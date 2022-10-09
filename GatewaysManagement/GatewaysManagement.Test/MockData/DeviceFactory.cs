using GatewaysManagement.Data.Entities;
using GatewaysManagement.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GatewaysManagement.Test.MockData
{
    public class DeviceFactory : MockModelFactory<Device>
    {
        public override Device GetSingleObject(Guid id)
        {
            Gateway gateway = new Gateway
            {
                Id = Guid.Parse("1E5DB10J-6HL8-4T5B-85B4-7B05C8DBH59G"),
                Name = "Test Name",
                IPAdress = "192.168.1.1",
                SerialNumber = "123456789"
            };

            return new Device
            {
                Id = id,
                UID = 123,
                Vendor = "Vendor Test",
                Status = StatusEnum.ONLINE,
                CreatedAt = DateTime.Now,
                GatewayId = gateway.Id,
                Gateway = gateway
            };
        }
    }
}