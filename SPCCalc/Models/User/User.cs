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
 * @description :   model for User
 */

namespace SPCCalc.Models.User
{
    public class User
    {
        /*
         * initialize connection
         */
        string strConn = ConfigurationManager.ConnectionStrings["SPCContext"].ConnectionString;
        SqlDataReader rdr = null;

        public SPCDBContext spcDbContext_ = new SPCDBContext();

        public string validateUser(string UserId)
        {
            string result = "";
            string storedProc = ConfigurationManager.AppSettings["spEmployee"].ToString();
            using (SqlConnection con = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(storedProc, con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@strEmployee", SqlDbType.NVarChar).Value = UserId;

                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        result = rdr["FullName"].ToString();
                    }
                }
            }

            if(result==""){
                result = "invalid";
            }

            return result;
        }

        public string GetUserFullname(string username)
        {
            if (!String.IsNullOrWhiteSpace(username))
            {
                string employeeFullname = "";
                var employee = spcDbContext_.Database.SqlQuery<vmUserFullName>("spEmployee @strEmployee", new SqlParameter("@strEmployee", username)).ToList();

                employeeFullname = employee.Count == 0 ? "" : employee[0].FullName.ToString();

                return employeeFullname;
            }

            else
            {
                return "";
            }

        }

    }
    
}