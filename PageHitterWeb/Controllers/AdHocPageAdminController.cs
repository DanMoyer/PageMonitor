using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PageHitter;
using PageHitterWeb.Models;
using PageMonitorRepository.AdHoc;

namespace PageHitterWeb.Controllers
{
	public class AdHocPageAdminController : Controller
	{
		// GET: PageAdmin
		public ActionResult Index()
		{
			var models = new List<AdHocPageModel>();

			var user = User;
			var userName = user.Identity.GetUserName();

			using (var repo = new AdHocPageRepository())
			{
				var shortName = UserIdentity.GetShortName(User);
				var entities = repo.GetAllByUser(shortName);

				models.AddRange(entities.Select(page => new AdHocPageModel
				{
					Id   = page.Id,
					Url  = page.Url,
					Test = page.Test
				}));
			}

			return View(models);
		}


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
					User   = UserIdentity.GetShortName(User),
					Test   = true
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
				model.Test = entity.Test;
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
					entity.User   = UserIdentity.GetShortName(User);
					entity.Test   = model.Test;

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
				model.Test = entity.Test;
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
	}
}
