using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPCCalc.Models.Sensor
{
    public class vmSensor
    {
        public string Module { get; set; }
        public string LotNumber { get; set; }
        public string Device { get; set; }
        public string FramePosition { get; set; }
        public string Machine { get; set; }
        public string A2Operator { get; set; }
        public string Remarks { get; set; }
        public int HEDA { get; set; }
        public int HEDB { get; set; }
        public int HEDC { get; set; }
        public int PkgWidth { get; set; }
        public int PkgHeight { get; set; }
        public int EPIN { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string PackageGroup { get; set; }

    }
}