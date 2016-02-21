using System.ComponentModel.DataAnnotations;

namespace PageMonitorRepository.AdHoc
{
	public class AdHocPage
	{
		public short Id { get; set; }

		[Required]
		[StringLength(200)]
		public string Url { get; set; }

		[Required]
		[StringLength(128)]
		public string User { get; set; }

		public bool Test { get; set; }
	}
}
