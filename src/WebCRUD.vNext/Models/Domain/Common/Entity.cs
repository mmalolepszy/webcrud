using System;
using System.ComponentModel.DataAnnotations;

namespace WebCRUD.vNext.Models.Domain.Common
{
    /// <summary>
    /// Base class for classes that we want to save to databse using NHiberate
    /// </summary>
    /// <typeparam name="TKey">type of primary key</typeparam>
    public class Entity<TKey>
        where TKey : IComparable<TKey> , IEquatable<TKey>
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public TKey Id { get; set; }
    }
}