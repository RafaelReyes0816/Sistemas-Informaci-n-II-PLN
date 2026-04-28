using System.ComponentModel.DataAnnotations;

namespace AlmacenMis.Dominio
{
    public class Proveedor
    {
        [Key]
        public int id_proveedor { get; set; }
        public string Código { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string Estado { get; set; } = "Activo";
    }
}