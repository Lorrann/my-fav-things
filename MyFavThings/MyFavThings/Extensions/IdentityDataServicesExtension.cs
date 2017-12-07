using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyFavThings.Database;
using MyFavThings.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFavThings.Extensions
{
    public static class IdentityDataServicesExtension
    {
        public static IServiceCollection ConfigureIdentityProvider(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyFavThingsDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<MyFavThingsDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });

            return services;
        }
    }
}
