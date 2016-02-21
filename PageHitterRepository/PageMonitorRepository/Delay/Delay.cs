using System.ComponentModel.DataAnnotations;

namespace PageMonitorRepository.Delay
{
	public class Delay
	{
		[Required]
		public short Id { get; set; }

		[Required]
		public short IterationHour { get; set; }

		[Required]
		public short IterationMinute { get; set; }

		[Required]
		public short IterationSecond { get; set; }

		[Required]
		public short PageHour { get; set; }

		[Required]
		public short PageMinute { get; set; }

		[Required]
		public short PageSecond { get; set; }
	}
}
