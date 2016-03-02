using System;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Models.Domain.Common;

namespace WebCRUD.vNext.Models.Domain.Orders
{
    /// <summary>
    /// represents one item from placed order
    /// </summary>
    public class OrderItem : Entity<Guid>
    {
        /// <summary>
        /// quantity of products in order
        /// </summary>
        public int Quantity { get; protected set; }

        /// <summary>
        /// total amount to be paid for product
        /// </summary>
        public decimal TotalAmount { get; protected set; }

        /// <summary>
        /// discount (in percent)
        /// </summary>
        public decimal Discount { get; protected set; }

        /// <summary>
        /// product price at the moment of placing order
        /// </summary>
        public decimal Price { get; protected set; }

        public Guid ProductId { get; protected set; }
        public Guid OrderId { get; protected set; }

        /// <summary>
        /// Order in which this Item belongs
        /// </summary>
        public virtual Order Order { get; protected set; }

        /// <summary>
        /// ordered product
        /// </summary>
        public virtual Product Product { get; protected set; }

        public OrderItem()
        { }

        public OrderItem(Product product, int quantity, decimal discount)
        {
            ChangeProduct(product);
            ChangeQuantity(quantity);
            ChangeDiscount(discount);
        }

        /// <summary>
        /// Calculates total amount for ordered products including disocunt
        /// </summary>
        /// <returns></returns>
        public decimal CalculateTotalAmount()
        {
            decimal totalAmountBeforeDiscount = Price * Quantity;
            decimal dicountAmount = totalAmountBeforeDiscount * Discount / 100;
            return totalAmountBeforeDiscount - dicountAmount;
        }

        public void ChangeProduct(Product product)
        {
            if(product == null)
                throw new BusinessException(Common.BusinessErrorCodes.DomainModelRulesViolation, "OrderItem must have Product");

            Product = product;
            Price = product.Price;

            TotalAmount = CalculateTotalAmount();
        }

        internal void ChangeQuantity(int quantity)
        {
            if (quantity < 0)
                throw new BusinessException(Common.BusinessErrorCodes.DomainModelRulesViolation, "Quantity cannot be negative");

            Quantity = quantity;
            TotalAmount = CalculateTotalAmount();
        }

        internal void ChangeDiscount(decimal discount)
        {
            if (discount < 0)
                throw new BusinessException(Common.BusinessErrorCodes.DomainModelRulesViolation, "Discount cannot be negative");

            Discount = discount;
            TotalAmount = CalculateTotalAmount();
        }
    }
}