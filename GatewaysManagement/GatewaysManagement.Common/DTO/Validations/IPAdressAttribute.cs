using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;

namespace GatewaysManagement.Common.DTO.Validations
{
    class IPAdressAttribute : ValidationAttribute
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public IPAdressAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            string strValue = value as string;
            if (!string.IsNullOrEmpty(strValue))
            {
                if (strValue.Count(c => c == '.') != 3) return false;

                return IPAddress.TryParse(strValue, out IPAddress address);
            }
            return true;
        }
    }
}
