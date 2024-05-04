using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Minimal_API.Auth;
using Minimal_API.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITokenService>(new TokenService());

builder.Services.AddSingleton<IUserRepository>(new UserRepository());

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(options =>

    {

        options.TokenValidationParameters = new()

        {

            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(

                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };

    });

var app = builder.Build();

app.UseMiddleware<CustomMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<RequestOneLogging_Middleware>();
app.UseMiddleware<HeaderValidationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();

app.MapGet("/login", [AllowAnonymous] async (HttpContext context,
    ITokenService tokenService, IUserRepository userRepository) => {
        UserDto userModel = new()
        {
            UserName = context.Request.Query["username"],

            Password = context.Request.Query["password"]
        };
        var userDto = userRepository.GetUser(userModel);
        if (userDto == null) return Results.Unauthorized();
        var token = tokenService.BuildToken(builder.Configuration["Jwt:Key"],
            builder.Configuration["Jwt:Issuer"], userDto);
        return Results.Ok(token);
    });

var hotels = new List<Hotel>();

app.MapGet("/hotels", () => hotels);
app.MapGet("/hotels/{id}", (int id) => hotels.FirstOrDefault(h => h.Id == id));
app.MapPost("/hotels", (Hotel hotel) => hotels.Add(hotel));
app.MapPut("/hotels/{id}", (Hotel hotel) => {
    var index = hotels.FindIndex(h => h.Id == hotel.Id);
    if (index < 0)
    {
        throw new Exception("Not found");
    }
    hotels[index] = hotel;
});
app.MapDelete("/hotels/{id}", (int id) => {
    var index = hotels.FindIndex(h => h.Id == id);
    if (index < 0) throw new Exception("Not found");
    hotels.RemoveAt(index);
});



app.Run();



public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}