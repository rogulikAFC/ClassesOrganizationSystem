using ClassesOrganizationSistem.Domain.Entities.RoomEntities;

namespace ClassesOrganizationSistem.Application.Models.RoomDtos
{
    public class RoomStatusDto
    {
        public bool IsOpened { get; set; } = true;

        public string? Description { get; set; }

        public static RoomStatusDto MapFromRoomStatus(RoomStatus roomStatus)
        {
            return new RoomStatusDto
            {
                IsOpened = roomStatus.IsOpened,
                Description = roomStatus.Description,
            };
        }
    }
}
