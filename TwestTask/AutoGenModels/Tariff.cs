using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Energy2.AutoGen
{
    [Index("Id", IsUnique = true)]
    public partial class Tariff
    {
        [Key]
        public long Id { get; set; }

        [Column("\nServiceName")]
        public string ServiceName { get; set; } = null!;

        [Column(TypeName = "NUMERIC (6, 2)")]
        public decimal? TariffPrice { get; set; }

        [Column(TypeName = "NUMERIC (10, 5)")]
        public decimal? Standard { get; set; }

        public string? Unit { get; set; }
    }

}

