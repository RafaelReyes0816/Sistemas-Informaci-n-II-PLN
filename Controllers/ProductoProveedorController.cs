using AlmacenMis.Data;
using AlmacenMis.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenMis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoProveedorController : ControllerBase
    {
        private readonly AlmacenMisDbContext _context;

        public ProductoProveedorController(AlmacenMisDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            var relaciones = await _context.ProductoProveedores
                .Join(_context.Productos, r => r.producto_id, p => p.id_Producto, (r, p) => new { r, p })
                .Join(_context.Proveedores, rp => rp.r.proveedor_id, pr => pr.id_proveedor, (rp, pr) => new
                {
                    Producto = rp.p.Nombre,
                    Proveedor = pr.nombre
                })
                .ToListAsync();
            return Ok(relaciones);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            var relacion = await _context.ProductoProveedores
                .Where(r => r.id == id)
                .Join(_context.Productos, r => r.producto_id, p => p.id_Producto, (r, p) => new { r, p })
                .Join(_context.Proveedores, rp => rp.r.proveedor_id, pr => pr.id_proveedor, (rp, pr) => new
                {
                    Producto = rp.p.Nombre,
                    Proveedor = pr.nombre
                })
                .FirstOrDefaultAsync();
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-proveedor con id {id}.");
            }

            return Ok(relacion);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoProveedor>> Crear([FromBody] ProductoProveedor nuevaRelacion)
        {
            _context.ProductoProveedores.Add(nuevaRelacion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaRelacion.id }, nuevaRelacion);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductoProveedor>> Actualizar(int id, [FromBody] ProductoProveedor relacionActualizada)
        {
            var relacion = await _context.ProductoProveedores.FirstOrDefaultAsync(r => r.id == id);
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-proveedor con id {id}.");
            }

            relacion.producto_id = relacionActualizada.producto_id;
            relacion.proveedor_id = relacionActualizada.proveedor_id;

            await _context.SaveChangesAsync();
            return Ok(relacion);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var relacion = await _context.ProductoProveedores.FirstOrDefaultAsync(r => r.id == id);
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-proveedor con id {id}.");
            }

            _context.ProductoProveedores.Remove(relacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
