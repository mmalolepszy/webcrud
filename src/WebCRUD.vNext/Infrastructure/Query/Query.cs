using System;
using Microsoft.Extensions.DependencyInjection;
using WebCRUD.vNext.Models;

namespace WebCRUD.vNext.Infrastructure.Query
{
    /// <summary>
    /// Base class for Queries
    /// </summary>
    /// <typeparam name="TResult">type of returned result</typeparam>
    public abstract class Query<TResult>
    {
        public ApplicationDbContext Context { get; set; }
        /// <summary>
        /// Implement this method to construct and execute a query against provided EF context
        /// </summary>
        /// <param name="context">EF context</param>
        public abstract TResult Execute();

        public virtual void SetupDependencies(IServiceProvider provider)
        {
            Context = provider.GetService<ApplicationDbContext>();
        }
    }
}