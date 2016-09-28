using algCamstarXMLIfx;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SPCCalc.Models;
using SPCCalc.Models.User;
using SPCCalc.Models.Wirebond;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SPCCalc.Controllers
{
    public class WirebondSensorController : Controller
    {

        public SPCDBContext spcDbContext_ = new SPCDBContext();
        string strConn = ConfigurationManager.ConnectionStrings["SPCContext"].ConnectionString;
        public string xmlConfigPath = ConfigurationManager.AppSettings["xmlConfigPath"].ToString();
        public string cserver = ConfigurationManager.AppSettings["cserver"].ToString();

        User user = new User();
        Logs logs = new Logs();

        //
        // GET: /WirebondSensor/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of wip data setup available for Sensor
        /// </summary>
        /// <returns>Filter for Sensor Bs/Pbo only</returns>
        public JsonResult WipDataSetupSensorBs(string strLotNoWbSensorBs, string strLotNoWbSensorBsPbo)
        {
            var lotNo = "";

            if (strLotNoWbSensorBs != null && strLotNoWbSensorBs != "")
            {
                lotNo = strLotNoWbSensorBs;
            }
            else
            {
                lotNo = strLotNoWbSensorBsPbo;
            }

            var testGroup = (from a in spcDbContext_.ViewModelWirebondSensorBs
                             where a.LotNumber == lotNo
                             select a.DageProgramBST).Distinct().ToList();

            if (testGroup == null || testGroup.Count == 0)
            {
                testGroup = (from a in spcDbContext_.ViewModelWirebondSensorBsPbo
                             where a.LotNumber == lotNo
                             select a.DageProgramBST).Distinct().ToList();
            }

            if (testGroup.Count > 1)
            {
                return Json("");
            }
            else if (testGroup.Count == 1)
            {
                var distinct = (from a in spcDbContext_.ViewModelWirebondSensorBs
                                where a.LotNumber == lotNo
                                select new
                                {
                                    DageProgramBST = a.DageProgramBST
                                }).FirstOrDefault();

                if (distinct == null)
                {
                    distinct = (from a in spcDbContext_.ViewModelWirebondSensorBsPbo
                                where a.LotNumber == lotNo
                                select new
                                {
                                    DageProgramBST = a.DageProgramBST
                                }).FirstOrDefault();
                }

                if (distinct != null)
                {
                    var resultString = Regex.Match(distinct.DageProgramBST, @"\d+").Value;

                    if (distinct.DageProgramBST.Contains("KMATRIX"))
                    {
                        return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("bst-tok") || a.SensorWBSetups.Contains("bst-tkb") || (a.SensorWBSetups.Contains("bst-" + resultString) && (a.SensorWBSetups.TrimEnd().EndsWith("NOCTRLLIMITS")))).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
                    }

                    else if (distinct.DageProgramBST.Contains("LL"))
                    {
                        return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("bst-" + resultString + "w-ll") || (a.SensorWBSetups.Contains("bst-" + resultString) && (a.SensorWBSetups.TrimEnd().EndsWith("NOCTRLLIMITS")))).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
                    }

                    else if (distinct.DageProgramBST.Contains("UA"))
                    {
                        if (distinct.DageProgramBST.Contains("UAM"))
                        {
                            return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("bst-" + resultString + "w-uam") || (a.SensorWBSetups.Contains("bst-" + resultString) && (a.SensorWBSetups.TrimEnd().EndsWith("NOCTRLLIMITS")))).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("bst-" + resultString + "w-ua_") || (a.SensorWBSetups.Contains("bst-" + resultString) && (a.SensorWBSetups.TrimEnd().EndsWith("NOCTRLLIMITS")))).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
                        }
                    }

                    else
                    {
                        return Json("");
                    }

                }
                else
                {
                    return Json("");
                }
            }
            else
            {
                return Json("");
            }

            //return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("bst")).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// List of wip data setup available for Sensor
        /// </summary>
        /// <returns>Filter for Sensor Wp only</returns>
        public JsonResult WipDataSetupSensorWp(string strLotNoWbSensorWp)
        {
            var lotNo = "";
            lotNo = strLotNoWbSensorWp;

            var testGroup = (from a in spcDbContext_.ViewModelWirebondSensorWp
                             where a.LotNumber == lotNo
                             select a.DageProgramWPT).Distinct().ToList();

            if (testGroup.Count > 1)
            {
                return Json("");
            }
            else if (testGroup.Count == 1)
            {
                var distinct = (from a in spcDbContext_.ViewModelWirebondSensorWp
                                where a.LotNumber == lotNo
                                select new
                                {
                                    DageProgramWPT = a.DageProgramWPT
                                }).FirstOrDefault();

                if (distinct != null)
                {
                    var resultString = Regex.Match(distinct.DageProgramWPT, @"\d+").Value;
                    //var keywords = new[] { resultString, "NOCTRLLIMITS" };

                    if (distinct.DageProgramWPT.Contains("KMATRIX"))
                    {
                        return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("wpt-" + resultString + "w_tok") || a.SensorWBSetups.Contains("wpt-" + resultString + "w_tkb") || (a.SensorWBSetups.Contains("wpt-" + resultString) && (a.SensorWBSetups.TrimEnd().EndsWith("NOCTRLLIMITS")))).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
                    }

                    else if (distinct.DageProgramWPT.Contains("LL"))
                    {
                        return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("wpt-" + resultString + "w-ll") || (a.SensorWBSetups.Contains("wpt-" + resultString) && (a.SensorWBSetups.TrimEnd().EndsWith("NOCTRLLIMITS")))).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
                    }

                    else if (distinct.DageProgramWPT.Contains("UA"))
                    {
                        if (distinct.DageProgramWPT.Contains("UAM"))
                        {
                            return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("wpt-" + resultString + "w-uam") || (a.SensorWBSetups.Contains("wpt-" + resultString) && (a.SensorWBSetups.TrimEnd().EndsWith("NOCTRLLIMITS")))).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("wpt-" + resultString + "w-ua_") || (a.SensorWBSetups.Contains("wpt-" + resultString) && (a.SensorWBSetups.TrimEnd().EndsWith("NOCTRLLIMITS")))).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
                        }
                    }

                    else
                    {
                        return Json("");
                    }

                }
                else
                {
                    return Json("");
                }
            }
            else
            {
                return Json("");
            }

            //return Json(spcDbContext_.ViewModelSensorDataSetup.Where(a => a.SensorWBSetups.Contains("wpt")).OrderBy(b => b.SensorWBSetups), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Wip data values automatically send to CAMSTAR
        /// </summary>
        /// <param name="module">Either Bst, Bst Pbo or Wpt or Wpt Pbo</param>
        /// <param name="strLotNo">Lot number</param>
        /// <param name="strMachine">machine</param>
        /// <param name="strEmployee">Employee username (either 4 digit employee number or AD account) that enrolled in CAMSTAR</param>
        /// <param name="strWbSensorSetup">Wip data setup for Sensor</param>
        /// <param name="strCompName">Computer name</param>
        /// <returns>result tag (msg, err)</returns>
        public JsonResult SubmitWipDataValuesSensor(string module, string strLotNo, string strMachine, string strEmployee, string strWbSensorSetup, string strCompName)
        {
            try
            {
                __AdHocWIPDataReq algifxReq = null;
                __algCamstarIfxResponse algifxResp = null;
                string xmltmp = strWbSensorSetup;
                algifxReq = new __AdHocWIPDataReq(xmlConfigPath + "\\" + xmltmp + ".xml");

                // Set the input parameter - programmatically
                algifxReq.setIDComputerName(strCompName);
                algifxReq.setIDEmployee(strEmployee);
                algifxReq.setIDObjectType("Equipment");
                algifxReq.setIDObjectName(strMachine);
                algifxReq.setIDWipDataSetup(xmltmp);


                // setup DCProps container
                __DCPropsDetails dCPropsDetails = new __DCPropsDetails();

                // setup lot props
                __DCProps dCProps = null;
                dCProps = new __DCProps();
                dCProps.setIDDCProps("false", "Lot No.", strLotNo);
                dCPropsDetails.addDCPropsDetails(dCProps);

                var dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        if (module == "wirebondSensorBs")
                        {
                            cmd.CommandText = "[spWipDataValuesWbSensorBs]";
                        }

                        if (module == "wirebondSensorBsPbo")
                        {
                            cmd.CommandText = "[spWipDataValuesWbSensorBsPbo]";
                        }

                        if (module == "wirebondSensorWp")
                        {
                            cmd.CommandText = "[spWipDataValuesWbSensorWp]";
                        }

                        cmd.Parameters.Add("@lotnumber", SqlDbType.NVarChar);
                        cmd.Parameters["@lotnumber"].Value = strLotNo;
                        cmd.Parameters.Add("@machine", SqlDbType.NVarChar);
                        cmd.Parameters["@machine"].Value = strMachine;

                        dt.Load(cmd.ExecuteReader());

                        if (module == "wirebondSensorBs" || module == "wirebondSensorBsPbo")
                        {

                            // setup the x BST props
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName == "BST")
                                    {
                                        //Debug.WriteLine("BST_" + row["RowNo"] + "," + row[column.ColumnName].ToString());

                                        dCProps = new __DCProps();
                                        dCProps.setIDDCProps("false", "BST_" + row["RowNo"], row[column.ColumnName].ToString());
                                        dCPropsDetails.addDCPropsDetails(dCProps);
                                    }
                                }
                            }

                            // setup the x BSFM props
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName == "BSFailureMode")
                                    {
                                        //Debug.WriteLine("BSFM_" + row["RowNo"] + "," + row[column.ColumnName].ToString());

                                        dCProps = new __DCProps();
                                        dCProps.setIDDCProps("false", "BSFM_" + row["RowNo"], row[column.ColumnName].ToString());
                                        dCPropsDetails.addDCPropsDetails(dCProps);
                                    }
                                }
                            }

                        }

                        if (module == "wirebondSensorWp")
                        {
                            // setup the x WPT props
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName == "WPT")
                                    {
                                        //Debug.WriteLine("WPT_" + row["RowNo"] + "," + row[column.ColumnName].ToString());

                                        dCProps = new __DCProps();
                                        dCProps.setIDDCProps("false", "WPT_" + row["RowNo"], row[column.ColumnName].ToString());
                                        dCPropsDetails.addDCPropsDetails(dCProps);
                                    }
                                }
                            }

                            // setup the x WPFM props
                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    if (column.ColumnName == "WPFailureMode")
                                    {
                                        //Debug.WriteLine("WPFM_" + row["RowNo"] + "," + row[column.ColumnName].ToString());

                                        dCProps = new __DCProps();
                                        dCProps.setIDDCProps("false", "WPFM_" + row["RowNo"], row[column.ColumnName].ToString());
                                        dCPropsDetails.addDCPropsDetails(dCProps);
                                    }
                                }
                            }
                        }

                        conn.Close();
                    }

                }

                string reasonForSpc = "";

                if (module == "wirebondSensorBs")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondSensorBs
                                    where a.LotNumber == strLotNo && a.Status == "Saved"
                                    select a.Remarks).First();
                    //Debug.WriteLine(reasonForSpc);
                }

                if (module == "wirebondSensorBsPbo")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondSensorBsPbo
                                    where a.LotNumber == strLotNo && a.Status == "Saved"
                                    select a.Remarks).First();
                }

                if (module == "wirebondSensorWp")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondSensorWp
                                    where a.LotNumber == strLotNo && a.Status == "Saved"
                                    select a.Remarks).First();
                }

                // setup the reason props
                dCProps = new __DCProps();
                dCProps.setIDDCProps("false", "Reason for SPC", reasonForSpc);
                dCPropsDetails.addDCPropsDetails(dCProps);

                // setup the comments
                dCProps = new __DCProps();
                dCProps.setIDDCProps("false", "Comments", "");
                dCPropsDetails.addDCPropsDetails(dCProps);

                // Save the parameter into the packet
                algifxReq.setIDDCPropsDetails(dCPropsDetails);

                // PROCESS THE REQUEST

                algCamstarIfxProcessor ifxPx = new algCamstarIfxProcessor(cserver, 2881);
                ifxPx.Debug = true;
                algifxResp = ifxPx.ProcessRequest(algifxReq);

                // DISPLAY THE RESPONSE
                if (algifxResp.getExceptionData().ErrorFlag)
                {
                    string msg = null;
                    string err = null;
                    err = "";
                    msg = "Error : " + algifxResp.getExceptionData().ErrorDescription;
                    var result = new { msg, err };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string msg = null;
                    string chart = null;

                    __AdHocWIPDataResp ifxResp = (__AdHocWIPDataResp) algifxResp;

                    msg = "Results : " + algifxResp.CompletionMsg;
                    chart = ifxResp.SPCResultFilename;
                    var result = new { msg, chart };

                    System.IO.File.WriteAllText(xmlConfigPath + "\\out\\output.xml", algifxResp.toXML());

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// List of Sensor wirebond machines
        /// </summary>
        /// <returns>list</returns>
        public JsonResult MachineListWbSensor()
        {
            return Json(spcDbContext_.ViewModelWirebondSensorMachines.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Inserts data to the base table
        /// </summary>
        /// <param name="module">Either Bst, Bst Pbo or Wpt or Wpt Pbo</param>
        /// <param name="lotNumber">Lot number</param>
        /// <returns>result</returns>
        public ActionResult UpdateTableWbSensor(string module, string lotNumber)
        {
            string storedProc = "";

            if (module == "wirebondSensorBs")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSensorBs"].ToString();
            }


            if (module == "wirebondSensorBsPbo")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSensorBsPbo"].ToString();
            }


            if (module == "wirebondSensorWp")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSensorWp"].ToString();
            }


            if (module == "wirebondSensorWpPbo")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSensorWpPbo"].ToString();
            }


            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(storedProc, con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@LotNumber", SqlDbType.NVarChar).Value = lotNumber;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return Json("success", JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Wip Data Values that need to paste in CAMSTAR
        /// </summary>
        /// <param name="module">Either Bst, Bst Pbo or Wpt or Wpt Pbo</param>
        /// <param name="strLotNo">Lot number</param>
        /// <param name="strMachine">machine</param>
        /// <returns>Transposed data</returns>
        public ActionResult SPCCalcWirebondSensor_CamstarReading(string module, string strLotNo, string strMachine)
        {
            if (module == "wirebondSensorBs")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondSensorBsWipDataValues>("spWipDataPromptWbSensorBsref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            if (module == "wirebondSensorBsPbo")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondSensorBsPboWipDataValues>("spWipDataPromptWbSensorBsPboref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            if (module == "wirebondSensorWp")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondSensorWpWipDataValues>("spWipDataPromptWbSensorWpref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve Sensor ballshear data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbSensorBs">Lot number</param>
        /// <param name="strMachineWbSensorBs">Machine</param>
        /// <param name="strEmployeeWbSensorBs">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSensorBs_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbSensorBs, string strMachineWbSensorBs, string strEmployeeWbSensorBs)
        {
            var employee = user.GetUserFullname(strEmployeeWbSensorBs);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSensorBs
                             where a.LotNumber == strLotNoWbSensorBs && a.MachineName == strMachineWbSensorBs && new[] { "New", "Saved" }.Contains(a.Status)
                             select new
                             {
                                 GUID = a.GUID,
                                 RowNo = a.RowNo,
                                 LotNumber = a.LotNumber,
                                 Device = a.Device,
                                 DageSerialNo = a.DageSerialNo,
                                 Date = a.Date,
                                 Time = a.Time,
                                 RunNo = a.RunNo,
                                 WireSize = a.WireSize,
                                 MachineName = a.MachineName,
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbSensorBs : a.Machine,
                                 Operator = (a.Operator == "" || a.Operator == null) ? employee : a.Operator,
                                 Remarks = a.Remarks,
                                 BST = a.BST,
                                 BSFailureMode = a.BSFailureMode,
                                 DageProgramBST = a.DageProgramBST,
                                 Status = a.Status,
                                 DateCreated = a.DateCreated,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderByDescending(b => b.Date).ThenByDescending(b => b.Time).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve Sensor ballshear passivation data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbSensorBsPbo">Lot number</param>
        /// <param name="strMachineWbSensorBsPbo">Machine</param>
        /// <param name="strEmployeeWbSensorBsPbo">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSensorBsPbo_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbSensorBsPbo, string strMachineWbSensorBsPbo, string strEmployeeWbSensorBsPbo)
        {
            var employee = user.GetUserFullname(strEmployeeWbSensorBsPbo);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSensorBsPbo
                             where a.LotNumber == strLotNoWbSensorBsPbo && a.MachineName == strMachineWbSensorBsPbo && new[] { "New", "Saved" }.Contains(a.Status)
                             select new
                             {
                                 GUID = a.GUID,
                                 RowNo = a.RowNo,
                                 LotNumber = a.LotNumber,
                                 Device = a.Device,
                                 DageSerialNo = a.DageSerialNo,
                                 Date = a.Date,
                                 Time = a.Time,
                                 RunNo = a.RunNo,
                                 WireSize = a.WireSize,
                                 MachineName = a.MachineName,
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbSensorBsPbo : a.Machine,
                                 Operator = (a.Operator == "" || a.Operator == null) ? employee : a.Operator,
                                 Remarks = a.Remarks,
                                 BST = a.BST,
                                 BSFailureMode = a.BSFailureMode,
                                 DageProgramBST = a.DageProgramBST,
                                 Status = a.Status,
                                 DateCreated = a.DateCreated,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderByDescending(b => b.Date).ThenByDescending(b => b.Time).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve Sensor wirepull data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbSensorWp">Lot number</param>
        /// <param name="strMachineWbSensorWp">Machine</param>
        /// <param name="strEmployeeWbSensorWp">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSensorWp_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbSensorWp, string strMachineWbSensorWp, string strEmployeeWbSensorWp)
        {
            var employee = user.GetUserFullname(strEmployeeWbSensorWp);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSensorWp
                             where a.LotNumber == strLotNoWbSensorWp && a.MachineName == strMachineWbSensorWp && new[] { "New", "Saved" }.Contains(a.Status)
                             select new
                             {
                                 GUID = a.GUID,
                                 RowNo = a.RowNo,
                                 LotNumber = a.LotNumber,
                                 Device = a.Device,
                                 DageSerialNo = a.DageSerialNo,
                                 Date = a.Date,
                                 Time = a.Time,
                                 RunNo = a.RunNo,
                                 WireSize = a.WireSize,
                                 MachineName = a.MachineName,
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbSensorWp : a.Machine,
                                 Operator = (a.Operator == "" || a.Operator == null) ? employee : a.Operator,
                                 Remarks = a.Remarks,
                                 WPT = a.WPT,
                                 WPFailureMode = a.WPFailureMode,
                                 DageProgramWPT = a.DageProgramWPT,
                                 Status = a.Status,
                                 DateCreated = a.DateCreated,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderByDescending(b => b.Date).ThenByDescending(b => b.Time).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve Sensor wirepull passivation data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbSensorWpPbo">Lot number</param>
        /// <param name="strMachineWbSensorWpPbo">MAchine</param>
        /// <param name="strEmployeeWbSensorWpPbo">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSensorWpPbo_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbSensorWpPbo, string strMachineWbSensorWpPbo, string strEmployeeWbSensorWpPbo)
        {
            var employee = user.GetUserFullname(strEmployeeWbSensorWpPbo);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSensorWpPbo
                             where a.LotNumber == strLotNoWbSensorWpPbo && a.MachineName == strMachineWbSensorWpPbo && new[] { "New", "Saved" }.Contains(a.Status)
                             select new
                             {
                                 GUID = a.GUID,
                                 RowNo = a.RowNo,
                                 LotNumber = a.LotNumber,
                                 Device = a.Device,
                                 DageSerialNo = a.DageSerialNo,
                                 Date = a.Date,
                                 Time = a.Time,
                                 RunNo = a.RunNo,
                                 WireSize = a.WireSize,
                                 MachineName = a.MachineName,
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbSensorWpPbo : a.Machine,
                                 Operator = (a.Operator == "" || a.Operator == null) ? employee : a.Operator,
                                 Remarks = a.Remarks,
                                 WPT = a.WPT,
                                 WPFailureMode = a.WPFailureMode,
                                 Status = a.Status,
                                 DateCreated = a.DateCreated,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderByDescending(b => b.Date).ThenByDescending(b => b.Time).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Batch update for SensorBs
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSensorBs_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSensorBs> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSensorBs>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSensorBs
                        {
                            GUID = data.GUID,
                            RowNo = data.RowNo,
                            LotNumber = data.LotNumber,
                            Device = data.Device,
                            DageSerialNo = data.DageSerialNo,
                            Date = data.Date,
                            Time = data.Time,
                            RunNo = data.RunNo,
                            WireSize = data.WireSize,
                            MachineName = data.MachineName,
                            Machine = data.Machine,
                            Operator = data.Operator,
                            Remarks = data.Remarks,
                            BST = data.BST,
                            BSFailureMode = data.BSFailureMode,
                            DageProgramBST = data.DageProgramBST,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            DateCreated = data.DateCreated,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelWirebondSensorBs.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSensorBs", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSensorBs
            {
                GUID = data.GUID,
                RowNo = data.RowNo,
                LotNumber = data.LotNumber,
                Device = data.Device,
                DageSerialNo = data.DageSerialNo,
                Date = data.Date,
                Time = data.Time,
                RunNo = data.RunNo,
                WireSize = data.WireSize,
                MachineName = data.MachineName,
                Machine = data.Machine,
                Operator = data.Operator,
                Remarks = data.Remarks,
                BST = data.BST,
                BSFailureMode = data.BSFailureMode,
                DageProgramBST = data.DageProgramBST,
                Status = data.Status,
                DateUpdated = data.DateUpdated
            }));
        }


        /// <summary>
        /// Batch update for SensorBsPbo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSensorBsPbo_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSensorBsPbo> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSensorBsPbo>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSensorBsPbo
                        {
                            GUID = data.GUID,
                            RowNo = data.RowNo,
                            LotNumber = data.LotNumber,
                            Device = data.Device,
                            DageSerialNo = data.DageSerialNo,
                            Date = data.Date,
                            Time = data.Time,
                            RunNo = data.RunNo,
                            WireSize = data.WireSize,
                            MachineName = data.MachineName,
                            Machine = data.Machine,
                            Operator = data.Operator,
                            Remarks = data.Remarks,
                            BST = data.BST,
                            BSFailureMode = data.BSFailureMode,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            DateCreated = data.DateCreated,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelWirebondSensorBsPbo.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSensorBsPbo", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSensorBsPbo
            {
                GUID = data.GUID,
                RowNo = data.RowNo,
                LotNumber = data.LotNumber,
                Device = data.Device,
                DageSerialNo = data.DageSerialNo,
                Date = data.Date,
                Time = data.Time,
                RunNo = data.RunNo,
                WireSize = data.WireSize,
                MachineName = data.MachineName,
                Machine = data.Machine,
                Operator = data.Operator,
                Remarks = data.Remarks,
                BST = data.BST,
                BSFailureMode = data.BSFailureMode,
                Status = data.Status,
                DateUpdated = data.DateUpdated
            }));
        }

        /// <summary>
        /// Batch update for SensorWp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSensorWp_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSensorWp> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSensorWp>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSensorWp
                        {
                            GUID = data.GUID,
                            RowNo = data.RowNo,
                            LotNumber = data.LotNumber,
                            Device = data.Device,
                            DageSerialNo = data.DageSerialNo,
                            Date = data.Date,
                            Time = data.Time,
                            RunNo = data.RunNo,
                            WireSize = data.WireSize,
                            MachineName = data.MachineName,
                            Machine = data.Machine,
                            Operator = data.Operator,
                            Remarks = data.Remarks,
                            WPT = data.WPT,
                            WPFailureMode = data.WPFailureMode,
                            DageProgramWPT = data.DageProgramWPT,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            DateCreated = data.DateCreated,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelWirebondSensorWp.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSensorWp", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSensorWp
            {
                GUID = data.GUID,
                RowNo = data.RowNo,
                LotNumber = data.LotNumber,
                Device = data.Device,
                DageSerialNo = data.DageSerialNo,
                Date = data.Date,
                Time = data.Time,
                RunNo = data.RunNo,
                WireSize = data.WireSize,
                MachineName = data.MachineName,
                Machine = data.Machine,
                Operator = data.Operator,
                Remarks = data.Remarks,
                WPT = data.WPT,
                WPFailureMode = data.WPFailureMode,
                DageProgramWPT = data.DageProgramWPT,
                Status = data.Status,
                DateUpdated = data.DateUpdated
            }));
        }

        /// <summary>
        /// Batch update for SensorWpPbo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSensorWpPbo_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSensorWpPbo> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSensorWpPbo>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSensorWpPbo
                        {
                            GUID = data.GUID,
                            RowNo = data.RowNo,
                            LotNumber = data.LotNumber,
                            Device = data.Device,
                            DageSerialNo = data.DageSerialNo,
                            Date = data.Date,
                            Time = data.Time,
                            RunNo = data.RunNo,
                            WireSize = data.WireSize,
                            MachineName = data.MachineName,
                            Machine = data.Machine,
                            Operator = data.Operator,
                            Remarks = data.Remarks,
                            WPT = data.WPT,
                            WPFailureMode = data.WPFailureMode,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            DateCreated = data.DateCreated,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelWirebondSensorWpPbo.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSensorWpPbo", lotno);
                }

            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSensorWpPbo
            {
                GUID = data.GUID,
                RowNo = data.RowNo,
                LotNumber = data.LotNumber,
                Device = data.Device,
                DageSerialNo = data.DageSerialNo,
                Date = data.Date,
                Time = data.Time,
                RunNo = data.RunNo,
                WireSize = data.WireSize,
                MachineName = data.MachineName,
                Machine = data.Machine,
                Operator = data.Operator,
                Remarks = data.Remarks,
                WPT = data.WPT,
                WPFailureMode = data.WPFailureMode,
                DageProgramWPT = data.DageProgramWPT,
                Status = data.Status,
                DateUpdated = data.DateUpdated
            }));
        }
    }
}
