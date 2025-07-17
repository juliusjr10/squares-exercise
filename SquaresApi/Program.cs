using Microsoft.EntityFrameworkCore;
using SquaresApi.Data;
using SquaresApi.Repositories;
using SquaresApi.Services;
using SquaresApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PointsDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddDbContext<PointsDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPointsRepository, PointsRepository>();
builder.Services.AddScoped<IPointsService, PointsService>();
builder.Services.AddScoped<ISquareService, SquareService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Squares API V1");
    c.RoutePrefix = string.Empty;
});

app.UseMiddleware<RequestTimeoutMiddleware>(TimeSpan.FromSeconds(5));

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PointsDbContext>();
    db.Database.Migrate();
}

app.Run();
