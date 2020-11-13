using AspNet.Security.OAuth.Validation;
using AuthorizationServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.Threading.Tasks;

namespace AuthorizationServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
                
            });

            services.AddTransient<IProfileRepository, ProfileRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();

            services.AddOpenIddict()
             .AddCore(options =>
             {
                 options.UseEntityFrameworkCore()
                 .UseDbContext<ApplicationDbContext>();
             })

             .AddServer(options =>
             {
                 options.UseMvc();

                 options.EnableTokenEndpoint("/connect/token");

                 options.AllowClientCredentialsFlow()
                 .AllowRefreshTokenFlow();

                 options.SetAccessTokenLifetime(TimeSpan.FromHours(1));
                 options.SetRefreshTokenLifetime(TimeSpan.FromDays(1));

                 options.EnableRequestCaching();

                 options.DisableHttpsRequirement();
             });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OAuthValidationDefaults.AuthenticationScheme;
            }).AddOAuthValidation();
        }

                
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseCors("MyPolicy");

            app.UseMvcWithDefaultRoute();

            InitializeAsync(app.ApplicationServices).GetAwaiter().GetResult();
        }
        private async Task InitializeAsync(IServiceProvider services)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.EnsureCreatedAsync();

                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                if (await manager.FindByClientIdAsync("console") == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "console",
                        ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                        DisplayName = "My client application",
                        Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.ClientCredentials
                        }
                    };

                    await manager.CreateAsync(descriptor);
                }
            }
        }
    }
}
