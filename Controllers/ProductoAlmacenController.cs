using AlmacenMis.Data;
using AlmacenMis.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenMis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoAlmacenController : ControllerBase
    {
        private readonly AlmacenMisDbContext _context;

        public ProductoAlmacenController(AlmacenMisDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            var relaciones = await _context.ProductoAlmacenes
                .Join(_context.Productos, r => r.producto_id, p => p.id_Producto, (r, p) => new { r, p })
                .Join(_context.Almacenes, rp => rp.r.almacen_id, a => a.almacen_id, (rp, a) => new
                {
                    Producto = rp.p.Nombre,
                    Almacen = a.nombre
                })
                .ToListAsync();
            return Ok(relaciones);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            var relacion = await _context.ProductoAlmacenes
                .Where(r => r.id == id)
                .Join(_context.Productos, r => r.producto_id, p => p.id_Producto, (r, p) => new { r, p })
                .Join(_context.Almacenes, rp => rp.r.almacen_id, a => a.almacen_id, (rp, a) => new
                {
                    Producto = rp.p.Nombre,
                    Almacen = a.nombre
                })
                .FirstOrDefaultAsync();
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-almacen con id {id}.");
            }

            return Ok(relacion);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoAlmacen>> Crear([FromBody] ProductoAlmacen nuevaRelacion)
        {
            _context.ProductoAlmacenes.Add(nuevaRelacion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaRelacion.id }, nuevaRelacion);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductoAlmacen>> Actualizar(int id, [FromBody] ProductoAlmacen relacionActualizada)
        {
            var relacion = await _context.ProductoAlmacenes.FirstOrDefaultAsync(r => r.id == id);
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-almacen con id {id}.");
            }

            relacion.producto_id = relacionActualizada.producto_id;
            relacion.almacen_id = relacionActualizada.almacen_id;

            await _context.SaveChangesAsync();
            return Ok(relacion);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var relacion = await _context.ProductoAlmacenes.FirstOrDefaultAsync(r => r.id == id);
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-almacen con id {id}.");
            }

            _context.ProductoAlmacenes.Remove(relacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
