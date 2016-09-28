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

namespace SPCCalc.Controllers
{
    public class CurrentSensorController : Controller
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();

        User user = new User();
        Logs logs = new Logs();

        //
        // GET: /CurrentSensor/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// List of CurrentSensor machines
        /// </summary>
        /// <returns>list of machines</returns>
        public JsonResult MachineListMoldCS()
        {
            return Json(spcDbContext_.ViewModelCurrentSensorMachines.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieve CurrentSensor data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoCS">Lot number</param>
        /// <param name="strMachineCS">machine</param>
        /// <param name="strEmployeeCS">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCCalcMoldCS_Read([DataSourceRequest]DataSourceRequest request, string strLotNoCS, string strMachineCS, string strEmployeeCS)
        {
            var employee = user.GetUserFullname(strEmployeeCS);

            var spcResult = (from a in spcDbContext_.ViewModelCurrentSensor
                             where a.LotNumber == strLotNoCS && new[] { "New", "Saved" }.Contains(a.Status)
                             select new
                             {
                                 GUID = a.GUID,                
                                 RowNo = a.RowNo,
                                 LotNumber = a.LotNumber,
                                 Device = a.Device,
                                 RunNumber = a.RunNumber,
                                 FramePosition = a.FramePosition,
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineCS : a.Machine,
                                 A2Operator = (a.A2Operator == "" || a.A2Operator == null) ? employee : a.A2Operator,
                                 Remarks = a.Remarks,
                                 PkgHeight = a.PkgHeight == null ? (double?)0 : a.PkgHeight,
                                 PkgWidth = a.PkgWidth,
                                 PfssMisalign = a.PfssMisalign,
                                 PkgThickness = a.PkgThickness == null ? (double?)0 : a.PkgThickness,
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
        public ActionResult SPCCalcMoldCS_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelCurrentSensor> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelCurrentSensor>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelCurrentSensor
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
                            PkgHeight = data.PkgHeight,
                            PkgWidth = data.PkgWidth,
                            PfssMisalign = data.PfssMisalign,
                            PkgThickness = data.PkgThickness,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            CreatedBy = data.CreatedBy,
                            DateCreated = data.DateCreated,
                            UpdatedBy = data.UpdatedBy,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelCurrentSensor.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);

                        lotno = data.LotNumber;

                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteMoldLogsTable2("moldCurrentSensor", lotno);
                }              

            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelCurrentSensor
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
                PkgHeight = data.PkgHeight,
                PkgWidth = data.PkgWidth,
                PfssMisalign = data.PfssMisalign,
                PkgThickness = data.PkgThickness,
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
        public ActionResult SPCCalcMoldCS_CamstarReading(string strLotNo)
        {
            var spcResult = spcDbContext_.Database.SqlQuery<ViewModelCurrentSensorWipDataValues>("spViewDataForCamstarMoldCurrentSensor @LotNbr", new SqlParameter("@LotNbr", strLotNo)).ToList();

            return Json(spcResult, JsonRequestBehavior.AllowGet);
        }
    }
}
