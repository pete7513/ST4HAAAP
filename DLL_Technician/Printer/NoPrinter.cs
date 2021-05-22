using System;
using System.Collections.Generic;
using System.Text;
using CoreEFTest.Models;
using System.Threading;

namespace DLL_Technician.Printer
{
    public class NoPrinter : IPrinter
    {
        private Random random = new Random();
        private RawEarPrint earPrint;
        private ITimeStamp timeStamp;

        public NoPrinter(ITimeStamp timeStamp)
        {
            this.timeStamp = timeStamp;
        }
        public bool connectToPrinter()
        {
            int trigger = random.Next(1, 10);

            if (trigger > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public RawEarPrint StartPrint(int printTechID, List<RawEarScan> earScans)
        {
            earPrint = new RawEarPrint();

            earPrint.StaffLoginFK = printTechID;
            earPrint.PrintDate = timeStamp.getDate();

            Thread.Sleep(3000);

            return earPrint;
        }
    }
}
