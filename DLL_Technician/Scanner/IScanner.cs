using System;
using System.Collections.Generic;
using System.Text;
using CoreEFTest.Models;

namespace DLL_Technician
{
    public interface IScanner
    {
        bool connectTo3DScanner();
        RawEarScan StartScanning(Ear earside);

    }
}
