using System.ComponentModel.DataAnnotations;

namespace AlmacenMis.Dominio
{
    public class Producto
    {
        [Key]
        public int id_Producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Código { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

    }
}