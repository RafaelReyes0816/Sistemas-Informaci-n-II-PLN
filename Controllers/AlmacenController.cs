using AlmacenMis.Data;
using AlmacenMis.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenMis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlmacenController : ControllerBase
    {
        private readonly AlmacenMisDbContext _context;

        public AlmacenController(AlmacenMisDbContext context)
        {
            _context = context;
        }

        [HttpGet("Get Admin")]
        public async Task<ActionResult> ObtenerTodos()
        {
            var almacenes = await _context.Almacenes
                .Select(a => new
                {
                    a.Código,
                    a.nombre,
                    a.Estado
                })
                .ToListAsync();
            return Ok(almacenes);
        }
        [HttpGet("QueryNavegacion")]
        public async Task<ActionResult> QueryNavegacion()
        {
            var query = await (
                from a in _context.Almacenes
                join pa in _context.ProductoAlmacenes on a.almacen_id equals pa.almacen_id
                join p in _context.Productos on pa.producto_id equals p.id_Producto
                select new
                {
                    Almacen = a.nombre,
                    CodigoAlmacen = a.Código,
                    Producto = p.Nombre,
                    CodigoProducto = p.Código,
                    EstadoAlmacen = a.Estado
                })
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("ListaReal")]
        public async Task<ActionResult> GetAlmacenReal()
        {
            var almacenes = await (
                from a in _context.Almacenes
                where a.Estado != "Inactivo"
                select new
                {
                    a.Código,
                    a.nombre,
                    a.Estado
                }).ToListAsync();

            return Ok(almacenes);
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult> ObtenerPorCodigo(string codigo)
        {
            var almacen = await _context.Almacenes
                .Where(a => a.Código == codigo)
                .Select(a => new
                {
                    a.Código,
                    a.nombre,
                    a.Estado
                })
                .FirstOrDefaultAsync();
            if (almacen is null)
            {
                return NotFound($"No se encontro el almacen con codigo {codigo}.");
            }

            return Ok(almacen);
        }

        [HttpPost]
        public async Task<ActionResult<Almacen>> Crear([FromBody] Almacen nuevoAlmacen)
        {
            _context.Almacenes.Add(nuevoAlmacen);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorCodigo), new { codigo = nuevoAlmacen.Código }, nuevoAlmacen);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Almacen>> Actualizar(int id, [FromBody] Almacen almacenActualizado)
        {
            var almacen = await _context.Almacenes.FirstOrDefaultAsync(a => a.almacen_id == id);
            if (almacen is null)
            {
                return NotFound($"No se encontro el almacen con id {id}.");
            }

            almacen.Código = almacenActualizado.Código;
            almacen.nombre = almacenActualizado.nombre;
            almacen.Estado = almacenActualizado.Estado;

            await _context.SaveChangesAsync();
            return Ok(almacen);
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Eliminar(string codigo)
        {
            var almacen = await _context.Almacenes
                .FirstOrDefaultAsync(a => a.Código == codigo);
            if (almacen is null)
            {
                return NotFound($"No se encontro el almacen con codigo {codigo}.");
            }

            almacen.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
