﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPCCalc.Models.Mold;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using SPCCalc.Models;
using SPCCalc.Models.User;
using System.Data.Objects.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace SPCCalc.Controllers
{
    public class Gts2Controller : Controller
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();

        User user = new User();
        Logs logs = new Logs();

        //
        // GET: /Gts2/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  Retrieve Gts2 data with "NEW" or "SAVED" status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="strLotNoGts2">Lot number</param>
        /// <param name="strMachineGts2">Machine</param>
        /// <param name="strEmployeeGts2">Employee username (either 4 digit employee number or AD account)</param>
        /// <returns>table</returns>
        public ActionResult SPCCalcMoldGts2_Read([DataSourceRequest]DataSourceRequest request, string strLotNoGts2, string strMachineGts2, string strEmployeeGts2)
        {
            var employee = user.GetUserFullname(strEmployeeGts2);

            var spcResult = (from a in spcDbContext_.ViewModelGts2
                             where a.LotNumber == strLotNoGts2 && new[] { "New", "Saved" }.Contains(a.Status)
                             select new
                             {
                                 GUID = a.GUID,                 
                                 RowNo = a.RowNo,
                                 LotNumber = a.LotNumber,
                                 Device = a.Device,
                                 RunNumber = a.RunNumber,
                                 FramePosition = a.FramePosition,
                                 Machine = (a.Machine == "" || a.Machine == null) ? strMachineGts2 : a.Machine,
                                 A2Operator = (a.A2Operator == "" || a.A2Operator == null) ? employee : a.A2Operator,
                                 Remarks = a.Remarks,
                                 PkgThickness = SqlFunctions.StringConvert((double?)a.PkgThickness, 16, 4), 
                                 PkgWidth = a.PkgWidth == null ? (double?)0 : a.PkgWidth,
                                 PkgHeight = a.PkgHeight == null ? (double?)0 : a.PkgHeight,
                                 AValue = a.AValue,
                                 DXValue = a.DXValue,
                                 SSPM = a.SSPM,
                                 DYValue = a.DYValue,
                                 TBPM = a.TBPM,
                                 EPIN = SqlFunctions.StringConvert((double?)a.EPIN, 16, 4),
                                 Status = a.Status,
                                 CreatedBy = a.CreatedBy,
                                 DateCreated = a.DateCreated,
                                 UpdatedBy = a.UpdatedBy,
                                 DateUpdated = a.DateUpdated
                             }
                                ).ToList().OrderBy(b => b.DateCreated).ThenBy(b => b.FramePosition).ThenBy(b => int.Parse(b.RowNo));
            return Json(spcResult.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Batch update
        /// </summary>
        /// <param name="request"></param>
        /// <param name="calculating"></param>
        /// <returns>Updated table, reflected asap on the database</returns>
        public ActionResult SPCCalcMoldGts2_Update([DataSourceRequest]DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ViewModelGts2> calculating)
        {
            string lotno = "";
            var entities = new List<ViewModelGts2>();
            if (ModelState.IsValid)
            {
                using (spcDbContext_)
                {
                    foreach (var data in calculating)
                    {
                        // Create a new Product entity and set its properties from the posted ProductViewModel
                        var entity = new ViewModelGts2
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
                            PkgThickness = data.PkgThickness,
                            PkgWidth = data.PkgWidth,
                            PkgHeight = data.PkgHeight,
                            AValue = data.AValue,
                            DXValue = data.DXValue,
                            SSPM = data.SSPM,
                            DYValue = data.DYValue,
                            TBPM = data.TBPM,
                            EPIN = data.EPIN,
                            Status = data.Status == "New" ? "Saved" : data.Status,
                            CreatedBy = data.CreatedBy,
                            DateCreated = data.DateCreated,
                            UpdatedBy = data.A2Operator,
                            DateUpdated = DateTime.Now // By default get the current date and time value
                        };
                        // Store the entity for later use
                        entities.Add(entity);
                        // Attach the entity
                        spcDbContext_.ViewModelGts2.Attach(entity);
                        // Change its state to Modified so Entity Framework can update the existing product instead of creating a new one
                        spcDbContext_.Entry(entity).State = EntityState.Modified;
                        // Or use ObjectStateManager if using a previous version of Entity Framework
                        // northwind.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);         

                        lotno = data.LotNumber;
                    }
                    // Update the entities in the database
                    spcDbContext_.SaveChanges();

                    logs.DeleteMoldLogsTable2("moldGts2", lotno);

                }


            }
            // Return the updated entities. Also return any validation errors.
            return Json(entities.ToDataSourceResult(request, ModelState, data => new ViewModelGts2
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
                PkgThickness = data.PkgThickness,
                PkgWidth = data.PkgWidth,
                PkgHeight = data.PkgHeight,
                AValue = data.AValue,
                DXValue = data.DXValue,
                SSPM = (data.AValue - data.DXValue),
                DYValue = data.DYValue,
                TBPM = data.TBPM,
                EPIN = data.EPIN,
                Status = data.Status,
                CreatedBy = data.CreatedBy,
                DateCreated = data.DateCreated,
                UpdatedBy = data.A2Operator,
                DateUpdated = data.DateUpdated
            }));
        }

        /// <summary>
        /// Wip Data Values that need to paste in CAMSTAR
        /// </summary>
        /// <param name="strLotNo">Lot number</param>
        /// <returns>Transposed values</returns>
        public ActionResult SPCCalcMoldGts2_CamstarReading(string strLotNo)
        {
            var spcResult = spcDbContext_.Database.SqlQuery<ViewModelGts2WipDataValues>("spViewDataForCamstarMoldGts2 @LotNbr", new SqlParameter("@LotNbr", strLotNo)).ToList();

            return Json(spcResult, JsonRequestBehavior.AllowGet);
        }

    }
}
