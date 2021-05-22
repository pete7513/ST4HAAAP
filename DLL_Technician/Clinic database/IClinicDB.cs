using System;
using System.Collections.Generic;
using System.Text;
using CoreEFTest.Models;
using DTO;


namespace DLL_Technician
{
   public interface IClinicDB
   {
      //UC 3.2
      Patient GetPatientWithGeneralSpecAndTechnicalSpec(string CPR);

      //UC3.3
      bool SaveTechnicalSpec(TecnicalSpec techSpec);
      
      //UC4
      Patient GetPatientInformations(string EarCastID);

      bool SaveScan(RawEarScan scan, int PatientId);

      bool UpdateGeneralspec(GeneralSpec generalSpec);

      //UC5
      List<TecnicalSpec> GetTechnicalSpecs(string CPR);

      List<TecnicalSpec> GetEarScans();

      //UC6
      List<ProcesSpec> GetProcesInfo(string CPR);

      bool SavePrint(RawEarPrint rawEarPrint);
   }
}
