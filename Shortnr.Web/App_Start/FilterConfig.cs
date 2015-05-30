using Shortnr.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace Shortnr.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new ShortnrErrorFilter());
		}
	}
}
