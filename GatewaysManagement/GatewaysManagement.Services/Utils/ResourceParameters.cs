using System;
using System.Collections.Generic;
using System.Text;

namespace GatewaysManagement.Services.Utils
{
    public class ResourceParameters
    {
        public string SearchQuery { get; set; }
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string OrderDirection { get; set; }
        public string OrderBy { get; set; }
    }
}