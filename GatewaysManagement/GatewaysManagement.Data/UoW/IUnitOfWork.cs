using GatewaysManagement.Data.Entities;
using GatewaysManagement.Data.Repository;
using System;
using System.Threading.Tasks;

namespace GatewaysManagement.Data.UoW
{
    public interface IUnitOfWork
    {
        IGatewayRepository GatewayRepository { get; set; }
        IDeviceRepository DeviceRepository { get; set; }
        Task<int> CommitAsync();
    }
}