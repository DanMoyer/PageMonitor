using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using PageHitter;
using PageHitterWeb.Models;
using PageMonitorRepository;

namespace PageHitterWeb.Controllers
{

	/// <summary>
	/// http://stackoverflow.com/questions/13222998/calling-external-http-service-using-httpclient-from-a-web-api-action
	/// http://blog.stephencleary.com/2012/02/async-and-await.html
	/// 
	/// http://localhost:9476/api/Values/?json=true
	/// </summary>
	//[Authorize]
	public class ValuesController : ApiController
	{
		// GET api/values
		//public async Task<IEnumerable<PageResponseModel>> Get()
		//public async Task<JsonResult<List<PageResponseModel>>> Get()
		public async Task<object> Get(bool html = false)
		{
			var listPageStatus = await HitPages(1);

			var pageStatuses = listPageStatus as IList<PageStatus> ?? listPageStatus.ToList();
			var listPageResponse = GetPageResponses(pageStatuses.ToList());

			if (!html)
			{
				var jsonResult = Json(listPageResponse);
				return jsonResult;

				//return new string[] { "value1", "value2" };
			}
			else
			{
				return GetPageData(listPageResponse);
			}
		}

		// GET api/values/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/values
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}


		private async Task<IEnumerable<PageStatus>> HitPages(int count)
		{
			var repoPages = new PagesRepository();
			var pages = repoPages.GetAllProdMonitor();
			//var pages = repoPages.GetAllStgMonitor();

			var repoPageStatus = new PageStatusRepository();

			var pageGetter = new PageGetter();

			var counter = count;

			var pageStati = new List<PageStatus>();

			while (counter > 0)
			{
				foreach (var page in pages)
				{
					var pageStats = new PageStats {Url = page.Url};
					var stats = await pageGetter.HTTP_GET(pageStats);
					//var stats = result.Result;

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

					pageStati.Add(pageStatus);
				}

				counter--;
			}

			return pageStati;
		}

		private HttpResponseMessage GetPageData(IEnumerable<PageResponseModel> pageResponses)
		{
			var strBody = new StringBuilder();

			strBody.Append("<html><body><table>");

			foreach (var row in pageResponses
				.Select(page => $"<TR><TD>{page.Url}</TD><TD>{page.ResponseTime}</TD><TR/>"))
			{
				strBody.Append(row);
			}

			strBody.Append("</table></body></html>");


			var response = new HttpResponseMessage
			{
				Content = new StringContent(strBody.ToString())
			};

			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

			return response;
		}

		private IEnumerable<PageResponseModel> GetPageResponses(IEnumerable<PageStatus> listPageStatus )
		{
			var pageResponses = new List<PageResponseModel>();

			foreach (var pageStatus in listPageStatus)
			{
				var pageResponse = new PageResponseModel()
				{
					ResponseTime = pageStatus.ResponseTime,
					Url = pageStatus.Url
				};

				pageResponses.Add(pageResponse);
			}

			return pageResponses;
		}
	}
}
