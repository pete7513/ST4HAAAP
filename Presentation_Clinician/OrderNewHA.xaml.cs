using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CoreEFTest.Models;
using BLL_Clinician;

namespace Presentation_Clinician
{
   /// <summary>
   /// Interaction logic for OrderNewHA.xaml
   /// </summary>
   public partial class OrderNewHA : Window
   {
      UC3_ManageHA manageHA = new UC3_ManageHA();

      private ClinicianMainWindow _clinicianMainWindow;
      private GeneralSpec generalSpec;
      private EarCast earCast;

      public OrderNewHA(ClinicianMainWindow clinicianMainWindow, UC3_ManageHA manageHa)
      {
          InitializeComponent();

            this._clinicianMainWindow = clinicianMainWindow;
            this.manageHA = manageHa;

      }

      private void BtnSave_Click(object sender, RoutedEventArgs e)
      {
         generalSpec = new GeneralSpec();
         earCast = new EarCast();

         if (Cb_LeftEar.IsChecked == true)
         {
            generalSpec.EarSide = Ear.Left;
            earCast.EarSide = Ear.Left;
         }
         else if (Cb_RightEar.IsChecked == true)
         {
            generalSpec.EarSide = Ear.Right;
            earCast.EarSide = Ear.Right;
         }

         generalSpec.PatientFK = _clinicianMainWindow.Patient.PatientId;
         //generalSpec.Patient = _clinicianMainWindow.Patient;
         generalSpec.CreateDate = DateTime.Now;
         generalSpec.StaffLoginFK = _clinicianMainWindow.clinician.StaffID;
         //generalSpec.StaffLogin = _clinicianMainWindow.clinician;
         CbNewColor.Text = generalSpec.Color.ToString();
         CbNewType.Text = generalSpec.Type.ToString();

         earCast.CastDate = DateTime.Now;
         earCast.PatientFK = _clinicianMainWindow.Patient.PatientId;
         //earCast.Patient = _clinicianMainWindow.Patient;

         manageHA.CreateHA(generalSpec);

         manageHA.createEC(earCast);

         CbNewType.SelectedIndex = -1;
         CbNewColor.SelectedIndex = -1;

            MessageBox.Show("Høreapparatet er bestilt","Bekræftigelse",MessageBoxButton.OK,MessageBoxImage.Information);

            

      }

      

   }
}
