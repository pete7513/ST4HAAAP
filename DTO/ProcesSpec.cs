using System;

namespace DTO
{
   public class ProcesSpec
   {
      public DateTime GeneralSpecCreateDateTime { get; set; }

      public int ClinicianId { get; set; }



      public DateTime TechSpecCreateDateTime { get; set; }

      public int TechnicalId { get; set; }




      public bool Printed { get; set; }

      public DateTime PrintDateTime { get; set; }

      public int PrintTechId { get; set; }



      public DateTime scanDateTime { get; set; }

      public int scanTechId { get; set; }



  
   }
}