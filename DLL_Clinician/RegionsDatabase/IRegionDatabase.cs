using System;
using System.Collections.Generic;
using System.Text;
using CoreEFTest.Models;

namespace DLL_Clinician.RegionsDatabase
{
   public interface IRegionDatabase
    {
       bool CheckCPR(string CPR);

       Patient GetPatient(string CPR);
    }
}
