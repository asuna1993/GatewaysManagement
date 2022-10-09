using GatewaysManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GatewaysManagement.Test.MockData
{
    public class MockModelBuilder
    {
        public GatewayFactory GatewayPrototype { get; set; }
        public DeviceFactory DevicePrototype { get; set; }

        public MockModelBuilder()
        {
            GatewayPrototype = new GatewayFactory();
            DevicePrototype = new DeviceFactory();
        }

        public Gateway GetMockGateway(Guid gatewayId)
        {
            return GatewayPrototype.GetSingleObject(gatewayId);
        }

        public Device GetMockDevice(Guid deviceId)
        {
            return DevicePrototype.GetSingleObject(deviceId);
        }
    }
}