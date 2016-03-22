using System.Collections.Generic;
using System.Linq;
using WebCRUD.vNext.Infrastructure.Query;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Query that returns all sale products
    /// </summary>
    public class GetAllProductsQuery : Query<IList<Product>>
    {
        public override IList<Product> Execute()
        {
            return Context.Product.ToList();
        }
    }
}