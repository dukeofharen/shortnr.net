using MySql.Data.EntityFramework;
using Shortnr.Web.Entities;
using System.Data.Entity;

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
