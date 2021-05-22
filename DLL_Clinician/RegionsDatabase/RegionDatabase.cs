using System;
using CoreEFTest.Models;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using DLL_Clinician.RegionsDatabase;
using Hl7.Fhir.Serialization;
using HL7_FHIR;

namespace DLL_Clinician
{
   public class RegionDatabase : IRegionDatabase
   {
      private FhirClient client;
      HL7FHIRClient client2 = new HL7FHIRClient();


        public RegionDatabase()
      {
         client = new FhirClient("https://aseecest3fhirservice.azurewebsites.net/");
         client2 = new HL7FHIRClient();
         client.Timeout = 120000;
      }

      public bool CheckCPR(string CPR)
      {
          try
          {
              CoreEFTest.Models.Patient patient = GetPatient(CPR);
              if (patient.CPR == CPR)
                  return true;
              else
                  return false;
          }
          catch 
          {
              return false;
          }
      }

        /// <summary>
        /// Metoden finder en patient i HL7-Fhir databasen med det givne PCPR og returnere et patient objekt. 
        /// </summary>
        /// <param name="CPR"></param>
        /// <returns></returns>
        public CoreEFTest.Models.Patient GetPatient(string CPR)
      {
          CoreEFTest.Models.Patient patient = null;

         var con = new SearchParams();

         con.Add("identifier", CPR);

         Bundle result = client.Search<Hl7.Fhir.Model.Patient>(con);

         foreach (Bundle.EntryComponent component in result.Entry)
         {
             patient = new CoreEFTest.Models.Patient(); 
             Hl7.Fhir.Model.Patient Hl7patient = (Hl7.Fhir.Model.Patient)component.Resource;

            patient.CPR = Hl7patient.Identifier[0].Value;
            patient.Name = Hl7patient.Name[0].Text;
            patient.Lastname = Hl7patient.Name[0].Family;
            Date date = Hl7patient.BirthDateElement;
            //patient.Age = date.
            patient.Adress = Hl7patient.Address[0].District;
            patient.City = Hl7patient.Address[0].City;
            patient.zipcode = Convert.ToInt32(Hl7patient.Address[0].PostalCode);
            break;
         }
         return patient;
      }
   }
}
