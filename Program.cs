using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using CouncilsManagmentSystem.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;
using CouncilsManagmentSystem.notfication;
using CouncilsManagmentSystem.Configurations;
using CouncilsManagmentSystem.Seeds;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:3000", "http://localhost:5117")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Enable credentials for these specific origins
        });
});

// SignalR Configuration
builder.Services.AddSignalR();

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAddMembersPermission", policy =>
        policy.Requirements.Add(new PermissionRequirement("AddMembers")));
    // Add other policies as needed
});

// Identity Configuration
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
})
.AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
    jwt.SaveToken = true;
    jwt.RequireHttpsMetadata = false;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("JwtConfig:ValidIss").Value,
        ValidateAudience = true,
        ValidAudiences = builder.Configuration.GetSection("JwtConfig:ValidAud").Value.Split(','),
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Swagger Configuration
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

// Configure Excel package license
OfficeOpenXml.LicenseContext licenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
ExcelPackage.LicenseContext = licenseContext;

// Register services
builder.Services.AddTransient<ICollageServies, CollageServies>();
builder.Services.AddTransient<IDepartmentServies, DepartmentServies>();
builder.Services.AddTransient<IUserServies, UserServies>();
builder.Services.AddTransient<ITypeCouncilServies, TypeCouncilServies>();
builder.Services.AddTransient<ICouncilsServies, CouncilsServies>();
builder.Services.AddTransient<ICouncilMembersServies, CouncilMembersServies>();
builder.Services.AddTransient<IPermissionsServies, PermissionsServies>();
builder.Services.AddTransient<INotificationServies, NotificationServies>();
builder.Services.AddTransient<IMailingService, MailingService>();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
});

// Add scoped authorization handler
builder.Services.AddScoped<IAuthorizationHandler, ScopedPermissionsAuthorizationHandler>();

// Configure Mail settings
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Setup logging
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseRouting();

// CORS middleware
app.UseCors("AllowSpecificOrigins");

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Static files and endpoint mapping
app.UseStaticFiles();
app.MapControllers();

// Map SignalR hub
app.MapHub<NotificationHub>("/NotificationHub");

// Seed data and configure logging
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("app");

    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(roleManager);
        await DefaultUser.SeedBasicUserAsync(userManager, dbContext);
        await DefaultUser.SeedSuperAdminUserAsync(userManager, roleManager, dbContext);

        logger.LogInformation("Data seeded");
        logger.LogInformation("Application started");
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "An error occurred while seeding data");
    }
}

app.Run();
