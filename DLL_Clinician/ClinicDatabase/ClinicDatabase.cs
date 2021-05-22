using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreEFTest.Context;
using CoreEFTest.Models;


namespace DLL_Clinician
{
   public class ClinicDatabase : IClinicDatabase
   {
      private readonly ClinicDBContext _dbContext;

      public ClinicDatabase()
      {
         _dbContext = new ClinicDBContext();

      }

      #region Patient
      /// <summary>
      /// Denne metode benyttes til at hente alle Patienter fra ClinicDatabase
      /// </summary>
      /// <returns></returns>
      public List<Patient> GetAllPatients()
      {
         try
         {
            List<Patient> Patient = _dbContext.Patient.ToList();

            return Patient;
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// Denne metode benyttes til at hente en specifik Patient fra ClinicDatabase, ud fra deres PCPR
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public Patient GetPatient(string CPR)
      {
         try
         {
            Patient patient = _dbContext.Patient.Single(x => x.CPR == CPR);
            patient.GeneralSpecs = this.GetAllGeneralSpecs(patient.PatientId);

            return patient;
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// Denne metode benyttes til at hente en specifik Patient med knyttede EarCast fra ClinicDatabase, ud fra deres PCPR
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public Patient GetPatientWithEarCast(string CPR)
      {
         try
         {
            Patient patient = _dbContext.Patient.Single(x => x.CPR == CPR);
            patient.EarCasts = _dbContext.EarCast.Where(x => x.PatientFK == patient.PatientId).ToList();

            return patient;
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// Denne metode benyttes til at oprette en specifik Patient i ClinicDatabase.
      /// </summary>
      /// <param name="patient"></param>
      public void CreatePatient(Patient patient)
      {
         try
         {
            _dbContext.Patient.Add(patient);
            _dbContext.SaveChanges();
         }
         catch
         {

         }
      }

      /// <summary>
      /// Denne metode benyttes til at slette en specifik Patient i ClinicDatabase.
      /// </summary>
      /// <param name="patient"></param>
      public void DeletePatient(Patient patient)
      {
         try
         {
            _dbContext.Patient.Remove(patient);
            _dbContext.SaveChanges();
         }
         catch
         {

         }
      }

      /// <summary>
      /// Denne metode benyttes til at updatere en specifik Patient i ClinicDatabase.
      /// </summary>
      /// <param name="patient"></param>
      public void UpdatePatient(Patient patient)
      {
         try
         {
            _dbContext.Patient.Add(patient);
            //Patient DBpatient = _dbContext.Patient.Single(x => x.CPR == patient.CPR);

            //if (DBpatient != null)
            //{
            //   if (DBpatient.Name != patient.Name && patient.Name != null)
            //   {
            //      DBpatient.Name = patient.Name;
            //   }

            //   if (DBpatient.Lastname != patient.Lastname && patient.Lastname != null)
            //   {
            //      DBpatient.Lastname = patient.Lastname;
            //   }

            //   if (DBpatient.Adress != patient.Adress && patient.Adress != null)
            //   {
            //      DBpatient.Adress = patient.Adress;
            //   }

            //   if (DBpatient.City != patient.City && patient.City != null)
            //   {
            //      DBpatient.City = patient.City;
            //   }

            //   if (DBpatient.zipcode != patient.zipcode && patient.zipcode != 0)
            //   {
            //      DBpatient.zipcode = patient.zipcode;
            //   }

            //   if (DBpatient.Age != patient.Age && patient.Age != 0)
            //   {
            //      DBpatient.Age = patient.Age;
            //   }

            //   if (DBpatient.EarCasts != patient.EarCasts && patient.EarCasts != null)
            //   {
            //      DBpatient.EarCasts = patient.EarCasts;
            //   }

            //   if (DBpatient.Email != patient.Email && patient.Email != null)
            //   {
            //      DBpatient.Email = patient.Email;
            //   }

            //   if (DBpatient.MobilNummer != patient.MobilNummer && patient.MobilNummer != null)
            //   {
            //      DBpatient.MobilNummer = patient.MobilNummer;
            //   }

            //   _dbContext.Patient.Update(DBpatient);
            //}

            _dbContext.SaveChanges();
         }
         catch
         {

         }
      }
      

      /// <summary>
      /// Henter den seneste generalspec som er i db tilhørende PCPR'et for både højre og venstre.
      /// Hvis ingen Generalspec i Db returnes null
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public List<GeneralSpec> GetLatestGeneralSpecs(int patientId)
      {
         try
         {
            //Henter genneralspec for højre og venstre øre        //x afgører hvilken kolonne 
            GeneralSpec generalSpecL = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.PatientFK == patientId && x.EarSide == Ear.Left);
            GeneralSpec generalSpecR = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.PatientFK == patientId && x.EarSide == Ear.Right);

            //Placere generalSpec i listen 
            List<GeneralSpec> generalSpecs = new List<GeneralSpec>();
            generalSpecs.Add(generalSpecL); generalSpecs.Add(generalSpecR);

            //Retunere listen. 

            return generalSpecs;

         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// Henter alle GeneralSpecs ind som ligger i databasen tilhørende det givne PCPR. 
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public List<GeneralSpec> GetAllGeneralSpecs(int PatientId)
      {
         try
         {
            List<GeneralSpec> generalSpecs = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).Where(x => x.PatientFK == PatientId).ToList();

            return generalSpecs;
         }
         catch 
         {
            return null;
         }
      }

      /// <summary>
      /// Gemmer en ny Genneralspec i DB og returnere en bool alt efter om den er i DB eller ej.
      /// </summary>
      /// <param name="generalSpec"></param>
      /// <returns></returns>
      public bool CreateNewGeneralSpec(GeneralSpec generalSpec)
      {
         try
         {
            _dbContext.GeneralSpecs.Add(generalSpec);
            _dbContext.SaveChanges();
         }
         catch
         {
            return false;
         }
         return _dbContext.GeneralSpecs.Contains(generalSpec);
      }


      /// <summary>
      /// Laver en ny earcast i Databasen
      /// </summary>
      /// <param name="earCast"></param>
      public void CreateEarCast(EarCast earCast)
      {
         try
         {
            _dbContext.EarCast.Attach(earCast);
            _dbContext.SaveChanges();
         }
         catch
         {

         }
      }

      /// <summary>
      /// Makes no sence to make <3 :*
      /// </summary>
      /// <param name="generalSpec"></param>
      public void UpdateGeneralSpec(GeneralSpec generalSpec)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
