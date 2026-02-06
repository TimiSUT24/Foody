
using Api.Middleware;
using Application.Abstractions;
using Application.Auth.Interfaces;
using Application.Auth.Mapper;
using Application.Auth.Service;
using Application.Category.Interfaces;
using Application.Category.Mapper;
using Application.Category.Service;
using Application.NutritionValue.Interfaces;
using Application.NutritionValue.Mapper;
using Application.NutritionValue.Service;
using Application.Offer.Interfaces;
using Application.Offer.Mapper;
using Application.Offer.Service;
using Application.Order.Handlers;
using Application.Order.Interfaces;
using Application.Order.Mapper;
using Application.Order.Service;
using Application.Postnord.Interfaces;
using Application.Postnord.Service;
using Application.Product.Interfaces;
using Application.Product.Mapper;
using Application.Product.Service;
using Application.Product.Validator;
using Application.StripeChargeShippingOptions.Interfaces;
using Application.StripeChargeShippingOptions.Service;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Caching;
using Infrastructure.Data;
using Infrastructure.ExternalService;
using Infrastructure.HelperMethod;
using Infrastructure.Repositories;
using Infrastructure.Seeding;
using Infrastructure.UnitOfWork;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Stripe;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProductService = Application.Product.Service.ProductService;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
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
                    BearerFormat = "Jwt",
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

            //Stripe 
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            //CacheSettings
            builder.Services.Configure<CacheSettings>(
                builder.Configuration.GetSection("CacheSettings"));

            //Redis 
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["Redis:ConnectionString"];         
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = builder.Configuration["Redis:ConnectionString"];
                var options = ConfigurationOptions.Parse(configuration);
                options.AbortOnConnectFail = true;
                return ConnectionMultiplexer.Connect(options);
                
            });

            //RabbitMq/MassTransit
            builder.Services.AddMassTransit(x =>
            {       
                x.AddConsumer<BookShipmentConsumer>();               
                x.AddConsumer<UpdateOrderStatusConsumer>();
                x.AddConsumer<SendOrderEmailConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);
                });

            });
          

            //Services 
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<INutritionValueService, NutritionValueService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddHttpClient<IPostnordService, PostnordService>();
            builder.Services.AddSingleton<IStripeService, StripeService>();
            builder.Services.AddScoped<ICalculateDiscount, CalculateDiscount>();
            builder.Services.AddScoped<IOfferService, OfferService>();
            builder.Services.AddScoped<ICacheService, HybridCacheService>();
            builder.Services.AddHybridCache();


            //Unit Of Work + Repositories
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<INutritionValueRepository, NutritionValueRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOfferRepository, OfferRepository>();

            //Mapper
            builder.Services.AddAutoMapper(cfg =>
            {

            },
            typeof(ProductProfile),
            typeof(AuthProfile),
            typeof(NutritionValueProfile),
            typeof(OrderProfile),
            typeof(CategoryProfile),
            typeof(OfferProfile));

            //AutoValidation
            builder.Services.AddValidatorsFromAssembly(typeof(CreateProductValidator).Assembly);
            builder.Services.AddFluentValidationAutoValidation();


            var connectionString = builder.Configuration["DefaultConnection"];

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
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

            //Cors
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(allowedOrigins!)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            //Seed users and roles
  
                    using (var scope = app.Services.CreateScope())
                    {
                        var services = scope.ServiceProvider;
                        var dbContext = services.GetRequiredService<FoodyDbContext>();
                        dbContext.Database.Migrate();   

                        var userManager = services.GetRequiredService<UserManager<User>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>(); ;
                        

                        await IcaDataSeeding.IcaSeed(dbContext);
                        await UserSeed.SeedUsersAndRolesAsync(userManager, roleManager);

                    }
      
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend"); // Cors after httpsredirection
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization(); //Authorization after authentication


            app.MapControllers();

            app.Run();
        }
    }
}
