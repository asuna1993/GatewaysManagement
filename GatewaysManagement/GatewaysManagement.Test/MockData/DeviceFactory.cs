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

            return new Device
            {
                Id = id,
                UID = 123,
                Vendor = "Vendor Test",
                Status = StatusEnum.ONLINE,
                CreatedAt = DateTime.Now,
                GatewayId = Guid.Parse("0c13787a-91a6-4f47-b06c-3912d91e0f5a")
            };
        }
    }
}