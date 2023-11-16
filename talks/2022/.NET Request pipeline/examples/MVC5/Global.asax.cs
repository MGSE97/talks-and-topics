using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace MVC5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            BeginRequest += MvcApplication_BeginRequest;
            EndRequest += MvcApplication_EndRequest;
            
            OnExecuteRequestStep((ctx, next) =>
            {
                // Log steps to trace
                if (!ctx.IsPostNotification)
                    ctx.Trace.Write(ctx.CurrentNotification.ToString());
                next();
            });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void MvcApplication_BeginRequest(object sender, EventArgs e)
        {
            // Enable trace and print it after request ends
            Context.Trace.IsEnabled = true;
            Context.Trace.TraceFinished += Trace_TraceFinished;
            Context.Trace.Write("BeginRequest");

            Log("Begin Requests", $"<-- Requests starts HERE ({Request?.Url?.PathAndQuery})");
        }

        private void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            Log("End Request", $"<-- Requests ends HERE");
        }

        private void Trace_TraceFinished(object sender, TraceContextEventArgs e)
        {
            // Get unique steps names
            var trace = e.TraceRecords.Cast<TraceContextRecord>().Select(r => r.Message).Distinct();

            Log($"Trace ({trace.Count()}/{e.TraceRecords.Count} items)", $"\n\t{string.Join("\n\t", trace)}");
        }

        protected void Log(string part, string msg = null)
        {
            Debug.WriteLine($"LOG > {part}{(!string.IsNullOrEmpty(msg) ? $": {msg}" : "")}");
        }
    }
}
