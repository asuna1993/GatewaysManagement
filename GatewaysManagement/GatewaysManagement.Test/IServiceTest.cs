using GatewaysManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GatewaysManagement.Test
{
    public interface IServiceTest<T>
    {
        CoreDbContext GetSampleData(string db);

        T InitService(CoreDbContext context);
    }
}