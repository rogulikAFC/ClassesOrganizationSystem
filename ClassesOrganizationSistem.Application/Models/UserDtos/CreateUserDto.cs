﻿using ClassesOrganizationSistem.Domain.Entities.UserEntites;
using ClassesOrganizationSistem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSistem.Application.Models.UserDtos
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public string Surname { get; set; } = null!;

        [Required]
        public int RoleId { get; set; }

        public int? SchoolId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public string UserName { get; set; } = null!;
        
        [Required]
        public string Password { get; set; } = null!;

        public User MapToUser()
        {
            return new User()
            {
                Name = Name,
                Surname = Surname,
                RoleId = RoleId,
                SchoolId = SchoolId,
                Email = Email,
                PhoneNumber = PhoneNumber,
                UserName = UserName,
            };
        }
    }
}
