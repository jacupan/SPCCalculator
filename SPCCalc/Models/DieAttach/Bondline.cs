using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

/*
 * @author      :   Dev Alvin (aabasolo@ALLEGROMICRO.COM)
 * @date        :   2016-08-23 2:43PM
 * @description :   model for dabondline
 */

namespace SPCCalc.Models.DieAttach
{
    public class Bondline
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
         *   string ->  LotNumber
         *   string ->  Device
         *   string ->  Machine
         *   string ->  A2Operator
         *   string ->  Remarks
         *   decimal ->  z1_w_epoxy
         *   decimal ->  z2_w_epoxy
         *   decimal ->  z3_w_epoxy
         *   decimal ->  z1_wo_epoxy
         *   decimal ->  z2_wo_epoxy
         *   decimal ->  z3_wo_epoxy
         *   decimal ->  z_avg_wo
         *   decimal ->  blt1
         *   decimal ->  blt2
         *   decimal ->  blt3
         *   datetime ->  DateCreated
         * calls for stored procedure spAddMoldSensor
         */
        public bool AddRecord(string LotNumber, string Device, string Machine, string A2Operator, string Remarks, decimal z1_w_epoxy, decimal z2_w_epoxy, decimal z3_w_epoxy, decimal z1_wo_epoxy, decimal z2_wo_epoxy, decimal z3_wo_epoxy, decimal z_avg_wo, decimal blt1, decimal blt2, decimal blt3, DateTime DateCreated)
        {
            string storedProc = ConfigurationManager.AppSettings["spAddDABondline"].ToString();
            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(storedProc, con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@LotNumber", SqlDbType.NVarChar).Value = LotNumber;
                    cmd.Parameters.Add("@Device", SqlDbType.NVarChar).Value = Device;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar).Value = Machine;
                    cmd.Parameters.Add("@A2Operator", SqlDbType.NVarChar).Value = A2Operator;
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = Remarks;

                    cmd.Parameters.Add("@z1_w_epoxy", SqlDbType.Decimal).Value = z1_w_epoxy;
                    cmd.Parameters.Add("@z2_w_epoxy", SqlDbType.Decimal).Value = z2_w_epoxy;
                    cmd.Parameters.Add("@z3_w_epoxy", SqlDbType.Decimal).Value = z3_w_epoxy;

                    cmd.Parameters.Add("@z1_wo_epoxy", SqlDbType.Decimal).Value = z1_wo_epoxy;
                    cmd.Parameters.Add("@z2_wo_epoxy", SqlDbType.Decimal).Value = z2_wo_epoxy;
                    cmd.Parameters.Add("@z3_wo_epoxy", SqlDbType.Decimal).Value = z3_wo_epoxy;

                    cmd.Parameters.Add("@z_avg_wo", SqlDbType.Decimal).Value = z_avg_wo;

                    cmd.Parameters.Add("@blt1", SqlDbType.Decimal).Value = blt1;
                    cmd.Parameters.Add("@blt2", SqlDbType.Decimal).Value = blt2;
                    cmd.Parameters.Add("@blt3", SqlDbType.Decimal).Value = blt3;

                    cmd.Parameters.Add("@Stat", SqlDbType.NVarChar).Value = "SAVED";
                    cmd.Parameters.Add("@DateCreated", SqlDbType.NVarChar).Value = DateCreated;

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
    }
}