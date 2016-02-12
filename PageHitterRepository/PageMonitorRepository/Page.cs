namespace PageMonitorRepository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Page
    {
        public short Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Url { get; set; }

        public bool Monitor { get; set; }
    }
}
