using GatewaysManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GatewaysManagement.Test
{
    public interface IRepositoryTest
    {
        CoreDbContext GetSampleData(string db);
    }
}