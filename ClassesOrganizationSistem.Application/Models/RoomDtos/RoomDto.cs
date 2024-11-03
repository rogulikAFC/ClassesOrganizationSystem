using ClassesOrganizationSistem.Application.Models.SchoolDtos;
using ClassesOrganizationSistem.Domain.Entities.RoomEntities;

namespace ClassesOrganizationSistem.Application.Models.RoomDtos
{
    public class RoomDto
    {
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public int? Capacity { get; set; }

        public SchoolDto School { get; set; } = null!;

        public RoomStatusDto Status { get; set; } = null!;

        public IEnumerable<EquipmentDto> Equipments { get; set; }
            = new List<EquipmentDto>();

        public static RoomDto MapFromRoom(Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                Number = room.Number,
                Capacity = room.Capacity,
                School = SchoolDto.MapFromSchool(room.School),
                Status = RoomStatusDto.MapFromRoomStatus(room.Status),
                Equipments = room.Equipments
                    .Select(EquipmentDto.MapFromEquipment)
            };
        }
    }
}
