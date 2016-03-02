using System;
using System.Collections.Generic;
using System.Linq;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Infrastructure.Query;
using WebCRUD.vNext.Models;
using WebCRUD.vNext.Models.Domain.Orders;

namespace WebCRUD.vNext.Services.Orders
{
    /// <summary>
    /// Gets single SaleProduct by entity id
    /// </summary>
    public class GetProductQuery : Query<Product>
    {
        private Guid productId;

        /// <summary>
        /// Creates the query
        /// </summary>
        /// <param name="productId">id of the product to load</param>
        public GetProductQuery(Guid productId)
        {
            this.productId = productId;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public override Product Execute()
        {
            var product = Context.Product.FirstOrDefault(x => x.Id == productId);
            if (product == null)
                throw new TechnicalException(String.Format("Unable to find Product with id: {0}", productId));

            return product;
        }
    }
}