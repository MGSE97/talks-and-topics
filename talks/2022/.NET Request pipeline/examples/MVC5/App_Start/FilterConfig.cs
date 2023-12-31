﻿using MVC5.Filters;
using System.Web;
using System.Web.Mvc;

namespace MVC5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogFilterAttribute());
        }
    }
}
