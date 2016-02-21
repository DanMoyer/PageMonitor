using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PageHitterWeb.Models
{
	public class DelayModel
	{
		[DisplayName("Iteration  Hours")]
		public short IterationHour { get; set; }

		[DisplayName("Iteration  Minutes")]
		public short IterationMinute { get; set; }

		[DisplayName("Iteration  Seconds")]
		public short IterationSecond { get; set; }

		[DisplayName("Page  Hours")]
		public short PageHour { get; set; }

		[DisplayName("Page  Minutes")]
		public short PageMinute { get; set; }

		[DisplayName("Page  Seconds")]
		public short PageSecond { get; set; }

	}
}