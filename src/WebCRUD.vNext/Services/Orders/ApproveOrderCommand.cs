using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebCRUD.vNext.Infrastructure.Command;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Models.Domain.Common;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    public class ApproveOrderCommand : Command<Order>
    {
        private Guid orderId;

        public ApproveOrderCommand(Guid orderId)
        {
            this.orderId = orderId;
        }

        public override Order Execute()
        {
            var order = Context.Order
                .Include(o => o.OrderedItems)
                .ThenInclude(i => i.Product)
                .Include(o => o.Customer)
                .FirstOrDefault(x => x.Id == orderId);

            if (order == null)
                throw new BusinessException(BusinessErrorCodes.BusinessRulesViolation, string.Format("Cannot find order with id {0}", orderId));

            if(order.CanBeApproved)
                order.Approve();

            Context.SaveChanges();

            return order;
        }
    }
}