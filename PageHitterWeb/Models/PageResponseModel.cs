using System;
using System.ComponentModel.DataAnnotations;

namespace PageHitterWeb.Models
{
	public class PageResponseModel
	{
		public string Url { get; set; }
		public double ResponseTime { get; set; }


		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss tt}", ApplyFormatInEditMode = true)]
		public DateTime Created { get; set; }

		public string Status { get; set; }
	}
}