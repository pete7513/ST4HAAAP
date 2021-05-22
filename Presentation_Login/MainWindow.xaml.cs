using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL_Login;
using CoreEFTest.Models;
using DTO;
using Presentation_Clinician;
using Presentation_Technician;

namespace Presentation_Login
{
   //Hello changes
   //Hello fresh changes

   /// <summary>
   /// Interaction logic for ClinicianMainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      //Class Init
      private readonly DB_BLL_Login dbBllLogin;
      private ClinicianMainWindow clinicianMainWindow;
      private TechnicianMainWindow technicianMainWindow;

      public MainWindow()
      {
         InitializeComponent();
         dbBllLogin = new DB_BLL_Login();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void LoginDB_Click(object sender, RoutedEventArgs e)
      {
         LoginMetode();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void PasswordTB_KeyUp(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
            LoginMetode();
      }

      /// <summary>
      /// 
      /// </summary>
      private void LoginMetode()
      {
         string StaffID = MedarbejderIDTB.Text;
         string PW = PasswordTB.Password;

         StaffLogin staffLogin = dbBllLogin.CheckLogin(StaffID, PW);

         if (staffLogin.StaffStatus == Status.Technician)
         {
            technicianMainWindow = new TechnicianMainWindow();
            technicianMainWindow.technician = staffLogin;
            technicianMainWindow.ShowDialog();
         }
         else if (staffLogin.StaffStatus == Status.Clinician)
         {
           
            clinicianMainWindow = new ClinicianMainWindow();
            clinicianMainWindow.clinician = staffLogin;
            clinicianMainWindow.ShowDialog();
         }
         else
         {
            MessageBox.Show("Forkert brugernavn eller password");
         }
      }

      private void LoginWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         Environment.Exit(1);
      }
   }
}
