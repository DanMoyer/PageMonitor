using System.Collections.Generic;

namespace PageHitterWeb.Models
{
	public class ResponseTimesBuckets
	{
		public string BucketLabel { get; set; }
		public int Count { get; set; }

		public int Order { get; set; }
	}

	public class ResponseTimes
	{
		public List<ResponseTimesBuckets> ResponseTimeBuckets { get; set; }

		public int TotalCount { get; set; }
	}
}