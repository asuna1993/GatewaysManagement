using System;

namespace GatewaysManagement.Common.DTO.Response
{
    public class GatewayResponse
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string IPAdress { get; set; }
    }
}