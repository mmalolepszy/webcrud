using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCRUD.vNext.Infrastructure.Web;
using WebCRUD.vNext.Services.Orders;
using WebCRUD.vNext.ViewModels.Orders;

namespace WebCRUD.vNext.Controllers
{
    [Authorize]
    public class OrdersController : WebCRUDController
    {
        /// <summary>
        /// Shows orders search screen
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var vm = new OrderSearchViewModel(customers: Query(new GetAllCustomersQuery()), products: Query(new GetAllProductsQuery()));
            return View(vm);
        }

        /// <summary>
        /// Displays search results
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult Find(OrderSearchViewModel model)
        {
            var results = Query(new FindOrderQuery(model.SearchForm));
            return PartialView("_OrderSearchResults", new OrderSearchViewModel(model.SearchForm, results: results, customers: Query(new GetAllCustomersQuery()), products: Query(new GetAllProductsQuery())));
        }

        /// <summary>
        /// Shows empy create order form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new OrderViewModel(products: Query(new GetAllProductsQuery())));
        }

        /// <summary>
        /// Saves newly created order
        /// </summary>
        /// <param name="orderForm">Form data</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(OrderForm orderForm)
        {
            var vm = new OrderViewModel(orderForm, products: Query(new GetAllProductsQuery()));
            var form = Request.Form;

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var cmdResult = ExecuteCommand(new CreateOrderCommand(orderForm));

            if (cmdResult.Success)
                return RedirectToAction("Edit", new { id = cmdResult.Result });
            else
                return View(vm);
        }

        /// <summary>
        /// Handles editing order data.
        /// </summary>
        /// <param name="orderId">id of order to edit</param>
        /// <returns>edit view</returns>
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var orderForm = Query(new GetOrderQuery(id));
            return View(new OrderViewModel(orderForm, products: Query(new GetAllProductsQuery())));
        }

        /// <summary>
        /// Saves changes from order edit form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(OrderViewModel model)
        {
            var vm = new OrderViewModel(model.OrderForm, products: Query(new GetAllProductsQuery()));

            if (!ModelState.IsValid)
                return View("Edit", vm);

            ExecuteCommand(new UpdateOrderCommand(model.OrderForm));
            return RedirectToAction("Edit", new { id = model.OrderForm.OrderId });
        }

        /// <summary>
        /// Returns new blank row to be inserted in orderItems table
        /// </summary>
        /// <returns></returns>
        public IActionResult AddBlankOrderItem()
        {
            return PartialView("_OrderItem", new OrderItemForm(products: Query(new GetAllProductsQuery())));
        }

        /// <summary>
        /// Approves order.
        /// </summary>
        /// <param name="orderId">id of the order to approve</param>
        /// <returns>search result row with new data</returns>
        [HttpPost]
        [Authorize("CanChangeStatus")]
        public ActionResult Approve(Guid orderId)
        {
            var approvalResult = ExecuteCommand(new ApproveOrderCommand(orderId));
            return PartialView("_OrderSearchResultsItem", OrderToModelMapper.MapToSearchResultsItem(approvalResult.Result));
        }

        /// <summary>
        /// Closes order.
        /// </summary>
        /// <param name="orderId">id of the order to close</param>
        /// <returns>>search result row with new data</returns>
        [HttpPost]
        [Authorize("CanChangeStatus")]
        public ActionResult Close(Guid orderId)
        {
            var commandResult = ExecuteCommand(new CloseOrderCommand(orderId));
            return PartialView("_OrderSearchResultsItem", OrderToModelMapper.MapToSearchResultsItem(commandResult.Result));
        }
    }
}
