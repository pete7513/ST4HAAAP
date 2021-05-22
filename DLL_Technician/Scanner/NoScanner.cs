using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CoreEFTest.Models;

namespace DLL_Technician
{
    public class NoScanner:IScanner
    {
        private Random random = new Random();
        private RawEarScan earscan;
        private ITimeStamp timeStamp;

        //Højre øreafstøbning
        private const string MODEL_PATH = "Mold_for_Ear_V1.7_R.stl";

        public NoScanner(ITimeStamp timeStamp)
        {
            this.timeStamp = timeStamp;
        }

        public bool connectTo3DScanner()
        {
            int trigger = random.Next(1, 10);

            if (trigger > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public RawEarScan StartScanning(Ear earside)
        {
            earscan = new RawEarScan();

            byte[] bytes = System.IO.File.ReadAllBytes(MODEL_PATH);

            earscan.Scan = new byte[bytes.Length];
            earscan.Scan = bytes;
            earscan.EarSide = earside;
            earscan.ScanDate = timeStamp.getDate();

            Thread.Sleep(3000);

            return earscan;
        }
    }
}
