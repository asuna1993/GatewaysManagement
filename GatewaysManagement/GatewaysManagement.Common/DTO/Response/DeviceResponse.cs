using System;

namespace GatewaysManagement.Common.DTO.Response
{
    public class DeviceResponse
    {
        public Guid Id { get; set; }
        public int UID { get; set; }
        public string Vendor { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public Guid GatewayId { get; set; }
        public string Gateway { get; set; }
    }
}