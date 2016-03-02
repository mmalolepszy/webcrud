using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebCRUD.vNext.Infrastructure.ExtensionMethods;
using WebCRUD.vNext.Models.Domain.Orders;
using WebCRUD.vNext.Resources;

namespace WebCRUD.vNext.ViewModels.Orders
{
    /// <summary>
    /// Form data for order creation and editing
    /// </summary>
    public class OrderForm
    {
        /// <summary>
        /// Id of order (hidden in form)
        /// </summary>
        public Guid? OrderId { get; set; }

        /// <summary>
        /// Number of order
        /// </summary>
        [Required, MaxLength(50)]
        
        [Display(Name = "OrderForm_OrderNumber", ResourceType = typeof(Labels))]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Order status (new/approved/closed)
        /// </summary>
        [Display(Name = "OrderForm_Status", ResourceType = typeof(Labels))]
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Order date (defaults to now)
        /// </summary>
        [Required, DataType(DataType.Date)]
        [Display(Name = "OrderForm_Date", ResourceType = typeof(Labels))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Id of customer (hidden in form)
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        [Required]
        [Display(Name = "OrderForm_CustomerName", ResourceType = typeof(Labels))]
        public string CustomerName{ get; set; }

        /// <summary>
        /// Form parts for each ordered product
        /// </summary>
        public List<OrderItemForm> Items { get; set; }

        public decimal TotalAmount { get; set; }

        /// <summary>
        /// List of all possible order statuses
        /// </summary>
        public List<SelectListItem> StatusList
        {
            get { return EnumHelper.EnumToSelectListItems<OrderStatus>().ToList(); }
        }

        /// <summary>
        /// Empty constructor for data binding
        /// </summary>
        public OrderForm() 
        { }

        /// <summary>
        /// Constructs empty form
        /// </summary>
        /// <param name="products">List of available products</param>
        public OrderForm(IList<Product> products)
        {
            Items = new List<OrderItemForm> {
                new OrderItemForm(products: products)
            };
        }

        /// <summary>
        /// Constructs form filled with data
        /// </summary>
        /// <param name="receivedOrderForm"></param>
        /// <param name="products"></param>
        public OrderForm(OrderForm receivedOrderForm, IList<Product> products) 
        {
            this.OrderId = receivedOrderForm.OrderId;
            this.CustomerId = receivedOrderForm.CustomerId;
            this.OrderNumber = receivedOrderForm.OrderNumber;
            this.Date = receivedOrderForm.Date;
            this.Status = receivedOrderForm.Status;
            this.CustomerName = receivedOrderForm.CustomerName;
            this.Items = new List<OrderItemForm>();
            this.TotalAmount = receivedOrderForm.TotalAmount;

            if (receivedOrderForm.Items != null)
            {
                receivedOrderForm.Items.ForEach(item => this.Items.Add(new OrderItemForm(item, products)));
            }
        }
    }

    /// <summary>
    /// Form data of single item from order
    /// </summary>
    public class OrderItemForm
    {
        /// <summary>
        /// Id of ordered item (hidden in form)
        /// </summary>
        public Guid? OrderItemId { get; set; }

        /// <summary>
        /// Id of ordered product
        /// </summary>
        [Required]
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Product unit price
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Quantity of selected product
        /// </summary>
        [Required]
        public int? Quantity { get; set; }

        /// <summary>
        /// Discount for product (percentage)
        /// </summary>
        public decimal? Discount { get; set; }

        /// <summary>
        /// Total amount for this product
        /// </summary>
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// List of available products
        /// </summary>
        public List<SelectListItem> Products { get; set; }

        /// <summary>
        /// Empty constructor for data binding
        /// </summary>
        public OrderItemForm()
        { }

        /// <summary>
        /// Cunstruct an empty form
        /// </summary>
        /// <param name="products">List of available products</param>
        public OrderItemForm(IList<Product> products)
        {
            this.Products = (from p in products select new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            this.Products.Insert(0, new SelectListItem());
        }

        /// <summary>
        /// Construct a form filled with data
        /// </summary>
        /// <param name="receivedForm">Form data received</param>
        /// <param name="products">List of available products</param>
        public OrderItemForm(OrderItemForm receivedForm, IList<Product> products) : this(products)
        {
            this.OrderItemId = receivedForm.OrderItemId;
            this.ProductId = receivedForm.ProductId;
            this.Price = receivedForm.Price;
            this.Quantity = receivedForm.Quantity;
            this.Discount = receivedForm.Discount;
            this.TotalAmount = receivedForm.TotalAmount;
        }
    }
}
