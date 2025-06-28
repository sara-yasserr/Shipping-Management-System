using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IDeliveryManService
    {
        Task<List<ReadDeliveryMan>> GetAllAsync();
        Task<ReadDeliveryMan> GetByIdAsync(int id);
        Task AddAsync(AddDeliveryMan dto);
        Task UpdateAsync(int id, AddDeliveryMan dto);
        Task DeleteAsync(int id);
    }
} 