using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEFTest.Models
{
    public class TecnicalSpec
    {
       [Required]
       [Key]
       [MaxLength(10)]
       public int HATechinalSpecID { get; set; }

        [Required]
        public Ear EarSide { get; set; }
        
        [Required]
        public DateTime CreateDate { get; set; } 

        [Required]
        public bool Printed { get; set; }

        [ForeignKey("StaffLoginFK")]
        public StaffLogin StaffLogin { get; set; }
        public int StaffLoginFK { get; set; }

        [ForeignKey("PatientFK")]
        public Patient Patient { get; set; }
        public int PatientFK { get; set; }

        [ForeignKey("GeneralSpecFK")]
        public GeneralSpec GeneralSpec { get; set; }
        public int GeneralSpecFK { get; set; }

        public RawEarScan RawEarScan { get; set; }

        public List<RawEarPrint> EarPrints { get; set; }
    }
}