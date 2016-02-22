using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PageHitterWeb.Models
{
	public class ChartViewModel
	{
		public Chart Chart { get; set; }

		public List<SelectListItem> Dates { get; set; }

		public string StartDate { get; set; }
	}
}