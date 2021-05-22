using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreEFTest.Context;
using CoreEFTest.Models;
using DTO;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DLL_Technician
{
   public class ClinicDB : IClinicDB
   {
      private readonly ClinicDBContext _dbContext;

      public ClinicDB(ClinicDBContext dbContext)
      {
         _dbContext = new ClinicDBContext();
            //= dbContext;
      }

      /// <summary>
      /// Metoden bliver benyttet til at hente en patient fra DB der passer til det pågældende PCPR
      /// og returnere et patient objekt.
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public Patient GetPatient(string CPR)
      {
         try
         {
            Patient patient = _dbContext.Patient.Single(x => x.CPR == CPR);

            return patient;
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// Metoden bliver benyttet til at hente en patient fra DB der passer til det pågældende PCPR
      /// og det nyeste technical- og generalspec for hvert øre fra databasen tilhørende patienten 
      /// og returnere et patient objekt.
      /// </summary>
      /// <param name="Patientid"></param>
      /// <returns></returns>
      public Patient GetPatientWithGeneralSpecAndTechnicalSpec(string CPR)
      {
         Patient patient = new Patient();
         try
         {
            patient = _dbContext.Patient.Single(x => x.CPR == CPR);

            TecnicalSpec TechspecL = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.PatientFK == patient.PatientId && x.EarSide == Ear.Left);
            TecnicalSpec TechspecR = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.PatientFK == patient.PatientId && x.EarSide == Ear.Right);

            patient.TecnicalSpecs = new List<TecnicalSpec>() { TechspecL, TechspecR};

            GeneralSpec GenSpecL = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.PatientFK == patient.PatientId && x.EarSide == Ear.Left/* && x.HAGeneralSpecID == TechspecL.GeneralSpecFK*/);
            GeneralSpec GenSpecR = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.PatientFK == patient.PatientId && x.EarSide == Ear.Right /*&& x.HAGeneralSpecID == TechspecR.GeneralSpecFK*/);

            patient.GeneralSpecs = new List<GeneralSpec>() { GenSpecL, GenSpecR };
            return patient;
         }
         catch
         {
            return patient;
         }
      }

      /// <summary>
      /// Metoden benyttes til at gemme en TechinalSpec, og returnere efterfølgende en bool
      /// hvorvidt den er gemt i DB eller ej
      /// </summary>
      /// <param name="techSpec"></param>
      /// <returns></returns>
      public bool SaveTechnicalSpec(TecnicalSpec techSpec)
      {
         //try
         //{
            TecnicalSpec tecnicalSpec = techSpec;
            GeneralSpec generalSpec = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).Last(x => x.PatientFK == techSpec.PatientFK && x.EarSide == techSpec.EarSide);

            tecnicalSpec.GeneralSpecFK = generalSpec.HAGeneralSpecID;
            //tecnicalSpec.GeneralSpec = generalSpec;

            _dbContext.TecnicalSpecs.Add(tecnicalSpec);
            _dbContext.SaveChanges();

            return _dbContext.TecnicalSpecs.Contains(techSpec);
      //}
      //   catch
      //   {
      //      return false;
      //   }
      }

      /// <summary>
      /// Der gemmes et specifikt rawEarScan i DB og efterfølgende returneres en bool som fortæller om det er gjort.
      /// </summary>
      /// <param name="rawEarPrint"></param>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public bool SavePrint(RawEarPrint rawEarPrint, int PatientId)
      {
         try
         {
            //Henter specifik techspec tilhørende det givne RawEarPrint og PCPR. 
            TecnicalSpec Techspec = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).Last(x => x.PatientFK == PatientId && x.EarSide == rawEarPrint.EarSide);

            //Sætter id i RawEarPrint
            rawEarPrint.TecnicalSpecFK = Techspec.HATechinalSpecID;

            //Gemmer RawEarPrint
            _dbContext.RawEarPrints.Add(rawEarPrint);
            _dbContext.SaveChanges();

            // Sætter printed parameteren til true
            Techspec.Printed = true;

            _dbContext.TecnicalSpecs.Update(Techspec);
            _dbContext.SaveChanges();

            return _dbContext.RawEarPrints.Contains(rawEarPrint);
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// Er ikke implementeret. 
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public bool DeleteHA(string CPR)
      {
         return false;
      }

      /// <summary>
      /// ID på earcast bliver brugt til at hente informationer omkring en patient.
      /// Den patient som passer med det specifikke earcast ID returneres.
      /// </summary>
      /// <param name="EarCastID"></param>
      /// <returns></returns>
      public Patient GetPatientInformations(string EarCastID)
      {
         try
         {
            int earCastId = Convert.ToInt32(EarCastID);
            EarCast earCast = _dbContext.EarCast.OrderBy(x => x.CastDate).Last(x => x.EarCastID == earCastId);
            Patient patient = _dbContext.Patient.Single(x => x.PatientId == earCast.PatientFK);

            patient.EarCasts.Add(earCast); 

            return patient;
         }
         catch
         {
            return null;
         }
      }


      /// <summary>
      /// Der gemmes et specikt earscan i DB og efterfølgende returneres en bool som fortæller om det er gjort.
      /// </summary>
      /// <param name="scan"></param>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public bool SaveScan(RawEarScan scan, int PatientId)
      {
         try
         {
            //Find det specifikke scans techspec
            TecnicalSpec Techspec = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).Last((x => x.PatientFK == PatientId && x.EarSide == scan.EarSide));

            scan.TecnicalSpecFK = Techspec.HATechinalSpecID;

            _dbContext.RawEarScans.Add(scan);
            _dbContext.SaveChanges();

            //RawEarScan rawEarScan = _dbContext.RawEarScans.OrderBy(x => x.ScanDate).Last(x => x.TecnicalSpecFK == Techspec.HATechinalSpecID && x.EarSide == Techspec.EarSide);

            //Techspec.ScanID = rawEarScan.ScanID;

            //_dbContext.TecnicalSpecs.Add(Techspec);
            //_dbContext.SaveChanges();

            return _dbContext.RawEarScans.Contains(scan);
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="generalSpec"></param>
      /// <returns></returns>
      public bool UpdateGeneralspec(GeneralSpec generalSpec)
      {
         try
         {
            _dbContext.GeneralSpecs.Update(generalSpec);
            return _dbContext.GeneralSpecs.Contains(generalSpec);
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// Der hentes et earscan fra DB ud fra et specifikt PCPR.
      /// Metoden returnerer en liste der indeholder scanning for både venstre og højre øre.
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public List<TecnicalSpec> GetTechnicalSpecs(string CPR)
      {
         try
         {
            Patient patient = GetPatient(CPR);
            TecnicalSpec TechspecL = new TecnicalSpec(); 
            try
            {
               //Henter TechSpec for V og H øre
                TechspecL = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).Last(x => x.PatientFK == patient.PatientId && x.EarSide == Ear.Left);
               //Henter Earscan for V og H øre 
               TechspecL.RawEarScan = _dbContext.RawEarScans.Single(x => x.TecnicalSpecFK == TechspecL.HATechinalSpecID);
               //Henter generelsepc for Techspec
               TechspecL.GeneralSpec = _dbContext.GeneralSpecs.Single(x => x.HAGeneralSpecID == TechspecL.GeneralSpecFK);

            }
            catch 
            {
             
            }

            TecnicalSpec TechspecR = new TecnicalSpec();
            try
            {
               TechspecR = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).Last(x => x.PatientFK == patient.PatientId && x.EarSide == Ear.Right);

               TechspecR.RawEarScan = _dbContext.RawEarScans.Single(x => x.TecnicalSpecFK == TechspecR.HATechinalSpecID);

               TechspecR.GeneralSpec = _dbContext.GeneralSpecs.Single(x => x.HAGeneralSpecID == TechspecR.GeneralSpecFK);
            }
            catch 
            {
             
            }
            
            //Oprettelse af listen
            List<TecnicalSpec> Techspec = new List<TecnicalSpec>(2);

            //Tilføjelse af techspec objekterne til listen 
            Techspec.Add(TechspecL); Techspec.Add(TechspecR);

            //Return the list;
            return Techspec;
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// Benyttes til at returnere en liste med alle earscan fra DB der endnu ikke er printet.
      /// </summary>
      /// <returns></returns>
      public List<TecnicalSpec> GetEarScans()
      {
         try
         {
            //Henter alle techSpec ud af DB'en
            List<TecnicalSpec> DBlist = _dbContext.TecnicalSpecs.ToList();

            //Oprettelse af listen, som skal returneres
            List<TecnicalSpec> TechSpeclist = new List<TecnicalSpec>();

            //For hver techspec som er i listen hentet fra database, tjekkes der på om Prited = false
            foreach (TecnicalSpec tecnicalSpec in DBlist)
            {
               //Hvis Printed = false, hentes generalspec og det tilknyttede Earscan
               //og lægges i techspec objektet og objektet tilføjes returneringslisten
               if (!tecnicalSpec.Printed)
               {
                  tecnicalSpec.RawEarScan = _dbContext.RawEarScans.Single(x => x.TecnicalSpecFK == tecnicalSpec.HATechinalSpecID);
                  tecnicalSpec.GeneralSpec = _dbContext.GeneralSpecs.Single(x => x.HAGeneralSpecID == tecnicalSpec.GeneralSpecFK);
                  TechSpeclist.Add(tecnicalSpec);
               }
            }
            return TechSpeclist;
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// Henter Process informationerne til det give PCPR. Metoden vil altid hente de nyeste informationer. 
      /// </summary>
      /// <param name="CPR"></param>
      /// <returns></returns>
      public List<ProcesSpec> GetProcesInfo(string CPR)
      {
         Patient patient = GetPatient(CPR);
         List<ProcesSpec> procesSpecs = new List<ProcesSpec>();
         ProcesSpec procesSpecL = new ProcesSpec();
         ProcesSpec procesSpecR = new ProcesSpec();

         try
         {
            try
            {
               //Venstre
               //Henter generalspec
               GeneralSpec generalSpecL = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.PatientFK == patient.PatientId && x.EarSide == Ear.Left);
               if (generalSpecL.CreateDate != null)
               {
                  procesSpecL.GeneralSpecCreateDateTime = generalSpecL.CreateDate;
                  procesSpecL.ClinicianId = generalSpecL.StaffLoginFK;
                  TecnicalSpec TechspecL = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.GeneralSpecFK == generalSpecL.HAGeneralSpecID);

                  //Henter techspec
                  if (TechspecL.CreateDate != null)
                  {
                     procesSpecL.TechSpecCreateDateTime = TechspecL.CreateDate;
                     procesSpecL.TechnicalId = TechspecL.StaffLoginFK;
                     procesSpecL.Printed = TechspecL.Printed;

                     //Henter EarScan
                     TechspecL.RawEarScan = _dbContext.RawEarScans.OrderBy(x => x.ScanDate).LastOrDefault(x => x.TecnicalSpecFK == TechspecL.HATechinalSpecID);
                     if (TechspecL.RawEarScan != null)
                     {
                        procesSpecL.scanTechId = TechspecL.StaffLoginFK;
                        procesSpecL.scanDateTime = TechspecL.CreateDate;
                     }

                     //Henter Earprint
                     if (procesSpecL.Printed)
                     {
                        RawEarPrint rawEarPrint = _dbContext.RawEarPrints.OrderBy(x => x.PrintDate).LastOrDefault(x => x.TecnicalSpecFK == TechspecL.HATechinalSpecID && x.EarSide == Ear.Left);
                        ;
                        procesSpecL.PrintDateTime = rawEarPrint.PrintDate;
                        procesSpecL.PrintTechId = rawEarPrint.StaffLoginFK;
                     }
                  }
               }

               procesSpecs.Add(procesSpecL);
            }
            catch 
            {
               procesSpecs.Add(procesSpecL);
            }

            try
            {
               //Højre
               //Henter GeneralSpec
               GeneralSpec generalSpecR = _dbContext.GeneralSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.PatientFK == patient.PatientId && x.EarSide == Ear.Right);
               if (generalSpecR.CreateDate != null)
               {
                  procesSpecR.GeneralSpecCreateDateTime = generalSpecR.CreateDate;
                  procesSpecR.ClinicianId = generalSpecR.StaffLoginFK;
                  TecnicalSpec TechspecR = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).LastOrDefault(x => x.GeneralSpecFK == generalSpecR.HAGeneralSpecID);

                  //Henter techspec
                  if (TechspecR.CreateDate != null)
                  {
                     procesSpecR.TechSpecCreateDateTime = TechspecR.CreateDate;
                     procesSpecR.TechnicalId = TechspecR.StaffLoginFK;
                     procesSpecR.Printed = TechspecR.Printed;

                     //Henter EarScan
                     TechspecR.RawEarScan = _dbContext.RawEarScans.OrderBy(x => x.ScanDate).LastOrDefault(x => x.TecnicalSpecFK == TechspecR.HATechinalSpecID);
                     if (TechspecR.RawEarScan != null)
                     {
                        procesSpecR.scanTechId = TechspecR.StaffLoginFK;
                        procesSpecR.scanDateTime = TechspecR.CreateDate;
                     }

                     //Henter Earprint
                     if (procesSpecR.Printed)
                     {
                        RawEarPrint rawEarPrint = _dbContext.RawEarPrints.OrderBy(x => x.PrintDate).LastOrDefault(x => x.TecnicalSpecFK == TechspecR.HATechinalSpecID && x.EarSide == Ear.Right);
                        procesSpecR.PrintDateTime = rawEarPrint.PrintDate;
                        procesSpecR.PrintTechId = rawEarPrint.StaffLoginFK;
                     }
                  }
               }

               procesSpecs.Add(procesSpecR);
            }
            catch 
            {
               procesSpecs.Add(procesSpecR);
            }

            return procesSpecs;
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="rawEarPrint"></param>
      /// <returns></returns>
      public bool SavePrint(RawEarPrint rawEarPrint)
      {
         try
         {
            _dbContext.RawEarPrints.Add(rawEarPrint);
            _dbContext.SaveChanges();

            TecnicalSpec tecnical = _dbContext.TecnicalSpecs.OrderBy(x => x.CreateDate).Last(x => x.HATechinalSpecID == rawEarPrint.TecnicalSpecFK && x.EarSide == rawEarPrint.EarSide);

            tecnical.Printed = true;

            _dbContext.TecnicalSpecs.Update(tecnical);
            _dbContext.SaveChanges();


            return _dbContext.RawEarPrints.Contains(rawEarPrint);
         }
         catch
         {
            return false;
         }

      }
   }
}
