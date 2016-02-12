using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PageHitter;
using PageMonitorRepository;

namespace PageMonitorConsole
{
	class Program
	{
		static void Main()
		{
			//DirectAccess();

			const string url = "http://pagehitterweb.azurewebsites.net/api/Values";
			//const string url = "http://localhost:9476/api/Values/?json=true";

			var msg = WebServiceAccess(url).Result;

			//Console.WriteLine(msg);
			//Console.WriteLine("complete");
			//Console.ReadLine();
		}

		private static void DirectAccess()
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
						Url = stats.Url,
						ResponseTime = stats.ResponseTime,
						ContentLength = stats.ContentLength,
						ExceptionMessage = stats.ExceptionMessage,
						Status = stats.Status.ToString(),
						Created = DateTime.Now
					};

					repoPageStatus.Add(pageStatus);
					repoPageStatus.SaveChanges();

					Console.WriteLine($"seconds: {stats.ResponseTime}  length: {stats.ContentLength} time: {DateTime.Now}");
					Console.WriteLine();
				}

				Thread.Sleep(10000);

				counter--;
			}
		}

		private static async Task<string> WebServiceAccess(string url)
		{
			try
			{
				var client = new HttpClient();

				var responseMessage = await client.GetAsync(url);
				var message = await responseMessage.Content.ReadAsStringAsync();

				return message;
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
				return msg;
			}
		}
	}
}
