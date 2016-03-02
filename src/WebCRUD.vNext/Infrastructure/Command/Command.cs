using System;
using Microsoft.Extensions.DependencyInjection;
using WebCRUD.vNext.Models;

namespace WebCRUD.vNext.Infrastructure.Command
{
    /// <summary>
    /// Base command class
    /// </summary>
    /// <typeparam name="T">type of returned result</typeparam>
    public abstract class Command<T>
    {
        public ApplicationDbContext Context { get; set; }

        /// <summary>
        /// Executes a command and returns result
        /// </summary>
        /// <returns>command result</returns>
        public abstract T Execute();

        /// <summary>
        /// Tells if command can be executed
        /// </summary>
        /// <returns>base implementation returns true</returns>
        public virtual bool CanExecute()
        {
            return true;
        }

        public virtual void SetupDependencies(IServiceProvider provider)
        {
            Context = provider.GetService<ApplicationDbContext>();
        }
    }
}