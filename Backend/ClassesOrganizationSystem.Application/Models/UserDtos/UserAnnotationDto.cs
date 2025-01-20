using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class UserAnnotationDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public static UserAnnotationDto MapFromUser(User user)
        {
            return new UserAnnotationDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
            };
        }
    }
}
