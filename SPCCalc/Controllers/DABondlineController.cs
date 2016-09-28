using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPCCalc.Models.DieAttach;
using SPCCalc.Models.User;

namespace SPCCalc.Controllers
{
    public class DABondlineController : Controller
    {
        /*
         * instantiate the objects
         */
        Bondline bondline = new Bondline();
        User user = new User();
        /*
         * method to call the dabondline model for adding new records
         * parameters:
         *   method ->  GET
         *   string ->  _data (that will be converted into array)
         * calls for the AddRecord() inside Sensor model
         */
        [HttpGet]
        public JsonResult AddRecord()
        {
            /*
             * convert string to array
             */
            string[] data = Request["_data"].ToString().Split('|');
            //string Module = Request["_module"].ToString();

            string LotNumber = null;
            string Device = null;
            string Machine = null;
            string A2Operator = null;
            string Remarks = null;

            decimal z1_w_epoxy = 0;
            decimal z2_w_epoxy = 0;
            decimal z3_w_epoxy = 0;

            decimal z1_wo_epoxy = 0;
            decimal z2_wo_epoxy = 0;
            decimal z3_wo_epoxy = 0;

            decimal z_avg_wo = 0;

            decimal blt1 = 0;
            decimal blt2 = 0;
            decimal blt3 = 0;

            DateTime DateCreated = DateTime.Now;

            /*
             * loop through the array
             */
            for (int i = 0; i < data.Length; i++)
            {
                string[] data_chunks = data[i].ToString().Split('^');

                //Module = "da_bondline";

                LotNumber = data_chunks[0].ToString();
                Device = data_chunks[1].ToString();
                Machine = data_chunks[2].ToString();
                A2Operator = data_chunks[3].ToString();
                Remarks = data_chunks[4].ToString();

                z1_w_epoxy = decimal.Parse(data_chunks[5]);
                z2_w_epoxy = decimal.Parse(data_chunks[6]);
                z3_w_epoxy = decimal.Parse(data_chunks[7]);

                z1_wo_epoxy = decimal.Parse(data_chunks[8]);
                z2_wo_epoxy = decimal.Parse(data_chunks[9]);
                z3_wo_epoxy = decimal.Parse(data_chunks[10]);

                z_avg_wo = decimal.Parse(data_chunks[11]);

                blt1 = decimal.Parse(data_chunks[12]);
                blt2 = decimal.Parse(data_chunks[13]);
                blt3 = decimal.Parse(data_chunks[14]);

                bondline.AddRecord(LotNumber, Device, Machine, A2Operator, Remarks, z1_w_epoxy, z2_w_epoxy, z3_w_epoxy, z1_wo_epoxy, z2_wo_epoxy, z3_wo_epoxy, z_avg_wo, blt1, blt2, blt3, DateCreated);
            }

            return Json(data.Length, JsonRequestBehavior.AllowGet);
        }

    }
}
