using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SPCCalc.Models.Wirebond
{

    [Table("WbSetupSohed")]
    public class ViewModelSohedDataSetup
    {
        [Key]
        public string Guid { get; set; }
        public string SOHEDWBSetups { get; set; }
    }
}