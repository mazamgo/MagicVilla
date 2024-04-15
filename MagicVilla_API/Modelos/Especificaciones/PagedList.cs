namespace MagicVilla_API.Modelos.Especificaciones
{
    //Clase Generica que recibir cualquier modelo, esta va hacer la encargada de hacer los cortes y los saltos.
    public class PagedList<T> : List<T> 
    {
        public MetaData MetaData { get; set; } //propiedad 

        //contructor va a recibir una lista,cantidad,pagenumber, 
        public PagedList(List<T> items, int count, int pageNumber, int pageSize) 
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count/(double)pageSize) //Por ejemplo 1.5 lo transforma en 2
            };
            
            AddRange(items); //agrego los items que estoy recibiendo.
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> entidad, int pageNumber, int pageSize)
        {
            var count = entidad.Count();
            var items = entidad.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items,count,pageNumber, pageSize);
        }

    }
}
