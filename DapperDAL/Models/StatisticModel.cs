using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDAL.Models
{
    public class StatisticModel
    {
        #region " Properties "

        public int TotalCDs { get; set; }

        public int RockDisks { get; set; }

        public int FolkDisks { get; set; }

        public int AcousticDisks { get; set; }

        public int JazzDisks { get; set; }

        public int BluesDisks { get; set; }

        public int CountryDisks { get; set; }

        public int ClassicalDisks { get; set; }

        public int SoundtrackDisks { get; set; }

        public int FourStarDisks { get; set; }

        public int ThreeStarDisks { get; set; }

        public int TwoStarDisks { get; set; }

        public int OneStarDisks { get; set; }

        [Column(TypeName = "money")]
        public decimal RecordCost { get; set; }

        [Column(TypeName = "money")]
        public decimal CDCost { get; set; }

        [Column(TypeName = "money")]
        public decimal AvCDCost { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalCost { get; set; }

        public int Disks2017 { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost2017 { get; set; }

        [Column(TypeName = "money")]
        public decimal Av2017 { get; set; }

        public int Disks2018 { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost2018 { get; set; }

        [Column(TypeName = "money")]
        public decimal Av2018 { get; set; }

        public int Disks2019 { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost2019 { get; set; }

        [Column(TypeName = "money")]
        public decimal Av2019 { get; set; }

        public int Disks2020 { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost2020 { get; set; }

        [Column(TypeName = "money")]
        public decimal Av2020 { get; set; }

        public int Disks2021 { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost2021 { get; set; }

        public decimal Av2021 { get; set; }

        public int Disks2022 { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost2022 { get; set; }

        [Column(TypeName = "money")]
        public decimal Av2022 { get; set; }

        public int TotalRecords { get; set; }

        #endregion
    }
}
