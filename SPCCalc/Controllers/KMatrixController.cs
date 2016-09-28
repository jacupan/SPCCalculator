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
using System.Data.Objects.SqlClient;
using System.Data;

namespace SPCCalc.Controllers
{
    public class KMatrixController : Controller
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();

        User user = new User();
        Logs logs = new Logs();

        //
        // GET: /KMatrix/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of Sensor machines for Kmatrix
        /// </summary>
        /// <returns>list of machines</returns>
        public JsonResult MachineListMoldKMatrixTOKTKN()
        {
            return Json(spcDbContext_.ViewModelKMatrixMachines.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve KMatrix data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoTOKTKN">Lot number</param>
        /// <param name="strMachineTOKTKN">machine</param>
        /// <param name="strEmployeeTOKTKN">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCCalcMoldKMatrixTOKTKN_Read([DataSourceRequest]DataSourceRequest request, string strLotNoTOKTKN, string strMachineTOKTKN, string strEmployeeTOKTKN)
        {
            var employee = user.GetUserFullname(strEmployeeTOKTKN);

            var spcResult = (from a in spcDbContext_.ViewModelKMatrix
                             where a.LotNumber == strLotNoTOKTKN && new[] { "New", "Saved" }.Contains(a.Status)
                             select new
                             {
                                 GUID = a.GUID,             
                                 RowNo = a.RowNo,
                                 LotNumber = a.LotNumber,
                                 Device = a.Device,
                                 RunNumber = a.RunNumber,
                                 FramePosition = a.FramePosition,
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineTOKTKN : a.Machine,
                                 A2Operator = (a.A2Operator == "" || a.A2Operator == null) ? employee : a.A2Operator,
                                 Remarks = a.Remarks,
                                 PkgWidth = a.PkgWidth,
                                 PkgHeight = a.PkgHeight == null ? (double?)0 : a.PkgHeight,
                                 XPlacement = a.XPlacement,
                                 YPlacement = a.YPlacement,
                                 EPIN = SqlFunctions.StringConvert((double?)a.EPIN, 16, 4),
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
        public ActionResult SPCCalcMoldKmatrixTOKTKN_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelKMatrix> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelKMatrix>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelKMatrix
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
                            PkgWidth = data.PkgWidth,
                            PkgHeight = data.PkgHeight,
                            XPlacement = data.XPlacement,
                            YPlacement = data.YPlacement,
                            EPIN = data.EPIN,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            CreatedBy = data.CreatedBy,
                            DateCreated = data.DateCreated,
                            UpdatedBy = data.UpdatedBy,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelKMatrix.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
                        
                        lotno = data.LotNumber;                       

                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteMoldLogsTable2("moldKMatrix", lotno);
                }              

            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelKMatrix
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
                PkgWidth = data.PkgWidth,
                PkgHeight = data.PkgHeight,
                XPlacement = data.XPlacement,
                YPlacement = data.YPlacement,
                EPIN = data.EPIN,
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
        public ActionResult SPCCalcMoldKMatrix_CamstarReading(string strLotNo)
        {
            var spcResult = spcDbContext_.Database.SqlQuery<ViewModelKMatrixWipDataValues>("spViewDataForCamstarMoldKMatrix @LotNbr", new SqlParameter("@LotNbr", strLotNo)).ToList();

            return Json(spcResult, JsonRequestBehavior.AllowGet);
        }
    }
}
