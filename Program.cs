﻿using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using CouncilsManagmentSystem.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CouncilsManagmentSystem.Configurations;
using CouncilsManagmentSystem.Seeds;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1. DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Configure CORS for both React and Flutter development environments
builder.Services.AddCors();

builder.Services.AddTransient<ICollageServies, CollageServies>();
builder.Services.AddTransient<IDepartmentServies, DepartmentServies>();
builder.Services.AddTransient<IUserServies, UserServies>();
builder.Services.AddTransient<ITypeCouncilServies, TypeCouncilServies>();
builder.Services.AddTransient<ICouncilsServies, CouncilsServies>();
builder.Services.AddTransient<ICouncilMembersServies, CouncilMembersServies>();
builder.Services.AddTransient<IPermissionsServies, PermissionsServies>();

// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAddMembersPermission", policy =>
        policy.Requirements.Add(new PermissionRequirement("AddMembers")));
    options.AddPolicy("RequireAddMembersByExcelPermission", policy =>
       policy.Requirements.Add(new PermissionRequirement("AddMembersByExcil")));
    options.AddPolicy("RequireAddResultPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("AddResult")));
    options.AddPolicy("RequireAddTopicPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("AddTopic")));
    options.AddPolicy("RequireEditTypeCouncilPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("EditTypeCouncil")));
    options.AddPolicy("RequireCreateTypeCouncilPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("CreateTypeCouncil")));
    options.AddPolicy("RequireEditCouncilPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("EditCouncil")));
    options.AddPolicy("RequireAddCouncilPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("AddCouncil")));
    options.AddPolicy("RequireUpdateUserPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("UpdateUser")));
    options.AddPolicy("RequireDeactiveUserPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("DeactiveUser")));
    options.AddPolicy("RequireUpdatepermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("Updatepermission")));
    options.AddPolicy("RequireAddCollagePermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("AddCollage")));
    options.AddPolicy("RequireAddDepartmentPermission", policy =>
      policy.Requirements.Add(new PermissionRequirement("AddDepartment")));
    options.AddPolicy("RequireAddHallPermission", policy =>
     policy.Requirements.Add(new PermissionRequirement("AddHall")));
});

// Register scoped authorization handler
builder.Services.AddScoped<IAuthorizationHandler, ScopedPermissionsAuthorizationHandler>();

// Configure Excel package license
OfficeOpenXml.LicenseContext licenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
ExcelPackage.LicenseContext = licenseContext;

builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // for development; set to true in production
        ValidateAudience = false, // for development; set to true in production
        RequireExpirationTime = false, // for development; set to true in production
        ValidateLifetime = true,
    };
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailingService, MailingService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 6 Web API",
        Description = "Council Management System API"
    });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer [space] and then your valid token'"
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseRouting();

// Enable CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

// Seed data and configure logging
using (var scope2 = app.Services.CreateScope())
{
    var services = scope2.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("app");

    try
    {
        var usermanager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(rolemanager);
        await DefaultUser.SeedBasicUserAsync(usermanager);
        await DefaultUser.SeedSuperAdminUserAsync(usermanager, rolemanager);

        logger.LogInformation("Data seeded");
        logger.LogInformation("Application Started");
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "An error occurred while seeding data");
    }
}

app.Run();
