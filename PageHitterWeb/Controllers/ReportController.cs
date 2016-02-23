using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PageHitterWeb.Models;
using PageMonitorRepository;
using PageMonitorRepository.Monitor;


/*
	http://weblogs.asp.net/jalpeshpvadgama/chart-helpers-in-asp-net-mvc3

	http://www.4guysfromrolla.com/articles/092210-1.aspx

	http://blog.smirne.com/2012/09/creating-interactive-charts-with-aspnet.html

	http://www.asp.net/web-pages/overview/data/7-displaying-data-in-a-chart

	https://msdn.microsoft.com/en-us/library/dd489233.aspx

	https://msdn.microsoft.com/en-us/library/system.web.helpers.chart(v=vs.99)

	http://www.danylkoweb.com/Blog/simplified-aspnet-mvc-charts-A5

	http://stackoverflow.com/questions/21430701/drawing-charts-into-asp-net-mvc-4-razor-c-web-sites

	http://www.codeproject.com/Articles/125230/ASP-NET-MVC-Chart-Control



	http://www.dotnetcurry.com/aspnet-mvc/822/html5-bar-chart-helper-aspnet-mvc

	http://blog.smirne.com/2012/09/creating-interactive-charts-with-aspnet.html

	http://www.jqplot.com/index.php

	http://www.highcharts.com/

	https://www.shieldui.com/purchase

	https://www.nuget.org/packages/Chart.Mvc/

	http://www.chartjs.org/docs/

	http://www.asp.net/mvc/overview/older-versions/using-the-html5-and-jquery-ui-datepicker-popup-calendar-with-aspnet-mvc/using-the-html5-and-jquery-ui-datepicker-popup-calendar-with-aspnet-mvc-part-4

*/


namespace PageHitterWeb.Controllers
{
	public class ReportController : Controller
	{
		// GET: Report
		public ActionResult Index(ChartViewModel model)
		{

			

			//model.Chart = chart;

			//var stream = chart.Write(format: "png");
			//var bytes = chart.GetBytes();
			//var webImage = chart.ToWebImage();

			return View(model);
		}

		public ActionResult DrawChart()
		{
			List<ResponseTimes> responseTimes;

			using (var context = new PageMonitorDb())
			{
				//var startDate = model.StartDate.ToShortDateString();
				//var endDate = model.EndDate.ToShortDateString();

				var startDate = "02/15/2016";
				var endDate = "02/17/2016";

				var query = GetQuery(startDate, endDate);

				responseTimes = context.Database.SqlQuery<ResponseTimes>(query).ToList();
			}

			var xValue = new string[responseTimes.Count];
			var yValue = new string[responseTimes.Count];

			for (var idx = 0; idx < responseTimes.Count; idx++)
			{
				xValue[idx] = responseTimes[idx].ResponseTime;
				yValue[idx] = responseTimes[idx].TotalCount.ToString();
			}

			var chart = GetChart(xValue, yValue);

			return File(chart.GetBytes(), "image/bytes");
		}


		public ActionResult Report()
		{
			var model = new ChartViewModel
			{
				//Dates = dates
			};

			return View(model);
		}

		[HttpPost]
		public ActionResult Report(FormCollection form)
		{
			var model = new ChartViewModel();

			UpdateModel(model);

			
			return RedirectToAction("Index", model);
		}


		private Chart GetChart(string[] xValues, string[] yValues)
		{
			//return new Chart(600, 400, ChartTheme.Blue)
			//	.AddTitle("Page Response Times")
			//	.AddLegend()
			//	.AddSeries(
			//		name: "WebSite",
			//		chartType: "Pie",
			//		xValue: new[] { "Digg", "DZone", "DotNetKicks", "StumbleUpon" },
			//		yValues: new[] { "150000", "180000", "120000", "250000" });


			var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Green)
				.AddTitle("Page Response Times")
				.AddSeries(
					name: "ChartTitle",
					xValue: xValues,
					yValues: yValues);

			return myChart;
		}

		private string GetQuery(string startDate, string endDate)
		{

			var createdClause = $"AND Created > '{startDate}' AND Created < '{endDate}' ";

			string query =
				"SELECT ResponseTime, Count(*) TotalCount FROM ( " +
				"SELECT ResponseTime = " +
				"CASE  " +
				"WHEN ResponseTime >= 0 and ResponseTime <= 1 THEN '0-1' " +
				"WHEN ResponseTime >= 1 and ResponseTime <= 5 THEN '1-5' " +
				"WHEN ResponseTime >= 5 and ResponseTime <= 10 THEN '6-10' " +
				"WHEN ResponseTime >= 10 and ResponseTime <= 15 THEN '10-15' " +
				"WHEN ResponseTime >= 15 and ResponseTime <= 20 THEN '15-20' " +
				"WHEN ResponseTime >= 20 and ResponseTime <= 25 THEN '20-25' " +
				"ELSE 'over 25' " +
				"END " +
				"FROM[PageMonitor].[dbo].[PageStatus] WHERE " +
				"Url = 'https://www.findlay.edu' " +
				"AND Status = 'OK' " +
				$"AND Created >= '{startDate}' AND Created <= '{endDate}' " +
				"GROUP BY ResponseTime) AS SourceTabel " +
				"GROUP BY ResponseTime ";

			return query;
		}
	}
}