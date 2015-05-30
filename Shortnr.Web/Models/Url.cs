using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shortnr.Web.Models
{
	public class Url
	{
		[Required]
		[JsonProperty("longUrl")]
		public string LongURL { get; set; }

		[JsonProperty("shortUrl")]
		public string ShortURL { get; set; }

		[JsonIgnore]
		public string CustomSegment { get; set; }
	}
}