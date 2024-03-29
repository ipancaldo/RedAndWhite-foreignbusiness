﻿using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class User : BaseDomain
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(15)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Surname { get; set; }

        [Required, MaxLength(100)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(50)]
        public string Password { get; set; }

        public virtual List<Role> Roles { get; set; }
    }
}
