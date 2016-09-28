using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SPCCalc.Models.Mold
{
    [Table("spDeleteMoldLogsTable2")]
    public class ViewModelLogs
    {
        [Key]
        public string LotNumber { get; set; }
        public string FramePosition { get; set; }
        public string Element { get; set; }
        public double Actual { get; set; }
        public double Nominal { get; set; }
        public double Deviation { get; set; }
        public double UpTol { get; set; }
        public double LowTol { get; set; }
        public string Status { get; set; }
        public string Status2 { get; set; }
        public DateTime DateCreated { get; set; }
    }
}