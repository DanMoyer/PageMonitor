using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using PageHitter;
using PageHitterWeb.Models;
using PageMonitorRepository.AdHoc;

namespace PageHitterWeb.Controllers
{
	public class AdHocTestController : Controller
	{
		// GET: AdHocTest
		public async Task<ActionResult> Index()
		{
			var listPageResponseModels = new List<PageResponseModel>();

			var timeZoneId  = "Eastern Standard Time";
			var easternZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

			List<AdHocPage> pages;
			List<AdHocPageStatu> listPageStatus;

			var user = UserIdentity.GetShortName(User);

			using (var adHocPageRepository = new AdHocPageRepository())
			{
				pages = adHocPageRepository.GetAllPagesToTestByUser(user);
			}

			//Hit each page from web job.
			foreach (var page in pages)
			{
				var aPageStatus = await HitPage(page.Url, user);
			}

			using (var adHocPageStatusRepository = new AdHocPageStatusRepository())
			{
				listPageStatus = adHocPageStatusRepository.GetPageStatuses();
			}

			foreach (var pageStatus in listPageStatus)
			{
				var utcTime = new DateTime(
											pageStatus.Created.Year,
											pageStatus.Created.Month,
											pageStatus.Created.Day,
											pageStatus.Created.Hour,
											pageStatus.Created.Minute,
											pageStatus.Created.Second);

				var easternTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, easternZone);

				listPageResponseModels.Add(
					new PageResponseModel
					{
						Created      = easternTime,
						Url          = pageStatus.Url,
						ResponseTime = pageStatus.ResponseTime,
						Status       = pageStatus.Status
					});


			}

			return View(listPageResponseModels);
		}

		public ActionResult Delete()
		{
			var user = UserIdentity.GetShortName(User);

			using (var adHocPageStatusRepository = new AdHocPageStatusRepository())
			{
				adHocPageStatusRepository.DeleteAll(user);
			}

			return RedirectToAction("Index");
		}

		private static async Task<IEnumerable<AdHocPageStatu>> HitPage(string pageUrl, string user)
		{
			var listPageStatus = new List<AdHocPageStatu>();

			try
			{
				using (var adHocPageStatusRepository = new AdHocPageStatusRepository())
				{
					var pageGetter = new PageGetter();

					var pageStats = new PageStats { Url = pageUrl };
					var stats = await pageGetter.HTTP_GET(pageStats);

					var pageStatus = new AdHocPageStatu
					{
						Url              = stats.Url,
						ResponseTime     = stats.ResponseTime,
						ContentLength    = stats.ContentLength,
						ExceptionMessage = stats.ExceptionMessage,
						Status           = stats.Status.ToString(),
						Created          = DateTime.Now,
						User             = user
					};

					adHocPageStatusRepository.Add(pageStatus);
					adHocPageStatusRepository.SaveChanges();

					listPageStatus.Add(pageStatus);
				}
			}
			catch (Exception ex)
			{
				// ReSharper disable once UnusedVariable
				var msg = ex.Message;
			}

			return listPageStatus;
		}
	}
}
