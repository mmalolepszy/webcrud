using System;
using WebCRUD.vNext.Models.Domain.Common;

namespace WebCRUD.vNext.Models.Domain.Orders
{
    /// <summary>
    /// represents product
    /// </summary>
    public class Product : Entity<Guid>
    {
        /// <summary>
        /// product code
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// product name
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// product price
        /// </summary>
        public virtual decimal Price { get; protected set; }

        public Product() { }

        public Product(string name, string code, decimal price)
        {
            Name = name;
            Code = code;
            Price = price;
        }
    }
}