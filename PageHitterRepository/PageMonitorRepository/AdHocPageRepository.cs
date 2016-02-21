using System;
using System.Collections.Generic;
using System.Linq;

namespace PageMonitorRepository
{
	public class AdHocPageRepository : PageMonitorRepository<AdHocPage>, IDisposable
	{
		public List<AdHocPage> GetAllByUser(string userName)
		{

			return DbSet
				.Where(r => r.UserId == userName)
				.ToList();

			//using (var context = new PageMonitorDb())
			//{

			//	//var data = context.AdHocPages
			//	//	.Join(context.AspNetUsers, p => p.UserId, users => users.Id,
			//	//	(p, users) => new AdHocPageModel
			//	//	{
			//	//		Url = p.Url,
			//	//		User = users.UserName
			//	//	})
			//	//	.ToList();

			//	//return data;
			//}

		}

		public AdHocPage GetById(int id)
		{
			return DbSet.FirstOrDefault(r => r.Id == id);
		}

	}
}
