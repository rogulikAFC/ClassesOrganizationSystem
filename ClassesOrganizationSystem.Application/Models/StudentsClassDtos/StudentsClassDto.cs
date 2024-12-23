﻿using ClassesOrganizationSystem.Domain.Entities;

namespace ClassesOrganizationSystem.Application.Models.StudentsClassDtos
{
    public class StudentsClassDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int StudentsCount { get; set; }

        public static StudentsClassDto MapFromStudentsClass(
            StudentsClass studentsClass)
        {
            return new StudentsClassDto
            {
                Id = studentsClass.Id,
                Title = studentsClass.Title,
                StudentsCount = studentsClass.Students.Count()
            };
        }
    }
}
