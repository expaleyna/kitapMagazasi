using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework with SQL Server
builder.Services.AddDbContext<kitapMagazaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS - HTTP portları da ekleyelim
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
        policy.WithOrigins("http://localhost:5000", "https://localhost:5001", "http://localhost:5001", "http://localhost:7000", "http://localhost:7001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "kitapMagaza API V1");
        c.RoutePrefix = "swagger";
    });
}

// app.UseHttpsRedirection(); // HTTPS redirect kapatıldı

app.UseCors("AllowMVC");

app.UseAuthorization();

app.MapControllers();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<kitapMagazaDbContext>();
    context.Database.EnsureCreated();
    
    // Seed data
    await DataSeeder.SeedAsync(context);
}

app.Run();
