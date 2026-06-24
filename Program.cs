using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_CNPM.Area.Admin.Services;
using Project_CNPM.Data;
using Project_CNPM.Models;
using Project_CNPM.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMedicineAdminService, MedicineAdminService>();
builder.Services.AddScoped<IDiseaseAdminService, DiseaseAdminService>();
builder.Services.AddScoped<ISymptomAdminService, SymptomAdminService>();
builder.Services.AddScoped<IUserAdminService, UserAdminService>();
builder.Services.AddScoped<ISafetyWarningAdminService, SafetyWarningAdminService>();
builder.Services.AddScoped<IDashboardAdminService, DashboardAdminService>();
builder.Services.AddHttpClient<IMedicineDiseaseMappingService, MedicineDiseaseMappingService>();
builder.Services.AddHttpClient<IUserPredictionService, UserPredictionService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (string.IsNullOrWhiteSpace(context.Token) &&
                    context.Request.Cookies.TryGetValue("token", out var cookieToken))
                {
                    context.Token = cookieToken;
                }

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                if (!context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
                {
                    context.HandleResponse();

                    var returnUrl = $"{context.Request.PathBase}{context.Request.Path}{context.Request.QueryString}";
                    var loginUrl = $"/Account/DangNhap?returnUrl={Uri.EscapeDataString(returnUrl)}";
                    context.Response.Redirect(loginUrl);
                }

                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                if (!context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.Redirect("/Error/Forbidden");
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var admin = db.NguoiDungs.FirstOrDefault(u => u.Email == "admin@example.com");
        const string defaultAdminPassword = "Admin@123";

        if (admin == null)
        {
            db.NguoiDungs.Add(new NguoiDung
            {
                HoTen = "Quan tri vien",
                Email = "admin@example.com",
                MatKhauMaHoa = BCrypt.Net.BCrypt.HashPassword(defaultAdminPassword),
                VaiTro = "Admin",
                BiKhoa = false,
                NgayTao = DateTime.Now
            });
        }
        else if (string.IsNullOrWhiteSpace(admin.MatKhauMaHoa) || !admin.MatKhauMaHoa.StartsWith("$2"))
        {
            admin.MatKhauMaHoa = BCrypt.Net.BCrypt.HashPassword(defaultAdminPassword);
            admin.VaiTro = "Admin";
            admin.BiKhoa = false;
            admin.DeleteAt = null;
        }

        db.SaveChanges();
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Could not seed default admin account. Make sure the database is created and reachable.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/ServerError");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Portal}/{action=Dashboard}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
