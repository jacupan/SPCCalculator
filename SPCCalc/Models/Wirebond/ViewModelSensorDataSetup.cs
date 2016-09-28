using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SPCCalc.Models.Wirebond
{

    [Table("WbSetupSensor")]
    public class ViewModelSensorDataSetup
    {
        [Key]
        public string Guid { get; set; }
        public string SensorWBSetups { get; set; }
    }
}