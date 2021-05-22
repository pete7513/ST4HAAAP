using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL_Technician;
using CoreEFTest.Models;
using DLL_Technician;
using DTO;
using HelixToolkit.Wpf;
using Newtonsoft.Json;
using QuantumConcepts.Formats.StereoLithography;

namespace Presentation_Technician
{
    /// <summary>
    /// Interaction logic for ScanPage.xaml
    /// </summary>
    public partial class ScanPage : Page
    {
        private IClinicDB db;
        private IScanner scanner;
        private StaffLogin technician;
        private UC4_Scan uc4_scan;
        private bool HentisRunning;
        private bool ScanisRunning;
        private bool SaveScanisRunning;
        private Patient patientAndHA;
        private Ear earside;

        
        private RawEarScan rawEarScan;
        private ModelImporter modelImporter;
        private FullRawEarScan fullRawEarScan;
        private BinaryFormatter binaryFormatter;
        private JsonSerializer jsonSerializer;
        private QuantumConcepts.Formats.StereoLithography.STLDocument stlDocument = new STLDocument(); 

        //Venstre øreafstøbning
        //private const string MODEL_PATH = "Mold_for_Ear_V1.7_L.stl";

        //Højre øreafstøbning
        private const string MODEL_PATH = "Mold_for_Ear_V1.7_R.stl";

        public ScanPage(IClinicDB db, IScanner scanner, StaffLogin technician)
        {
            InitializeComponent();
            this.db = db;
            this.scanner = scanner;
            this.technician = technician;

            uc4_scan = new UC4_Scan(this.db, scanner);

            modelImporter = new ModelImporter();
            binaryFormatter = new BinaryFormatter();
            jsonSerializer = new JsonSerializer();

        }

        #region Hent metoder
        private void HentInfoB_Click(object sender, RoutedEventArgs e)
        {
            if (HentisRunning != true)
            {
                HentisRunning = true;

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += UC4GetPatientInformation;

                worker.RunWorkerCompleted += UC4GetPatientInformationCompleted;

                string castID = HACastIDTB.Text;
                worker.RunWorkerAsync(castID);

                Loading.Visibility = Visibility.Visible;
                Loading.Spin = true;
            }
            
        }

        public void UC4GetPatientInformation(object sender, DoWorkEventArgs e)
        {
            e.Result = uc4_scan.GetPatientInformations((string)e.Argument);
        }

        public void UC4GetPatientInformationCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HentisRunning = false;
            Loading.Spin = false;
            Loading.Visibility = Visibility.Collapsed;

            PatientInformationTB.Visibility = Visibility.Visible;

            patientAndHA = (Patient) e.Result;

            if (patientAndHA != null)
            {
               if (Convert.ToInt32(HACastIDTB.Text) == patientAndHA.EarCasts[0].EarCastID)
               {
                  PatientInformationTB.Text = "PCPR: " + patientAndHA.CPR + "\r\nNavn: " + patientAndHA.Name + " " +
                                              patientAndHA.Lastname + "\r\nAlder: " + patientAndHA.Age +
                                              "\r\nØreside: " + patientAndHA.EarCasts[0].EarSide;
                  ScanB.IsEnabled = true;
                  HentInfoB.IsEnabled = false;
                  earside = patientAndHA.EarCasts[0].EarSide;
               }
               else if (Convert.ToInt32(HACastIDTB.Text) == patientAndHA.EarCasts[2].EarCastID)
               {
                  PatientInformationTB.Text = "PCPR: " + patientAndHA.CPR + "\r\nNavn: " + patientAndHA.Name + " " +
                                              patientAndHA.Lastname + "\r\nAlder: " + patientAndHA.Age +
                                              "\r\nØreside: " + patientAndHA.EarCasts[1].EarSide;
                  ScanB.IsEnabled = true;
                  HentInfoB.IsEnabled = false;
                  earside = patientAndHA.EarCasts[1].EarSide;
               }
            }
            else
            {
                PatientInformationTB.Text = "Det indtastede\r\nhøreafstøbningsID findes\r\nikke i databasen";
            }
        }
        #endregion

        #region Scan metoder
        private void ScanB_Click(object sender, RoutedEventArgs e)
        {
            bool connection = uc4_scan.ConnectToScanner();

            if (connection == true)
            {
                if (ScanisRunning != true)
                {
                    ScanisRunning = true;

                    BackgroundWorker worker = new BackgroundWorker();

                    worker.DoWork += UC4StartScan;

                    worker.RunWorkerCompleted += UC4ScanCompleted;

                    worker.RunWorkerAsync(earside);

                    ScanLoading.Visibility = Visibility.Visible;
                    ScanLoading.Spin = true;
                    ScannerL.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("Der kan ikke oprettes forbindelse til scanneren - prøv igen...","Advarelse", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void UC4StartScan(object sender, DoWorkEventArgs e)
        {
            e.Result = uc4_scan.StartScanning((Ear)e.Argument);
        }

        public void UC4ScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ScanisRunning = false;
            ScanLoading.Visibility = Visibility.Collapsed;
            ScanLoading.Spin = false;
            ScannerL.Visibility = Visibility.Collapsed;

            rawEarScan = (RawEarScan)e.Result;
            rawEarScan.StaffLoginFK = technician.StaffID;

            //Opretter en technicalSpec
            uc4_scan.CreateTechnicalSpec(patientAndHA, technician, rawEarScan.EarSide);
            
            Visual3D.Content = modelImporter.Load(MODEL_PATH);

            GemB.IsEnabled = true;
        }

       
        #endregion

        #region Gem metoder
        private void GemB_Click(object sender, RoutedEventArgs e)
        {

            if (SaveScanisRunning != true)
            {
                SaveScanisRunning = true;

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += UC4SaveScan;

                worker.RunWorkerCompleted += UC4SaveScanCompleted;
                
                //Der oprettes en intern DTO som indeholder de informationer der skal sendes med i metoden da der kun kan sendes en parameter med i RunWorkAsync
                fullRawEarScan = new FullRawEarScan();
                fullRawEarScan.scan = rawEarScan;
                fullRawEarScan.PatientId = patientAndHA.PatientId;
                fullRawEarScan.scan.StaffLoginFK = technician.StaffID;

                worker.RunWorkerAsync(fullRawEarScan);

                Save.Visibility = Visibility.Visible;
                Save.Spin = true;
                Save.Visibility = Visibility.Visible;
            }
        }

        public void UC4SaveScan(object sender, DoWorkEventArgs e)
        {
            FullRawEarScan parm = (FullRawEarScan) e.Argument;
            e.Result = uc4_scan.SaveScan(parm.scan, parm.PatientId);
        }

        public void UC4SaveScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveScanisRunning = false;

            Save.Visibility = Visibility.Collapsed;
            Save.Spin = true;

            if ((bool)e.Result)
            {
                MessageBox.Show("Scanning er gemt i databasen", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Scanning er IKKE gemt i databasen \r\n- Prøv igen", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #endregion

        }
}
