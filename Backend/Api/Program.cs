
using Application.Livsmedel.Interfaces;
using Application.Livsmedel.Service;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.ExternalService;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Services 
            builder.Services.AddScoped<ILivsmedelService, LivsmedelService>();
            builder.Services.AddHttpClient<ILivsmedelImportService,LivsmedelImportService>();

            //Unit Of Work + Repositories
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

            builder.Services.AddDbContext<FoodyDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddIdentity<User, IdentityRole<Guid>>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = true;
                option.Password.RequireDigit = true;
            })
           .AddEntityFrameworkStores<FoodyDbContext>()
           .AddDefaultTokenProviders();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
