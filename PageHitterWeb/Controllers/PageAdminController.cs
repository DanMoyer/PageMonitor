using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PageHitterWeb.Models;
using PageMonitorRepository.Monitor;

namespace PageHitterWeb.Controllers
{
	public class PageAdminController : Controller
	{
		// GET: PageAdmin
		public ActionResult Index()
		{
			var models = new List<PageModel>();

			using (var repo = new PagesRepository())
			{
				var entities = repo.GetAll();

				models.AddRange(entities.Select(page => new PageModel
				{
					Id      = page.Id,
					Url     = page.Url,
					Monitor = page.Monitor
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
				var model = new PageModel();
				UpdateModel(model);
				
				var entity = new Page {Monitor = model.Monitor, Url = model.Url};

				using (var repo = new PagesRepository())
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
			var model = new PageModel();

			using (var repo = new PagesRepository())
			{
				var entity = repo.GetById(id);

				model.Id      = id;
				model.Url     = entity.Url;
				model.Monitor = entity.Monitor;
			}
			
			return View(model);
		}

		// POST: PageAdmin/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				var model = new PageModel();

				UpdateModel(model);

				using (var repo = new PagesRepository())
				{
					var entity = repo.GetById(model.Id);

					entity.Url     = model.Url;
					entity.Monitor = model.Monitor;

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
			var model = new PageModel();

			using (var repo = new PagesRepository())
			{
				var entity = repo.GetById(id);

				model.Id      = id;
				model.Url     = entity.Url;
				model.Monitor = entity.Monitor;

			}

			return View(model);
		}

		// POST: PageAdmin/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				using (var repo = new PagesRepository())
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
