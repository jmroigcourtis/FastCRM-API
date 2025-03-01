using System.Text;
using CRM.Data;
using CRM.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
    
    

// Configuración de CORS
    builder.Services.AddScoped<MailService>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost4200",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200") // Permite Angular
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidIssuer = jwtSettings["Issuer"],
        };
    });


    builder.Services.AddScoped<PasswordService>();
    builder.Services.AddAuthorization();
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddDbContext<CrmdbContext>();

    var app = builder.Build();

    using (var migrationScope = app.Services.CreateScope())
    {
        var context = migrationScope.ServiceProvider.GetRequiredService<CrmdbContext>();
        context.Database.Migrate();
    }
    
    app.UseCors("AllowLocalhost4200");
    app.MapGet("/", () => "Hello World!");
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
    app.Run();
    
}

