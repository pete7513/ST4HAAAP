using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace CoreEFTest.Models
{
    public class GeneralSpec
    {
        [Required]
        [Key]
        [MaxLength(10)]
        public int HAGeneralSpecID { get; set; }

        [Required]
        public Material Type { get; set; }

        [Required]
        public PlugColor Color { get; set;}

        [Required]
        public Ear EarSide { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [ForeignKey("PatientFK")]
        public Patient Patient { get; set; }
        public int PatientFK { get; set; }

        [ForeignKey("StaffLoginFK")]
        public StaffLogin StaffLogin { get; set; }
        public int StaffLoginFK { get; set; }

        List<TecnicalSpec> tecnicalSpecslist { get; set; }


}
}