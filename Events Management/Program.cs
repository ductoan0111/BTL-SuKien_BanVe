using Events_Management.Data;
using Events_Management.Repositories;
using Events_Management.Repositories.Interfaces;
using Events_Management.Services;
using Events_Management.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
// Gán chuỗi kết nối cho DataHelper (ADO.NET)
DataHelper.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// DI Repositories
builder.Services.AddScoped<IDanhMucSuKienRepository, DanhMucSuKienRepository>();
builder.Services.AddScoped<IDiaDiemRepository, DiaDiemRepository>();
builder.Services.AddScoped<ISuKienRepository, SuKienRepository>();

// DI Services
builder.Services.AddScoped<IDanhMucSuKienService, DanhMucSuKienService>();
builder.Services.AddScoped<IDiaDiemService, DiaDiemService>();
builder.Services.AddScoped<ISuKienService, SuKienService>();
// Add services to the container.

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
