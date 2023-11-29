using BusinessLogic.Interfaces;
using BusinessLogic.Mapping;
using BusinessLogic.Services;
using DataAccess.DBContexts;
using DataAccess.Models;
using DataAccess.Repositories.user;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BreackFastContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BreackFastContext") ?? throw new InvalidOperationException("Connection string 'BreackFastContext' not found."), providerOptions => providerOptions.EnableRetryOnFailure()));
// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepository<Users>, UserRepository>();

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
