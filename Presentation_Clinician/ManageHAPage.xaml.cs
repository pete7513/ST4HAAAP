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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL_Clinician;
using CoreEFTest.Models;

namespace Presentation_Clinician
{
    /// <summary>
    /// Interaction logic for ManageHAPage.xaml
    /// </summary>
    public partial class ManageHAPage : Page
    {
        UC3_ManageHA manageHA = new UC3_ManageHA();

        //todo Her gør i det rigtigt. Se linje 36 og construktor :D - Dette skal overføres til OrderNewHA
        private ClinicianMainWindow _clinicianMainWindow;
        HAInformationWindow _haInformation;
        private HearingTestWindow _hearingTest;
        private OrderNewHA orderNewHa;

        
        public ManageHAPage(ClinicianMainWindow clinicianMainWindow, UC3_ManageHA manageHA)
        {
            InitializeComponent();
            this._clinicianMainWindow = clinicianMainWindow;
            this.manageHA = manageHA;
            
        }


        private void BtnRetrieveHearingTest_Click(object sender, RoutedEventArgs e)
        {
            _hearingTest = new HearingTestWindow();
            _hearingTest.Show();
        }

        private void BtnFormerHearingAids_Click(object sender, RoutedEventArgs e)
        {
            _haInformation = new HAInformationWindow(_clinicianMainWindow);
            _haInformation.Show();
         //TbAllHA.Text = Convert.ToString(manageHA.GetAllHA(clinicianMainWindow.PCPR));
        }

        private void HA_Page_Loaded(object sender, RoutedEventArgs e)
        {
            var HA_GeneralSpec = manageHA.GetHA(_clinicianMainWindow.Patient.PatientId);

            foreach (var generalSpec in HA_GeneralSpec)
            {
                if (generalSpec != null)
                {
                    if (generalSpec.EarSide == Ear.Left)
                    {
                        Tb_LeftEar_Color.Text = Convert.ToString(generalSpec.Color);
                        Tb_LeftEar_Type.Text = Convert.ToString(generalSpec.Type);
                        Tb_Left_HAID.Text = Convert.ToString(generalSpec.HAGeneralSpecID);
                        Tb_StaffID_Left.Text = Convert.ToString(generalSpec.StaffLoginFK);
                        Tb_Datetime_Left.Text = Convert.ToString(generalSpec.CreateDate);
                    }

                    if (generalSpec.EarSide == Ear.Right)
                    {
                        Tb_RightEar_Color.Text = Convert.ToString(generalSpec.Color);
                        Tb_RightEar_Type.Text = Convert.ToString(generalSpec.Type);
                        Tb_Right_HAID.Text = Convert.ToString(generalSpec.HAGeneralSpecID);
                        Tb_StaffID_Right.Text = Convert.ToString(generalSpec.StaffLoginFK);
                        Tb_Datetime_Right.Text = Convert.ToString(generalSpec.CreateDate);
                    }
                }
            }
        }

      

        private void BtnOrderHearingAids1_Click(object sender, RoutedEventArgs e)
        {
            orderNewHa = new OrderNewHA(_clinicianMainWindow, manageHA);
            orderNewHa.Show();
            
            
        }
    }
}
