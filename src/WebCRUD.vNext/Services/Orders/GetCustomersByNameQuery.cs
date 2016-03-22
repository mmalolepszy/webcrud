using System.Collections.Generic;
using System.Linq;
using WebCRUD.vNext.Infrastructure.Query;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Gets customers list by part of their name
    /// </summary>
    public class GetCustomersByNameQuery : Query<IList<Customer>>
    {
        private string query;

        /// <summary>
        /// constructs query
        /// </summary>
        /// <param name="query">Part of of customers name</param>
        public GetCustomersByNameQuery(string query)
        {
            this.query = query;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public override IList<Customer> Execute()
        {
            return Context.Customer.Where(c => c.Name.Contains(query)).ToList();
        }
    }
}