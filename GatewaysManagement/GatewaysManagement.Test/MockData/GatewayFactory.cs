using GatewaysManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GatewaysManagement.Test.MockData
{
    public class GatewayFactory : MockModelFactory<Gateway>
    {
        public override Gateway GetSingleObject(Guid id)
        {            
            return new Gateway
            {
                Id = id,
                Name = "Test Name",
                IPAdress = "192.168.1.1",
                SerialNumber = "123456789"
            };
        }
    }
}