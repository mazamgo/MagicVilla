using MagicVilla_API;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repositorio;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddControllers().AddNewtonsoftJson(); linea antes del agregar el perfil para el Catching.
builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("Default30",
        new CacheProfile()
        {
			Duration = 30
        });
}).AddNewtonsoftJson();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Ingresar Bearer [space] tuToken \r\n\r\n " +
					  "Ejemplo: Bearer 123456abcder",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Scheme = "Bearer"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement()
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id= "Bearer"
				},
				Scheme = "oauth2",
				Name="Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});

	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Magic Villa v1",
		Description = "Api para Villas"
	});

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "Magic Villa v2",
        Description = "Api para Villas"
    });

});

builder.Services.AddResponseCaching();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(x => {
		x.RequireHttpsMetadata = false;
		x.SaveToken = true;
		x.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});

builder.Services.AddDbContext<ApplicationDbContetxt>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Servicio seguridad: se van agregar las tablas de usuario,roles y se la queremos modificar, tenemos que hacer lo siguiente
builder.Services.AddIdentity<UsuarioAplicacion,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContetxt>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>();
builder.Services.AddScoped<INumeroVillaRepositorio, NumeroVillaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

//builder.Services.AddScoped    --> son servicios se crean por solicitud y luego se detruyen.
//builder.Services.AddSingleton --> se crean cuando se solicitan y luego cada vez que soliciten utilizara la misma instancia
//builder.Services.AddTransient --> sec rean cada vez que se solicitan, se utilizan para servicios livianos y sin estados  

builder.Services.AddApiVersioning(option =>
{
	//toma la version por defecto.
	option.AssumeDefaultVersionWhenUnspecified = true;
	option.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
	option.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options => 
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
	app.UseSwaggerUI(options => 
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json","Magic_VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
