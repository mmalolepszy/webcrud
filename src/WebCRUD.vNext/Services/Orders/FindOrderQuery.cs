using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebCRUD.vNext.Infrastructure.Query;
using WebCRUD.vNext.Models.Domain.Orders;
using WebCRUD.vNext.ViewModels.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Query for finding orders based on values selected in search form
    /// </summary>
    public class FindOrderQuery : Query<IList<OrderSearchResultItem>>
    {
        private OrderSearchForm criteria;

        public FindOrderQuery(OrderSearchForm criteria)
        {
            this.criteria = criteria;
        }

        /// <summary>
        /// Execute a query against provided NHibernate session
        /// </summary>
        /// <param name="session">NHibernate session</param>
        /// <returns></returns>
        public override IList<OrderSearchResultItem> Execute()
        {
            IEnumerable<Order> orders = Context.Order.Include(o => o.Customer).Include(o => o.OrderedItems);

            if(!string.IsNullOrWhiteSpace(criteria.OrderNumber))
                orders = orders.Where(order => order.Number.Contains(criteria.OrderNumber));

            if (criteria.OrderDateFrom.HasValue)
                orders = orders.Where(order => order.OrderDate >= criteria.OrderDateFrom.Value);

            if (criteria.OrderDateTo.HasValue)
                orders = orders.Where(order => order.OrderDate <= criteria.OrderDateTo.Value);

            if (criteria.CustomerId.HasValue)
                orders = orders.Where(order => order.Customer.Id == criteria.CustomerId);

            if (criteria.Status.HasValue)
                orders = orders.Where(order => order.Status == criteria.Status.Value);

            if (criteria.ProductId.HasValue)
                orders = orders.Where(order => order.OrderedItems.Any(item => item.ProductId == criteria.ProductId.Value));

            var results = orders.ToList();

            if (criteria.TotalAmountFrom.HasValue)
                results = results.Where(order => order.TotalAmount >= criteria.TotalAmountFrom.Value).ToList();

            if (criteria.TotalAmountTo.HasValue)
                results = results.Where(order => order.TotalAmount <= criteria.TotalAmountTo.Value).ToList();

            return results.Select(OrderToModelMapper.MapToSearchResultsItem).ToList();
        }
    }
}