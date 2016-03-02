using System.Collections.Generic;
using System.Linq;
using WebCRUD.vNext.Infrastructure.Query;
using WebCRUD.vNext.Models;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Query that return all customers
    /// </summary>
    public class GetAllCustomersQuery : Query<IList<Customer>>
    {
        public override IList<Customer> Execute()
        {
            return Context.Customer.ToList();
        }
    }
}