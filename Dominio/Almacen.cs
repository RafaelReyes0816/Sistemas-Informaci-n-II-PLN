using System.ComponentModel.DataAnnotations;

namespace AlmacenMis.Dominio
{
    public class Almacen
    {
        [Key]
        public int almacen_id { get; set; }
        public string Código { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
