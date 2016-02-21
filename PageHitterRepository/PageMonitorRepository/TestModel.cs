namespace PageMonitorRepository
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class TestModel : DbContext
	{
		public TestModel()
			: base("name=TestModel1")
		{
		}

		public virtual DbSet<AdHocPage> AdHocPages { get; set; }
		public virtual DbSet<AdHocPageStatu> AdHocPageStatus { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
