using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreEFTest.Models
{
   public class RawEarPrint
   {
      
      [Required]
      [Key]
      public int EarPrintID { get; set; }

      [Required]
      public Ear EarSide { get; set; }

      [Required]
      public DateTime PrintDate { get; set; } = DateTime.Now.Date;

      [ForeignKey("StaffLoginFK")]
      public StaffLogin StaffLogin { get; set; }
      public int StaffLoginFK { get; set; }

      [ForeignKey("TecnicalSpecFK")]
      public TecnicalSpec TecnicalSpec { get; set; }
      public int TecnicalSpecFK { get; set; }

      

   }
}
