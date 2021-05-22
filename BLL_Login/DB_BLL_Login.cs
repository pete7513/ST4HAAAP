using System;
using System.Data.SqlClient;
using System.Transactions;
using CoreEFTest.Models;
using DLL_Login;
using DTO;

namespace BLL_Login
{
   public class DB_BLL_Login
   {
      public DB_Login DbLogin { get; set; }

      public DB_BLL_Login()
      {
         DbLogin = new DB_Login();
      }

      /// <summary>
      /// Returns SystemStatus and staffDTO
      /// </summary>
      /// <param name="staffID"></param>
      /// <param name="pw"></param>
      /// <returns></returns>
      public StaffLogin CheckLogin(string staffID, string pw)
      {
         return DbLogin.LoginStaff(staffID, pw);
      }
   }
}
