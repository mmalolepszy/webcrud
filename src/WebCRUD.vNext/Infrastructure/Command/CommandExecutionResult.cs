using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCRUD.vNext.Infrastructure.Command
{
    /// <summary>
    /// Wraps result from command with information if executed succesfully or not
    /// if not finised sucesfully provides humen friendly message and error code for machines
    /// </summary>
    /// <typeparam name="T">wrapped command result type</typeparam>
    public class CommandExecutionResult<T>
    {
        /// <summary>
        /// True if execution succeeded
        /// </summary>
        public bool Success { get; protected set; }
        /// <summary>
        /// Error code
        /// </summary>
        public string ErrorCode { get; protected set; }
        /// <summary>
        /// Human friendly message
        /// </summary>
        public string MessageForHumans { get; protected set; }
        /// <summary>
        /// Result from wrapped command
        /// </summary>
        public T Result { get; protected set; }

        /// <summary>
        /// Creates success result
        /// </summary>
        public static CommandExecutionResult<T> SuccessResult(T result)
        {
            return new CommandExecutionResult<T> { Success = true, Result = result };
        }

        /// <summary>
        /// Creates failure result
        /// </summary>
        /// <param name="errCode">error code</param>
        /// <param name="msg">human friendly message</param>
        public static CommandExecutionResult<T> FailureResult(string errCode, string msg)
        {
            return new CommandExecutionResult<T> { Success = false, ErrorCode = errCode, MessageForHumans = msg };
        }
    }
}
