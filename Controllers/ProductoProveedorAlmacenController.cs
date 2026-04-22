using AlmacenMis.Data;
using AlmacenMis.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenMis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoProveedorAlmacenController : ControllerBase
    {
        private readonly AlmacenMisDbContext _context;

        public ProductoProveedorAlmacenController(AlmacenMisDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoProveedorAlmacen>>> ObtenerTodos()
        {
            var relaciones = await _context.ProductoProveedorAlmacenes.ToListAsync();
            return Ok(relaciones);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoProveedorAlmacen>> ObtenerPorId(int id)
        {
            var relacion = await _context.ProductoProveedorAlmacenes.FirstOrDefaultAsync(r => r.id == id);
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-proveedor-almacen con id {id}.");
            }

            return Ok(relacion);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoProveedorAlmacen>> Crear([FromBody] ProductoProveedorAlmacen nuevaRelacion)
        {
            _context.ProductoProveedorAlmacenes.Add(nuevaRelacion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaRelacion.id }, nuevaRelacion);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductoProveedorAlmacen>> Actualizar(int id, [FromBody] ProductoProveedorAlmacen relacionActualizada)
        {
            var relacion = await _context.ProductoProveedorAlmacenes.FirstOrDefaultAsync(r => r.id == id);
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-proveedor-almacen con id {id}.");
            }

            relacion.producto_id = relacionActualizada.producto_id;
            relacion.proveedor_id = relacionActualizada.proveedor_id;
            relacion.almacen_id = relacionActualizada.almacen_id;

            await _context.SaveChangesAsync();
            return Ok(relacion);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var relacion = await _context.ProductoProveedorAlmacenes.FirstOrDefaultAsync(r => r.id == id);
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-proveedor-almacen con id {id}.");
            }

            _context.ProductoProveedorAlmacenes.Remove(relacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
