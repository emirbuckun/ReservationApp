using Microsoft.AspNetCore.Diagnostics;
using ReservationApp.Api.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// builder.Services.AddDbContext<ReservationDbContext>(options =>
//     options.UseSqlServer("Data Source=localhost;Initial Catalog=ReservationApp;User id=SA;Password=2901Emir.2901;TrustServerCertificate=True"));

builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseInMemoryDatabase(databaseName: "ReservationDbContext"));
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

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        .Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

app.UseAuthorization();

app.MapControllers();

app.Run();
