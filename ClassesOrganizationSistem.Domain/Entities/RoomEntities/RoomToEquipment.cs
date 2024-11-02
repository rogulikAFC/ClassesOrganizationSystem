using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSistem.Domain.Entities.RoomEntities
{
    [PrimaryKey(nameof(RoomId), nameof(EquipmentId))]
    public class RoomToEquipment
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        public int EquipmentId { get; set; }

        public Room Room { get; set; } = null!;

        public Equipment Equipment { get; set; } = null!;
    }
}
