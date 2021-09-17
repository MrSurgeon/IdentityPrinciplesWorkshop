using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityPrinciplesWorkshop.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("resource_api1")
                {
                    Scopes = {"api1.read", "api1.create", "api1.update"},
                    ApiSecrets = {new Secret("secretapi1".Sha256())}
                },
                new ApiResource("resource_api2")
                {
                    Scopes = {"api2.read", "api2.create", "api2.update"},
                    ApiSecrets = {new Secret("secretapi2".Sha256())}
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read", "Api1 okuma izni"),
                new ApiScope("api1.create", "Api1 yazma izni"),
                new ApiScope("api1.update", "Api1 güncelleme izni"),
                new ApiScope("api2.read", "Api2 okuma izni"),
                new ApiScope("api2.create", "Api2 yazma izni"),
                new ApiScope("api2.update", "Api2 güncelleme izni"),
            };
        }

        public static IEnumerable<Client> Clients =>
            new List<Client>()
            {
                new Client()
                {
                    ClientId = "client1",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientName = "WebApp1",
                    AllowedScopes = {"api1.read", "api1.create"}
                },
                new Client()
                {
                    ClientId = "client2",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientName = "WebApp2",
                    AllowedScopes = {"api1.read", "api2.write", "api2.update"}
                },
                new Client()
                {
                    ClientId = "client1-mvc",
                    Description = "Client 1 Mvc App",
                    RequirePkce = false,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets =
                    {
                    new Secret("secret".Sha256())
                    },
                    RedirectUris = { "https://localhost:5016/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5016/signout-callback-oidc" },
                    AllowedScopes = new List<string>()
                    {
                        "openid",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1.read","CountryAndCity","Role"
                    },
                    AccessTokenLifetime = 2*60*60,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddMonths(2) - DateTime.Now).TotalSeconds,
                    RequireConsent = true,

                },
                new Client()
                {
                    ClientId = "client2-mvc",
                    RequirePkce = false,
                    Description = "Client 2 Mvc App",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris = { "https://localhost:5021/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5021/signout-callback-oidc" },
                    AllowedScopes = new List<string>()
                    {
                        "openid",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1.read","CountryAndCity","Role"
                    },
                    AccessTokenLifetime = 2*60*60,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddMonths(2) - DateTime.Now).TotalSeconds,
                    RequireConsent = true,
                }
            };

        public static IEnumerable<IdentityResource> GetIdentityResources =>
            new List<IdentityResource>()
            {
                new IdentityResources.Profile(),
                new IdentityResources.OpenId(),
                new IdentityResource()
                {
                    Name = "CountryAndCity",
                    DisplayName = "Country and City",
                    Description = "User's country informations",
                    UserClaims = {"country","city"}
                },
                new IdentityResource()
                {
                    Name = "Role",
                    DisplayName = "Role",
                    Description = "Role Authorization",
                    UserClaims = {"role"}
                }
            };

        public static IEnumerable<TestUser> GetTestUsers =>
            new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "enescerrah",
                    Password = "Shopapp123.",
                    Claims = new List<Claim>(){new Claim("name","Enes"), new Claim("family_name","Cerrah"),
                        new Claim("country","Türkiye"), new Claim("city","İzmir"), new Claim("role","customer")}
                },
                new TestUser()
                {
                    SubjectId = "2",
                    Username = "melikecerrah",
                    Password = "Shopapp123.",
                    Claims = new List<Claim>(){new Claim("name","Melike"), new Claim("family_name","Cerrah"),
                         new Claim("country", "Türkiye"), new Claim("city", "Ankara"), new Claim("role","admin")}
                },
            };
    }
}
