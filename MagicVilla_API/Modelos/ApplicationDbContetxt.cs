using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Modelos
{
    public class ApplicationDbContetxt: DbContext
    {
        public ApplicationDbContetxt(DbContextOptions<ApplicationDbContetxt> options) : base(options)
        {            
        }

		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Villa> Villas { get; set; }
        public DbSet<NumeroVilla> NumeroVillas { get; set; }        

        //Se va sobreescrbir el metodo para agregar registros en nuestra tabla villa.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Detalle de la Villa",
                    ImagenUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 50,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Premiun Villa Piscina",
                    Detalle = "Detalle de la Villa",
                    ImagenUrl = "",
                    Ocupantes = 4,
                    MetrosCuadrados = 40,
                    Tarifa = 150,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }


            );
        }


    }
}
