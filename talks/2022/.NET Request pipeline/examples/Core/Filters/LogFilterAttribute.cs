using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Core.Filters
{
    public class LogFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log($"{filterContext.ActionDescriptor.DisplayName}", "OnActionExecuting");
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log($"{filterContext.ActionDescriptor.DisplayName}", "OnActionExecuted");
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log($"{filterContext.ActionDescriptor.DisplayName}", "OnResultExecuting");
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log($"{filterContext.ActionDescriptor.DisplayName}", "OnResultExecuted");
            base.OnResultExecuted(filterContext);
        }

        private void Log(string action, string state)
        {
            Debug.WriteLine($"LOG > {action} > {state}");
        }
    }
}