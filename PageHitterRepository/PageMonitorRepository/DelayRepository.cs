using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
