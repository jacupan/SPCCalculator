﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SPCCalc.Models.Mold
{
    [Table("spc_MoldSensor")]
    public class ViewModelUaMatrix
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
        public double PkgWidth { get; set; }
        public double PkgThickness { get; set; }
        public double PkgHeight { get; set; }
        public double XPlacement { get; set; }
        public double YPlacement { get; set; }
        public double EPIN { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    [Table("VN_MachineList_MoldSensor")]
    public class ViewModelSensorMachines
    {
        [Key]
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
    }

    [Table("spViewDataForCamstarMoldUaMatrix")]
    public class ViewModelUaMatrixWipDataValues
    {
        [Key]
        public string RowNo { get; set; }
        public string LotNumber { get; set; }
        public string FramePosition { get; set; }
        public DateTime? DateCreated { get; set; }
        public string WIPDataPrompt { get; set; }
        public string WIPDataValue { get; set; }
    }
}