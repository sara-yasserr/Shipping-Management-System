using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.OrderDTOs
{
    public record ReadProductDTO
    (
        int ProductId,
        string Name,
        decimal Price,
        decimal Weight,
        int Quantity,
        int OrderId
    );

    public record ProductDTO
    (
        string Name,
        decimal Price,
        decimal Weight,
        int Quantity,
        int OrderId
    );

    public record AddProductDTO
    (
        string Name,
        decimal Price,
        decimal Weight,
        int Quantity
    );
}
