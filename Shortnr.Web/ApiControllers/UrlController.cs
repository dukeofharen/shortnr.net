using Shortnr.Web.Business;
using Shortnr.Web.Entities;
using Shortnr.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Shortnr.Web.ApiControllers
{
	[RoutePrefix("api/url")]
    public class UrlController : ApiController
    {
		private IUrlManager _urlManager;

		public UrlController(IUrlManager urlManager)
		{
			this._urlManager = urlManager;
		}

		[Route("shorten")]
		[HttpGet]
		public async Task<Url> Shorten([FromUri]string url, [FromUri]string segment = "")
		{
			ShortUrl shortUrl = await this._urlManager.ShortenUrl(HttpUtility.UrlDecode(url), HttpContext.Current.Request.UserHostAddress, segment);
			Url urlModel = new Url()
			{
				LongURL = url,
				ShortURL = string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, shortUrl.Segment)
			};
			return urlModel;
		}
    }
}