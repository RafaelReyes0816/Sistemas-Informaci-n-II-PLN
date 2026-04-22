using System.ComponentModel.DataAnnotations;

namespace AlmacenMis.Dominio
{
    public class Stock
    {
        [Key]
        public int id { get; set; }
        public int producto_id { get; set; }
        public int almacen_id { get; set; }
        public int cantidad { get; set; }
    }
}