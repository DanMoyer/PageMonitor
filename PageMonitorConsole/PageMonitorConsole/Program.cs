using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PageHitter;
using PageMonitorRepository;


/*
   http://blog.amitapple.com/post/2015/06/scheduling-azure-webjobs/#.Vr-aumbSl9A
   https://azure.microsoft.com/en-us/documentation/articles/web-sites-create-web-jobs/#CreateScheduledCRON
   https://azure.microsoft.com/en-us/documentation/articles/websites-dotnet-deploy-webjobs/

*/

namespace PageMonitorConsole
{
	class Program
	{
		static void Main()
		{
			//DirectAccess();

			//const string url = "http://pagehitterweb.azurewebsites.net/api/Values";
			//const string url = "http://localhost:9476/api/Values/?html=false&page=https://www.findlay.edu";
			const string url = "http://localhost:9476/api/Values/?html=false&page=";

			//const string url = "http://localhost:9476/api/Values/?json=true";

			var delayRepo = new DelayRepository();
			var repoPages = new PagesRepository();

			while (true)
			{
				var delay = delayRepo.GetDelay();
				var pages = repoPages.GetAllProdMonitor();

				//Hit each page from web job.
				foreach (var page in pages)
				{
					var msg = WebServiceAccess(url + page.Url).Result;
					Console.WriteLine(msg);

					var interPageSleepTime = new TimeSpan(0, delay.PageHour, delay.PageMinute, delay.PageSecond);
					Thread.Sleep(interPageSleepTime);
				}

				//Sleep before hitting collection of pages again
				Console.WriteLine("Sleeping {delay.iterationMinute}  {delay.IterationSecond}");
				var sleepTime = new TimeSpan(0, delay.IterationHour, delay.IterationMinute, delay.IterationSecond);  //1 min
				Thread.Sleep(sleepTime);  
			}			
			
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
