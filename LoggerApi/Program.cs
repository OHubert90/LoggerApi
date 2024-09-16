using Microsoft.EntityFrameworkCore;
using LoggerApi.Data;
using LoggerApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SourceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SourceDatabase")));

builder.Services.AddDbContext<DestinationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DestinationDatabase")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<LogEntriesService>();
builder.Services.AddHostedService<DataTransferService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("https://localhost:7239")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowSpecificOrigins");

app.Run();