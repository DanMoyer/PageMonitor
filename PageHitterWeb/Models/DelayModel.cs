using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PageHitterWeb.Models
{
	public class DelayModel
	{
		public short IterationHour { get; set; }

		public short IterationMinute { get; set; }

		public short IterationSecond { get; set; }

		public short PageHour { get; set; }

		public short PageMinute { get; set; }

		public short PageSecond { get; set; }

	}
}