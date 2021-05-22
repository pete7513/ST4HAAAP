using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace HL7_FHIR
{
   public class HL7FHIRClient
   {
      private FhirClient client;

      public HL7FHIRClient()
      {
         client = new FhirClient("https://aseecest3fhirservice.azurewebsites.net/");
         //client = new FhirClient("https://vonk.fire.ly");
         client.Timeout = 120000;
      }

      public string CreateHl7FHIRPatient(Patient model)
      {
         Patient models = client.Create<Patient>(model);

         return models.Id;
      }

      public Patient ReadHl7FHIRPatientByID(string id)
      {
         var location = new Uri("https://aseecest3fhirservice.azurewebsites.net/Patient/" + id);
         var patient = client.Read<Patient>(location);

         return patient;
      }

      public Patient ReadHl7FHIRPatientByName(string name)
      {
         //var location = new Uri("https://aseecest3fhirservice.azurewebsites.net/Patient?name=" + name);
         //var patient = client.Read<Patient>(location);

         //SearchParameter pram = new SearchParameter();

         SearchParams prammer = new SearchParams("/Patient?name=Asbjørn", "0");
         Bundle result = client.Search(prammer);


         foreach (Bundle.EntryComponent component in result.Entry)
         {
            try
            {
               Patient patient = (Patient) component.Resource;
               Console.WriteLine(patient.Name[0].ToString());
            }
            catch (Exception e)
            {

            }
         }

         return new Patient();
      }

      public Patient FindPatientByCPR(string CPR)
      {
         var con = new SearchParams();
         con.Add("identifier", CPR);

         Bundle result = client.Search<Patient>(con);

         foreach (Bundle.EntryComponent component in result.Entry)
         {
            Patient patient = (Patient) component.Resource;

            Console.WriteLine(patient.Name[0].ToString());
         }

         return new Patient();
      }

      public Patient FindPatientByCPRTry(string CPR)
      {
         Bundle result = client.Search<Patient>(null);

         foreach (Bundle.EntryComponent component in result.Entry)
         {
            Patient patient = (Patient) component.Resource;

            Console.WriteLine(patient.Name[0].ToString());
         }

         return new Patient();
      }

      public IEnumerable<Patient> GetObservationsByName(string text)
      {
         var searchParameters = new SearchParams();
         searchParameters.Add("identifier.value", text);

         var results = client.Search(searchParameters, ResourceType.Patient.ToString());
         return results.Entry.Select(s => (Patient) s.Resource);
      }

      public IEnumerable<Patient> GetPatientsByName(string firstName, string lastName)
      {
         var searchParameters = new SearchParams();
         searchParameters.Add("Given", firstName);
         searchParameters.Add("Family", lastName);

         var matches = new List<Patient>();
         var result = client.Search(searchParameters, ResourceType.Patient.ToString());
         return result.Entry.Select(x => (Patient)x.Resource);
      }

   }
}
