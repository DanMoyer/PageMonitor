using System;

namespace PageHitterWeb.Models
{
	public class PageResponseModel
	{
		public string Url { get; set; }
		public double ResponseTime { get; set; }

		public DateTime Created { get; set; }

		public string Status { get; set; }
	}
}