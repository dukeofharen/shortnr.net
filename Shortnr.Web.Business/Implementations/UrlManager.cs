using Shortnr.Web.Data;
using Shortnr.Web.Entities;
using Shortnr.Web.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shortnr.Web.Business.Implementations
{
	public class UrlManager : IUrlManager
	{
		public Task<ShortUrl> ShortenUrl(string longUrl, string ip, string segment = "")
		{
			return Task.Run(() =>
			{
				using (var ctx = new ShortnrContext())
				{
					ShortUrl url;

					url = ctx.ShortUrls.Where(u => u.LongUrl == longUrl).FirstOrDefault();
					if (url != null)
					{
						return url;
					}

					if (!longUrl.StartsWith("http://") && !longUrl.StartsWith("https://"))
					{
						throw new ArgumentException("Invalid URL format");
					}
					Uri urlCheck = new Uri(longUrl);
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlCheck);
					request.Timeout = 10000;
					try
					{
						HttpWebResponse response = (HttpWebResponse)request.GetResponse();
					}
					catch (Exception)
					{
						throw new ShortnrNotFoundException();
					}

					int cap = 0;
					string capString = ConfigurationManager.AppSettings["MaxNumberShortUrlsPerHour"];
					int.TryParse(capString, out cap);
					DateTime dateToCheck = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
					int count = ctx.ShortUrls.Where(u => u.Ip == ip && u.Added >= dateToCheck).Count();
					if (cap != 0 && count > cap)
					{
						throw new ArgumentException("Your hourly limit has exceeded");
					}

					if (!string.IsNullOrEmpty(segment))
					{
						if (ctx.ShortUrls.Where(u => u.Segment == segment).Any())
						{
							throw new ShortnrConflictException();
						}
						if (segment.Length > 20 || !Regex.IsMatch(segment, @"^[A-Za-z\d_-]+$"))
						{
							throw new ArgumentException("Malformed or too long segment");
						}
					}
					else
					{
						segment = this.NewSegment();
					}

					if (string.IsNullOrEmpty(segment))
					{
						throw new ArgumentException("Segment is empty");
					}

					url = new ShortUrl()
					{
						Added = DateTime.Now,
						Ip = ip,
						LongUrl = longUrl,
						NumOfClicks = 0,
						Segment = segment
					};

					ctx.ShortUrls.Add(url);

					ctx.SaveChanges();

					return url;
				}
			});
		}

		public Task<Stat> Click(string segment, string referer, string ip)
		{
			return Task.Run(() =>
			{
				using (var ctx = new ShortnrContext())
				{
					ShortUrl url = ctx.ShortUrls.Where(u => u.Segment == segment).FirstOrDefault();
					if (url == null)
					{
						throw new ShortnrNotFoundException();
					}

					url.NumOfClicks = url.NumOfClicks + 1;

					Stat stat = new Stat()
					{
						ClickDate = DateTime.Now,
						Ip = ip,
						Referer = referer,
						ShortUrl = url
					};

					ctx.Stats.Add(stat);

					ctx.SaveChanges();

					return stat;
				}
			});
		}

		private string NewSegment()
		{
			using (var ctx = new ShortnrContext())
			{
				int i = 0;
				while (true)
				{
					string segment = Guid.NewGuid().ToString().Substring(0, 6);
					if (!ctx.ShortUrls.Where(u => u.Segment == segment).Any())
					{
						return segment;
					}
					if (i > 30)
					{
						break;
					}
					i++;
				}
				return string.Empty;
			}
		}
	}
}
