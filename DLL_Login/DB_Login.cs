using System;
using System.Data.SqlClient;
using System.Linq;
using CoreEFTest.Context;
using CoreEFTest.Models;
using DTO;

namespace DLL_Login
{
   public class DB_Login
   {
      private ClinicDBContext context;
      private SqlConnection connection;
      private SqlDataReader reader;
      private SqlCommand command;
      //private const String DBlogin = "F20ST2ITS2201907648";
      private const string DBlogin = "F21ST4GRP1";
      private StaffLogin staffLogin;

      public DB_Login()
      {
         context = new ClinicDBContext();
      }

      /// <summary>
      /// Returns DTO_StaffLogin
      /// </summary>
      /// <param name="StaffID"> Entered in LoginWindow</param>
      /// <param name="pw">Entered in LoginWindow</param>
      /// <returns></returns>
      public StaffLogin LoginStaff(string StaffID, string pw)
      {
         //DTO Init
         staffLogin = new StaffLogin();
         staffLogin.StaffID = Convert.ToInt32(StaffID);
         staffLogin.Password = pw;
         staffLogin.StaffStatus = Status.Null;
         
         //DB connect and query
         string connectionString = (@"Data Source=st-i4dab.uni.au.dk;Initial Catalog=" + DBlogin + ";Integrated Security=False;User ID=" + DBlogin + ";Password=" + DBlogin + ";Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
         connection = new SqlConnection(connectionString);
         command = new SqlCommand();
         string queryString = "Select * from StaffLogin where StaffID = '" + StaffID + "'";

         //DB Open and request 
         try
         {
            connection.Open();
            using (command = new SqlCommand(queryString, connection))
            {
               reader = command.ExecuteReader();

               while (reader.Read())
               {
                  if (reader["StaffID"].ToString() == StaffID && reader["Password"].ToString() == pw)
                  {
                     staffLogin.Name = reader["Name"].ToString();

                     if (reader["StaffStatus"].ToString() == "1")
                        staffLogin.StaffStatus = Status.Clinician;
                     else if (reader["StaffStatus"].ToString() == "2")
                        staffLogin.StaffStatus = Status.Technician;
                  }
               }
            }
         }
         catch
         {
            Console.Write("Database not connected or data not found");
         }
         finally
         {
            connection.Close();
         }
         return staffLogin;
      }

      public StaffLogin EFStaffLogin(string StaffID, string pw)
      {
         try
         {
            StaffLogin staffLogin = context.StaffLogin.Single(x => x.StaffID == Convert.ToInt32(StaffID));
            staffLogin.Password = null;
         }
         catch
         {
            Console.Write("Database not connected or data not found");
         }

         return staffLogin;

      }
   }
}
