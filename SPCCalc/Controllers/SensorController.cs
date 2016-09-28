using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPCCalc.Models;
using System.Configuration;
using System.Data.SqlClient;
using SPCCalc.Models.Sensor;
using SPCCalc.Models.User;

/*
 * @author      :   Dev Alvin (aabasolo@ALLEGROMICRO.COM)
 * @date        :   2016-06-24 9:43AM
 * @description :   controller for the 3 sensor modules
 */

namespace SPCCalc.Controllers
{
    public class SensorController : Controller
    {

        /*
         * instantiate the objects
         */
        Sensor sensor = new Sensor();
        User user = new User();

        /*
         * method to return the device#
         * parameters:
         *   method ->  GET
         *   string ->  Lot number
         * calls for stored procedure spDevice
         */
        [HttpGet]
        public JsonResult GetDevice()
        {
            SqlDataReader rdr = null;

            string DevId = "";
            string storedProc = ConfigurationManager.AppSettings["StoredProcGetDevice"].ToString();
            string strConn = ConfigurationManager.ConnectionStrings["SPCContext"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(storedProc, con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@LotNumber", SqlDbType.NVarChar).Value = Request["LotNbr"].ToString();
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        DevId = rdr["Device"].ToString();
                    }
                }
            }

            return Json(DevId, JsonRequestBehavior.AllowGet);
        }

        /*
         * method to call the sensor model for adding new records
         * parameters:
         *   method ->  GET
         *   string ->  _data (that will be converted into array)
         *   string ->  _module (e.g. sensor_kektsipka,ll,epin)
         * calls for the AddRecord() inside Sensor model
         */
        [HttpGet]
        public JsonResult AddRecord()
        {
            /*
             * convert string to array
             */
            string[] data = Request["_data"].ToString().Split('|');
            string Module = Request["_module"].ToString();

            string LotNumber = null;
            string Device = null;
            string FramePosition = null;
            string Machine = null;
            string A2Operator = null;
            string Remarks = null;
            decimal HEDA = 0;
            decimal HEDB = 0;
            decimal HEDC = 0;
            decimal PkgWidth = 0;
            decimal PkgHeight = 0;
            decimal EPIN = 0;
            string Status = null;
            DateTime DateCreated = DateTime.Now;
            string PackageGroup = "";

            /*
             * loop through the array
             */
            for (int i = 0; i < data.Length; i++)
            {
                string[] data_chunks = data[i].ToString().Split('^');

                if (Module == "sensor_kektsipka" || Module == "kektsipka")
                {
                    Module = "kektsipka";

                    LotNumber = data_chunks[0].ToString();
                    Device = data_chunks[1].ToString();
                    FramePosition = data_chunks[2].ToString();
                    Machine = data_chunks[3].ToString();
                    A2Operator = data_chunks[4].ToString();
                    Remarks = data_chunks[5].ToString();
                    HEDA = decimal.Parse(data_chunks[6]);
                    HEDB = decimal.Parse(data_chunks[7]);
                    HEDC = decimal.Parse(data_chunks[8]);
                    PkgWidth = decimal.Parse(data_chunks[9]);
                    PkgHeight = decimal.Parse(data_chunks[10]);
                    EPIN = decimal.Parse(data_chunks[11]);
                    Status = data_chunks[12].ToString();
                    PackageGroup = data_chunks[13].ToString();
                }
                else if (Module == "ll")
                {
                    Module = "ll";

                    LotNumber = data_chunks[0].ToString();
                    Device = data_chunks[1].ToString();
                    FramePosition = data_chunks[2].ToString();
                    Machine = data_chunks[3].ToString();
                    A2Operator = data_chunks[4].ToString();
                    Remarks = data_chunks[5].ToString();
                    HEDA = decimal.Parse(data_chunks[6]);
                    HEDB = decimal.Parse(data_chunks[7]);
                    HEDC = decimal.Parse(data_chunks[8]);
                    EPIN = decimal.Parse(data_chunks[9]);
                    Status = data_chunks[10].ToString();
                    PackageGroup = data_chunks[11].ToString();
                }
                else if (Module == "epin")
                {
                    Module = "epin";

                    LotNumber = data_chunks[0].ToString();
                    Device = data_chunks[1].ToString();
                    FramePosition = data_chunks[2].ToString();
                    Machine = data_chunks[3].ToString();
                    A2Operator = data_chunks[4].ToString();
                    //Remarks = data_chunks[5].ToString();
                    //EPIN = decimal.Parse(data_chunks[6]);
                    //Status = data_chunks[7].ToString();
                    Remarks = "";
                    EPIN = decimal.Parse(data_chunks[5]);
                    Status = data_chunks[6].ToString();
                }

                sensor.AddRecord(Module, LotNumber, Device, FramePosition, Machine, A2Operator, Remarks, HEDA, HEDB, HEDC, PkgWidth, PkgHeight, EPIN, Status, DateCreated, PackageGroup);
            }

            return Json(data.Length, JsonRequestBehavior.AllowGet);
        }

        /*
         * method to call the sensor model for viewing records
         * parameters:
         *   method ->  GET
         *   string ->  LotNbr
         *   string ->  Module (e.g. sensor_kektsipka,ll,epin)
         * calls for the viewDataForCamstar() inside Sensor model
         */
        [HttpGet]
        public JsonResult GetDataForCamstarRecording()
        {
            string result = sensor.viewDataForCamstar(Request["LotNbr"].ToString(), Request["Module"].ToString(), Request["PackageGroup"].ToString());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /*
         * method to call the user model for validating userid
         * parameters:
         *   method ->  GET
         *   string ->  LotNbr
         *   string ->  Module (e.g. sensor_kektsipka,ll,epin)
         * calls for the viewDataForCamstar() inside Sensor model
         */
        [HttpGet]
        public JsonResult validateLotAndUser()
        {
            string[] result = sensor.validateLotAndUser(Request["LotNbr"].ToString(), Request["Username"].ToString());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}