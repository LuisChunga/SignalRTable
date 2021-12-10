using SignalRSingleTable.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRSingleTable.Models
{
    public class Repository
    {
        readonly string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Employee> GetEmployees()
        {
            var messages = new List<Employee>();

            string query = @"SELECT [EmployeeID],[Name],[Position],[Office],[Age],[Salary] FROM [dbo].[Employee] WHERE [Manager] IS NULL";

            using (var connection = new SqlConnection(connString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Notification = null;


                var dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var reader = command.ExecuteReader();


                while (reader.Read())
                {
                    messages.Add(item: new Employee
                    {
                        EmployeeID = (int)reader["EmployeeID"],
                        Name = (string)reader["Name"],
                        Position = (string)reader["Position"],
                        Office = (string)reader["Office"],
                        Age = (int)reader["Age"],
                        Salary = (int)reader["Salary"],


                    });
                }

            }

            return messages;

        }


        public IEnumerable<UpdatedEmployeeList> GetUpdatedEmpList()
        {
            var newEmployeesList = new List<UpdatedEmployeeList> ();


            string query = @"SELECT [EmployeeID],[Name],[Position], B.[CaseCoOrdinator] as[Manager],[AssignedDate] FROM [dbo].[Employee] A INNER JOIN [dbo].[Managers] B ON A.[Manager]=B.ID WHERE A.Manager IS NOT NULL";

            using (var connection = new SqlConnection(connString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Notification = null;


                var dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var reader = command.ExecuteReader();


                while (reader.Read())
                {
                    string _msgAssignDate = Convert.ToDateTime(reader["AssignedDate"].ToString()).ToString("MM/dd/yyyy hh:mm tt");

                    //string _msgDate = Convert.ToDateTime(reader["Date"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
                    newEmployeesList.Add(item: new UpdatedEmployeeList
                    {
                        EmployeeID = (int)reader["EmployeeID"] ,
                        Name = (string)reader["Name"],
                        Position = (string)reader["Position"],
                        Manager = (string)reader["Manager"],
                        AssignedDate = _msgAssignDate

                    });
                }

            }



            return newEmployeesList;
        }


        public IEnumerable<SelectListItem> DropDownItems()
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Clear();
            items.Add(new SelectListItem { Text = string.Empty, Value = "0" });

            string query = @"SELECT [ID],[CaseCoOrdinator]  FROM [dbo].[Managers]";

            using (var connection = new SqlConnection(connString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Notification = null;


                var dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var reader = command.ExecuteReader();


                while (reader.Read())
                {
                    items.Add( new SelectListItem 
                    {
                        Text = reader["CaseCoOrdinator"].ToString(), 
                        Value = reader["ID"].ToString(),

                    });
                }

            }


            return items;
        }


        public string GetNotes()
        {
            return "this is a new update";
        }


        public AllMessages GetEmployeeData()
        {
            var Allmessages = new AllMessages()
            {
                employee = GetEmployees().ToList(),
                newEmployeeList = GetUpdatedEmpList().ToList(),
                dropDownItems = DropDownItems(),
                Notes = GetNotes()
            };

            return Allmessages;
        }



        public void UpdateEmployeeTable (int EmployeeID, int Value)
        {
            string query = @"Update [dbo].[Employee] set [Manager]=" + Value + ",[AssignedDate]='" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "' WHERE EmployeeID=" + EmployeeID + "";

            using (var connection = new SqlConnection(connString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Notification = null;


                var dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

             
                var reader = command.ExecuteNonQuery(); // update database

            }
        }

    private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
                MyHub.SendMessages();

        }

    }
}