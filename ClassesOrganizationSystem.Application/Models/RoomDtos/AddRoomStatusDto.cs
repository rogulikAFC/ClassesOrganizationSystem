using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSystem.Application.Models.RoomDtos
{
    public class AddRoomStatusDto
    {
        [Required]
        public bool IsOpened { get; set; }

        public string? Description { get; set; }

        [Required]
        public int RoomId { get; set; }

        public RoomStatus MapToRoomStatus()
        {
            return new RoomStatus
            {
                IsOpened = IsOpened,
                Description = Description,
            };
        }
    }
}
