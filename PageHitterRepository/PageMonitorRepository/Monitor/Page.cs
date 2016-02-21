using System.ComponentModel.DataAnnotations;

namespace PageMonitorRepository.Monitor
{
	public class Page
	{
		public short Id { get; set; }

		[Required]
		[StringLength(200)]
		public string Url { get; set; }

		public bool Monitor { get; set; }
	}
}
