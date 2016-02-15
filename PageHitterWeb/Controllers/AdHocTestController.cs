using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PageHitter;
using PageHitterWeb.Models;
using PageMonitorRepository;

namespace PageHitterWeb.Controllers
{
	public class AdHocTestController : Controller
	{
		// GET: AdHocTest
		public async Task<ActionResult> Index()
		{
			IEnumerable<PageStatus> listPageStatus = new List<PageStatus>();
			var listPageResponseModels = new List<PageResponseModel>();

			using (var pagesRepository = new PagesRepository())
			{
					var pages = pagesRepository.GetAllProdMonitor();

					//Hit each page from web job.
					foreach (var page in pages)
					{
						listPageStatus = await HitPage(page.Url);

						var pageStatus = listPageStatus.First();

						listPageResponseModels.Add(new PageResponseModel
						{
							Created = pageStatus.Created,
							Url = pageStatus.Url,
							ResponseTime = pageStatus.ResponseTime
							
						});

						//var msg = WebServiceAccess(url + page.Url, interPageSleepTime).Result;
					}
			}

			
			return View(listPageResponseModels);
		}

		private static async Task<IEnumerable<PageStatus>> HitPage(string pageUrl)
		{
			var listPageStatus = new List<PageStatus>();

			try
			{
				using (var repoPageStatus = new PageStatusRepository())
				{
					var pageGetter = new PageGetter();

					var pageStats = new PageStats { Url = pageUrl };
					var stats = await pageGetter.HTTP_GET(pageStats);

					var pageStatus = new PageStatus
					{
						Url              = stats.Url,
						ResponseTime     = stats.ResponseTime,
						ContentLength    = stats.ContentLength,
						ExceptionMessage = stats.ExceptionMessage,
						Status           = stats.Status.ToString(),
						Created          = DateTime.Now
					};

					repoPageStatus.Add(pageStatus);
					repoPageStatus.SaveChanges();

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
