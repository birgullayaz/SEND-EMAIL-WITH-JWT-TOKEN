using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Bu satırı ekleyin veya mevcut AddControllers() satırını değiştirin

// Diğer servis kayıtları...
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT kimlik doğrulama ayarlarını yapılandırma
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.")))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Bu satırı ekleyin
app.UseRouting(); // Bu satırı ekleyin

app.UseAuthentication(); // Bu satırı ekleyin
app.UseAuthorization();

app.MapControllerRoute( // Bu satırı ekleyin
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();