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
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace SPCCalc.Controllers
{
    public class WirebondSohedController : Controller
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();
        string strConn = ConfigurationManager.ConnectionStrings["SPCContext"].ConnectionString;
        public string xmlConfigPath = ConfigurationManager.AppSettings["xmlConfigPath"].ToString();
        public string cserver = ConfigurationManager.AppSettings["cserver"].ToString();

        User user = new User();
        Logs logs = new Logs();

        //
        // GET: /WirebondSohed/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of wip data setup available for Sohed
        /// </summary>
        /// <returns>Filter for Sohed Bs/Pbo only</returns>
        public JsonResult WipDataSetupSohedBs()
        {
            return Json(spcDbContext_.ViewModelSohedDataSetup.Where(a => a.SOHEDWBSetups.Contains("bst")).OrderBy(b => b.SOHEDWBSetups), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// List of wip data setup available for Sohed
        /// </summary>
        /// <returns>Filter for Sohed Wp only</returns>
        public JsonResult WipDataSetupSohedWp()
        {
            return Json(spcDbContext_.ViewModelSohedDataSetup.Where(a => a.SOHEDWBSetups.Contains("wpt")).OrderBy(b => b.SOHEDWBSetups), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Wip data values automatically send to CAMSTAR
        /// </summary>
        /// <param name="module">Either Bst, Bst Pbo or Wpt or Wpt Pbo</param>
        /// <param name="strLotNo">Lot number</param>
        /// <param name="strMachine">machine</param>
        /// <param name="strEmployee">Employee username (either 4 digit employee number or AD account) that enrolled in CAMSTAR</param>
        /// <param name="strWbSohedSetup">Wip data setup for Sohed</param>
        /// <param name="strCompName">Computer name</param>
        /// <returns>result tag (msg, err)</returns>
        public ActionResult SubmitWipDataValuesSohed(string module, string strLotNo, string strMachine, string strEmployee, string strWbSohedSetup, string strCompName)
        {

            try
            {
                __AdHocWIPDataReq algifxReq = null;
                __algCamstarIfxResponse algifxResp = null;
                string xmltmp = strWbSohedSetup;
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
                        if (module == "wirebondSohedBs")
                        {
                            cmd.CommandText = "[spWipDataValuesWbSohedBs]";
                        }

                        if (module == "wirebondSohedBsPbo")
                        {
                            cmd.CommandText = "[spWipDataValuesWbSohedBsPbo]";
                        }

                        if (module == "wirebondSohedWp")
                        {
                            cmd.CommandText = "[spWipDataValuesWbSohedWp]";
                        }

                        cmd.Parameters.Add("@lotnumber", SqlDbType.NVarChar);
                        cmd.Parameters["@lotnumber"].Value = strLotNo;
                        cmd.Parameters.Add("@machine", SqlDbType.NVarChar);
                        cmd.Parameters["@machine"].Value = strMachine;

                        dt.Load(cmd.ExecuteReader());

                        if (module == "wirebondSohedBs" || module == "wirebondSohedBsPbo")
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

                        if (module == "wirebondSohedWp")
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
                                        Debug.WriteLine("WPFM_" + row["RowNo"] + "," + row[column.ColumnName].ToString());

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

                if (module == "wirebondSohedBs")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondSohedBs
                                    where a.LotNumber == strLotNo && a.Status == "Saved"
                                    select a.Remarks).First();
                    //Debug.WriteLine(reasonForSpc);
                }

                if (module == "wirebondSohedBsPbo")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondSohedBsPbo
                                    where a.LotNumber == strLotNo && a.Status == "Saved"
                                    select a.Remarks).First();
                    //Debug.WriteLine(reasonForSpc);
                }

                if (module == "wirebondSohedWp")
                {
                    reasonForSpc = (from a in spcDbContext_.ViewModelWirebondSohedWp
                                    where a.LotNumber == strLotNo && a.Status == "Saved"
                                    select a.Remarks).First();
                    //Debug.WriteLine(reasonForSpc);
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

                    //return Json(result, "application/json");
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

                    //return Json(result, "application/json");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// List of Sohed wirebond machines
        /// </summary>
        /// <returns>list</returns>
        public JsonResult MachineListWbSohed()
        {
            return Json(spcDbContext_.ViewModelWirebondSohedMachines.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Inserts data to the base table
        /// </summary>
        /// <param name="module">Either Bst, Bst Pbo or Wpt or Wpt Pbo</param>
        /// <param name="lotNumber">Lot number</param>
        /// <returns>result</returns>
        public ActionResult UpdateTableWbSohed(string module, string lotNumber)
        {
            string storedProc = "";

            if (module == "wirebondSohedBs")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSohedBs"].ToString();
            }


            if (module == "wirebondSohedBsPbo")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSohedBsPbo"].ToString();
            }


            if (module == "wirebondSohedWp")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSohedWp"].ToString();
            }


            if (module == "wirebondSohedWpPbo")
            {

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
        public ActionResult SPCCalcWirebondSohed_CamstarReading(string module, string strLotNo, string strMachine)
        {

            if (module == "wirebondSohedBs")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondSohedBsWipDataValues>("spWipDataPromptWbSohedBsref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            if (module == "wirebondSohedBsPbo")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondSohedBsPboWipDataValues>("spWipDataPromptWbSohedBsPboref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            if (module == "wirebondSohedWp")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondSohedWpWipDataValues>("spWipDataPromptWbSohedWpref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve Sohed Ballshear data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoSohedBs">Lot number</param>
        /// <param name="strMachineSohedBs">Machine</param>
        /// <param name="strEmployeeSohedBs">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSohedBs_Read([DataSourceRequest]DataSourceRequest request, string strLotNoSohedBs, string strMachineSohedBs, string strEmployeeSohedBs)
        {
            var employee = user.GetUserFullname(strEmployeeSohedBs);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSohedBs
                             where a.LotNumber == strLotNoSohedBs && a.MachineName == strMachineSohedBs && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineSohedBs : a.Machine,
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
        /// Retrieve Sohed Ballshear Passivation data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoSohedBsPbo">Lot number</param>
        /// <param name="strMachineSohedBsPbo">MAchine</param>
        /// <param name="strEmployeeSohedBsPbo">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSohedBsPbo_Read([DataSourceRequest]DataSourceRequest request, string strLotNoSohedBsPbo, string strMachineSohedBsPbo, string strEmployeeSohedBsPbo)
        {
            var employee = user.GetUserFullname(strEmployeeSohedBsPbo);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSohedBsPbo
                             where a.LotNumber == strLotNoSohedBsPbo && a.MachineName == strMachineSohedBsPbo && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineSohedBsPbo : a.Machine,
                                 Operator = (a.Operator == "" || a.Operator == null) ? employee : a.Operator,
                                 Remarks = a.Remarks,
                                 BST = a.BST,
                                 BSFailureMode = a.BSFailureMode,
                                 Status = a.Status,
                                 DateCreated = a.DateCreated,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderByDescending(b => b.Date).ThenByDescending(b => b.Time).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve Sohed Wirepull data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoSohedWp">Lot number</param>
        /// <param name="strMachineSohedWp">Machine</param>
        /// <param name="strEmployeeSohedWp">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSohedWp_Read([DataSourceRequest]DataSourceRequest request, string strLotNoSohedWp, string strMachineSohedWp, string strEmployeeSohedWp)
        {
            var employee = user.GetUserFullname(strEmployeeSohedWp);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSohedWp
                             where a.LotNumber == strLotNoSohedWp && a.MachineName == strMachineSohedWp && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineSohedWp : a.Machine,
                                 Operator = (a.Operator == "" || a.Operator == null) ? employee : a.Operator,
                                 Remarks = a.Remarks,
                                 WPT = a.WPT == null ? (double?) 0 : a.WPT,
                                 WPFailureMode = a.WPFailureMode == null ? "0" : a.WPFailureMode,
                                 DageProgramWPT = a.DageProgramWPT,
                                 Status = a.Status,
                                 DateCreated = a.DateCreated,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderByDescending(b => b.Date).ThenByDescending(b => b.Time).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve Sohed Wirepull Passivation data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoSohedWpPbo">Lot number</param>
        /// <param name="strMachineSohedWpPbo">Machine</param>
        /// <param name="strEmployeeSohedWpPbo">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSohedWpPbo_Read([DataSourceRequest]DataSourceRequest request, string strLotNoSohedWpPbo, string strMachineSohedWpPbo, string strEmployeeSohedWpPbo)
        {
            var employee = user.GetUserFullname(strEmployeeSohedWpPbo);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSohedWpPbo
                             where a.LotNumber == strLotNoSohedWpPbo && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineSohedWpPbo : a.Machine,
                                 Operator = (a.Operator == "" || a.Operator == null) ? employee : a.Operator,
                                 Remarks = a.Remarks,
                                 WPT = a.WPT == null ? (double?) 0 : a.WPT,
                                 WPFailureMode = a.WPFailureMode == null ? "0" : a.WPFailureMode,
                                 Status = a.Status,
                                 DateCreated = a.DateCreated,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderByDescending(b => b.Date).ThenByDescending(b => b.Time).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Batch update for SohedBs
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSohedBs_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSohedBs> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSohedBs>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSohedBs
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
                            MachineName = data.MachineName, // == "" ? strMachineSohedBs : data.MachineName,
                            Machine = data.Machine,
                            Operator = data.Operator,
                            Remarks = data.Remarks, //== "" ? strRemarksWbSohedBs : data.Remarks,
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
                        spcDbContext_.ViewModelWirebondSohedBs.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSohedBs", lotno);

                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSohedBs
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
        /// Batch update for SohedBsPbo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSohedBsPbo_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSohedBsPbo> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSohedBsPbo>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSohedBsPbo
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
                            MachineName = data.MachineName, // == "" ? strMachineSohedBs : data.MachineName,
                            Machine = data.Machine,
                            Operator = data.Operator,
                            Remarks = data.Remarks, //== "" ? strRemarksWbSohedBs : data.Remarks,
                            BST = data.BST,
                            BSFailureMode = data.BSFailureMode,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            DateCreated = data.DateCreated,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelWirebondSohedBsPbo.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSohedBsPbo", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSohedBsPbo
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
        /// Batch Update for SohedWp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSohedWp_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSohedWp> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSohedWp>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSohedWp
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
                        spcDbContext_.ViewModelWirebondSohedWp.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSohedWp", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSohedWp
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
        /// Barch update for SohedWpPbo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSohedWpPbo_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSohedWpPbo> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSohedWpPbo>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSohedWpPbo
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
                        spcDbContext_.ViewModelWirebondSohedWpPbo.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSohedWpPbo", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSohedWpPbo
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
                Status = data.Status,
                DateUpdated = data.DateUpdated
            }));
        }
    }
}
