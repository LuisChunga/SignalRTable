using Microsoft.AspNet.SignalR;
using SignalRSingleTable.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SignalRSingleTable.Models
{
    public class NotificationComponent
    {

        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification(DateTime currentTime)
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency

            string query = @"SELECT [ContactId], [ContactName], [ContctNo] from  [dbo].[Contacts] WHERE [AddedOn] > @AddedOn";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                command.Notification = null;

                command.Parameters.AddWithValue("@AddedOn", currentTime);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(sqlDep_OnChange);

                SqlDataReader reader = command.ExecuteReader();


                //we must have to execute the command here
                using (reader)
                {

            
                    // nothing need to add here now
                }
            }
        }


        public List<Contacts> GetContacts(DateTime afterDate)
        {
            var notificationData = new List<Contacts>();

            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency

            string query = @"SELECT [ContactId], [ContactName], [ContctNo], [AddedOn] FROM [dbo].[Contacts] WHERE [AddedOn] > @afterDate ORDER BY [AddedOn] DESC";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                command.Notification = null;

                command.Parameters.AddWithValue("@afterDate", afterDate);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();



                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += new OnChangeEventHandler(sqlDep_OnChange);
                //dependency.OnChange += sqlDep_OnChange;

                //we must have to execute the command here
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    string _msgAssignDate = Convert.ToDateTime(reader["AssignedDate"].ToString()).ToString("MM/dd/yyyy hh:mm tt");

                    //string _msgDate = Convert.ToDateTime(reader["Date"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
                    notificationData.Add(item: new Contacts
                    {
                        ContactId = (int)reader["ContactId"],
                        ContactName = (string)reader["ContactName"],
                        ContctNo = (string)reader["ContctNo"],
                        AddedOn = Convert.ToDateTime(reader["AddedOn"]).ToString()


                    });
                }

            }
                return notificationData;
            
              //return dc.Contacts.Where(a => a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();
         
        }


        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= new OnChangeEventHandler(sqlDep_OnChange);

                MyHub.SendNotifications();

                // re-register notification
                RegisterNotification(DateTime.Now);
            }
        }


    }
}