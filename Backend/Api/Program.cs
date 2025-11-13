
using Api.Middleware;
using Application.Auth.Interfaces;
using Application.Auth.Mapper;
using Application.Auth.Service;
using Application.Classification.Interfaces;
using Application.Classification.Mapper;
using Application.Classification.Service;
using Application.Ingredient.Interfaces;
using Application.Ingredient.Mapper;
using Application.Ingredient.Service;
using Application.Livsmedel.Interfaces;
using Application.Livsmedel.Service;
using Application.NutritionValue.Interfaces;
using Application.NutritionValue.Mapper;
using Application.NutritionValue.Service;
using Application.Order.Interfaces;
using Application.Order.Mapper;
using Application.Order.Service;
using Application.Product.Interfaces;
using Application.Product.Mapper;
using Application.Product.Service;
using Application.Product.Validator;
using Application.RawMaterial.Interfaces;
using Application.RawMaterial.Mapper;
using Application.RawMaterial.Service;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.ExternalService;
using Infrastructure.Repositories;
using Infrastructure.Seeding;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Foody API", Version = "v1" });

                // === JWT i Swagger (Authorize-knappen) ===
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Skriv: Bearer {ditt_jwt}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            builder.Services.AddAuthorization();

            //Services 
            builder.Services.AddScoped<ILivsmedelService, LivsmedelService>();
            builder.Services.AddHttpClient<ILivsmedelImportService,LivsmedelImportService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IIngredientService, IngredientService>();
            builder.Services.AddScoped<IRawMaterialService, RawMaterialService>();
            builder.Services.AddScoped<INutritionValueService, NutritionValueService>();
            builder.Services.AddScoped<IClassificationService, ClassificationService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddTransient<ImageSeeder>();


            //Unit Of Work + Repositories
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
            builder.Services.AddScoped<IRawMaterialRepository, RawMaterialRepository>();
            builder.Services.AddScoped<INutritionValueRepository, NutritionValueRepository>();
            builder.Services.AddScoped<IClassificationRepository, ClassificationRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            //Mapper
            builder.Services.AddAutoMapper(cfg =>
            {

            },
            typeof(ProductProfile),
            typeof(AuthProfile),
            typeof(IngredientProfile),
            typeof(RawMaterialProfile),
            typeof(NutritionValueProfile),
            typeof(ClassificationProfile),
            typeof(OrderProfile));

            //AutoValidation
            builder.Services.AddValidatorsFromAssembly(typeof(CreateProductValidator).Assembly);
            builder.Services.AddFluentValidationAutoValidation();


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
           .AddRoles<IdentityRole<Guid>>()
           .AddEntityFrameworkStores<FoodyDbContext>()
           .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
                };
            });
            var app = builder.Build();

            //Seed users and roles
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();;

                
                await UserSeed.SeedUsersAndRolesAsync(userManager, roleManager);
            }

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
