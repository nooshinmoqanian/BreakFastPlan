using BusinessLogic.Interfaces;

using BusinessLogic.Mapping;

using BusinessLogic.Services;

using BusinessLogic.Validators;

using DataAccess.DBContexts;
using DataAccess.Models;

using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddSingleton(jwtSettings);


builder.Services.AddDbContext<BreakfastContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BreakFastContext") ?? throw new InvalidOperationException("Connection string 'BreakFastContext' not found."), providerOptions => providerOptions.EnableRetryOnFailure()));

///identity



// Add services to the container.

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRepositories<Users>, UserRepository>();

builder.Services.AddScoped<IRepositories<Breakfast>, BreakfastRepositories>();

builder.Services.AddScoped<IRepositories<Tag>, BreakfastTagRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IBreakfastService, BreakfastService>();

builder.Services.AddScoped<IRoleService, RoleService>();

//Mapper

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.KeyAccess)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())

{
    app.UseSwagger();


    app.UseSwaggerUI();

}

app.UseSwagger();

app.UseSwaggerUI(c =>

{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

    c.OAuthClientId("YOUR_CLIENT_ID");

    c.OAuthAppName("Demo API - Swagger");
});


app.UseHttpsRedirection();


app.UseAuthentication();


app.UseAuthorization();


app.MapControllers();


app.Run();


