using System;
using System.Threading;
using PageHitter;
using PageMonitor.Repository;

namespace PageMonitor
{
	class Program
	{
		static void Main(string[] args)
		{

			var repoPages = new PagesRepository();
			//var pages = repoPages.GetAllProdMonitor();
			var pages = repoPages.GetAllStgMonitor();

			var repoPageStatus = new PageStatusRepository();

			var pageGetter = new PageGetter();

			var counter = 1000;

			while (counter > 0)
			{
				foreach (var page in pages)
				{
					Console.WriteLine($"Page: {page.Url}");

					var pageStats = new PageStats { Url = page.Url };
					var result = pageGetter.HTTP_GET(pageStats);
					var stats = result.Result;

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

					Console.WriteLine($"seconds: {stats.ResponseTime}  length: {stats.ContentLength} time: {DateTime.Now}"  );
					Console.WriteLine();
				}

				Thread.Sleep(10000);

				counter --;
			}
		}
	}
}
