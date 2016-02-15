using System;
using System.Linq;

namespace PageMonitorRepository
{
	public class DelayRepository : PageMonitorRepository<Delay>, IDisposable
	{
		public Delay GetDelay()
		{
			return DbSet.FirstOrDefault(r => r.Id == 1);
		}
	}
}
