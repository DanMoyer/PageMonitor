using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageMonitorRepository
{
	public class DelayRepository : PageMonitorRepository<Delay>
	{
		public Delay GetDelay()
		{
			return DbSet.FirstOrDefault(r => r.Id == 1);
		}
	}
}
