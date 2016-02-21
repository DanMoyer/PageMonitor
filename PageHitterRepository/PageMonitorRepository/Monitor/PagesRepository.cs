using System;
using System.Collections.Generic;
using System.Linq;

namespace PageMonitorRepository.Monitor
{
	public class PagesRepository : PageMonitorRepository<Page>, IDisposable
	{

		public List<Page> GetAllMonitor()
		{
			return DbSet.Where(r => r.Monitor).ToList();
		}

		public Page GetById(int id)
		{
			return DbSet.FirstOrDefault(r => r.Id == id);
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
