using EventManagement.Organizer.Data;
using EventManagement.Organizer.Repositories;
using EventManagement.Organizer.Repositories.Interfaces;
using EventManagement.Organizer.Services;
using EventManagement.Organizer.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Gán chuỗi kết nối cho DataHelper (ADO.NET)
DataHelper.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<ILoaiVeRepository, LoaiVeRepository>();
builder.Services.AddScoped<ILoaiVeService, LoaiVeService>();
builder.Services.AddScoped<ISuKienRepository, SuKienRepository>();
builder.Services.AddScoped<ISuKienService, SuKienService>();
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
