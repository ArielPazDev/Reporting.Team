﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.API.RESTful.Models
{
    [Table("Roles")]
    public class RolModel
    {
        [Key]
        public int IDRol { get; set; }

        public string Name { get; set; }
    }
}