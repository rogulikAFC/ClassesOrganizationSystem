﻿using ClassesOrganizationSystem.Application.Models.SchoolDtos;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Role { get; set; } = null!;

        public IEnumerable<SchoolAnnotationDto> Schools { get; set; }
            = new List<SchoolAnnotationDto>();

        public static UserDto MapFromUser(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role.Name,
                Schools = user.Schools
                    .Select(SchoolAnnotationDto.MapFromSchool)
            };
        }
    }
}
