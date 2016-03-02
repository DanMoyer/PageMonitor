using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using PageHitterWeb.Models;
using PageMonitorRepository.Monitor;
using Chart = System.Web.Helpers.Chart;


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
			return View(model);
		}

		public ActionResult DrawChart(string startDate, string endDate, string url)
		{
				try
				{
					//var totalPages = 0;
					ResponseTimes responseTimes;
					using (var context = new PageStatusRepository())
					{
						var responseTimesList = context.GetPageStatusByDate(startDate, endDate, url);

						responseTimes = GetTimeBuckets(responseTimesList);
					}

					var urlAndPages = url + $"     Total Pages: {responseTimes.TotalCount}";

					var xValue = new string[responseTimes.ResponseTimeBuckets.Count];
					var yValue = new string[responseTimes.ResponseTimeBuckets.Count];

					for (var idx = 0; idx < responseTimes.ResponseTimeBuckets.Count; idx++)
					{
						xValue[idx] = responseTimes.ResponseTimeBuckets[idx].BucketLabel;
						yValue[idx] = responseTimes.ResponseTimeBuckets[idx].Count.ToString();
					}

					var chart = GetChart2(xValue, yValue, urlAndPages);

					var memStream = new MemoryStream();
					chart.SaveImage(memStream);
					return File(memStream.GetBuffer(), "image/bytes");

					//return File(chart.GetBytes(), "image/bytes");
				}

				catch (Exception ex)
				{
					// ReSharper disable once UnusedVariable
					var msg = ex.Message;
					return RedirectToAction("Index", "Home");
				}
		}


		private static ResponseTimes GetTimeBuckets(IEnumerable<double> responseTimes)
		{
			var times = new ResponseTimes();

			//this is the total buckets used in the graph.
			var allTimeBuckets = new List<string> { "0-1", "1-2", "2-3", "3-4", "4-5", "5-10", "10-15", "15-20", "Unknown" };

			var responseTimeBuckets = new List<ResponseTimesBuckets>();

			//Find what 'bucket' each response time belongs in
			var query =
			(from n in responseTimes
				 where n > 0
				 select new ResponseTimesBuckets
				 {
					 BucketLabel =
						(
							 n >= 0 && n <= 0.999 ? "0-1" :
							 n >= 1 && n <= 1.999 ? "1-2" :
							 n >= 2 && n <= 2.999 ? "2-3" :
							 n >= 3 && n <= 3.999 ? "3-4" :
							 n >= 4 && n <= 4.999 ? "4-5" :
							 n >= 5 && n <= 9.999 ? "5-10" :
							 n >= 10 && n <= 14.999 ? "10-15" :
							 n >= 15 && n <= 19.999 ? "15-20" :
							 n >= 20 ? ">20" : "Unknown"
						)
				 }
			 );

			var responseTimeses = query as ResponseTimesBuckets[] ?? query.ToArray();
			var bucketList      = responseTimeses.Select(x => x.BucketLabel);
			var buckets         = responseTimeses.GroupBy(x => x.BucketLabel);
			
			//Group the buckets into a collection. Calculate number of page hits for each bucket
			responseTimeBuckets.AddRange(
					responseTimeses
					.GroupBy(x => x.BucketLabel)
					.Select(bucket => new { bucket, count = bucketList.Count(x => x == bucket.Key) })
					.Select(@t => new ResponseTimesBuckets { BucketLabel = @t.bucket.Key, Count = @t.count }));
			
			//Of the all the available buckets, find the buckets with no page hits.
			responseTimeBuckets.AddRange(
				allTimeBuckets.Except(buckets.Select(x => x.Key))
				.Select(item => new ResponseTimesBuckets { BucketLabel = item, Count = 0 }));


			//Calculate the total number of pages hit
			var allpageCounts = responseTimeBuckets.Select(x => x.Count).ToList();
			var totalCount    = allpageCounts.Sum();

			//Using the numeric value of the bucket label, set a order value
			// so the buckets display in correct order on the graph
			foreach (var item in responseTimeBuckets)
			{
				var index = item.BucketLabel.IndexOf("-", StringComparison.Ordinal);

				item.Order = index > 0 ? Convert.ToInt32(item.BucketLabel.Substring(0, index)) : 100;
			}

			times.ResponseTimeBuckets = responseTimeBuckets.OrderBy(x => x.Order).ToList();
			times.TotalCount = totalCount;

			return times;
		}

		public ActionResult Report()
		{
			var model = new ChartViewModel
			{
				StartDate = DateTime.Now.AddDays(-1),
				EndDate   = DateTime.Now
			};


			return View(model);
		}

		[HttpPost]
		public ActionResult Report(FormCollection form)
		{
			var model = new ChartViewModel();

			UpdateModel(model);

			if (model.ButtonClicked == "grid")
			{
				//The URL should come from a selection in the UI.
				//hardcode here for debugging
				model.Url = "https://www.findlay.edu";

				return RedirectToAction("Index", "Grid", model);
			}

			return RedirectToAction("Grid", model);
		}


		public ActionResult Grid(ChartViewModel model)
		{

			//The URL should come from a selection in the UI.
			//hardcode here for debugging
			model.Url = "https://www.findlay.edu";

			return RedirectToAction("Index", model);
		}



		private System.Web.UI.DataVisualization.Charting.Chart GetChart2(string[] xValues, string[] yValues, string url)
		{
			var chart = new System.Web.UI.DataVisualization.Charting.Chart
			{
				Width                   = 600,
				Height                  = 300,
				BackColor               = Color.FromArgb(255, 140, 0),
				BorderlineDashStyle     = ChartDashStyle.Solid,
				BackSecondaryColor      = Color.White,
				BackGradientStyle       = GradientStyle.TopBottom,
				BorderlineWidth         = 1,
				Palette                 = ChartColorPalette.BrightPastel,
				BorderlineColor         = Color.FromArgb(0, 0, 0),
				RenderType              = RenderType.BinaryStreaming,
				BorderSkin              = {SkinStyle = BorderSkinStyle.Emboss},
				AntiAliasing            = AntiAliasingStyles.All,
				TextAntiAliasingQuality = TextAntiAliasingQuality.Normal
			};

			chart.Titles.Add(CreateTitle(url));
			//chart.Legends.Add(CreateLegend());
			chart.Series.Add(CreateSeries(xValues, yValues, url));
			chart.ChartAreas.Add(CreateChartArea(url));


			return chart;
		}


		private Chart GetChart(string[] xValues, string[] yValues, string url)
		{
			var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Green)
				.AddTitle(url)
				.AddSeries(
					name: "ChartTitle",
					xValue: xValues,
					yValues: yValues
					)
				.SetXAxis("Seconds")
				.SetYAxis("Page Hits");
			
			return myChart;
		}

		private Series CreateSeries(string[] xValues, string[] yValues, string url)
		{
			var seriesDetail = new Series
			{
				Name                = "A", //$"{url}",
				IsValueShownAsLabel = true,
				ChartType           = SeriesChartType.Bar,
				Color               = Color.Black,
				BorderWidth         = 2,
			};

			for (var idx = 0; idx < xValues.Length; idx++)
			{
				var point = new DataPoint
				{
					AxisLabel = xValues[idx],
					YValues = new double[] {double.Parse(yValues[idx])}
				};

				seriesDetail.Points.Add(point);
			}
			
			seriesDetail.ChartArea = $"{url}";

			return seriesDetail;
		}

		private Legend CreateLegend()
		{
			var legend = new Legend
			{
				ShadowColor  = Color.FromArgb(32, 0, 0),
				Font         = new Font("Trebuchet MS", 14F, FontStyle.Bold),
				ShadowOffset = 3,
				ForeColor    = Color.FromArgb(26, 59, 105)
			};

			return legend;
		}

		private Title CreateTitle(string url)
		{
			var title = new Title
			{
				Text         = $"{url}",
				ShadowColor  = Color.FromArgb(32, 0, 0, 0),
				Font         = new Font("Trebuchet MS", 10F, FontStyle.Bold),
				ShadowOffset = 0,
				ForeColor    = Color.FromArgb(0, 0, 0)
			};
			return title;

		}

		private ChartArea CreateChartArea(string url)
		{
			ChartArea chartArea = new ChartArea
			{
				Name          = $"{url}",
				//Name        = "",
				BackColor     = Color.Transparent,
				AxisX         = {IsLabelAutoFit = false},
				AxisY         = {IsLabelAutoFit = false}
			};

			chartArea.AxisX.LabelStyle.Font     = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
			chartArea.AxisY.LabelStyle.Font     = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
			chartArea.AxisY.LineColor           = Color.FromArgb(0, 0, 0);
			chartArea.AxisX.LineColor           = Color.FromArgb(0, 0, 0);
			chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(0, 0, 0, 0);
			chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(0, 0, 0, 0);
			chartArea.AxisX.Interval            = 1;

			return chartArea;
		}
	}
}
