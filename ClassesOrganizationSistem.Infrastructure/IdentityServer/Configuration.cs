using Duende.IdentityServer.Models;

namespace ClassesOrganizationSistem.Infrastructure.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> Resources =>
            new List<IdentityResource>
            {
                new IdentityResources.Profile(),
                new IdentityResources.OpenId(),
            };

        public static IEnumerable<ApiScope> Scopes =>
            new List<ApiScope>
            {
                new ApiScope("student"),
                new ApiScope("teacher"),
                new ApiScope("admin")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "application",
                    AllowedScopes =
                    {
                        "student",
                        "teacher",
                        "admin"
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials
                }
            };
    }
}
