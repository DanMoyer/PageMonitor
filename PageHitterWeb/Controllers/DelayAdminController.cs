using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PageHitterWeb.Models;
using PageMonitorRepository;

namespace PageHitterWeb.Controllers
{
    public class DelayAdminController : Controller
    {
        //// GET: DelayAdmin
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: DelayAdmin/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: DelayAdmin/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: DelayAdmin/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
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

        // GET: DelayAdmin/Edit/5
        public ActionResult Edit()
        {
			using (var delayRepo = new DelayRepository())
	        {
		        var entity = delayRepo.GetDelay();

				var model = new DelayModel
				{
					IterationHour   = entity.IterationHour,
					IterationMinute = entity.IterationMinute, 
					IterationSecond = entity.IterationSecond,
					PageHour        = entity.PageHour,
					PageMinute      = entity.PageMinute,
					PageSecond      =entity.PageSecond
				};

				return View(model);

			}
        }

        // POST: DelayAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
				var model = new DelayModel();
				UpdateModel(model);

				using (var delayRepo = new DelayRepository())
				{
					var entity = delayRepo.GetDelay();

					entity.IterationHour   = model.IterationHour;
					entity.IterationMinute = model.IterationMinute;
					entity.IterationSecond = model.IterationSecond;
					entity.PageHour        = model.PageHour;
					entity.PageMinute      = model.PageMinute;
					entity.PageSecond      = model.PageSecond;

					delayRepo.SaveChanges();
				}


				return RedirectToAction("Edit");
            }
            catch
            {
                return View();
            }
        }

        //// GET: DelayAdmin/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: DelayAdmin/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
