using System;
using System.Collections.Generic;
using System.Text;
using CoreEFTest.Models;

namespace DTO
{
    public class FullRawEarPrint
    {
        public int PrintTechID;
        public List<RawEarScan> EarScans;
        public string CPR;
    }
}
