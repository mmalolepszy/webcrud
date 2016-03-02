using System;
using System.Collections.Generic;
using System.Linq;
using WebCRUD.vNext.Infrastructure.Query;
using WebCRUD.vNext.Models;

namespace WebCRUD.vNext.Infrastructure.Service
{
    /// <summary>
    /// Base class for business services.
    /// Business services are classes that contain:
    /// business algorithm implentation
    /// validation rules
    /// state management and workflow for business entities.
    /// 
    /// Base class provides ability to access common services like queries execution.
    /// Business servies should not execute commands - it should be the other way around - threfeore there are no methods 
    /// to execute commands.
    /// 
    /// To implement a business service derive your class from business service. Business service installer by default will register
    /// it in container.
    /// </summary>
    public abstract class BusinessService
    {
        protected IServiceProvider Provider { get; private set; }

        /// <summary>
        /// Construct new instance, expects EF context to be injcected
        /// </summary>
        public BusinessService(IServiceProvider provider)
        {
            this.Provider = provider;
        }

        /// <summary>
        /// Executes query
        /// </summary>
        protected virtual TResult Query<TResult>(Query<TResult> queryToExecute)
        {
            queryToExecute.SetupDependencies(Provider);
            return queryToExecute.Execute();
        }
    }
}