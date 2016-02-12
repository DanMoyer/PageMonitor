namespace PageMonitorRepository
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class PageMonitorDb : DbContext
	{
		public PageMonitorDb()
			: base("name=PageMonitor")
		{
		}

		public virtual DbSet<Page> Pages { get; set; }
		public virtual DbSet<PageStatus> PageStatus { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
