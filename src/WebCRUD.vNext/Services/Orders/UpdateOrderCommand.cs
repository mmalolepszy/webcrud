using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using WebCRUD.vNext.Infrastructure.ExtensionMethods;
using WebCRUD.vNext.Infrastructure.Command;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Models.Domain.Orders;
using WebCRUD.vNext.Services.Validation;
using WebCRUD.vNext.ViewModels.Orders;
using Microsoft.EntityFrameworkCore;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Creates order
    /// </summary>
    public class UpdateOrderCommand : Command<Order>
    {
        private OrderForm orderForm;
        private IOrderValidationService orderValidationService;

        public UpdateOrderCommand(OrderForm orderForm)
        {
            this.orderForm = orderForm;
        }

        /// <summary>
        /// Builds entity object from form data, checks business validation rules and saves entity
        /// </summary>
        /// <returns>newly inserted order id</returns>
        public override Order Execute()
        {
            var order = Context.Order
                .Include(o => o.OrderedItems)
                .ThenInclude(i => i.Product)
                .Include(o => o.Customer)
                .FirstOrDefault(x => x.Id == orderForm.OrderId);

            if (order == null)
                throw new TechnicalException(string.Format("Cannot find order with id {0}", orderForm.OrderId));

            UpdateOrderEntity(order, orderForm);

            Context.SaveChanges();

            return order;
        }

        public override void SetupDependencies(IServiceProvider provider)
        {
            base.SetupDependencies(provider);
            orderValidationService = provider.GetService<IOrderValidationService>();
        }

        private void UpdateOrderEntity(Order order, OrderForm form)
        {
            if (order.CanBeApproved && form.Status == OrderStatus.Approved)
                order.Approve();

            if (order.CanBeClosed && form.Status == OrderStatus.Closed)
                order.Close();

            if (order.Customer.Id != form.CustomerId)
            {
                var customer = Context.Customer.First(x => x.Id == form.CustomerId);
                order.ChangeCustomer(customer);
            }

            order.OrderedItems.MergeUsing(form.Items)
                .On((orderItem, orderItemForm) => orderItem.Id == orderItemForm.OrderItemId)
                .WhenNotMatchedCreate(orderItemForm => ConstructOrdredItem(order, orderItemForm))
                .WhenMatchedUpdate((orderItem, orderItemForm) => UpdateOrderItem(orderItem, orderItemForm))
                .WhenRemoved(orderItem => order.RemoveItem(orderItem))
                .ExecuteMerge();
        }

        private void UpdateOrderItem(OrderItem orderItem, OrderItemForm item)
        {
            if (orderItem.ProductId != item.ProductId)
            {
                orderItem.ChangeProduct(LoadProduct(item.ProductId));
            }

            if(item.Quantity.HasValue && item.Quantity.Value != orderItem.Quantity)
                orderItem.ChangeQuantity(item.Quantity.Value);

            if (item.Discount.HasValue && item.Discount.Value != orderItem.Discount)
                orderItem.ChangeDiscount(item.Discount.Value);
        }

        private OrderItem ConstructOrdredItem(Order order, OrderItemForm item)
        {
            var orderedItem = new OrderItem(
                LoadProduct(item.ProductId), 
                item.Quantity.HasValue ? item.Quantity.Value : 0, 
                item.Discount.HasValue ? item.Discount.Value : 0);

            order.AddItem(orderedItem);

            return orderedItem;
        }

        private Product LoadProduct(Guid? productId)
        {
            return productId.HasValue ? Context.Product.First(x => x.Id == productId) : null;
        }
    }
}