using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure;
using Infrastructure.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<IQADbContext>(x=>x.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
//For Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configurations = configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configurations);
});
//Adding CORS 
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});

//For Adding Caching Second Method
builder.Services.AddMemoryCache();

var app = builder.Build();

//Create Data Migration and Seed Data
#region Create Migration and Seed Data
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var context = services.GetRequiredService<IQADbContext>();
    context.Database.EnsureDeleted();
    await context.Database.MigrateAsync();    
    await SeedData.SeedAsync(context, loggerFactory);  
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred during migration");
}
#endregion

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Content")
    ),
    RequestPath = "/Content"
});

app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
