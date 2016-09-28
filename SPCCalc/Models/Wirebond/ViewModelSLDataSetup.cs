using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPCCalc.Models.Wirebond
{

    [Table("WbSetupSL")]
    public class ViewModelSLDataSetup
    {
        [Key]
        public string Guid { get; set; }
        public string SLWBSetups { get; set; }
    }
}