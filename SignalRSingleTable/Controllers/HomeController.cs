using SignalRSingleTable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace SignalRSingleTable.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Repository messages = new Repository();
            return View(messages.GetEmployeeData()); // this function is in Repository
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public JsonResult GetMessages()
        //{
        //    IEnumerable<Employee> messages = new List<Employee>();
        //    Repository r = new Repository();
        //    messages = r.GetEmployees();
        //    return Json(messages, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetNotificationContacts()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetContacts(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public ActionResult UpdateTable(Employee msg)
        {
            Repository _messageRepository = new Repository();
            _messageRepository.UpdateEmployeeTable(msg.EmployeeID, Convert.ToInt32(msg.AssignedTo));
            return new EmptyResult();
        }
        public ActionResult GetMessages()
        {
            Repository employeeData = new Repository();

            return PartialView("_DataList", employeeData.GetEmployeeData()); // this function is in Repository
        }


    }
}