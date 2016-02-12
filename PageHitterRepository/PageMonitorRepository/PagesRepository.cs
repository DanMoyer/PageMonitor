using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageMonitorRepository
{
	public class PagesRepository : PageMonitorRepository<Page>
	{

		public List<Page> GetAllMonitor()
		{
			return DbSet.Where(r => r.Monitor == true).ToList();
		}

		public List<Page> GetAllProdMonitor()
		{
			var recs = DbSet
						.Where(r => r.Monitor == true && !r.Url.Contains("stg"))
						.ToList();




			return recs;
		}

		public List<Page> GetAllStgMonitor()
		{
			var recs = DbSet
						.Where(r => r.Monitor == true && r.Url.Contains("stg"))
						.ToList();

			return recs;
		}
	}
}
