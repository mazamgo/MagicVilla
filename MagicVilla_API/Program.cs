using MagicVilla_API;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repositorio;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContetxt>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>();
builder.Services.AddScoped<INumeroVillaRepositorio, NumeroVillaRepositorio>();
//builder.Services.AddScoped    --> son servicios se crean por solicitud y luego se detruyen.
//builder.Services.AddSingleton --> se crean cuando se solicitan y luego cada vez que soliciten utilizara la misma instancia
//builder.Services.AddTransient --> sec rean cada vez que se solicitan, se utilizan para servicios livianos y sin estados  


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
