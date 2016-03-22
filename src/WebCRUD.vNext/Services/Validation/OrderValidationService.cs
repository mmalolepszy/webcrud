using System;
using System.Linq;
using WebCRUD.vNext.Infrastructure.Service;
using WebCRUD.vNext.Models.Domain.Orders;
using WebCRUD.vNext.Services.Orders;

namespace WebCRUD.vNext.Services.Validation
{
    /// <summary>
    /// Check business rules related to order objects
    /// </summary>
    public interface IOrderValidationService
    {
        bool IsOrderNumberUnique(Order order);
    }

    /// <summary>
    /// Check business rules related to order objects
    /// </summary>
    public class OrderValidationService : BusinessService, IOrderValidationService
    {
        public OrderValidationService(IServiceProvider provider)
            : base(provider)
        { }

        /// <summary>
        /// Validates if order number is unique across all orders
        /// </summary>
        /// <param name="order">order to check</param>
        /// <returns>true if order number is unique</returns>
        public bool IsOrderNumberUnique(Order order)
        {
            var ordersWithTheSameNumber = Query(new GetOrdersByNumberQuery(order.Number));
            return ordersWithTheSameNumber.Count == 0 || 
                (ordersWithTheSameNumber.Count == 1 && order.Equals(ordersWithTheSameNumber.First()));
        }
    }
}