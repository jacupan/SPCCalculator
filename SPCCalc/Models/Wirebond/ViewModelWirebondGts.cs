using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SPCCalc.Models.Wirebond
{
    [Table("spc_WbGtsBs")]
    public class ViewModelWirebondGtsBs
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

    [Table("spc_WbGtsBsPbo")]
    public partial class ViewModelWirebondGtsBsPbo
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

    [Table("spc_WbGtsWp")]
    public partial class ViewModelWirebondGtsWp
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

    [Table("spc_WbGtsWpPbo")]
    public partial class ViewModelWirebondGtsWpPbo
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

    [Table("VN_MachineList_WbGts")]
    public partial class ViewModelWirebondGtsMachines
    {
        [Key]
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
    }

    [Table("spWipDataPromptWbGtsBsref1")]       
    public partial class ViewModelWirebondGtsBsWipDataValues
    {
        [Key]
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string WIPDataPrompt { get; set; }
        public string WIPDataValue { get; set; }
    }

    [Table("spWipDataPromptWbGtsBsPboref1")]        
    public partial class ViewModelWirebondGtsBsPboWipDataValues
    {
        [Key]
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string WIPDataPrompt { get; set; }
        public string WIPDataValue { get; set; }
    }

    [Table("spWipDataPromptWbGtsWpref1")]        
    public partial class ViewModelWirebondGtsWpWipDataValues
    {
        [Key]
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string WIPDataPrompt { get; set; }
        public string WIPDataValue { get; set; }
    }

}