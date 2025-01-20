using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesOrganizationSystem.Domain.Entities.RoomEntities
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Number { get; set; } = null!;

        public int? Capacity { get; set; }

        [Required]
        public int SchoolId { get; set; }

        public School School { get; set; } = null!;

        public int? StatusId { get; set; }

        public RoomStatus? Status { get; set; }

        public List<RoomToEquipment> RoomsToEqipments { get; set; } 
            = new List<RoomToEquipment>();

        public List<Equipment> Equipments => 
            RoomsToEqipments
                .Select(roomToEquipment => roomToEquipment.Equipment)
                .ToList();
    }
}
