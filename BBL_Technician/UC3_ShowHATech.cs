using System;
using DLL_Technician;
using CoreEFTest;
using CoreEFTest.Models;

namespace BLL_Technician
{
   public class UC3_ShowHATech
   {
       private IClinicDB clinicDB;


       public UC3_ShowHATech(IClinicDB clinicDB)
       {
           this.clinicDB = clinicDB;
       }

       public Patient GetPatient(string CPR)
       {
           return clinicDB.GetPatientWithGeneralSpecAndTechnicalSpec(CPR);
       }
   }
}
