﻿using ClassesOrganizationSystem.Application.Models.SchoolDtos;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;

namespace ClassesOrganizationSystem.Application.Models.RoomDtos
{
    public class RoomDto
    {
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public int? Capacity { get; set; }

        public SchoolAnnotationDto School { get; set; } = null!;

        public RoomStatusDto? Status { get; set; }

        public List<EquipmentDto> Equipments { get; set; }
            = new List<EquipmentDto>();

        public static RoomDto MapFromRoom(Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                Number = room.Number,
                Capacity = room.Capacity,
                School = SchoolAnnotationDto.MapFromSchool(room.School),
                Status = room.Status != null ? RoomStatusDto.MapFromRoomStatus(room.Status) : null,
                Equipments = room.Equipments
                    .Select(EquipmentDto.MapFromEquipment)
                    .ToList()
            };
        }
    }
}
