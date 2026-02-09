using Task5_MusicShowcase.Services;
using Task5_MusicShowcase.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISeedCalculator, SeedCalculator>();
builder.Services.AddScoped<ICoverGenerator, CoverGenerator>();
builder.Services.AddScoped<IMusicGenerator, MusicGenerator>();
builder.Services.AddScoped<ISongService, SongService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();