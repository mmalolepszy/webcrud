using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCRUD.vNext.Infrastructure.Web;
using WebCRUD.vNext.Models.Domain.Orders;
using WebCRUD.vNext.Services.Orders;

namespace WebCRUD.vNext.Controllers
{
    [Authorize]
    public class CustomersController : WebCRUDController
    {
        /// <summary>
        /// Gets customers matching search string
        /// </summary>
        /// <param name="productId">search string</param>
        /// <returns>JSON formatted string containing customer names and ids</returns>
        [HttpGet]
        public JsonResult GetCustomers(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new Customer());

            var customers = Query(new GetCustomersByNameQuery(query)).Select(c => new { Value = c.Name, id = c.Id }).ToList();
            return Json(customers);
        }
    }
}
