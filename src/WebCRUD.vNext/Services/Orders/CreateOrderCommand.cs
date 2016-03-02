using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using WebCRUD.vNext.Infrastructure.Command;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Models.Domain.Orders;
using WebCRUD.vNext.Services.Validation;
using WebCRUD.vNext.ViewModels.Orders;
using WebCRUD.vNext.Models.Domain.Common;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Creates order
    /// </summary>
    public class CreateOrderCommand : Command<Guid>
    {
        private OrderForm orderForm;


        private IOrderValidationService orderValidationService;

        public CreateOrderCommand(OrderForm orderForm)
        {
            this.orderForm = orderForm;
        }

        /// <summary>
        /// Builds entity object from form data, checks business validation rules and saves entity
        /// </summary>
        /// <returns>newly inserted order id</returns>
        public override Guid Execute()
        {
            var customer = Context.Customer.First(x => x.Id == orderForm.CustomerId);
            if (customer == null)
                throw new BusinessException(BusinessErrorCodes.BusinessRulesViolation, string.Format("No customer with id {0}", orderForm.CustomerId));

            var newOrder = customer.PlaceOrder(orderForm.OrderNumber, orderForm.Date);

            foreach(var item in orderForm.Items)
            {
                var product = Context.Product.First(x => x.Id == item.ProductId);
                if (product == null)
                    throw new BusinessException(BusinessErrorCodes.BusinessRulesViolation, string.Format("No product with id {0}", item.ProductId));

                newOrder.AddItem(new OrderItem(product, item.Quantity.Value, item.Discount.Value));
            }

            if (!orderValidationService.IsOrderNumberUnique(newOrder))
                throw new BusinessException(BusinessErrorCodes.BusinessRulesViolation, "Order number not unique");

            Context.SaveChanges();

            return newOrder.Id;
        }

        public override void SetupDependencies(IServiceProvider provider)
        {
            base.SetupDependencies(provider);
            orderValidationService = provider.GetService<IOrderValidationService>();
        }
    }
}