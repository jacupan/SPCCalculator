using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPCCalc.Models.Wirebond
{
    [Table("spc_WbSLBs")]
    public class ViewModelWirebondSLBs
    {
        [Key]
        public string GUID { get; set; }
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string Device { get; set; }
        public string DageSerialNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string RunNo { get; set; }
        public string WireSize { get; set; }
        public string MachineName { get; set; }
        public string Machine { get; set; }
        public string Operator { get; set; }
        public string Remarks { get; set; }
        public Nullable<double> BST { get; set; }
        public string BSFailureMode { get; set; }
        public string DageProgramBST { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }


    [Table("spc_WbSLWp")]
    public partial class ViewModelWirebondSLWp
    {
        [Key]
        public string GUID { get; set; }
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string Device { get; set; }
        public string DageSerialNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string RunNo { get; set; }
        public string WireSize { get; set; }
        public string MachineName { get; set; }
        public string Machine { get; set; }
        public string Operator { get; set; }
        public string Remarks { get; set; }
        public Nullable<double> WPT { get; set; }
        public string WPFailureMode { get; set; }
        public string DageProgramWPT { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }


    [Table("VN_MachineList_WbSL")]
    public partial class ViewModelWirebondSLMachines
    {
        [Key]
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
    }

    [Table("spWipDataPromptWbSLBsref1")]
    public partial class ViewModelWirebondSLBsWipDataValues
    {
        [Key]
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string WIPDataPrompt { get; set; }
        public string WIPDataValue { get; set; }
    }

    [Table("spWipDataPromptWbSLWpref1")]
    public partial class ViewModelWirebondSLWpWipDataValues
    {
        [Key]
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string WIPDataPrompt { get; set; }
        public string WIPDataValue { get; set; }
    }

}