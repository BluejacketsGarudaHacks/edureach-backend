using Backend.Middlewares;
using Backend.Infrastructure.Database;
using Backend.Repositories;
using Backend.Shared.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token"
    });
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection"))
);

builder.Services.AddScoped<UserRepository>();

builder.Services.AddSingleton<JwtUtil>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var secret = configuration.GetValue<string>("Jwt:Secret");
    var issuer = configuration.GetValue<string>("Jwt:Issuer");
    var audience = configuration.GetValue<string>("Jwt:Audience");
    return new JwtUtil(secret!, issuer!, audience!);
});

builder.Services.AddSingleton<ImageUtil>(
    provider => new ImageUtil()
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Backend.Seeders.LocationSeeding.Seed(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCorsMiddleware();
app.UseAuthenticationMiddleware();
app.MapControllers();

app.Run();