using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pied_Piper.Data;
using Pied_Piper.Repositories;
using Pied_Piper.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CONTROLLERS
// ============================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// ============================================
// DATABASE
// ============================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

// ============================================
// REPOSITORIES
// ============================================
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ============================================
// SERVICES
// ============================================
builder.Services.AddScoped<IJwtService, JwtService>();

// ============================================
// JWT AUTHENTICATION
// ============================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JWT Secret is not configured in appsettings.json");
}

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ============================================
// CORS
// ============================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",      // Angular dev
                "http://localhost:3000",      // React dev
                "http://localhost:8100"       // Mobile dev
                                              // TODO: Add production domain when deployed
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ============================================
// SWAGGER / OPENAPI
// ============================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pied Piper Event Management API",
        Version = "v1",
        Description = "Event management system with user authentication and registration"
    });

    // JWT Authentication in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
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

// ============================================
// BUILD APP
// ============================================
var app = builder.Build();

// ============================================
// SEED DATABASE (Development only)
// ============================================
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Seeding database...");

            SeedData.Initialize(services);

            logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the database.");
        }
    }
}

// ============================================
// MIDDLEWARE PIPELINE
// ============================================

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pied Piper API v1");
    });
}

// HTTPS Redirection
app.UseHttpsRedirection();

// CORS - Must be before Authentication and Authorization
app.UseCors("AllowFrontend");

// Authentication - Must come BEFORE Authorization
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// ============================================
// RUN APPLICATION
// ============================================
app.Run();