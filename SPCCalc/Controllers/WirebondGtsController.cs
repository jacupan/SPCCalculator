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
    public class WirebondGtsController : Controller
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();
        string strConn = ConfigurationManager.ConnectionStrings["SPCContext"].ConnectionString;
        public string xmlConfigPath = ConfigurationManager.AppSettings["xmlConfigPath"].ToString();
        public string cserver = ConfigurationManager.AppSettings["cserver"].ToString();

        User user = new User();
        Logs logs = new Logs();

        //
        // GET: /WirebondGts/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of wip data setup available for Gts
        /// </summary>
        /// <param name="strLotNoWbGtsBs">Lot number if GtsBs</param>
        /// <param name="strLotNoWbGtsBsPbo">Lot number if GtsBsPbo</param>
        /// <returns>Filter for Gts Bs/Pbo only (3W, 4W, 5W)</returns>
        public JsonResult WipDataSetupGtsBs(string strLotNoWbGtsBs, string strLotNoWbGtsBsPbo)
        {
            var lotNo = "";

            if (strLotNoWbGtsBs != null && strLotNoWbGtsBs != "")
            {
                lotNo = strLotNoWbGtsBs;
            }
            else
            {
                lotNo = strLotNoWbGtsBsPbo;
            }

            var testGroup = (from a in spcDbContext_.ViewModelWirebondGtsBs
                             where a.LotNumber == lotNo
                             select a.DageProgramBST).Distinct().ToList();

            if (testGroup == null || testGroup.Count == 0)
            {
                testGroup = (from a in spcDbContext_.ViewModelWirebondGtsBsPbo
                             where a.LotNumber == lotNo
                             select a.DageProgramBST).Distinct().ToList();
            }

            if (testGroup.Count > 1)
            {
                return Json("");
            }
            else if (testGroup.Count == 1)
            {
                var distinct = (from a in spcDbContext_.ViewModelWirebondGtsBs
                                where a.LotNumber == lotNo
                                select new
                                {
                                    DageProgramBST = a.DageProgramBST
                                }).FirstOrDefault();

                if (distinct == null)
                {
                    distinct = (from a in spcDbContext_.ViewModelWirebondGtsBsPbo
                                where a.LotNumber == lotNo
                                select new
                                {
                                    DageProgramBST = a.DageProgramBST
                                }).FirstOrDefault();
                }

                if (distinct != null)
                {
                    var resultString = Regex.Match(distinct.DageProgramBST, @"\d+").Value;

                    return Json(spcDbContext_.ViewModelGtsDataSetup.Where(a => a.GTSWBSetups.Contains("bst-" + resultString)).OrderBy(b => b.GTSWBSetups), JsonRequestBehavior.AllowGet);

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

        /// <summary>
        /// List of wip data setup available for Gts
        /// </summary>
        /// <param name="strLotNoWbGtsWp">Lot number if GtsWp</param>
        /// <returns>Filter for Gts Wp only (3W, 4W, 5W)</returns>
        public JsonResult WipDataSetupGtsWp(string strLotNoWbGtsWp)
        {
            var lotNo = "";
            lotNo = strLotNoWbGtsWp;

            var testGroup = (from a in spcDbContext_.ViewModelWirebondGtsWp
                             where a.LotNumber == lotNo
                             select a.DageProgramWPT).Distinct().ToList();

            if (testGroup.Count > 1)
            {
                return Json("");
            }
            else if (testGroup.Count == 1)
            {
                var distinct = (from a in spcDbContext_.ViewModelWirebondGtsWp
                                where a.LotNumber == lotNo
                                select new
                                {
                                    DageProgramWPT = a.DageProgramWPT
                                }).FirstOrDefault();

                if (distinct != null)
                {
                    var resultString = Regex.Match(distinct.DageProgramWPT, @"\d+").Value;

                    return Json(spcDbContext_.ViewModelGtsDataSetup.Where(a => a.GTSWBSetups.Contains("wpt-" + resultString)).OrderBy(b => b.GTSWBSetups), JsonRequestBehavior.AllowGet);

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

        /// <summary>
        /// Wip data values automatically send to CAMSTAR
        /// </summary>
        /// <param name="module">Either Bst, Bst Pbo or Wpt or Wpt Pbo</param>
        /// <param name="strLotNo">Lot number</param>
        /// <param name="strMachine">machine</param>
        /// <param name="strEmployee">Employee username (either 4 digit employee number or AD account) that enrolled in CAMSTAR</param>
        /// <param name="strWbGtsSetup">Wip data setup for Gts</param>
        /// <param name="strCompName">Computer name</param>
        /// <returns>result tag (msg, err)</returns>
        public JsonResult SubmitWipDataValuesGts(string module, string strLotNo, string strMachine, string strEmployee, string strWbGtsSetup, string strCompName)
        {
            try
            {
                __AdHocWIPDataReq algifxReq = null;
                __algCamstarIfxResponse algifxResp = null;
                string xmltmp = strWbGtsSetup;
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

                        if (module == "wirebondGtsBs")
                        {
                            cmd.CommandText = "[spWipDataValuesWbGtsBs]";
                        }

                        if (module == "wirebondGtsBsPbo")
                        {
                            cmd.CommandText = "[spWipDataValuesWbGtsBsPbo]";
                        }

                        if (module == "wirebondGtsWp")
                        {
                            cmd.CommandText = "[spWipDataValuesWbGtsWp]";
                        }

                        cmd.Parameters.Add("@lotnumber", SqlDbType.NVarChar);
                        cmd.Parameters["@lotnumber"].Value = strLotNo;
                        cmd.Parameters.Add("@machine", SqlDbType.NVarChar);
                        cmd.Parameters["@machine"].Value = strMachine;

                        dt.Load(cmd.ExecuteReader());

                        if (module == "wirebondGtsBs" || module == "wirebondGtsBsPbo")
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

                        if (module == "wirebondGtsWp")
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

                if (module == "wirebondGtsBs")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondGtsBs
                                    where a.LotNumber == strLotNo && a.Status == "Saved"
                                    select a.Remarks).First();
                    //Debug.WriteLine(reasonForSpc);
                }

                if (module == "wirebondGtsBsPbo")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondGtsBsPbo
                                    where a.LotNumber == strLotNo && a.Status == "Saved"
                                    select a.Remarks).First();
                }

                if (module == "wirebondGtsWp")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondGtsWp
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

                    __AdHocWIPDataResp ifxResp = (__AdHocWIPDataResp)algifxResp;

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
        /// List of Gts wirebond machines
        /// </summary>
        /// <returns>list</returns>
        public JsonResult MachineListWbGts()
        {
            return Json(spcDbContext_.ViewModelWirebondGtsMachines.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Inserts data to the base table
        /// </summary>
        /// <param name="module">Either Bst, Bst Pbo or Wpt or Wpt Pbo</param>
        /// <param name="lotNumber">Lot number</param>
        /// <returns>result</returns>
        public ActionResult UpdateTableWbGts(string module, string lotNumber)
        {
            string storedProc = "";

            if (module == "wirebondGtsBs")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbGtsBs"].ToString();
            }


            if (module == "wirebondGtsBsPbo")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbGtsBsPbo"].ToString();
            }


            if (module == "wirebondGtsWp")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbGtsWp"].ToString();
            }


            if (module == "wirebondGtsWpPbo")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbGtsWpPbo"].ToString();
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
        public ActionResult SPCCalcWirebondGts_CamstarReading(string module, string strLotNo, string strMachine)
        {
            if (module == "wirebondGtsBs")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondGtsBsWipDataValues>("spWipDataPromptWbGtsBsref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            if (module == "wirebondGtsBsPbo")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondGtsBsPboWipDataValues>("spWipDataPromptWbGtsBsPboref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            if (module == "wirebondGtsWp")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondGtsWpWipDataValues>("spWipDataPromptWbGtsWpref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve Gts ballshear data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbGtsBs">Lot number</param>
        /// <param name="strMachineWbGtsBs">Machine</param>
        /// <param name="strEmployeeWbGtsBs">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBGtsBs_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbGtsBs, string strMachineWbGtsBs, string strEmployeeWbGtsBs)
        {
            var employee = user.GetUserFullname(strEmployeeWbGtsBs);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondGtsBs
                             where a.LotNumber == strLotNoWbGtsBs && a.MachineName == strMachineWbGtsBs && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbGtsBs : a.Machine,
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
        /// Retrieve Gts ballshear passivation data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbGtsBsPbo">Lot number</param>
        /// <param name="strMachineWbGtsBsPbo">Machine</param>
        /// <param name="strEmployeeWbGtsBsPbo">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBGtsBsPbo_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbGtsBsPbo, string strMachineWbGtsBsPbo, string strEmployeeWbGtsBsPbo)
        {
            var employee = user.GetUserFullname(strEmployeeWbGtsBsPbo);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondGtsBsPbo
                             where a.LotNumber == strLotNoWbGtsBsPbo && a.MachineName == strMachineWbGtsBsPbo && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbGtsBsPbo : a.Machine,
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
        /// Retrieve Gts wirepull data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbGtsWp">Lot number</param>
        /// <param name="strMachineWbGtsWp">Machine</param>
        /// <param name="strEmployeeWbGtsWp">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBGtsWp_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbGtsWp, string strMachineWbGtsWp, string strEmployeeWbGtsWp)
        {
            var employee = user.GetUserFullname(strEmployeeWbGtsWp);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondGtsWp
                             where a.LotNumber == strLotNoWbGtsWp && a.MachineName == strMachineWbGtsWp && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbGtsWp : a.Machine,
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
        /// Retrieve Gts wirepull passivation data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbGtsWpPbo">Lot number</param>
        /// <param name="strMachineWbGtsWpPbo">MAchine</param>
        /// <param name="strEmployeeWbGtsWpPbo">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBGtsWpPbo_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbGtsWpPbo, string strMachineWbGtsWpPbo, string strEmployeeWbGtsWpPbo)
        {
            var employee = user.GetUserFullname(strEmployeeWbGtsWpPbo);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondGtsWpPbo
                             where a.LotNumber == strLotNoWbGtsWpPbo && a.MachineName == strMachineWbGtsWpPbo && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbGtsWpPbo : a.Machine,
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
        /// Batch update for GtsBs
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbGtsBs_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondGtsBs> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondGtsBs>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondGtsBs
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
                        spcDbContext_.ViewModelWirebondGtsBs.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondGtsBs", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondGtsBs
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
        /// Batch update for GtsBsPbo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbGtsBsPbo_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondGtsBsPbo> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondGtsBsPbo>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondGtsBsPbo
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
                        spcDbContext_.ViewModelWirebondGtsBsPbo.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondGtsBsPbo", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondGtsBsPbo
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
        /// Batch update for GtsWp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbGtsWp_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondGtsWp> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondGtsWp>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondGtsWp
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
                        spcDbContext_.ViewModelWirebondGtsWp.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondGtsWp", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondGtsWp
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
        /// Batch update for GtsWpPbo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbGtsWpPbo_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondGtsWpPbo> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondGtsWpPbo>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondGtsWpPbo
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
                        spcDbContext_.ViewModelWirebondGtsWpPbo.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondGtsWpPbo", lotno);
                }

            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondGtsWpPbo
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
