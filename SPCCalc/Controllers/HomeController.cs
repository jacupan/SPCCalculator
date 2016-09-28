using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using SPCCalc.Models;
using Kendo.Mvc.UI;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SPCCalc.Controllers
{


    public class HomeController : Controller
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();

        public ActionResult Index()
        {
            var computerName = Environment.MachineName.ToString();

            ViewData["computerName"] = computerName;

            return View();
        }


        /*
         * @description     :   for Mold > Sensor (KE_KT_SIP_KA)
         * @author          :   Dev_AC <aabasolo@ALLEGROMICRO.COM>
         * @date            :   JUNE 16, 2016 11:01 AM
         */
        public JsonResult MachineListSensorKEKTSIPKA()
        {
            return Json(spcDbContext_.MachineList_MoldSensor_KEKTSIPKA.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

        /*
         * @description     :   for DieAttach > Bondline
         * @author          :   Dev_AC <aabasolo@ALLEGROMICRO.COM>
         * @date            :   AUGUST 23, 2016 11:04 AM
         */
        public JsonResult MachineListDieAttachBondline()
        {
            return Json(spcDbContext_.MachineList_DieAttach_Bondline.OrderBy(a => a.ResourceName), JsonRequestBehavior.AllowGet);
        }

    }
}
