using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
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
using DLL_Technician;
using CoreEFTest.Models;
using DLL_Technician.Printer;
using DTO;

namespace Presentation_Technician
{
   /// <summary>
   /// Interaction logic for PrintPage.xaml
   /// </summary>
   public partial class PrintPage : Page
   {
      private bool isRunning;
      private IClinicDB db;
      private IPrinter printer;
      private StaffLogin technician;
      private UC5_Print uc5_print;
      private bool HentisRunning;
      private bool ScanisRunning;
      private List<TecnicalSpec> patientInformationsAll;
      private List<TecnicalSpec> patientInformations;
      private RawEarScan rawEarScan;
      private RawEarPrint printedEarPrint;
      private FullRawEarPrint fullRawEarPrint;


      public PrintPage(IClinicDB db, IPrinter printer, StaffLogin technician)
      {
         InitializeComponent();
         this.db = db;
         this.printer = printer;
         this.technician = technician;

         uc5_print = new UC5_Print(db, printer);
         PrintB.IsEnabled = false;

      }

      #region Hent

      #region Find alle høreapparater

      private void FindAllPatientsB_Click(object sender, RoutedEventArgs e)
      {
         FindScanB.IsEnabled = false;
         if (isRunning != true)
         {
            isRunning = true;

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += UC4GetPatientInformationAll;
            worker.RunWorkerCompleted += UC4GetPatientInformationAllCompleted;

            worker.RunWorkerAsync();

            Loading.Visibility = Visibility.Visible;
            Loading.Spin = true;

            //Thread.Sleep(1500);
            //MessageBox.Show("Der blev ikke fundet nogle høreapparater, der er klar til print")
         }
      }


      public void UC4GetPatientInformationAll(object sender, DoWorkEventArgs e)
      {
         e.Result = uc5_print.GetEarScans();
      }

      public void UC4GetPatientInformationAllCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         HentisRunning = false;
         Loading.Spin = false;
         Loading.Visibility = Visibility.Collapsed;

         FindAllPatientsB.Visibility = Visibility.Visible;

         patientInformationsAll = (List<TecnicalSpec>)e.Result;
         if (patientInformationsAll.Count > 1)
         {
            PatientInformationLB.Items.Add("Vælg alle");
         }

         foreach (var tecnicalSpec in patientInformationsAll)
         {
            if (tecnicalSpec != null)
            {
               PatientInformationLB.Items.Add("PID: " + tecnicalSpec.PatientFK + "\r\nØre: " + tecnicalSpec.EarSide);
            }
            else
            {
               PatientInformationLB.Items.Add("Der er ingen ørepropper klar til print");
            }
         }

         PrintB.IsEnabled = true;
      }

      #endregion

      #region Find en patient

      private void FindScanB_Click(object sender, RoutedEventArgs e)
      {
         if (CPRnummerTB.Text == "")
         {
            MessageBox.Show("Indtast et PCPR-nummer", "Fejl");
         }
         else
         {
            FindAllPatientsB.IsEnabled = false;
            if (isRunning != true)
            {
               isRunning = true;

               BackgroundWorker worker = new BackgroundWorker();

               worker.DoWork += UC5GetPatientInformation;
               worker.RunWorkerCompleted += UC5GetPatientInformationCompleted;

               string CPR = CPRnummerTB.Text;
               worker.RunWorkerAsync(CPR);

               Loading.Visibility = Visibility.Visible;
               Loading.Spin = true;

               //Thread.Sleep(1500);
               //MessageBox.Show("Der blev ikke fundet nogle høreapparater, der er klar til print")
            }
         }
      }

      public void UC5GetPatientInformation(object sender, DoWorkEventArgs e)
      {
         e.Result = uc5_print.GetEarScan((string)e.Argument);
      }

      public void UC5GetPatientInformationCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         HentisRunning = false;
         Loading.Spin = false;
         Loading.Visibility = Visibility.Collapsed;

         FindAllPatientsB.Visibility = Visibility.Visible;

         patientInformations = (List<TecnicalSpec>)e.Result;

         int count = 0;

         foreach (var tecnicalSpec in patientInformations)
         {
            if (tecnicalSpec.EarSide != Ear.Null)
            {
               if (count == 0)
               {
                  PatientInformationTB.Text = "PID: " +
                                              tecnicalSpec.PatientFK+ /*"\r\nNavn: " + tecnicalSpec.Patient.Name + " " + tecnicalSpec.Patient.Lastname +*/
                                              "\r\nØre: " + tecnicalSpec.EarSide;
                  count++;
               }
               else
               {
                  PatientInformationTB.Text += "\r\nØre: " + tecnicalSpec.EarSide;

               }
            }
         }
         PrintB.IsEnabled = true;
      }

      #endregion

      #endregion

      #region Print metoder

      private void PrintB_Click(object sender, RoutedEventArgs e)
      {
         bool connect = uc5_print.ConnectToPrinter();
         if (connect)
         {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += UC5AddToPrintQueue;
            worker.RunWorkerCompleted += UC5AddToPrintQueueCompleted;


            fullRawEarPrint = new FullRawEarPrint();
            fullRawEarPrint.PrintTechID = technician.StaffID;
            fullRawEarPrint.CPR = CPRnummerTB.Text;
            fullRawEarPrint.EarScans = new List<RawEarScan>();


            if (PatientInformationLB.Items.Count > 0)
            {
               if (PatientInformationLB.SelectedIndex == 0)
               {
                  foreach (var tecnical in patientInformationsAll)
                  {
                     fullRawEarPrint.EarScans.Add(tecnical.RawEarScan);
                  }
               }
               else
               {
                  int EarScanIndex = PatientInformationLB.SelectedIndex - 1;
                  fullRawEarPrint.EarScans.Add(patientInformationsAll[EarScanIndex].RawEarScan);
               }
            }

            else
            {
               foreach (var tecnical in patientInformations)
               {
                  fullRawEarPrint.EarScans.Add(tecnical.RawEarScan);
               }
            }

            worker.RunWorkerAsync(fullRawEarPrint);

            Loading.Visibility = Visibility.Visible;
            Loading.Spin = true;
         }
      }

      public void UC5AddToPrintQueue(object sender, DoWorkEventArgs e)
      {
         FullRawEarPrint parm = (FullRawEarPrint)e.Argument;
         e.Result = uc5_print.AddToPrintQueue(parm.PrintTechID, parm.EarScans);
      }

      public void UC5AddToPrintQueueCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         HentisRunning = false;
         Loading.Spin = false;
         Loading.Visibility = Visibility.Collapsed;

         //FindAllPatientsB.Visibility = Visibility.Visible;

         printedEarPrint = (RawEarPrint)e.Result;

         if (printedEarPrint == null)
         {
            MessageBox.Show("Øreprop kunne ikke tilføjes til printkøen");
         }
         else
         {
            MessageBox.Show("Øreproppen er tilføjet til printkøen");
            uc5_print.DBPrint(fullRawEarPrint);
         }
      }

      #endregion
   }
}
