using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSystem.Application.Models.RoomDtos
{
    public class CreateEquipmentDto
    {
        [Required]
        public string Title { get; set; } = null!;

        public Equipment MapToEquipment()
        {
            return new Equipment
            {
                Title = Title,
            };
        }
    }
}
