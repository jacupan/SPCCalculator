using System;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SPCCalc.Models
{
    public class MoldSensorKEKTSIPKA
    {
    }

    [Table("VN_MachineList_Mold_Sensor")]
    public class MachineList_MoldSensor_KEKTSIPKA
    {
        [Key]
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
    }

    [Table("VN_MachineList_DieAttach_Bondline")]
    public class MachineList_DieAttach_Bondline
    {
        [Key]
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
    }
}