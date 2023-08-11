using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;
using WebAPI.Models;
using WebAPI.Services;
using WebApi.Authorization;
using BCryptNet = BCrypt.Net.BCrypt;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();
}

builder.Services.Configure<OCRDataCollectDatabaseSettings>(
    builder.Configuration.GetSection(nameof(OCRDataCollectDatabaseSettings)));

builder.Services.AddSingleton<IOCRDataCollectDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<OCRDataCollectDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("OCRDataCollectDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IDataService, DataService>();

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

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

//{
//    var testUsers = new List<User>
//    {
//        new User { Id = 1, FirstName = "Admin", LastName = "User", Username = "admin", PasswordHash = BCryptNet.HashPassword("admin"), Role = Role.Admin },
//        new User { Id = 2, FirstName = "Normal", LastName = "User", Username = "user", PasswordHash = BCryptNet.HashPassword("user"), Role = Role.User }
//    };

//    using var scope = app.Services.CreateScope();
//    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
//    dataContext.Users.AddRange(testUsers);
//    dataContext.SaveChanges();
//}

app.Run();
