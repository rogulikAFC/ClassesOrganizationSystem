using ClassesOrganizationSystem.Domain.Entities.RoomEntities;

namespace ClassesOrganizationSystem.Application.Models.RoomDtos
{
    public class RoomAnnotationDto
    {
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public int SchoolId { get; set; }

        public static RoomAnnotationDto MapFromRoom(Room room)
        {
            return new RoomAnnotationDto
            {
                Id = room.Id,
                Number = room.Number,
                SchoolId = room.SchoolId,
            };
        }
    }
}
