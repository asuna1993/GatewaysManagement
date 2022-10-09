using GatewaysManagement.Common.DTO.Validations;
using System.ComponentModel.DataAnnotations;

namespace GatewaysManagement.Common.DTO.Request
{
    public class CreateGatewayRequest
    {
        [Required]
        public string SerialNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [IPAdress]
        public string IPAdress { get; set; }
    }
}