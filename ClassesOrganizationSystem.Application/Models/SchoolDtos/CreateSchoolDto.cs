using ClassesOrganizationSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSystem.Application.Models.SchoolDtos
{
    public class CreateSchoolDto
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; } = null!;

        public string? Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public int CreatorId { get; set; }

        public School MapToSchool()
        {
            return new School
            {
                Title = Title,
                Address = Address,
                Phone = Phone,
                Email = Email
            };
        }
    }
}
