using ClassesOrganizationSystem.Domain.Entities.RoomEntities;

namespace ClassesOrganizationSystem.Application.Models.RoomDtos
{
    public class EquipmentDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public static EquipmentDto MapFromEquipment(Equipment equipment)
        {
            return new EquipmentDto
            {
                Id = equipment.Id,
                Title = equipment.Title,
            };
        }
    }
}
