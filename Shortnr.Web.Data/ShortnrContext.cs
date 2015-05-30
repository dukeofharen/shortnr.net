using MySql.Data.Entity;
using Shortnr.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortnr.Web.Data
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class ShortnrContext : DbContext
	{
		public ShortnrContext()
			: base("name=Shortnr")
		{

		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Stat>()
				.HasRequired(s => s.ShortUrl)
				.WithMany(u => u.Stats)
				.Map(m => m.MapKey("shortUrl_id"));
		}

		public virtual DbSet<ShortUrl> ShortUrls { get; set; }
		public virtual DbSet<Stat> Stats { get; set; }
	}
}
