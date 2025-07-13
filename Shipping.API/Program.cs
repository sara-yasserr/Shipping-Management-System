using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shipping.API.Middleware;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.Helper.RolePermissionHelpers;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.BusinessLogicLayer.Services;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;


namespace Shipping.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string AllowAllOrigins = "AllowAll";


            builder.Services.AddDbContext<ShippingDBContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("ShippingCS")));

            // Identity 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ShippingDBContext>()
                .AddDefaultTokenProviders();

            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //        ValidAudience = builder.Configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //    };
            //});
            // Authentication by Jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization : 'Bearer Generated-JWT-Token'",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
            });


            builder.Services.AddAutoMapper(typeof(MappConfig));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowAllOrigins, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin.Permissions.View", policy =>
     policy.Requirements.Add(new DepartmentPermissionRequirement(PermissionType.View)));
                options.AddPolicy("Permissions.View.All", policy =>
     policy.RequireRole("Admin"));
                options.AddPolicy("Admin.AllPermissions.Edit", policy =>
    policy.RequireRole("Admin"));

                options.AddPolicy("Admin.Permissions.Edit", policy =>
                    policy.Requirements.Add(new DepartmentPermissionRequirement(PermissionType.Edit)));

                options.AddPolicy("Admin.Permissions.Add", policy =>
                    policy.Requirements.Add(new DepartmentPermissionRequirement(PermissionType.Add)));


                // Department-specific policies
                foreach (var department in Enum.GetValues<Department>())
                {
                    options.AddPolicy($"{department}.View",
                        policy => policy.Requirements.Add(new DepartmentPermissionRequirement(department, PermissionType.View)));

                    options.AddPolicy($"{department}.Add",
                        policy => policy.Requirements.Add(new DepartmentPermissionRequirement(department, PermissionType.Add)));

                    options.AddPolicy($"{department}.Edit",
                        policy => policy.Requirements.Add(new DepartmentPermissionRequirement(department, PermissionType.Edit)));

                    options.AddPolicy($"{department}.Delete",
                        policy => policy.Requirements.Add(new DepartmentPermissionRequirement(department, PermissionType.Delete)));
                }
            });
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IAuthorizationHandler, DepartmentPermissionHandler>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<JwtHelper>();
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<CityService>();

            builder.Services.AddScoped<SellerService>();    
            builder.Services.AddScoped<IGovernorateService,GovernorateService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IPermissionCheckerService, PermissionCheckerService>();
            builder.Services.AddScoped<IBranchService, BranchService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IPermissionCheckerService, PermissionCheckerService>();

            builder.Services.AddScoped< IGeneralSettingsService,GeneralSettingsService>();
            builder.Services.AddScoped<IOrderService, OrderService>();



            builder.Services.AddScoped<IGovernorateService, GovernorateService>();
            builder.Services.AddScoped<IDeliveryManService, DeliveryManService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            builder.Services.AddCustomRateLimiting();

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.MapOpenApi();
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(AllowAllOrigins);

            app.UseHttpsRedirection();
            //app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
