using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineResortBooking.Configuration;
using OnlineResortBooking.Data;
using OnlineResortBooking.ExceptionHandling;
using OnlineResortBooking.Models;
using OnlineResortBooking.Repository;
using OnlineResortBooking.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ---------------- SERILOG ----------------
builder.Host.UseSerilog((context, cfg) =>
    cfg.ReadFrom.Configuration(context.Configuration));

// ---------------- CONFIG ----------------
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>() ?? new JwtSettings();

// ---------------- DB + IDENTITY ----------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ---------------- JWT ----------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,

        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(jwtSettings.Secret ?? "")),

        ValidateLifetime = true,

        // 🔥 MATCH Identity
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name
    };

    // Debug logs (keep this)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            var auth = ctx.Request.Headers["Authorization"].ToString();
            Log.Information("Authorization header: {Auth}",
                string.IsNullOrEmpty(auth) ? "[none]" : auth);
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = ctx =>
        {
            Log.Error(ctx.Exception, "JWT Failed");
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            var role = ctx.Principal?.FindFirst("role")?.Value;
            Log.Information("Token validated. Role: {Role}", role);
            return Task.CompletedTask;
        }
    };
});

// ---------------- CORS ----------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ---------------- DI ----------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IResortRepository, ResortRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IResortService, ResortService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddTransient<DataSeeder>();

// ---------------- CONTROLLERS ----------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ---------------- SWAGGER (FIXED) ----------------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo { Title = "ResortAPI", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter ONLY your token (no Bearer prefix)"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// ---------------- APP ----------------
var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();

app.UseStaticFiles();
app.UseRouting();

app.UseCors("FrontendPolicy");

// 🔥 ORDER MATTERS
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// ---------------- SEED ----------------
using (var scope = app.Services.CreateScope())
{
    try
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAdminAsync();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Seeding failed");
    }
}

app.Run();
