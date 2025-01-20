﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesOrganizationSystem.Domain.Entities.RoomEntities
{
    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Title { get; set; } = null!;
    }
}
