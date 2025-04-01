using System.Text;
using CRM.Data;
using CRM.Services.Implementations;
using CRM.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
    
    builder.Services.AddScoped<MailService>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "AllowLocalhost4200", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyOrigin()
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



    builder.Services.AddScoped<ILogger, Logger<object>>();
    builder.Services.AddScoped<IMailService, MailService>();
    builder.Services.AddScoped<IUserSecurityService, UserSecurityService>();
    builder.Services.AddScoped<PasswordHasher<object>>();
    builder.Services.AddScoped<IUserAccountService, UserAccountService>();
    builder.Services.AddScoped<IQueryDbService, QueryDbService>();
    builder.Services.AddScoped<IPasswordService, PasswordService>();
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

