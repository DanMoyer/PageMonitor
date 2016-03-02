using System;
using System.ComponentModel.DataAnnotations;

namespace PageHitterWeb.Models
{
	public class ChartViewModel
	{
		public string Url { get; set; }

		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime EndDate { get; set; }

		public string ButtonClicked { get; set; }
	}
}