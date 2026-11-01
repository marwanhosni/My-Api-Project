
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiProject.APIs.Errors;
using MyApiProject.APIs.Extensions;
using MyApiProject.APIs.Helpers;
using MyApiProject.APIs.Middlewares;
using MyApiProject.Core.Entities.Identity;
using MyApiProject.Repository;
using MyApiProject.Repository.Identity;
using MyApiProject.Repository.Identity.DataSeed;
using MyApiProject.Repository.Implementations;
using MyApiProject.Repository.Interfaces;
using StackExchange.Redis;

namespace MyApiProject.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            //Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionString);
            });

            //Identity DbContext
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IBasketRepository,BasketRepository>() ;
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();
            // Depandency Injection for Generic Repository
           builder.Services.ApplicationServices();

            var app = builder.Build();

            #region Update DataBase 

            var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _context = services.GetRequiredService<AppDbContext>();
            var _IdentityDBContext = services.GetRequiredService<AppIdentityDbContext>();


            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();


            try
            {
                await _context.Database.MigrateAsync(); // Update DataBase (Business )
                //Data Seeding
                await AppDbContextSeed.SeedAsync(_context);
                await _IdentityDBContext.Database.MigrateAsync(); // Update DataBase (Identity)
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUSerAsync(userManager);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, message: "an Error has been occured during Appling The Migrations ");

                //Console.WriteLine(ex.Message);
            }

            #endregion
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // Handle Status Code Pages
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
