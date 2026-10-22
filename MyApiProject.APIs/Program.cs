
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiProject.APIs.Errors;
using MyApiProject.APIs.Extensions;
using MyApiProject.APIs.Helpers;
using MyApiProject.APIs.Middlewares;
using MyApiProject.Repository;
using MyApiProject.Repository.Implementations;
using MyApiProject.Repository.Interfaces;

namespace MyApiProject.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            // Add services to the container.

            builder.Services.AddControllers();
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


            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();


            try
            {
                _context.Database.MigrateAsync();

                //Data Seeding
                 AppDbContextSeed.SeedAsync(_context);

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
