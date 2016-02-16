namespace PageMonitorRepository
{
	using System.ComponentModel.DataAnnotations;

	public class Page
	{
		public short Id { get; set; }

		[Required]
		[StringLength(200)]
		public string Url { get; set; }

		public bool Monitor { get; set; }

		public bool AdHoc { get; set; }
	}
}
