using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace SPCCalc.Models
{
    public class Logs
    {
        public SPCDBContext spcDbContext_ = new SPCDBContext();

        public void DeleteMoldLogsTable2(string module, string lotNo)
        {
            var spcResult = spcDbContext_.Database.SqlQuery<SPCCalc.Models.Mold.ViewModelLogs>("spDeleteMoldLogsTable2 @Module, @LotNumber", new SqlParameter("@Module", module), new SqlParameter("@LotNumber", lotNo)).ToList();
        }

        public void DeleteWirebondLogsTable2(string module, string lotNo)
        {
            var spcResult = spcDbContext_.Database.SqlQuery<SPCCalc.Models.Wirebond.ViewModelLogs>("spDeleteWirebondLogsTable2 @Module, @LotNumber", new SqlParameter("@Module", module), new SqlParameter("@LotNumber", lotNo)).ToList();
        }
    }
}