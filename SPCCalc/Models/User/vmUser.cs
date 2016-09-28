using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPCCalc.Models.User
{
    public class vmUser
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FullName { get; set; }
    }

    [Table("spEmployee")]
    public class vmUserFullName
    {
        public string FullName { get; set; }
    }
}