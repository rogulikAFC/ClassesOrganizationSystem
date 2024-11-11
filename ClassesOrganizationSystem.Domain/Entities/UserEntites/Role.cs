using Microsoft.AspNetCore.Identity;

namespace ClassesOrganizationSystem.Domain.Entities.UserEntites
{
    public class Role : IdentityRole<int>
    {
        public override string Name { get; set; } = null!;
    }
}
