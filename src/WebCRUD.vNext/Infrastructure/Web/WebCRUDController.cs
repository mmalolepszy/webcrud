using Microsoft.AspNetCore.Mvc;
using WebCRUD.vNext.Infrastructure.Command;
using WebCRUD.vNext.Infrastructure.Exception;
using WebCRUD.vNext.Infrastructure.Query;

namespace WebCRUD.vNext.Infrastructure.Web
{
    public abstract class WebCRUDController : Controller
    {
        protected TResult Query<TResult>(Query<TResult> queryToRun)
        {
            queryToRun.SetupDependencies(HttpContext.RequestServices);
            return queryToRun.Execute();
        }

        protected CommandExecutionResult<TResult> ExecuteCommand<TResult>(Command<TResult> commandToRun)
        {
            try
            {
                commandToRun.SetupDependencies(HttpContext.RequestServices);
                var result = commandToRun.Execute();

                return CommandExecutionResult<TResult>.SuccessResult(result);
            }
            catch (BusinessException businessEx)
            {
                var result = CommandExecutionResult<TResult>.FailureResult(businessEx.ErrorCode, businessEx.Message);
                AddErrors(result);
                return result;
            }
            catch (System.Exception exception)
            {
                var result = CommandExecutionResult<TResult>.FailureResult("UnexpectedError", string.Format("Unexpected error: {0}", exception.Message));
                AddErrors(result);
                return result;
            }

        }

        protected void AddErrors<T>(CommandExecutionResult<T> result)
        {
            ModelState.AddModelError(string.Empty, result.MessageForHumans);
        }
    }
}
