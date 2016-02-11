using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PageHitter
{
	public class PageStats
	{
		public string Url { get; set; }
		public double ResponseTime { get; set; }
		public int ContentLength { get; set; }
		public HttpStatusCode Status { get; set; }
		public string ExceptionMessage { get; set; }
	}
}
