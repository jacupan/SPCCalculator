using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPCCalc.Models
{
    [Table("spc_MoldGts2")]
    public partial class MoldGts2
    {
        [Key]
        public string GUID { get; set; }
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string Device { get; set; }
        public string RunNumber { get; set; }
        public string FramePosition { get; set; }
        public string Machine { get; set; }
        public string A2Operator { get; set; }
        public string Remarks { get; set; }        
        public double PkgThickness { get; set; }
        public double PkgWidth { get; set; }        
        public double PkgHeight { get; set; }        
        public double AValue { get; set; }
        public double DXValue { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double SSPM { get; set; }
        public double DYValue { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TBPM { get; set; }
        public double EPIN { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }

    }

    [Table("VN_WIPDataPrompt_MoldGts2_ref3")]
    public partial class MoldGTS2WIPData
    {
        [Key]
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string FramePosition { get; set; }
        public DateTime DateCreated { get; set; }
        public string WIPDataPrompt { get; set; }
        public string WIPDataValue { get; set; }
    }

    [Table("logsMoldGts2")]
    public partial class MoldGTS2Logs
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