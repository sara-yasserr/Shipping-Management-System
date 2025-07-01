using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using Shipping.DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IDeliveryManService
    {
        Task<List<ReadDeliveryMan>> GetAllAsync();
        Task<ReadDeliveryMan> GetByIdAsync(int id);
        Task<(bool success, int deliveryManId)> AddAsync(AddDeliveryMan dto);
        Task<bool> UpdateAsync(int id, UpdateDeliveryMan dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> HardDeleteAsync(int id);
    }
} 