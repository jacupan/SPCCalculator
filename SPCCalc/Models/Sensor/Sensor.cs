using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

/*
 * @author      :   Dev Alvin (aabasolo@ALLEGROMICRO.COM)
 * @date        :   2016-06-24 9:43AM
 * @description :   model for Sensor
 */

namespace SPCCalc.Models.Sensor
{
    public class Sensor
    {

        /*
         * initialize connection
         */
        string strConn = ConfigurationManager.ConnectionStrings["SPCContext"].ConnectionString;
        SqlDataReader rdr = null;

        /*
         * method to add record to the database
         * parameters:
         *   method ->  GET
         *   string ->  Module
         *   string ->  LotNumber
         *   string ->  Device
         *   string ->  FramePosition
         *   string ->  Machine
         *   string ->  A2Operator
         *   string ->  Remarks
         *   decimal ->  HEDA
         *   decimal ->  HEDB
         *   decimal ->  HEDC
         *   decimal ->  PkgWidth
         *   decimal ->  PkgHeight
         *   decimal ->  EPIN
         *   string ->  Status
         *   datetime ->  DateCreated
         *   string ->  PackageGroup
         * calls for stored procedure spAddMoldSensor
         */
        public bool AddRecord(string Module, string LotNumber, string Device, string FramePosition, string Machine, string A2Operator, string Remarks, decimal HEDA, decimal HEDB, decimal HEDC, decimal PkgWidth, decimal PkgHeight, decimal EPIN, string Status, DateTime DateCreated, string PackageGroup)
        {
            string storedProc = ConfigurationManager.AppSettings["spAddMoldSensor"].ToString();
            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(storedProc, con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Module", SqlDbType.NVarChar).Value = Module;
                    cmd.Parameters.Add("@LotNumber", SqlDbType.NVarChar).Value = LotNumber;
                    cmd.Parameters.Add("@Device", SqlDbType.NVarChar).Value = Device;
                    cmd.Parameters.Add("@FramePosition", SqlDbType.NVarChar).Value = FramePosition;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar).Value = Machine;
                    cmd.Parameters.Add("@A2Operator", SqlDbType.NVarChar).Value = A2Operator;
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;
                    cmd.Parameters.Add("@HEDA", SqlDbType.Decimal).Value = HEDA;
                    cmd.Parameters.Add("@HEDB", SqlDbType.Decimal).Value = HEDB;
                    cmd.Parameters.Add("@HEDC", SqlDbType.Decimal).Value = HEDC;
                    cmd.Parameters.Add("@PkgWidth", SqlDbType.Decimal).Value = PkgWidth;
                    cmd.Parameters.Add("@PkgHeight", SqlDbType.Decimal).Value = PkgHeight;
                    cmd.Parameters.Add("@EPIN", SqlDbType.Decimal).Value = EPIN;
                    cmd.Parameters.Add("@Stat", SqlDbType.NVarChar).Value = Status;
                    cmd.Parameters.Add("@DateCreated", SqlDbType.NVarChar).Value = DateCreated;
                    cmd.Parameters.Add("@PackageGroup", SqlDbType.NVarChar).Value = PackageGroup;

                    cmd.CommandTimeout = 0;

                    int res = cmd.ExecuteNonQuery();
                    if (res == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        /*
         * method to view data
         * parameters:
         *   string ->  LotNbr
         *   string ->  Module
         * calls for stored procedure spViewDataForCamstarMoldSensor
         */
        public string viewDataForCamstar(string LotNumber, string Module, string PackageGroup)
        {
            string data = "";
            string storedProc = ConfigurationManager.AppSettings["spViewDataForCamstarMoldSensor"].ToString();
            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(storedProc, con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@LotNbr", SqlDbType.NVarChar).Value = LotNumber;
                    cmd.Parameters.Add("@Module", SqlDbType.NVarChar).Value = Module;
                    cmd.Parameters.Add("@PackageGroup", SqlDbType.NVarChar).Value = PackageGroup;

                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        data += rdr["data"].ToString() + "\n";
                    }
                }
            }
            return data;
        }

        public string[] validateLotAndUser(string LotNbr, string Username)
        {
            string[] result = new string[2];
            string storedProc = ConfigurationManager.AppSettings["spValidateLotAndUser"].ToString();
            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(storedProc, con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@LotNbr_", SqlDbType.NVarChar).Value = LotNbr;
                    cmd.Parameters.Add("@UserId_", SqlDbType.NVarChar).Value = Username;

                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        //data += rdr["data"].ToString() + "\n";
                        //string[] result = new string[] { rdr["Device"].ToString(), rdr["UserName"].ToString() };
                        result[0] = rdr["Device"].ToString();
                        result[1] = rdr["UserName"].ToString();
                    }
                }
            }

            return result;
        }


        internal void AddRecord()
        {
            throw new NotImplementedException();
        }
    }
}