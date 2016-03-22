using System;
using System.Collections.Generic;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.ViewModels.Orders
{
    /// <summary>
    /// View model for order creation and editing
    /// </summary>
    public class OrderViewModel
    {
        public OrderForm OrderForm { get; set; }

        public OrderViewModel()
        { }

        /// <summary>
        /// Constructs empty viewmodel
        /// </summary>
        /// <param name="products">list of all available products</param>
        public OrderViewModel(IList<Product> products)
        {
            this.OrderForm = new OrderForm(products)
                {
                    Date = DateTime.Now
                };
        }

        /// <summary>
        /// Constructs view model containing form data
        /// </summary>
        /// <param name="receivedOrderForm">form data retrieved from client or db</param>
        /// <param name="products">list of all available products</param>
        public OrderViewModel(OrderForm receivedOrderForm, IList<Product> products)
        {
            this.OrderForm = new OrderForm(receivedOrderForm, products);
        }
    }
}