using System;
using WebCRUD.vNext.Infrastructure.ExtensionMethods;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.ViewModels.Orders
{
    public class OrderSearchResultItem
    {
        /// <summary>
        /// Id of order
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Order number
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Date of order
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// name of customer who placed order
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Order status
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Returns translated order status
        /// </summary>
        public string StatusString => Status.TranslateEnum();

        /// <summary>
        /// Total amount of order
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Indicates if order can be approved.
        /// </summary>
        public bool CanBeApproved { get; set; }

        /// <summary>
        /// Indicates if order can be closed.
        /// </summary>
        public bool CanBeClosed { get; set; }
        
    }
}
