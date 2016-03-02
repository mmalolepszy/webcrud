using System;
using System.Collections.Generic;
using WebCRUD.vNext.Models.Domain.Common;

namespace WebCRUD.vNext.Models.Domain.Orders
{
    public class Customer : Entity<Guid>
    {
        public string Name { get; protected set; }
        public string TaxId { get; protected set; }

        public string Street { get; protected set; }
        public string City { get; protected set; }
        public string ZipCode { get; protected set; }
        public string Country { get; protected set; }

        public virtual ICollection<Order> Orders { get; protected set; }

        public Customer() { }

        public Customer(string name, string taxId, string street, string city, string zipCode, string country)
        {
            Name = name;
            TaxId = taxId;

            Street = street;
            City = city;
            ZipCode = zipCode;
            Country = country;
        }

        public Order PlaceOrder(string orderNumber, DateTime date)
        {
            var order = new Order(orderNumber, date);

            if (Orders == null)
                Orders = new List<Order>();

            Orders.Add(order);

            return order;
        }
    }
}
