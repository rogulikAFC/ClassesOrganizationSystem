using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSystem.Application.Models.RoomDtos
{
    public class CreateRoomDto
    {
        [Required]
        [MaxLength(32)]
        public string Number { get; set; } = null!;

        public int? Capacity { get; set; }

        [Required]
        public int SchoolId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public IEnumerable<int> Equipments { get; set; }
            = new List<int>();

        public Room MapToRoom()
        {
            return new Room
            {
                Number = Number,
                Capacity = Capacity,
                SchoolId = SchoolId,
                StatusId = StatusId
            };
        }
    }
}
