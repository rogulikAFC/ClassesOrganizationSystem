using Microsoft.AspNetCore.Identity;

namespace ClassesOrganizationSistem.Domain.Entities.UserEntites
{
    public class Role : IdentityRole<int>
    {
        public override string Name { get; set; } = null!;
    }
}
