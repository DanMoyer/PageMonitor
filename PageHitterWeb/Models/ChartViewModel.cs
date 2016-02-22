using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PageHitterWeb.Models
{
	public class ChartViewModel
	{
		public Chart Chart { get; set; }

		//public List<SelectListItem> Dates { get; set; }

		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime EndDate { get; set; }
	}
}