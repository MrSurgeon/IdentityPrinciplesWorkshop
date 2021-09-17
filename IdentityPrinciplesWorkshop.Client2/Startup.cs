using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;

namespace IdentityPrinciplesWorkshop.Client2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(opts =>
                {
                    opts.DefaultScheme = "Cookies";
                    opts.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", opt =>
                {
                    opt.AccessDeniedPath = "/User/AccessDenied";
                })
                .AddOpenIdConnect("oidc", opt =>
                {
                    opt.SignInScheme = "Cookies";
                    opt.Authority = "https://localhost:5001";
                    opt.ClientId = "client2-mvc";
                    opt.ClientSecret = "secret";
                    opt.ResponseType = "code id_token";
                    opt.GetClaimsFromUserInfoEndpoint = true;
                    opt.SaveTokens = true;

                    opt.Scope.Add("api1.read");
                    opt.Scope.Add("offline_access");
                    opt.Scope.Add("CountryAndCity");
                    opt.Scope.Add("Role");

                    opt.ClaimActions.MapUniqueJsonKey("country", "country");
                    opt.ClaimActions.MapUniqueJsonKey("city", "city");
                    opt.ClaimActions.MapUniqueJsonKey("role", "role");
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        RoleClaimType = "role"
                    };

                });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
