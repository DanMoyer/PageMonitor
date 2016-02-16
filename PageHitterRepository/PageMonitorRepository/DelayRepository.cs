using System.Linq;

namespace PageMonitorRepository
{
	public class DelayRepository : PageMonitorRepository<Delay>
	{
		public Delay GetDelay()
		{
			return DbSet.First(r => true);
		}
	}
}
