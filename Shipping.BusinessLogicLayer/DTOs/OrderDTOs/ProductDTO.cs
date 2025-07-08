using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.OrderDTOs
{
    public class ReadProductDTO
    {
        public ReadProductDTO() { }
        public int ProductId;
        public string Name;
        public decimal Price;
        public decimal Weight;
        public int Quantity;
        public int OrderId;
    }

    public record UpdateProductDTO
    (
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
