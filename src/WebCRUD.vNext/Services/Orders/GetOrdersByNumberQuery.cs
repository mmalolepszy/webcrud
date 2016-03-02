using System;
using System.Collections.Generic;
using System.Linq;
using WebCRUD.vNext.Infrastructure.Query;
using WebCRUD.vNext.Models;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Gets order by its number
    /// </summary>
    public class GetOrdersByNumberQuery : Query<IList<Order>>
    {
        private string orderNumber;

        /// <summary>
        /// Construct query
        /// </summary>
        /// <param name="number">order number to search for</param>
        public GetOrdersByNumberQuery(string number)
        {
            orderNumber = number;
        }

        /// <summary>
        /// Execute query and return order object
        /// </summary>
        /// <returns>Order object if found, null otherwise</returns>
        public override IList<Order> Execute()
        {
            return Context.Order.Where(x => x.Number == orderNumber).ToList();
        }
    }
}