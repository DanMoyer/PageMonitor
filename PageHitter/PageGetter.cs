using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PageHitter
{
	public class PageGetter
	{
		public async Task<PageStats> HTTP_GET(PageStats pageStats)
		{
			try
			{
				var credential = new NetworkCredential("SPTester1", "Password1", "NTSERVERS");
				var myCache = new CredentialCache { { new Uri(pageStats.Url), "NTLM", credential } };

				var handler = new HttpClientHandler
				{
					AllowAutoRedirect = true,
					Credentials = myCache
				};

				var client = new HttpClient(handler);

				var stopWatch = new Stopwatch();
				stopWatch.Start();

				var responseMessage = await client.GetAsync(pageStats.Url);
				var message = await responseMessage.Content.ReadAsStringAsync();

				stopWatch.Stop();
				var elapsed = stopWatch.Elapsed;

				pageStats.ResponseTime = Convert.ToDouble($"{elapsed.Seconds}.{elapsed.Milliseconds}");
				pageStats.Status = responseMessage.StatusCode;
				pageStats.ContentLength = message.Length;

				return pageStats;
			}
			catch (Exception ex)
			{
				pageStats.ContentLength = 0;
				pageStats.ResponseTime = 0;
				pageStats.Status = HttpStatusCode.ExpectationFailed;
				pageStats.ExceptionMessage = ex.Message;

				return pageStats;
			}
		}
	}
}
