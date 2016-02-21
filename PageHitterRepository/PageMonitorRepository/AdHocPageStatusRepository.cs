using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageMonitorRepository
{
	public class AdHocPageStatusRepository : PageMonitorRepository<AdHocPageStatu>, IDisposable
	{
		public List<AdHocPageStatu> GetPageStatuses()
		{
			return DbSet.
						OrderByDescending(r => r.Id)
						.Take(50)
						.ToList();
		}
	}
}
