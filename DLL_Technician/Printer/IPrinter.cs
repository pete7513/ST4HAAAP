using System;
using System.Collections.Generic;
using System.Text;
using CoreEFTest.Models;

namespace DLL_Technician.Printer
{
    public interface IPrinter
    {
        bool connectToPrinter();
        RawEarPrint StartPrint(int ScanTechID, List<RawEarScan>earScans);
    }
}
