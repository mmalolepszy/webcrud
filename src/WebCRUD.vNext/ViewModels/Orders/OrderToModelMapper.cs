using System.Linq;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.ViewModels.Orders
{
    /// <summary>
    /// Maps order entities to application models
    /// </summary>
    public class OrderToModelMapper
    {
        public static OrderSearchResultItem MapToSearchResultsItem(Order order)
        {
            return new OrderSearchResultItem
            {
                OrderId = order.Id,
                OrderNumber = order.Number,
                OrderDate = order.OrderDate,
                CustomerName = order.Customer.Name,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CanBeApproved = order.CanBeApproved,
                CanBeClosed = order.CanBeClosed
            };
        }

        public static OrderForm MapToForm(Order entity)
        {
            return new OrderForm
            {
                CustomerId = entity.Customer.Id,
                CustomerName = entity.Customer.Name,
                Date = entity.OrderDate,
                OrderId = entity.Id,
                OrderNumber = entity.Number,
                Status = entity.Status,
                TotalAmount = entity.TotalAmount,
                Items = entity.OrderedItems.Select(OrderToModelMapper.MapOrderItemToForm).ToList()
            };
        }
        private static OrderItemForm MapOrderItemToForm(OrderItem entity)
        {
            return new OrderItemForm
            {
                OrderItemId = entity.Id,
                ProductId = entity.Product.Id,
                Discount = entity.Discount,
                Price = entity.Price,
                Quantity = entity.Quantity,
                TotalAmount = entity.CalculateTotalAmount()
            };
        }
    
    }
}