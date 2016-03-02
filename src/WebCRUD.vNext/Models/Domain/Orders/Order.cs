using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Models.Domain.Common;

namespace WebCRUD.vNext.Models.Domain.Orders
{
    /// <summary>
    /// Represents order
    /// </summary>
    public class Order : Entity<Guid>
    {
        /// <summary>
        /// Order number
        /// </summary>
        public string Number { get; protected set; }

        /// <summary>
        /// Date when order was placed
        /// </summary>
        public DateTime OrderDate { get; protected set; }

        /// <summary>
        /// Current status of order
        /// </summary>
        public OrderStatus Status { get; protected set; }

        /// <summary>
        /// Customer id
        /// </summary>
        public Guid CustomerId { get; protected set; }

        /// <summary>
        /// Customer who placed the order
        /// </summary>
        public virtual Customer Customer { get; protected set; }

        /// <summary>
        /// Collection of ordered items
        /// </summary>
        public virtual ICollection<OrderItem> OrderedItems { get; protected set; }

        /// <summary>
        /// Total amount of order
        /// </summary>
        [NotMapped]
        public decimal TotalAmount => OrderedItems != null ? OrderedItems.Sum(x => x.TotalAmount) : 0;

        /// <summary>
        /// Checks if order can be approved
        /// </summary>
        [NotMapped]
        public bool CanBeApproved => Status == OrderStatus.New;

        /// <summary>
        /// Checks if order can be closeds
        /// </summary>
        [NotMapped]
        public bool CanBeClosed => (Status == OrderStatus.New || Status == OrderStatus.Approved);

        /// <summary>
        /// Validates if all product items has a product associated
        /// </summary>
        /// <returns>
        /// true  - if all order items associated with this order has a product associated
        /// false - otherwise
        /// </returns>
        [NotMapped]
        public bool AllItemsHasProducts => OrderedItems == null || OrderedItems.All(x => x.Product != null);

        public Order()
        { }

        public Order(string number, DateTime orderDate)
        {
            this.Number = number;
            this.OrderDate = orderDate;
            this.Status = OrderStatus.New;
        }

        /// <summary>
        /// Add item to order
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(OrderItem item)
        {
            if (OrderedItems == null)
                OrderedItems = new List<OrderItem>();

            OrderedItems.Add(item);
        }

        /// <summary>
        /// Remove item from order
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(OrderItem item)
        {
            if (OrderedItems == null || !OrderedItems.Contains(item))
                return;

            OrderedItems.Remove(item);
        }

        /// <summary>
        /// Approve order
        /// </summary>
        public void Approve()
        {
            if (!CanBeApproved)
                throw new BusinessException(Common.BusinessErrorCodes.DomainModelRulesViolation, "Cannot aprove order in status {0}", Status);

            Status = OrderStatus.Approved;
        }

        /// <summary>
        /// Mark order as closed
        /// </summary>
        public void Close()
        {
            if (!CanBeClosed)
                throw new BusinessException(Common.BusinessErrorCodes.DomainModelRulesViolation, "Cannot close order in status {0}", Status);

            Status = OrderStatus.Closed;
        }

        /// <summary>
        /// Change customer who placed order
        /// </summary>
        /// <param name="customer"></param>
        public void ChangeCustomer(Customer customer)
        {
            if (customer == null)
                throw new BusinessException(Common.BusinessErrorCodes.DomainModelRulesViolation, "Order must have customer");

            Customer = customer;
        }
    }

    /// <summary>
    /// Status of order
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Initial status - order can be approved or closed
        /// </summary>
        //[Display(Name = "OrderStatus_New", ResourceType = typeof(Labels))]
        New,

        /// <summary>
        /// Intermediate status - order is approved for filling, status can be changed to closed
        /// </summary>
        //[Display(Name = "OrderStatus_Approved", ResourceType = typeof(Labels))]
        Approved,

        /// <summary>
        /// Final status - order is filled or rejected
        /// </summary>
        //[Display(Name = "OrderStatus_Closed", ResourceType = typeof(Labels))]
        Closed
    }
}