namespace PageMonitor.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PageStatus
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
    }
}
