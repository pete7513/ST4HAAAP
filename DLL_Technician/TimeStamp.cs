using System;

namespace DLL_Technician
{
    public class TimeStamp : ITimeStamp
    {
        public DateTime getDate()
        {
            return DateTime.Now;
        }
    }
}