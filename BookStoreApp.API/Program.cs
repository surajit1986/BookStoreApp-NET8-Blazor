using BookStoreApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add serilog service for logging
builder.Host.UseSerilog((context, loggingConfiguration) =>
{
    loggingConfiguration.WriteTo.Console().ReadFrom.Configuration(context.Configuration);

});

//allow CORS policy
builder.Services.AddCors(options => options.AddPolicy("AllowAll", 
    b => b.AllowAnyMethod().
    AllowAnyHeader().
    AllowAnyOrigin()));

//Get the onnection string
var connectionString = builder.Configuration.GetConnectionString("BookStoreAppDbConnection");
builder.Services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
