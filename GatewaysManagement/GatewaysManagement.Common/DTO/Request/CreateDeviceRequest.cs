using System;
using System.ComponentModel.DataAnnotations;

namespace GatewaysManagement.Common.DTO.Request
{
    public class CreateDeviceRequest
    {
        [Required]
        public int UID { get; set; }
        [Required]
        public string Vendor { get; set; }
        [Required]
        public int StatusId { get; set; }
    }
}