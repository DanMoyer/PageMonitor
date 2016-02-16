using System;
using System.Collections.Generic;
using System.Linq;

namespace PageMonitorRepository
{
	public class PageStatusRepository :   PageMonitorRepository<PageStatus>, IDisposable
	{

		public List<PageStatus> GetPageStatuses()
		{
			return  DbSet.
						OrderByDescending(r => r.Id)
						.Take(50)
						.ToList();
		}
	}
}
