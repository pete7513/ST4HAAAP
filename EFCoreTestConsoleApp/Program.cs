using CoreEFTest.Context;
using CoreEFTest.Models;
using Hl7.Fhir.Rest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreTestConsoleApp
{
    class Program
   {
      static void Main(string[] args)
      {
         ClinicDBContext dbContext = new ClinicDBContext();
         ClinicianDBLogic clinicianDbLogic = new ClinicianDBLogic(dbContext);

         ProgramHL7 program = new ProgramHL7();

         program.metode();


         #region Create (CRUD)

         #region Create patient
         Patient newPatient = new Patient()
         {
            CPR = "123456-4321",
            Name = "Perfekt",
            Lastname = "Person",
            Adress = "Testvej 15",
            zipcode = 8200,
            Age = 25,
            City = "TestBy",
            Email = "test@test.dk",
            MobilNummer = "30405060"
         };

         clinicianDbLogic.CreatePatient(newPatient);


         #endregion

         #region Create Staff

         StaffLogin newStaffLogin = new StaffLogin()
         {
            Name = "Kliniker",
            Password = "123",
            StaffStatus = Status.Clinician,
         };

         //clinicianDbLogic.CreateStaffLogin(newStaffLogin);

         StaffLogin nStaffLogin = new StaffLogin()
         {
            Name = "Tekniker",
            Password = "123",
            StaffStatus = Status.Technician
         };

         //clinicianDbLogic.CreateStaffLogin(nStaffLogin);

         #region Create EarCast 2

         EarCast newCast = new EarCast()
         {
            EarSide = Ear.Right,
         };

         //clinicianDbLogic.CreateEarCast(newCast);

         #endregion

         #endregion

         #region Create GeneralSpec

         GeneralSpec newGeneralSpecL = new GeneralSpec()
         {
            //CPR = "111111-1111",
            Type = Material.AntiAllergi,
            Color = PlugColor.Honey,
            EarSide = Ear.Left,
            CreateDate = DateTime.Now,
            StaffLoginFK = 1,
         };

         //clinicianDbLogic.CreateNewGeneralSpec(newGeneralSpecL);

         GeneralSpec newGeneralSpecR = new GeneralSpec()
         {
            //CPR = "111111-1111",
            Type = Material.AntiAllergi,
            Color = PlugColor.Honey,
            EarSide = Ear.Right,
            CreateDate = DateTime.Now,
            StaffLoginFK = 1,
         };

         //clinicianDbLogic.CreateNewGeneralSpec(newGeneralSpecR);

         #endregion

         #region Create TecnicalSpec

         TecnicalSpec newTecnicalSpecR = new TecnicalSpec()
         {
            EarSide = Ear.Right,
            CreateDate = DateTime.Now,
            StaffLoginFK = 1,
            //CPR = "111111-1111"
         };

         //clinicianDbLogic.CreateTechnicalSpec(newTecnicalSpecR);

         TecnicalSpec newTecnicalSpecL= new TecnicalSpec()
         {
            EarSide = Ear.Left,
            CreateDate = DateTime.Now,
            StaffLoginFK = 1,
            //CPR = "111111-1111",
         };

         //clinicianDbLogic.CreateTechnicalSpec(newTecnicalSpecL);

         #endregion

         #region Create rawEarPrint

         RawEarPrint print = new RawEarPrint()
         {
            EarSide = Ear.Left,
            StaffLoginFK = 1,
            PrintDate = DateTime.Now,
         };

         //clinicianDbLogic.SavePrint(print, "111111-1111");

         #endregion

         #endregion

         #region Delete (CRUD)

         #region Delete patient

         //Patient DeletePatient = new Patient()
         //{
         //   PCPR = "110396-0000",
         //};

         //clinicianDbLogic.DeletePatient(DeletePatient);

         #endregion

         #region Delete EarCast

         //EarCast DeleteEarCast = new EarCast()
         //{
         //   EarCastID = 2,
         //};

         // clinicianDbLogic.DeleteEarCast(DeleteEarCast);

         #endregion

         #endregion

         #region Update (CRUD)

         #region Update patient

         //Patient UpdatePatient = new Patient()
         //{
         //   PCPR = "110396-0000",
         //   Lastname = "Nedergaard Enevoldsen",
         //   Adress = "Trøjbordvej 72",

         //};

         //clinicianDbLogic.UpdatePatient(UpdatePatient);

         #endregion

         #endregion

         #region Retrive (CRUD)

         #region Retrieve Alle patienter

         //List<Patient> Patients = clinicianDbLogic.GetAllPatients();

         //foreach (Patient patient in Patients)
         //{
         //   Console.WriteLine(patient.Name);
         //}

         #endregion

         #region Retrieve Patient tilhørende øre afstøbning

         //Patient Patient = clinicianDbLogic.GetPatientFromEarCast(2);

         //Console.WriteLine(Patient.Name);

         #endregion

         #region Create afstøbning

         //EarCast earCast = clinicianDbLogic.Get
         //Patient Patient = clinicianDbLogic.GetPatient("250997-0000");

         // Console.WriteLine(Patient.Name);

         #endregion

         #region Retrieve Patient tilhørende øre afstøbning

         //Patient earPatient = clinicianDbLogic.GetPatientWithEarCast("250997-0000");

         //foreach (EarCast earPatientEarCast in earPatient.EarCasts)
         //{
         //    Console.WriteLine($"PCPR: {earPatientEarCast.PCPR} Øre side: {earPatientEarCast.EarSide.ToString()} ID: {earPatientEarCast.EarCastID}");
         //}

         #endregion

         #region Hent en patient med alle parametre udfyldt

         //Patient patient = clinicianDbLogic.GetPatientWithGeneralSpecAndTechnicalSpec("111111-1111");

         #endregion


         #endregion


      }
   }


   internal class ClinicianDBLogic
   {
      private readonly ClinicDBContext _dbContext;

      public ClinicianDBLogic(ClinicDBContext dbContext)
      {
         _dbContext = dbContext;
      }
  /// <summary>
      /// 
      /// </summary>
      /// <param name="earCast"></param>
      public void CreateEarCast(EarCast earCast)
      {
         try
         {
            _dbContext.EarCast.Add(earCast);
            _dbContext.SaveChanges();
         }
         catch 
         {

         }
      }
      #region Patient

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public List<Patient> GetAllPatients()
      {
         List<Patient> Patient = _dbContext.Patient.ToList();

         return Patient;
      }

        /// <summary>
        /// Metoden bliver benyttet til at hente en patient fra DB der passer til det pågældende PCPR
        /// og det nyeste technical- og generalspec for hvert øre fra databasen tilhørende patienten 
        /// og returnere et patient objekt.
        /// </summary>
        /// <param name="CPR"></param>
        /// <returns></returns>
        public Patient GetPatientWithGeneralSpecAndTechnicalSpec(int PatientId)
        {
            Patient patient = _dbContext.Patient.Single(x => x.PatientId == PatientId);

            TecnicalSpec TechspecL = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate)
               .Last(x => x.PatientFK == PatientId && x.EarSide == Ear.Left);
            TecnicalSpec TechspecR = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate)
               .Last(x => x.PatientFK == PatientId && x.EarSide == Ear.Right);

            patient.TecnicalSpecs = new List<TecnicalSpec>() { TechspecR, TechspecL };

            GeneralSpec GenSpecL = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).Last(x =>
               x.PatientFK == PatientId && x.EarSide == Ear.Left && x.HAGeneralSpecID == TechspecL.GeneralSpecFK);
            GeneralSpec GenSpecR = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).Last(x =>
               x.PatientFK == PatientId && x.EarSide == Ear.Right && x.HAGeneralSpecID == TechspecR.GeneralSpecFK);

            patient.GeneralSpecs = new List<GeneralSpec>() { GenSpecR, GenSpecL };

            return patient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public Patient GetPatient(int patientId)
      {
         Patient patient = _dbContext.Patient.Single(x => x.PatientId == patientId);

         return patient;
      }

      public Patient GetPatientWithEarCast(string CPR)
      {
         Patient patient = _dbContext.Patient.Single(x => x.CPR == CPR);
         patient.EarCasts = _dbContext.EarCast.Where(x => x.PatientFK == patient.PatientId).ToList();

         return patient;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="patient"></param>
      public void CreatePatient(Patient patient)
      {
         _dbContext.Patient.Add(patient);
         _dbContext.SaveChanges();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="patient"></param>
      public void DeletePatient(Patient patient)
      {
         _dbContext.Patient.Remove(patient);
         _dbContext.SaveChanges();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="patient"></param>
      public void UpdatePatient(Patient patient)
      {
         Patient DBpatient = _dbContext.Patient.Find(patient.CPR);
         if (DBpatient != null)
         {
            if (DBpatient.Name != patient.Name && patient.Name != null)
            {
               DBpatient.Name = patient.Name;
            }

            if (DBpatient.Lastname != patient.Lastname && patient.Lastname != null)
            {
               DBpatient.Lastname = patient.Lastname;
            }

            if (DBpatient.Adress != patient.Adress && patient.Adress != null)
            {
               DBpatient.Adress = patient.Adress;
            }

            if (DBpatient.City != patient.City && patient.City != null)
            {
               DBpatient.City = patient.City;
            }

            if (DBpatient.zipcode != patient.zipcode && patient.zipcode != 0)
            {
               DBpatient.zipcode = patient.zipcode;
            }

            if (DBpatient.Age != patient.Age && patient.Age != 0)
            {
               DBpatient.Age = patient.Age;
            }

            if (DBpatient.EarCasts != patient.EarCasts && patient.EarCasts != null)
            {
               DBpatient.EarCasts = patient.EarCasts;
            }

            _dbContext.Patient.Update(DBpatient);
         }

         _dbContext.SaveChanges();
      }

      #endregion

      #region EarCast

    

      /// <summary>
      /// 
      /// </summary>
      /// <param name="earCastID"></param>
      /// <returns></returns>
      public Patient GetPatientFromEarCast(int earCastID)
      {
         EarCast earCast = _dbContext.EarCast.Single(x => x.EarCastID == earCastID);
         Patient patient = GetPatient(earCast.PatientFK);

         return patient;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="earCast"></param>
      public void DeleteEarCast(EarCast earCast)
      {
         _dbContext.EarCast.Remove(earCast);
         _dbContext.SaveChanges();
      }

      #endregion

      #region StaffLogin

      public StaffLogin CheckLogin(int staffID, string pw)
      {
         StaffLogin staffLogin = _dbContext.StaffLogin.Find(staffID);

         try
         {
            if (staffLogin.Password == pw)
            {
               if (staffLogin.StaffStatus == Status.Clinician)
               {
                  // åben cliniker vindue
               }
               else if (staffLogin.StaffStatus == Status.Technician)
               {
                  // åben teknikker vindue
               }
            }
         }
         catch
         {
            Console.Write("Database not connected or data not found");
         }

         return staffLogin;
      }

      #endregion

      #region Create StaffLogin

      public void CreateStaffLogin(StaffLogin staffLogin)
      {
         _dbContext.StaffLogin.Add(staffLogin);
         _dbContext.SaveChanges();
      }

      #endregion

      #region Create GeneralSpec

      public void CreateGeneralSpec(GeneralSpec generalSpec)
      {
         _dbContext.GeneralSpecs.Add(generalSpec);
         _dbContext.SaveChanges();

      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public List<GeneralSpec> GetAlleGeneralSpecs(int PatientId)
      {
         List<GeneralSpec> generalSpecs = _dbContext.GeneralSpecs.Where(x => x.PatientFK == PatientId).ToList();

         return generalSpecs;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="generalSpec"></param>
      /// <returns></returns>
      public bool CreateNewGeneralSpec(GeneralSpec generalSpec)
      {
         _dbContext.GeneralSpecs.Add(generalSpec);
         _dbContext.SaveChanges();

         return _dbContext.GeneralSpecs.Contains(generalSpec);
      }

      /// <summary>
      /// Henter den seneste generalspec som er i db tilhørende PCPR'et for både højre og venstre.
      /// Hvis ingen Generalspec i Db returnes null
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public List<GeneralSpec> GetLatestGeneralSpecs(int patientid)
      {
         try
         {
            //Henter genneralspec for højre og venstre øre
            GeneralSpec generalSpecL = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate)
               .LastOrDefault(x => x.PatientFK == patientid && x.EarSide == Ear.Left);
            GeneralSpec generalSpecR = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate)
               .LastOrDefault(x => x.PatientFK == patientid && x.EarSide == Ear.Right);

            //Placere generalSpec i listen 
            List<GeneralSpec> generalSpecs = new List<GeneralSpec>();
            generalSpecs.Add(generalSpecL);
            generalSpecs.Add(generalSpecR);

            //Retunere listen. 

            return generalSpecs;

         }
         catch
         {
            return null;
         }
      }

      #endregion

      #region Create TechnicalSpec

      public void CreateTechnicalSpec(TecnicalSpec techSpec)
      {
         GeneralSpec generalSpec = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate)
            .Last(x => x.PatientFK == techSpec.PatientFK && x.EarSide == techSpec.EarSide);

         techSpec.GeneralSpecFK = generalSpec.HAGeneralSpecID;

         _dbContext.TecnicalSpecs.Add(techSpec);
         _dbContext.SaveChanges();
      }

      #endregion

      #region EarPrint

      public bool SavePrint(RawEarPrint rawEarPrint, int PatientId)
      {
         try
         {
            //Henter specifik techspec tilhørende det givne RawEarPrint og PCPR. 
            TecnicalSpec Techspec =
               _dbContext.TecnicalSpecs.Single((x => x.PatientFK == PatientId && x.EarSide == rawEarPrint.EarSide));

            //Sætter id i RawEarPrint
            rawEarPrint.TecnicalSpecFK = Techspec.HATechinalSpecID;

            //Gemmer RawEarPrint
            _dbContext.RawEarPrints.Add(rawEarPrint);
            _dbContext.SaveChanges();

            // Sætter printed parameteren til true
            Techspec.Printed = true;

            _dbContext.TecnicalSpecs.Update(Techspec);
            _dbContext.SaveChanges();

            return true;
         }
         catch
         {
            return false;
         }

         #endregion
      }
   }
}
