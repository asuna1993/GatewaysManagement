using System;
using System.Collections.Generic;
using System.Text;

namespace GatewaysManagement.Test.MockData
{
    public abstract class MockModelFactory<T>
    {
        public abstract T GetSingleObject(Guid id);
    }
}