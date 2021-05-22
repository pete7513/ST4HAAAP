using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
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
using CoreEFTest.Models;
using DLL_Technician;

namespace Presentation_Technician
{
   /// <summary>
   /// Interaction logic for HAInfoPage.xaml
   /// </summary>
   public partial class HAInfoPage : Page
   {
      private UC3_ShowHATech uc3_ShowHATech;
      private UC3_UpdateHATech uc3_UpdateHATech;

      private IClinicDB db;
      private bool isRunning;
      private bool rediger;
      private Patient patientAndHA;
      private int count = 0;

      public HAInfoPage(IClinicDB db)
      {
         InitializeComponent();
         this.db = db;
         uc3_ShowHATech = new UC3_ShowHATech(db);
         uc3_UpdateHATech = new UC3_UpdateHATech(db);
         ShowHAInfoB.IsEnabled = false;
         RedigerB.IsEnabled = false;
      }

      private void OKB_Click(object sender, RoutedEventArgs e)
      {
         if (isRunning != true)
         {
            HAList.Items.Clear();
            string CPR = CPRnummerTB.Text;
            if (CPR.Length == 11 && CPR.Contains('-'))
            {
               isRunning = true;

               BackgroundWorker worker = new BackgroundWorker();


               worker.DoWork += UC3GetPatient;

               worker.RunWorkerCompleted += UC3GetPatientCompleted;

               worker.RunWorkerAsync(CPR);

               Loading.Visibility = Visibility.Visible;
               Loading.Spin = true;
            }
            else
            {
               MessageBox.Show("Indtast gyldigt PCPR");

            }
         }
      }

      public void UC3GetPatient(object sender, DoWorkEventArgs e)
      {
         string CPR = (string)e.Argument;
         e.Result = uc3_ShowHATech.GetPatient(CPR);
      }

      public void UC3GetPatientCompleted(object sender, RunWorkerCompletedEventArgs e)
      {// Bude det ikke være generalspec som skal kigges på og ikke tecspec? 
         isRunning = false;
         Loading.Spin = false;
         Loading.Visibility = Visibility.Collapsed;

         patientAndHA = (Patient)e.Result;

         if (patientAndHA != null)
         {
            patientInfoTB.Text = "CPR: " + patientAndHA.CPR + "\r\nNavn: " + patientAndHA.Name + " " +
                                 patientAndHA.Lastname + "\r\nAlder: " + patientAndHA.Age;

            foreach (TecnicalSpec spec in patientAndHA.TecnicalSpecs)
            {
               if (spec != null)
               HAList.Items.Add(spec.EarSide.ToString());
            }

            if (patientAndHA.TecnicalSpecs[0] == null && patientAndHA.TecnicalSpecs[1] == null)
            {
               patientInfoTB.Text = "CPR: " + patientAndHA.CPR + "\r\nNavn: " + patientAndHA.Name + " " +
                                    patientAndHA.Lastname + "\r\nAlder: " + patientAndHA.Age +
                                    "\r\n\n Ingen scanning fortaget";
            }
         }
         else
         {
            patientInfoTB.Text = "Det indtastede PCPR nummer findes ikke i databasen";
         }

         ShowHAInfoB.IsEnabled = true;

      }

      private void ShowHAInfoB_Click(object sender, RoutedEventArgs e)
      {
      //   if (HAList.SelectedIndex == -1)
      //   {
      //      MessageBox.Show("Vælg et høreapparat og prøv igen", "Information");
      //   }
      //   else
      //   {
      //      GeneralSpec selectedGeneralSpec = (GeneralSpec)patientAndHA.GeneralSpecs[HAList.SelectedIndex];
      //      TecnicalSpec selectedTecnicalSpec = (TecnicalSpec)patientAndHA.TecnicalSpecs[HAList.SelectedIndex];

      //      TypeTB.Text = selectedGeneralSpec.Type.ToString();
      //      ColorTB.Text = selectedGeneralSpec.Color.ToString();
      //      GenDateTB.Text = selectedGeneralSpec.CreateDate.ToShortDateString();

      //      TechDateTB.Text = selectedTecnicalSpec.CreateDate.ToShortDateString();

      //      //Todo Kommenter dette ind
      //      //if (selectedTecnicalSpec.RawEarScan == null)
      //      //{
      //      //    PrintStatusTB.Text = "Ikke scannet endnu";
      //      //}
      //      //else
      //      //{
      //      if (selectedTecnicalSpec.Printed == false)
      //      {

      //         PrintStatusTB.Text = "Ikke printet endnu";
      //      }

      //      if (selectedTecnicalSpec.Printed == true)
      //      {

      //         PrintStatusTB.Text = "Printet";
      //      }

      //      //}
      //      RedigerB.IsEnabled = true;
      //   }
      }

