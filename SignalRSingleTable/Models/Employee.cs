using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRSingleTable.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public String Position { get; set; }
        public string Office { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public string AssignedTo { get; set; }

    }



    public class UpdatedEmployeeList
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Position  { get; set; }
        public string Manager { get; set; }
        public string AssignedDate { get; set; }
    }

    public class AllMessages
    {
        public List<Employee> employee { get; set; }
        public List<UpdatedEmployeeList> newEmployeeList { get; set; }
        public IEnumerable<SelectListItem> dropDownItems { get; set; }
        public string Notes { get; set; }
    }

    public class Contacts
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContctNo { get; set; }
        public string AddedOn { get; set; }
    }

}