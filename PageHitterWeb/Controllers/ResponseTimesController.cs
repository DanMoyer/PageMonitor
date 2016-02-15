using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PageHitterWeb.Models;
using PageMonitorRepository;

namespace PageHitterWeb.Controllers
{
    public class ResponseTimesController : Controller
    {


        // GET: ResponseTimes/Create
        public ActionResult ShowPageTimes()
        {
	        var listPageResponseModel = new List<PageResponseModel>();

	        using (var pageStatusRepository = new PageStatusRepository())
	        {
		        var pageStatuses = pageStatusRepository.GetPageStatuses();

		        foreach (var pageStatus in pageStatuses)
		        {
			        listPageResponseModel.Add(
						new PageResponseModel
						{
							Url          = pageStatus.Url,
							ResponseTime = pageStatus.ResponseTime,
							Created      = pageStatus.Created
						});
		        }
	        }

            return View(listPageResponseModel);
        }

        // POST: ResponseTimes/Create
        //[HttpPost]
        //public ActionResult ShowPageTimes(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


    }
}
