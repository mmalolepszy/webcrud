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
    public class OrderSearchForm
    {
        /// <summary>
        /// Selected customer id
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// List of customers to select from
        /// </summary>
        [Display(Name = "OrderSearchForm_Customers", ResourceType = typeof(Labels))]
        public List<SelectListItem> Customers { get; set; }

        /// <summary>
        /// Number of order, allows * wilcards, search for partial matches (LIKE)
        /// </summary>
        [Display(Name = "OrderSearchForm_OrderNumber", ResourceType = typeof(Labels))]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Order date from.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "OrderSearchForm_OrderDateFrom", ResourceType = typeof(Labels))]
        public DateTime? OrderDateFrom { get; set; }
        
        /// <summary>
        /// Order date to.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? OrderDateTo { get; set; }

        /// <summary>
        /// Total order amount from
        /// </summary>
        [DataType(DataType.Currency)]
        [Display(Name = "OrderSearchForm_TotalAmountFrom", ResourceType = typeof(Labels))]
        public decimal? TotalAmountFrom { get; set; }

        /// <summary>
        /// Total order amount to
        /// </summary>
        [DataType(DataType.Currency)]
        public decimal? TotalAmountTo { get; set; }
        
        /// <summary>
        /// Id of product
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Products to select from
        /// </summary>
        [Display(Name = "OrderSearchForm_Products", ResourceType = typeof(Labels))]
        public List<SelectListItem> Products { get; set; }

        /// <summary>
        /// Status name
        /// </summary>
        public OrderStatus? Status { get; set; }

        /// <summary>
        /// List of possible statuses
        /// </summary>
        public List<SelectListItem> Statuses => EnumHelper.EnumToSelectListItemsWithEmptyElement<OrderStatus>().ToList();

        /// <summary>
        /// empty constructor for databinding scenarios
        /// </summary>
        public OrderSearchForm()
        {
            Customers = new List<SelectListItem>();
            Products = new List<SelectListItem>();
        }
        
        /// <summary>
        /// Construct new empty order search form
        /// </summary>
        /// <param name="customers">list of all customers</param>
        /// <param name="products">list of all products</param>
        public OrderSearchForm(IList<Customer> customers, IList<Product> products)
        {
            this.Customers = customers.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            this.Customers.Insert(0, new SelectListItem());

            this.Products = products.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
            this.Products.Insert(0, new SelectListItem());
        }

        /// <summary>
        /// Constructs order search form with values from previous search
        /// </summary>
        /// <param name="receivedForm">previously searched values</param>
        /// <param name="customers">list of all customers</param>
        /// <param name="products">list of all products</param>
        public OrderSearchForm(OrderSearchForm receivedForm, IList<Customer> customers, IList<Product> products)
            : this(customers, products)
        {
            this.CustomerId = receivedForm.CustomerId;
            this.OrderDateFrom = receivedForm.OrderDateFrom;
            this.OrderDateTo = receivedForm.OrderDateTo;
            this.OrderNumber = receivedForm.OrderNumber;
            this.ProductId = receivedForm.ProductId;
            this.TotalAmountFrom = receivedForm.TotalAmountTo;
            this.Status = receivedForm.Status;
        }
    }
}