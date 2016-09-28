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
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SPCCalc.Controllers
{
    public class WirebondSLController : Controller
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();
        string strConn = ConfigurationManager.ConnectionStrings["SPCContext"].ConnectionString;
        public string xmlConfigPath = ConfigurationManager.AppSettings["xmlConfigPath"].ToString();
        public string cserver = ConfigurationManager.AppSettings["cserver"].ToString();

        User user = new User();
        Logs logs = new Logs();

        //
        // GET: /WirebondSL/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of wip data setup available for SL
        /// </summary>
        /// <returns>Filter for SL Bs/Pbo only</returns>
        public JsonResult WipDataSetupSLBs(string strLotNoWbSLBs)
        {
            var lotNo = "";
            lotNo = strLotNoWbSLBs;

            var testGroup = (from a in spcDbContext_.ViewModelWirebondSLBs
                             where a.LotNumber == lotNo
                             select a.DageProgramBST).Distinct().ToList();

            if (testGroup.Count > 1)
            {
                return Json("");
            }
            else if (testGroup.Count == 1)
            {
                var distinct = (from a in spcDbContext_.ViewModelWirebondSLBs
                                where a.LotNumber == lotNo
                                select new
                                {
                                    DageProgramBST = a.DageProgramBST
                                }).FirstOrDefault();

                if (distinct != null)
                {
                    var resultString = Regex.Match(distinct.DageProgramBST, @"\d+").Value;

                    return Json(spcDbContext_.ViewModelSLDataSetup.Where(a => a.SLWBSetups.Contains("bst-" + resultString)).OrderBy(b => b.SLWBSetups), JsonRequestBehavior.AllowGet);

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
        /// List of wip data setup available for SL
        /// </summary>
        /// <returns>Filter for SL Wp only</returns>
        public JsonResult WipDataSetupSLWp(string strLotNoWbSLWp)
        {
            var lotNo = "";
            lotNo = strLotNoWbSLWp;

            var testGroup = (from a in spcDbContext_.ViewModelWirebondSLWp
                             where a.LotNumber == lotNo
                             select a.DageProgramWPT).Distinct().ToList();

            if (testGroup.Count > 1)
            {
                return Json("");
            }
            else if (testGroup.Count == 1)
            {
                var distinct = (from a in spcDbContext_.ViewModelWirebondSLWp
                                where a.LotNumber == lotNo
                                select new
                                {
                                    DageProgramWPT = a.DageProgramWPT
                                }).FirstOrDefault();

                if (distinct != null)
                {
                    var resultString = Regex.Match(distinct.DageProgramWPT, @"\d+").Value;

                    return Json(spcDbContext_.ViewModelSLDataSetup.Where(a => a.SLWBSetups.Contains("wpt-" + resultString)).OrderBy(b => b.SLWBSetups), JsonRequestBehavior.AllowGet);

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
        /// <param name="strWbSLSetup">Wip data setup for SL</param>
        /// <param name="strCompName">Computer name</param>
        /// <returns>result tag (msg, err)</returns>
        public ActionResult SubmitWipDataValuesSL(string module, string strLotNo, string strMachine, string strEmployee, string strWbSLSetup, string strCompName)
        {

            try
            {
                __AdHocWIPDataReq algifxReq = null;
                __algCamstarIfxResponse algifxResp = null;
                string xmltmp = strWbSLSetup;
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
                        if (module == "wirebondSLBs")
                        {
                            cmd.CommandText = "[spWipDataValuesWbSLBs]";
                        }

                        if (module == "wirebondSLWp")
                        {
                            cmd.CommandText = "[spWipDataValuesWbSLWp]";
                        }

                        cmd.Parameters.Add("@lotnumber", SqlDbType.NVarChar);
                        cmd.Parameters["@lotnumber"].Value = strLotNo;
                        cmd.Parameters.Add("@machine", SqlDbType.NVarChar);
                        cmd.Parameters["@machine"].Value = strMachine;

                        dt.Load(cmd.ExecuteReader());

                        if (module == "wirebondSLBs")
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

                        if (module == "wirebondSLWp")
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

                if (module == "wirebondSLBs")
                {
                     reasonForSpc = (from a in spcDbContext_.ViewModelWirebondSLBs
                                        where a.LotNumber == strLotNo && a.Status == "Saved"
                                     select a.Remarks).First();
                    //Debug.WriteLine(reasonForSpc);
                }

                if (module == "wirebondSLWp")
                {
                     reasonForSpc = (from a in spcDbContext_.ViewModelWirebondSLWp
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

                    __AdHocWIPDataResp ifxResp = (__AdHocWIPDataResp)algifxResp;

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
        /// List of SL wirebond machines
        /// </summary>
        /// <returns>list</returns>
        public JsonResult MachineListWbSL()
        {
            return Json(spcDbContext_.ViewModelWirebondSLMachines.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Inserts data to the base table
        /// </summary>
        /// <param name="module">Either Bst, Bst Pbo or Wpt or Wpt Pbo</param>
        /// <param name="lotNumber">Lot number</param>
        /// <returns>result</returns>
        public ActionResult UpdateTableWbSL(string module, string lotNumber)
        {
            string storedProc = "";

            if (module == "wirebondSLBs")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSLBs"].ToString();
            }

            if (module == "wirebondSLWp")
            {
                storedProc = ConfigurationManager.AppSettings["StoredProcWbSLWp"].ToString();
            }

            if (module == "wirebondSLWpPbo")
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
        public ActionResult SPCCalcWirebondSL_CamstarReading(string module, string strLotNo, string strMachine)
        {

            if (module == "wirebondSLBs")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondSLBsWipDataValues>("spWipDataPromptWbSLBsref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            if (module == "wirebondSLWp")
            {
                var spcResult = spcDbContext_.Database.SqlQuery<ViewModelWirebondSLWpWipDataValues>("spWipDataPromptWbSLWpref1 @LotNumber, @Machine", new SqlParameter("@LotNumber", strLotNo), new SqlParameter("@Machine", strMachine)).ToList();

                 return Json(spcResult, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve SL Ballshear data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbSLBs">Lot number</param>
        /// <param name="strMachineWbSLBs">Machine</param>
        /// <param name="strEmployeeWbSLBs">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSLBs_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbSLBs, string strMachineWbSLBs, string strEmployeeWbSLBs)
        {
            var employee = user.GetUserFullname(strEmployeeWbSLBs);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSLBs
                             where a.LotNumber == strLotNoWbSLBs && a.MachineName == strMachineWbSLBs && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbSLBs : a.Machine,
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
        /// Retrieve SL Wirepull data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoWbSLWp">Lot number</param>
        /// <param name="strMachineWbSLWp">Machine</param>
        /// <param name="strEmployeeWbSLWp">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCWBSLWp_Read([DataSourceRequest]DataSourceRequest request, string strLotNoWbSLWp, string strMachineWbSLWp, string strEmployeeWbSLWp)
        {
            var employee = user.GetUserFullname(strEmployeeWbSLWp);

            var spcResult = (from a in spcDbContext_.ViewModelWirebondSLWp
                             where a.LotNumber == strLotNoWbSLWp && a.MachineName == strMachineWbSLWp && new[] { "New", "Saved" }.Contains(a.Status)
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
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineWbSLWp : a.Machine,
                                 Operator = (a.Operator == "" || a.Operator == null) ? employee : a.Operator,
                                 Remarks = a.Remarks,
                                 WPT = a.WPT == null ? (double?)0 : a.WPT,
                                 WPFailureMode = a.WPFailureMode == null ? "0" : a.WPFailureMode,
                                 DageProgramWPT = a.DageProgramWPT,
                                 Status = a.Status,
                                 DateCreated = a.DateCreated,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderByDescending(b => b.Date).ThenByDescending(b => b.Time).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Batch update for SLBs
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSLBs_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSLBs> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSLBs>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSLBs
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
                            MachineName = data.MachineName, // == "" ? strMachineWbSLBs : data.MachineName,
                            Machine = data.Machine,
                            Operator = data.Operator,
                            Remarks = data.Remarks, //== "" ? strRemarksWbSLBs : data.Remarks,
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
                        spcDbContext_.ViewModelWirebondSLBs.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSLBs", lotno);

                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSLBs
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
        /// Batch Update for SLWp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcWbSLWp_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelWirebondSLWp> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelWirebondSLWp>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelWirebondSLWp
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
                        spcDbContext_.ViewModelWirebondSLWp.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteWirebondLogsTable2("wirebondSLWp", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelWirebondSLWp
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
