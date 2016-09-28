using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPCCalc.Models.User;
using SPCCalc.Models;
using SPCCalc.Models.Mold;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace SPCCalc.Controllers
{
    public class SohedController : Controller
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();

        User user = new User();
        Logs logs = new Logs();

        //
        // GET: /Sohed/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of Sohed machines
        /// </summary>
        /// <returns>list of machines</returns>
        public JsonResult MachineListMoldSohed()
        {
            return Json(spcDbContext_.ViewModelSohedMachines.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Insert values to base table, delete data from logfilestable1
        /// </summary>
        /// <param name="lotNumber">Lot number</param>
        /// <param name="framepos">Frame position</param>
        /// <returns>success tag</returns>
        public ActionResult UpdateTableMoldSohed(string lotNumber, string framepos)
        {
            framepos = "";
            string storedProc = ConfigurationManager.AppSettings["StoredProcMoldSohed"].ToString();

            string strConn = ConfigurationManager.ConnectionStrings["SPCContext"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(storedProc, con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@LotNumber", SqlDbType.NVarChar).Value = lotNumber;

                    cmd.Parameters.Add("@FramePosition", SqlDbType.NVarChar).Value = framepos;

                    con.Open();

                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();
                }
            }

            return Json("success", JsonRequestBehavior.AllowGet);

        }
        
        /// <summary>
        /// Retrieve Sohed data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoSohed">Lot number</param>
        /// <param name="strMachineSohed">machine</param>
        /// <param name="strEmployeeSohed">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns></returns>
        public ActionResult SPCCalcMoldSohed_Read([DataSourceRequest]DataSourceRequest request, string strLotNoSohed, string strMachineSohed, string strEmployeeSohed)
        {
            var employee = user.GetUserFullname(strEmployeeSohed);

            var spcResult = (from a in spcDbContext_.ViewModelSohed
                             where a.LotNumber == strLotNoSohed && new[] { "New", "Saved" }.Contains(a.Status)
                             select new
                             {
                                 GUID = a.GUID,              
                                 RowNo = a.RowNo,
                                 LotNumber = a.LotNumber,
                                 Device = a.Device,
                                 RunNumber = a.RunNumber,
                                 FramePosition = a.FramePosition,
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineSohed : a.Machine,
                                 A2Operator = (a.A2Operator == "" || a.A2Operator == null) ? employee : a.A2Operator,
                                 Remarks = a.Remarks,
                                 SSPFO = a.SSPFO,
                                 TBPO = a.TBPO,
                                 PSSM = a.PSSM,
                                 PkgWidth = a.PkgWidth,
                                 PkgThickness = a.PkgThickness,
                                 PTBM = a.PTBM,
                                 PkgHeight = a.PkgHeight,
                                 Status = a.Status,
                                 CreatedBy = a.CreatedBy,
                                 DateCreated = a.DateCreated,
                                 UpdatedBy = a.UpdatedBy,
                                 DateUpdated = a.DateUpdated

                             }).ToList().OrderBy(b => b.DateCreated).ThenBy(b => b.FramePosition).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Batch update
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcMoldSohed_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelSohed> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelSohed>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelSohed
                        {
                            GUID = data.GUID,
                            RowNo = data.RowNo,
                            LotNumber = data.LotNumber,
                            Device = data.Device,
                            RunNumber = data.RunNumber,
                            FramePosition = data.FramePosition,
                            Machine = data.Machine,
                            A2Operator = data.A2Operator,
                            Remarks = data.Remarks,
                            SSPFO = data.SSPFO,
                            TBPO = data.TBPO,
                            PSSM = data.PSSM,
                            PkgWidth = data.PkgWidth,
                            PkgThickness = data.PkgThickness,
                            PTBM = data.PTBM,
                            PkgHeight = data.PkgHeight,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            CreatedBy = data.CreatedBy,
                            DateCreated = data.DateCreated,
                            UpdatedBy = data.UpdatedBy,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelSohed.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;     
                       
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteMoldLogsTable2("moldSohed", lotno);
                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelSohed
            {
                GUID = data.GUID,
                RowNo = data.RowNo,
                LotNumber = data.LotNumber,
                Device = data.Device,
                RunNumber = data.RunNumber,
                FramePosition = data.FramePosition,
                Machine = data.Machine,
                A2Operator = data.A2Operator,
                Remarks = data.Remarks,
                SSPFO = data.SSPFO,
                TBPO = data.TBPO,
                PSSM = data.PSSM,
                PkgWidth = data.PkgWidth,
                PkgThickness = data.PkgThickness,
                PTBM = data.PTBM,
                PkgHeight = data.PkgHeight,
                Status = data.Status,
                CreatedBy = data.CreatedBy,
                DateCreated = data.DateCreated,
                UpdatedBy = data.UpdatedBy,
                DateUpdated = data.DateUpdated
            }));
        }

        /// <summary>
        /// Wip Data Values that need to paste in CAMSTAR
        /// </summary>
        /// <param name="strLotNo">Lot number</param>
        /// <returns>Transposed values</returns>
        public ActionResult SPCCalcMoldSohed_CamstarReading(string strLotNo)
        {
            var spcResult = spcDbContext_.Database.SqlQuery<ViewModelSohedWipDataValues>("spViewDataForCamstarMoldSohed @LotNbr", new SqlParameter("@LotNbr", strLotNo)).ToList();

            return Json(spcResult, JsonRequestBehavior.AllowGet);
        }

    }
}
