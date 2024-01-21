using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDAL.Models
{
    public class TotalModel
    {
        [Key]
        public int ArtistId { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        public int TotalDiscs { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalCost { get; set; }
    }
}
