using System;
using System.Collections.Generic;
using System.Text;
using DLL_Technician;
using DTO;

namespace ShowProcess
{
    public class UC6_ShowProcess
    {
        private IClinicDB clinicDB;
        public UC6_ShowProcess(IClinicDB clinicDb)
        {
            clinicDB = clinicDb;
        }

        public List<ProcesSpec> GetProccesInformations(string CPR)
        {
            return clinicDB.GetProcesInfo(CPR);
        }
    }
}
