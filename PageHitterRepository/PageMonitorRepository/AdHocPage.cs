namespace PageMonitorRepository
{
	using System.ComponentModel.DataAnnotations;

	public class AdHocPage
	{
		public short Id { get; set; }

		[Required]
		[StringLength(200)]
		public string Url { get; set; }

		[Required]
		[StringLength(128)]
		public string UserId { get; set; }
	}
}
