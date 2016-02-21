using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PageHitterWeb.Models;
using PageMonitorRepository;

namespace PageHitterWeb.Controllers
{
    public class AdHocPageAdminController : Controller
    {
		// GET: PageAdmin
		public ActionResult Index()
		{
			var models = new List<AdHocPageModel>();

			using (var repo = new AdHocPageRepository())
			{
				var shortName = GetShortName();
				var entities = repo.GetAllByUser(shortName);

				models.AddRange(entities.Select(page => new AdHocPageModel
				{
					Id = page.Id,
					Url = page.Url
				}));
			}

			return View(models);
		}

		//GET: PageAdmin/Details/5
		//public ActionResult Details(int id)
		//{
		//	return View();
		//}

		// GET: PageAdmin/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: PageAdmin/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				var model = new AdHocPageModel();
				UpdateModel(model);

				var entity = new AdHocPage
				{
					Url    = model.Url,
					UserId = GetShortName()
				};

				using (var repo = new AdHocPageRepository())
				{
					repo.Add(entity);
					repo.SaveChanges();
				}

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: PageAdmin/Edit/5
		public ActionResult Edit(short id)
		{
			var model = new AdHocPageModel();

			using (var repo = new AdHocPageRepository())
			{
				var entity = repo.GetById(id);

				model.Id   = id;
				model.Url  = entity.Url;
				model.User = GetShortName();
			}

			return View(model);
		}

		// POST: PageAdmin/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				var model = new AdHocPageModel();

				UpdateModel(model);

				using (var repo = new AdHocPageRepository())
				{
					var entity = repo.GetById(model.Id);

					entity.Url    = model.Url;
					entity.UserId = GetShortName();

					repo.SaveChanges();
				}

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: PageAdmin/Delete/5
		public ActionResult Delete(short id)
		{
			var model = new AdHocPageModel();

			using (var repo = new AdHocPageRepository())
			{
				var entity = repo.GetById(id);

				model.Id   = id;
				model.Url  = entity.Url;
				model.User = GetShortName();
			}

			return View(model);
		}

		// POST: PageAdmin/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				using (var repo = new AdHocPageRepository())
				{
					var entity = repo.GetById(id);
					repo.DbSet.Remove(entity);
					repo.SaveChanges();
				}

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

	    private string GetShortName()
	    {
			var userName = User.Identity.GetUserName();
			var index = userName.IndexOf("@", 1, StringComparison.Ordinal);
			var shortName = userName.Substring(0, index);

			return shortName;
	    }
	}
}
