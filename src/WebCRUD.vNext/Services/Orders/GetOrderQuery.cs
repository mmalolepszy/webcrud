using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Infrastructure.Query;
using WebCRUD.vNext.ViewModels.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Gets the order from database for edit.
    /// </summary>
    public class GetOrderQuery : Query<OrderForm>
    {
        private readonly Guid orderId;
        
        /// <summary>
        /// Creates the query.
        /// </summary>
        /// <param name="orderId">id of the order to load</param>
        public GetOrderQuery(Guid orderId)
        {
            this.orderId = orderId;
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        public override OrderForm Execute()
        {
            var order = Context.Order
                .Include(o => o.OrderedItems)
                .ThenInclude(i => i.Product)
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new TechnicalException(string.Format("Unable to find order with id: {0}", orderId));
            
            return OrderToModelMapper.MapToForm(order);
        }
    }
}