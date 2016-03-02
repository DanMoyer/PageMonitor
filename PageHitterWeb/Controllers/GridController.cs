using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PageHitterWeb.Models;
using PageMonitorRepository.Monitor;

namespace PageHitterWeb.Controllers
{
	public class GridController : Controller
	{
		// GET: Grid
		public ActionResult Index(ChartViewModel model)
		{

			var listPageResponseModel = new List<PageResponseModel>();

			using (var pageStatusRepository = new PageStatusRepository())
			{
				//Default to set date for debugging
				var startDate = DateTime.Now.Date.AddDays(-5).ToString(CultureInfo.InvariantCulture);
				var endDate = DateTime.Now.Date.ToString(CultureInfo.InvariantCulture);

				if (model != null)
				{
					startDate = model.StartDate.ToString(CultureInfo.InvariantCulture);
					endDate = model.EndDate.ToString(CultureInfo.InvariantCulture);
				}

				var pageStatuses = pageStatusRepository.GetPageStatusesByDate(startDate, endDate);

				var timeZoneId = "Eastern Standard Time";
				var easternZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);


				foreach (var pageStatus in pageStatuses)
				{
					var utcTime = new DateTime(
						pageStatus.Created.Year,
						pageStatus.Created.Month,
						pageStatus.Created.Day,
						pageStatus.Created.Hour,
						pageStatus.Created.Minute,
						pageStatus.Created.Second);

					var easternTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, easternZone);

					listPageResponseModel.Add(
						new PageResponseModel
						{
							Url = pageStatus.Url,
							ResponseTime = pageStatus.ResponseTime,
							Created = easternTime
						});
				}
			}

			ViewBag.datasource = listPageResponseModel;
			
			return View();
		}
	}
}