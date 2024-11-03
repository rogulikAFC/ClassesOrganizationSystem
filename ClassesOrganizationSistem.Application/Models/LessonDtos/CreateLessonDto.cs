﻿using ClassesOrganizationSistem.Domain.Entities.ScheduleEntites;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSistem.Application.Models.LessonDtos
{
    public class CreateLessonDto
    {
        [Required]
        public int SerialNumber { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int LessonsScheduleId { get; set; }

        public Lesson MapToLesson()
        {
            return new Lesson
            {
                SerialNumber = SerialNumber,
                TeacherId = TeacherId,
                RoomId = RoomId,
                SubjectId = SubjectId,
            };
        }
    }
}
