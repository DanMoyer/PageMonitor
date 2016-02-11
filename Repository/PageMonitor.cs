namespace PageMonitor.Repository
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class PageMonitor : DbContext
	{
		public PageMonitor()
			: base("name=PageMonitor")
		{
		}

		public virtual DbSet<Page> Pages { get; set; }
		public virtual DbSet<PageStatus> PagesStatus { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
