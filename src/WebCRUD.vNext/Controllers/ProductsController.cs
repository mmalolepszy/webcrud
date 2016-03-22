using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCRUD.vNext.Infrastructure.Web;
using WebCRUD.vNext.Models.Domain.Orders;
using WebCRUD.vNext.Services.Orders;

namespace WebCRUD.vNext.Controllers
{
    [Authorize]
    public class ProductsController : WebCRUDController
    {
        /// <summary>
        /// Gets single product data and returns it formatted as JSON
        /// </summary>
        /// <param name="productId">product id</param>
        /// <returns>JSON formatted string</returns>
        [HttpGet]
        public JsonResult GetProductDetails(Guid? productId)
        {
            if (!productId.HasValue)
                return Json(new Product());

            var product = Query(new GetProductQuery(productId.Value));
            return Json(product);
        }
    }
}
