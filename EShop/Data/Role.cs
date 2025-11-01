﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Data
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        [Required,MinLength(5),MaxLength(100)]
        public string? Description { get; set; } 

        [Required,MinLength(3),MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
