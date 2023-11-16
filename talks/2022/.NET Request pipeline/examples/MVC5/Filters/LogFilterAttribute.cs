using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVC5.Filters
{
    public class LogFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log($"{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}/{filterContext.ActionDescriptor.ActionName}", "OnActionExecuting");
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log($"{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}/{filterContext.ActionDescriptor.ActionName}", "OnActionExecuted");
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log($"{filterContext.Controller.GetType().Name}", "OnResultExecuting");
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log($"{filterContext.Controller.GetType().Name}", "OnResultExecuted");
            base.OnResultExecuted(filterContext);
        }

        private void Log(string action, string state)
        {
            Debug.WriteLine($"LOG > {action} > {state}");
        }
    }
}