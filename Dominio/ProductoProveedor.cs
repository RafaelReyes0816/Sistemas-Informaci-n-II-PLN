using System.ComponentModel.DataAnnotations;

namespace AlmacenMis.Dominio
{
    public class ProductoProveedor
    {
        [Key]
        public int id { get; set; }
        public int producto_id { get; set; }
        public int proveedor_id { get; set; }
    }
}
