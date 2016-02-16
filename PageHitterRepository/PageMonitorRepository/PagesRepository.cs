using System.Collections.Generic;
using System.Linq;

namespace PageMonitorRepository
{
	public class PagesRepository : PageMonitorRepository<Page>
	{

		public List<Page> GetAllMonitor()
		{
			return DbSet.Where(r => r.Monitor).ToList();
		}

		public List<Page> GetAllAdHoc()
		{
			return DbSet.Where(r => r.AdHoc).ToList();
		}


		public List<Page> GetAllProdMonitor()
		{
			var recs = DbSet
						.Where(r => r.Monitor && !r.Url.Contains("stg"))
						.ToList();

			return recs;
		}

		public List<Page> GetAllStgMonitor()
		{
			var recs = DbSet
						.Where(r => r.Monitor && r.Url.Contains("stg"))
						.ToList();

			return recs;
		}
	}
}
