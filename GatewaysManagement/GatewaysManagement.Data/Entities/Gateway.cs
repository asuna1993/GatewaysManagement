using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GatewaysManagement.Data.Entities
{
    public class Gateway
    {
        public Gateway()
        {
            Devices = new HashSet<Device>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        [MinLength(7)]
        [MaxLength(15)]
        public string IPAdress { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}