using System;
using System.ComponentModel.DataAnnotations;

namespace PageMonitorRepository.AdHoc
{
	public class AdHocPageStatu
	{
		public int Id { get; set; }

		[Required]
		[StringLength(200)]
		public string Url { get; set; }

		public double ResponseTime { get; set; }

		public int ContentLength { get; set; }

		[Required]
		[StringLength(200)]
		public string Status { get; set; }

		[StringLength(200)]
		public string ExceptionMessage { get; set; }

		public DateTime Created { get; set; }

		[Required]
		[StringLength(128)]
		public string User { get; set; }
	}
}
