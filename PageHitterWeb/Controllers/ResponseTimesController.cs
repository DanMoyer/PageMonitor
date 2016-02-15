﻿using System;
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

			        var easterTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, easternZone);

			        listPageResponseModel.Add(
						new PageResponseModel
						{
							Url          = pageStatus.Url,
							ResponseTime = pageStatus.ResponseTime,
							Created      = easterTime
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
