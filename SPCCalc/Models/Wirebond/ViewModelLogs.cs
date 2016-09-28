using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SPCCalc.Models.Wirebond
{
    [Table("spDeleteWirebondLogsTable2")]
    public class ViewModelLogs
    {
        [Key]        
        public string SetNo { get; set; }
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string Device { get; set; }
        public string DageSerialNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string RunNo { get; set; }
        public string WireSize { get; set; }
        public string MachineName { get; set; }  
        public string TestGroup { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}