using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text;
using Newtonsoft.Json;

namespace CoreEFTest.Models
{
    public class StaffLogin
    {
        [Required]
        [Key]
        [MaxLength(10)]
        public int StaffID { get; set; }

        [Required]
        [MaxLength(15)]
        [Column(TypeName = "varchar(15)")]
        public string Name { get; set; }

        [Required]
        [MaxLength(25)]
        [Column(TypeName = "varchar(25)")]
        public string Password { get; set; }

        [Required]
        public Status StaffStatus { get; set; }
    }
}
