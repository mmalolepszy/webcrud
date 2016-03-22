using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebCRUD.vNext.Infrastructure.Command;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Models.Domain.Common;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    public class CloseOrderCommand : Command<Order>
    {
        private Guid orderId;

        public CloseOrderCommand(Guid orderId)
        {
            this.orderId = orderId;
        }

        public override Order Execute()
        {
            var order = Context.Order
                .Include(o => o.OrderedItems)
                .ThenInclude(i => i.Product)
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                throw new BusinessException(BusinessErrorCodes.BusinessRulesViolation, string.Format("Cannot find order with id {0}", orderId));

            if (order.CanBeClosed)
                order.Close();

            Context.SaveChanges();

            return order;
        }
    }
}