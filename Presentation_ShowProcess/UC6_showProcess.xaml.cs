using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BLL_Technician;
using CoreEFTest.Context;
using CoreEFTest.Models;
using DLL_Technician;
using DTO;

namespace Presentation_ShowProcess
{
    /// <summary>
    /// Interaction logic for UC6_showProcess.xaml
    /// </summary>
    public partial class UC6_showProcess : Page
    {
        private IClinicDB db;
        private StaffLogin technician;
        private bool okIsRunning;
        private UC6_ShowProcess uc6_showProcess;
        private List<ProcesSpec> procesSpec;

        public UC6_showProcess(ClinicDBContext dbContext, StaffLogin login)
        {
            InitializeComponent();

            this.db = new ClinicDB(dbContext);
            technician = login;
            uc6_showProcess = new UC6_ShowProcess(db);
            procesSpec = new List<ProcesSpec>();
        }

        private void OKB_Click(object sender, RoutedEventArgs e)
        {
            if (okIsRunning != true)
            {
                okIsRunning = true;

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += UC6GetProcessInformation;

                worker.RunWorkerCompleted += UC6GetProcessInformationCompleted;

                string CPR = CPRTB.Text;
                worker.RunWorkerAsync(CPR);

                Loading.Visibility = Visibility.Visible;
                Loading.Spin = true;
            }
        }

        public void UC6GetProcessInformation(object sender, DoWorkEventArgs e)
        {
            e.Result = uc6_showProcess.GetProccesInformations((string)e.Argument);
        }

        public void UC6GetProcessInformationCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Loading.Visibility = Visibility.Collapsed;
            Loading.Spin = false;
            okIsRunning = false;

            StatusL.Visibility = Visibility.Visible;

            procesSpec = (List<ProcesSpec>)e.Result;

            //Hvis der kun er et høreapparat:  - melder fejl med rigtig DB
            if (procesSpec.Count == 1)
            {
                TwoHALGrid.Visibility = Visibility.Collapsed;
                TwoHATBGrid.Visibility = Visibility.Collapsed;
                OneHAStatusTB.Visibility = Visibility.Visible;

                if (procesSpec[0].Printed == true)
                {
                    ProgressBar.Value = 100;
                    StatusL.Content = "100%";

                    OneHAStatusTB.Text = "Printet den " + procesSpec[0].PrintDateTime.ToShortDateString();
                }

                else if (procesSpec[0].scanTechId != 0)
                {
                    ProgressBar.Value = 50;
                    StatusL.Content = "50%";
                    OneHAStatusTB.Text = "Scannet den " + procesSpec[0].scanDateTime.ToShortDateString();

                }

                else if (procesSpec[0].ClinicianId != 0)
                {
                    ProgressBar.Value = 25;
                    StatusL.Content = "25%";
                    OneHAStatusTB.Text = "HA oprettet den " + procesSpec[0].GeneralSpecCreateDateTime.ToShortDateString();

                }
                else
                {
                    ProgressBar.Value = 0;
                    StatusL.Content = "0%";
                    OneHAStatusTB.Text = "Der er ikke oprettet nogle høreapparat for den pågældende patient";

                }
            }
            //Hvis der er to høreapparat:
            if (procesSpec.Count == 2)
            {
                if (procesSpec[0].Printed == true || procesSpec[1].Printed == true)
                {
                    if (procesSpec[1].Printed == true && procesSpec[0].Printed == true)
                    {
                        ProgressBar.Value = 100;
                        StatusL.Content = "100%";
                        StatusLeftTB.Text = "Printet den " + procesSpec[0].PrintDateTime.ToShortDateString();
                        StatusRightTB.Text = "Printet den " + procesSpec[1].PrintDateTime.ToShortDateString();
                    }
                    else
                    {
                        ProgressBar.Value = 80;
                        StatusL.Content = "80%";

                        if (procesSpec[0].Printed == true)
                        {
                            StatusLeftTB.Text = "Printet den " + procesSpec[0].PrintDateTime.ToShortDateString();
                            StatusRightTB.Text = "Scannet den " + procesSpec[1].scanDateTime.ToShortDateString();
                        }
                        else
                        {
                            StatusRightTB.Text = "Printet den " + procesSpec[1].PrintDateTime.ToShortDateString();
                            StatusLeftTB.Text = "Scannet den " + procesSpec[0].scanDateTime.ToShortDateString();

                        }
                    }
                }
                else if (procesSpec[0].scanTechId != 0 || procesSpec[1].scanTechId != 0)
                {
                    if (procesSpec[0].scanTechId != 0 && procesSpec[1].scanTechId != 0)
                    {
                        ProgressBar.Value = 60;
                        StatusL.Content = "60%";
                        StatusLeftTB.Text = "Scannet den " + procesSpec[0].scanDateTime.ToShortDateString();
                        StatusRightTB.Text = "Scannet den " + procesSpec[1].scanDateTime.ToShortDateString();

                    }
                    else
                    {
                        ProgressBar.Value = 40;
                        StatusL.Content = "40%";

                        if (procesSpec[0].scanTechId != 0)
                        {
                            StatusLeftTB.Text = "Scannet den " + procesSpec[0].scanDateTime.ToShortDateString();
                            StatusRightTB.Text = "HA oprettet den " + procesSpec[1].GeneralSpecCreateDateTime.ToShortDateString();
                        }
                        else
                        {
                            StatusRightTB.Text = "Scannet den " + procesSpec[1].scanDateTime.ToShortDateString();
                            StatusLeftTB.Text = "HA oprettet den " + procesSpec[0].GeneralSpecCreateDateTime.ToShortDateString();

                        }
                    }
                }

                else if (procesSpec[0].ClinicianId != 0 && procesSpec[1].ClinicianId != 0)
                {
                    if (procesSpec[0].ClinicianId != 0 && procesSpec[1].ClinicianId != 0)
                    {
                        ProgressBar.Value = 20;
                        StatusL.Content = "20%";

                        StatusLeftTB.Text = "HA oprettet den " + procesSpec[0].GeneralSpecCreateDateTime.ToShortDateString();
                        StatusRightTB.Text = "HA oprettet den " + procesSpec[1].GeneralSpecCreateDateTime.ToShortDateString();

                    }
                    else
                    {
                        ProgressBar.Value = 10;
                        StatusL.Content = "10%";

                        if (procesSpec[0].ClinicianId != 0)
                        {
                            StatusLeftTB.Text = "HA oprettet den " + procesSpec[0].GeneralSpecCreateDateTime.ToShortDateString();
                            StatusRightTB.Text = "Patient kke oprettet endnu";
                        }
                        else
                        {
                            StatusRightTB.Text = "Oprettet den " + procesSpec[1].GeneralSpecCreateDateTime.ToShortDateString();
                            StatusLeftTB.Text = "Patient ikke oprettet endnu";
                        }
                    }
                }
                else
                {
                    ProgressBar.Value = 0;
                    StatusL.Content = "0%";

                    StatusLeftTB.Text = "Patient ikke oprettet endnu";
                    StatusRightTB.Text = "Patient ikke oprettet endnu";

                }

            }


        }
    }
}
