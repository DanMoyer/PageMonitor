using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace PageMonitorRepository.Monitor
{
	public class PageStatusRepository :   PageMonitorRepository<PageStatus>, IDisposable
	{

		public List<PageStatus> GetPageStatuses()
		{
			return  DbSet
						.OrderByDescending(r => r.Id)
						.Take(50)
						.ToList();
		}

		public List<PageStatus> GetPageStatusesByDate(string startDate, string endDate)
		{
			var startTime = DateTime.Parse(startDate);
			var endTime = DateTime.Parse(endDate);

			return DbSet
				.OrderByDescending(r => r.Id)
				.Where(r => r.Created >= startTime)
				.Where(r => r.Created <= endTime)
				.ToList();
		}

		public List<double> GetPageStatusByDate(string startDate, string endDate, string url)
		{
			var responseTimes = new List<double>();

			//Context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

			try
			{
				var startTime = DateTime.Parse(startDate);
				var endTime = DateTime.Parse(endDate);

				responseTimes = DbSet
										.Where(r => r.Created >= startTime)
										.Where(r => r.Created <= endTime)
										.Where(r => r.Status == "OK")
										.Where(r => r.Url == url)
										.Select(r => r.ResponseTime)
										.ToList();
			}
			catch (Exception ex)
			{
				var msg = ex.Message;

			}
			
			return responseTimes;
		}
	}
}
