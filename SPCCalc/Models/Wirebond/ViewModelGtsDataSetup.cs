using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SPCCalc.Models.Wirebond
{
    [Table("WbSetupGts")]
    public class ViewModelGtsDataSetup
    {
        [Key]
        public string Guid { get; set; }
        public string GTSWBSetups { get; set; }
    }
}