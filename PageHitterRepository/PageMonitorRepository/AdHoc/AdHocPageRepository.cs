using System;
using System.Collections.Generic;
using System.Linq;
using PageMonitorRepository.Monitor;

namespace PageMonitorRepository.AdHoc
{
	public class AdHocPageRepository : PageMonitorRepository<AdHocPage>, IDisposable
	{
		public List<AdHocPage> GetAllByUser(string userName)
		{

			return DbSet
				.Where(r => r.User == userName)
				.ToList();
		}

		public List<AdHocPage> GetAllPagesToTestByUser(string userName)
		{

			return DbSet
				.Where(r => r.User == userName && r.Test == true)
				.ToList();
		}



		public AdHocPage GetById(int id)
		{
			return DbSet.FirstOrDefault(r => r.Id == id);
		}
	}
}
