using System;
using System.Collections.Generic;
using System.Text;
using CoreEFTest.Models;
using DLL_Clinician;
using DLL_Clinician.RegionsDatabase;
using EFCoreTestConsoleApp;
using Microsoft.EntityFrameworkCore.Internal;

namespace BLL_Clinician
{
    public class UC2_ManagePatient
    {
        private IClinicDatabase clinicDatabase;
        private IRegionDatabase regionDatabase;
        private bool CPRCorrect;
        //private bool RegionCPRCorrect;
        

        public UC2_ManagePatient()
        {
            clinicDatabase = new ClinicDatabase();
            regionDatabase = new RegionDatabase();
        }

        public void SaveUpdates(Patient patient)
        {
           clinicDatabase.UpdatePatient(patient);
        }

        public void SavePatient(Patient patient)
        {
            clinicDatabase.CreatePatient(patient);
            
        }

        public bool CheckCPRClinicDatabase(string CPRnumber)
        {
            int patientRegistered = 0;
            List<Patient> PatientListe = clinicDatabase.GetAllPatients();
            foreach (var patient in PatientListe)
            {
                if (patient.CPR == CPRnumber)
                {
                    patientRegistered++;
                }

                if (patientRegistered == 1)
                {
                    CPRCorrect = true;
                }
                else
                {
                    CPRCorrect = false;
                }
            }
            return CPRCorrect;
        }

        //public bool CheckCPRRegionDatabase(string CPRnumber)
        //{
        //    if (regionDatabase.CheckCPR(CPRnumber))
        //    {
        //        RegionCPRCorrect = true;
        //    }
        //    else
        //    {
        //        RegionCPRCorrect = false;
        //    }

        //    return RegionCPRCorrect;
        //}

        public Patient GetPatientInformationRegionsDatabase(string CPRnumber)
        {
            return regionDatabase.GetPatient(CPRnumber);
        }

        public Patient GetPatientInformation(string CPRnumber)
        {
            return clinicDatabase.GetPatient(CPRnumber);
        }
    }
}
