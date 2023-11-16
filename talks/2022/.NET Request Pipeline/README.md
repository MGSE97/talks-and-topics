[< Back to talks](../README.md)

---

# ASP.NET Request pipeline comparison

ASP.NET MVC 5 vs ASP.NET Core request pipeline processing comparison.
Understanding of request processing is beneficial for more advanced flows, that you can often find in old projects.

## First run

If you encounter issue with `Roslyn` missing in `MVC5` project, run the command bellow in `Package Management Console`.

```sh
PM> Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
```

If you encounter issue with `Dev certificate` missing or outdated in `Core` project, run following command in console. Also, make sure you have `.NET 6.0 sdk` installed ([downloads](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks?cid=getdotnetsdk), [sdk](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.320-windows-x64-installer)).

```sh
CMD> dotnet dev-certs https
```

## Running code

1. Open `examples/RequestHandling.sln` in Visual Studio 2019+
2. Run `MVC5` project
   1. Web page will be opened
   2. You should see Logs in console, that correspond to request steps thought pipeline.
   3. Play around with example.
3. Run `Core` project
   1. Web page will be opened
   2. You should see Logs in console, that correspond to request steps thought pipeline.
   3. Play around with example.

## Results

Steps will be logged into console.

### MVC5

ASP.NET MVC 5 is .NET Framework web app, and you should see following logs.

```js
LOG > Begin Requests: <-- Requests starts HERE (/)
LOG > Home/Index > OnActionExecuting
LOG > Home/Index > OnActionExecuted
LOG > HomeController > OnResultExecuting
LOG > HomeController > OnResultExecuted
LOG > End Request: <-- Requests ends HERE
LOG > Trace (11/23 items):
    BeginRequest
    AuthenticateRequest
    AuthorizeRequest
    ResolveRequestCache
    MapRequestHandler
    AcquireRequestState
    PreExecuteRequestHandler
    ExecuteRequestHandler
    ReleaseRequestState
    UpdateRequestCache
    EndRequest
```

The `OnAction...` and `OnResult...` logs are from `LogFilterAttribute`, and they are globally registered for each controller action. Attributes are usually used to do some login, authentication or other action modifications.

Rest that you can see, are `Global.aspx.cs` hooks. We are using `OnExecuteRequestStep`, to log these items without need to tap to each event. There are a lot of them, and they provide request handling outside of controllers (e.g. logs, metrics, compression, ...).

For more details you can refer to documentation ([docs](https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/lifecycle-of-an-aspnet-mvc-5-application), [pipeline](https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/lifecycle-of-an-aspnet-mvc-5-application/_static/lifecycle-of-an-aspnet-mvc-5-application1.pdf)).

### Core

ASP.NET Core is .NET 6.0 web app, and you should see following logs.

```js
LOG > Begin Requests: <-- Requests starts HERE (/)
LOG > Core.Controllers.HomeController.Index (Core) > OnActionExecuting
LOG > Core.Controllers.HomeController.Index (Core) > OnActionExecuted
LOG > Core.Controllers.HomeController.Index (Core) > OnResultExecuting
LOG > Core.Controllers.HomeController.Index (Core) > OnResultExecuted
LOG > End Request: <-- Requests ends HERE
...
LOG > Begin Requests: <-- Requests starts HERE (/css/site.css)
LOG > Begin Requests: <-- Requests starts HERE (/lib/bootstrap/dist/css/bootstrap.min.css)
LOG > Begin Requests: <-- Requests starts HERE (/Core.styles.css)
LOG > Begin Requests: <-- Requests starts HERE (/lib/jquery/dist/jquery.min.js)
LOG > Begin Requests: <-- Requests starts HERE (/lib/bootstrap/dist/js/bootstrap.bundle.min.js)
LOG > Begin Requests: <-- Requests starts HERE (/js/site.js)
LOG > End Request: <-- Requests ends HERE
LOG > End Request: <-- Requests ends HERE
LOG > End Request: <-- Requests ends HERE
LOG > End Request: <-- Requests ends HERE
LOG > End Request: <-- Requests ends HERE
LOG > End Request: <-- Requests ends HERE
```

The `OnAction...` and `OnResult...` logs are from `LogFilterAttribute`, same as before.

The `Begin Request` and `End Request` are from `log` function in `Program.cs`. We are using middleware to log these. Unlike before, we can't get logs of each step, without wrapping each of the middlewares used. Instead, we can look into all the `app.Use...` and `app.Map...` to see what will happen to request. Also, we can now fully control, what steps the request will go thought. This is the main difference between `.NET Core` and `.NET Framework` web apps.

We can also see other static files being logged. These files will be served by `app.UseStaticFiles` middleware, and they will not get into controllers. Order of these middlewares is important!

## Authors

- [**MGSE**](https://github.com/MGSE97)

---

<h6 align="center">

• &nbsp; [T&T](../../../README.md) &nbsp;
•>&nbsp; Talks &nbsp;<•
&nbsp; [Topics](../../../topics/README.md) &nbsp;
•

</h6>

<h6 align="center">

•>&nbsp; 2022 &nbsp;<•
&nbsp; [2023](../../2023/README.md) &nbsp;
•

</h6>

<h6 align="center">

[< Back to talks](../README.md)
&nbsp;&nbsp; • &nbsp;&nbsp;
<b><a href="https://github.com/MGSE97" target="_blank">MGSE</a> ☕ 2016 ... 2023</b>

</h6>
