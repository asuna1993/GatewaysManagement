using GatewaysManagement.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GatewaysManagement.Data.Entities
{
    public class Device
    {
        [Key]
        public Guid Id { get; set; }
        public int UID { get; set; }
        public string Vendor { get; set; }
        public DateTime CreatedAt { get; set; }
        public StatusEnum Status { get; set; }
        [ForeignKey("GatewayId")]
        public virtual Gateway Gateway { get; set; }
        public Guid GatewayId { get; set; }

    }
}