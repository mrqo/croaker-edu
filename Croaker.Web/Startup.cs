using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;
using LiteDB;
using AutoMapper;

using EmbeddedBlazorContent;
using edu_croaker.Areas.Identity;
using edu_croaker.Data.Entities;
using edu_croaker.Data.Interfaces;
using edu_croaker.Infrastructure.LiteDB;
using edu_croaker.Services;
using edu_croaker.Mapping;

namespace edu_croaker
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
            services.AddRazorPages();
            services.AddServerSideBlazor();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            services.AddSingleton<ILiteDbContext, LiteDbContext>();           
            services.AddSingleton<IRepository<Croak>, LiteDBRepository<Croak>>();
            services.AddSingleton<IRepository<Like>, LiteDBRepository<Like>>();
            services.AddSingleton<IRepository<Hashtag>, LiteDBHashtagRepository>();
            services.AddSingleton<IRepository<UserDetails>, LiteDBRepository<UserDetails>>();
            services.AddSingleton<IRepository<Follower>, LiteDBRepository<Follower>>();

            services.AddScoped<CroakService>();
            services.AddScoped<UserService>();

            services.AddIdentity<ApplicationUser, AspNetCore.Identity.LiteDB.IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;

                    options.Lockout.AllowedForNewUsers = true;
                    
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddUserStore<LiteDbUserStore<ApplicationUser>>()
                .AddRoleStore<LiteDbRoleStore<AspNetCore.Identity.LiteDB.IdentityRole>>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseEmbeddedBlazorContent(typeof(MatBlazor.BaseMatComponent).Assembly);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
