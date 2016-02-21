using System;
using System.Collections.Generic;
using System.Linq;
using PageMonitorRepository.Monitor;

namespace PageMonitorRepository.AdHoc
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

		public void DeleteAll(string user)
		{
			var rows = DbSet.Where(x => x.User == user).ToList();

			DbSet.RemoveRange(rows);

			//foreach (var row in rows)
			//{
			//	Delete(row);
			//}

			SaveChanges();
		}
	}
}