      private void RedigerB_Click(object sender, RoutedEventArgs e)
      {
          rediger = true; 
          string type = TypeTB.Text;
         string farve = ColorTB.Text;

         TypeCB.Visibility = Visibility.Visible;
         TypeTB.Visibility = Visibility.Collapsed;
         TypeCB.Background = Brushes.LightGoldenrodYellow;

         ColorCB.Visibility = Visibility.Visible;
         ColorTB.Visibility = Visibility.Collapsed;
         ColorCB.Background = Brushes.LightGoldenrodYellow;


         if (count == 0)
         {
            foreach (var types in Enum.GetValues(typeof(Material)))
            {
               TypeCB.Items.Add(types);
            }

            foreach (var colors in Enum.GetValues(typeof(PlugColor)))
            {
               ColorCB.Items.Add(colors);
            }
            count++;
         }

         TypeCB.Text = type; //Viser den type der var på forhånd i Combobox
         ColorCB.Text = farve; //Viser den farve der var på forhånd i Combobox

         GemB.Visibility = Visibility.Visible;
         RedigerB.Visibility = Visibility.Collapsed;
      }

      private void GemB_Click(object sender, RoutedEventArgs e)
      {
          TypeCB.Visibility = Visibility.Collapsed;
         TypeTB.Visibility = Visibility.Visible;
         TypeTB.Text = TypeCB.SelectionBoxItem.ToString();


         ColorCB.Visibility = Visibility.Collapsed;
         ColorTB.Visibility = Visibility.Visible;
         ColorTB.Text = ColorCB.SelectionBoxItem.ToString();

         GemB.Visibility = Visibility.Collapsed;
         RedigerB.Visibility = Visibility.Visible;

         patientAndHA.GeneralSpecs[HAList.SelectedIndex].Type = (Material)TypeCB.SelectionBoxItem;
         patientAndHA.GeneralSpecs[HAList.SelectedIndex].Color = (PlugColor)ColorCB.SelectionBoxItem;

         uc3_UpdateHATech.UpdateGeneralSpec(patientAndHA.GeneralSpecs[HAList.SelectedIndex]);

         ColorCB.Text = "";
         TypeCB.Text = "";
         rediger = false;
      }

        private void HAList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!rediger)
            {
                GeneralSpec selectedGeneralSpec = (GeneralSpec) patientAndHA.GeneralSpecs[HAList.SelectedIndex];
                TecnicalSpec selectedTecnicalSpec = (TecnicalSpec) patientAndHA.TecnicalSpecs[HAList.SelectedIndex];

                TypeTB.Text = selectedGeneralSpec.Type.ToString();
                ColorTB.Text = selectedGeneralSpec.Color.ToString();
                GenDateTB.Text = selectedGeneralSpec.CreateDate.ToShortDateString();

                TechDateTB.Text = selectedTecnicalSpec.CreateDate.ToShortDateString();


                if (selectedTecnicalSpec.Printed == false)
                {

                    PrintStatusTB.Text = "Ikke printet endnu";
                }

                if (selectedTecnicalSpec.Printed == true)
                {

                    PrintStatusTB.Text = "Printet";
                }

                RedigerB.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Afslut redigering inden du vælger nyt høreapparat");
            }
        }
    }
}
