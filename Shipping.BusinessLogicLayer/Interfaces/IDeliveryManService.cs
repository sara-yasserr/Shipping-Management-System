using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IDeliveryManService
    {
        Task<PagedResponse<ReadDeliveryMan>> GetAllAsync( PaginationDTO pagination);
        Task<List<ReadDeliveryMan>> GetAllWithoutPaginationAsync();
        Task<ReadDeliveryMan> GetByIdAsync(int id);
        Task<(bool success, int deliveryManId)> AddAsync(AddDeliveryMan dto);
        Task<bool> UpdateAsync(int id, UpdateDeliveryMan dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> HardDeleteAsync(int id);
    }
} 