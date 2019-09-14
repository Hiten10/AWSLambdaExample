﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AWSExample.Models
{
    [Table("Person", Schema = "dbo")]
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "PersonId")]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "PersonName")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "int")]
        [Display(Name = "Age")]
        public int Age { get; set; }
    }
}
