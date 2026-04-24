using System.ComponentModel.DataAnnotations;

namespace AlmacenMis.Dominio
{
    public class ProductoAlmacen
    {
        [Key]
        public int id { get; set; }
        public int producto_id { get; set; }
        public int almacen_id { get; set; }
        public string Estado {get; set; }
    }
}
