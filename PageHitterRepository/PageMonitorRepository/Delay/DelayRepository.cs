using System;
using System.Linq;
using PageMonitorRepository.Monitor;

namespace PageMonitorRepository.Delay
{
	public class DelayRepository : PageMonitorRepository<Delay>, IDisposable
	{
		public PageMonitorRepository.Delay.Delay GetDelay()
		{
			return DbSet.First(r => true);
		}
	}
}
