using Shortnr.Web.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shortnr.Web.Filters
{
	public class ShortnrErrorFilter : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			HttpStatusCode code = HttpStatusCode.InternalServerError;
			var ex = filterContext.Exception;
			string viewName = "Error500";

			if (ex is ShortnrNotFoundException)
			{
				code = HttpStatusCode.NotFound;
				viewName = "Error404";
			}
			if (ex is ShortnrConflictException)
			{
				code = HttpStatusCode.Conflict;
				viewName = "Error409";
			}
			if (ex is ArgumentException)
			{
				code = HttpStatusCode.BadRequest;
				viewName = "Error400";
			}

			filterContext.Result = new ViewResult()
			{
				ViewName = viewName
			};

			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = (int)code;
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;  
		}
	}
}