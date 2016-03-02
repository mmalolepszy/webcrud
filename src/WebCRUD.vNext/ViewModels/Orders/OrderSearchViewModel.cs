using System;
using System.Collections.Generic;
using System.Linq;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.ViewModels.Orders
{
    public class OrderSearchViewModel
    {
        /// <summary>
        /// Search form
        /// </summary>
        public OrderSearchForm SearchForm { get; set; }

        public IList<OrderSearchResultItem> Results { get; set; }

        public OrderSearchViewModel()
        { }

        /// <summary>
        /// Construct an empty view
        /// </summary>
        /// <param name="customers">list of all customers</param>
        /// <param name="products">list of all products</param>
        public OrderSearchViewModel(IList<Customer> customers, IList<Product> products)
        {
            SearchForm = new OrderSearchForm(customers, products);
        }

        /// <summary>
        /// COnstruct a view with data from previous search
        /// </summary>
        /// <param name="receivedForm">previously searched values</param>
        /// <param name="customers">list of all customers</param>
        /// <param name="products">list of all products</param>
        public OrderSearchViewModel(OrderSearchForm receivedForm, IList<Customer> customers, IList<Product> products)
        {
            SearchForm = new OrderSearchForm(receivedForm, customers, products);
        }

        public OrderSearchViewModel(OrderSearchForm receivedForm, IList<Customer> customers, IList<Product> products, IList<OrderSearchResultItem> results)
            :this(receivedForm, customers, products)
        {
            Results = results;
        }
    }
}