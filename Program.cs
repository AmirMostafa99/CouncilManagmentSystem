using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using CouncilsManagmentSystem.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.Text;
using CouncilsManagmentSystem.Configurations;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI;
using CouncilsManagmentSystem.Seeds;
using System.ComponentModel;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;
using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1.DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//add servies
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
});

// Register scoped authorization handler
builder.Services.AddScoped<IAuthorizationHandler, ScopedPermissionsAuthorizationHandler>();








//excel
OfficeOpenXml.LicenseContext licenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
ExcelPackage.LicenseContext = licenseContext;


builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()   // for Forget password method
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
    jwt.TokenValidationParameters = new TokenValidationParameters() // this code create a token for user
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // for developement but when u are deployment will be true
        ValidateAudience = false, // for developement but when u are deployment will be true
        RequireExpirationTime = false, // for development --mean to be updated when refresh token is added
        ValidateLifetime = true, // ceck the time live for the token
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(swagger =>
{
    // this is generate the default UI for swagger documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 6 Web API",
        Description = "Council Managment System Privacy"

    });
    // to   enable authorization using swagger  JWT
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter bearer [space] and then your valid token in the text"
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

// Configure the HTTP request pipeline.
if (app.Environment. IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


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
