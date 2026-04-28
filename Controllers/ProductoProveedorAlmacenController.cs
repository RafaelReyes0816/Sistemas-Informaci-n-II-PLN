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
        public async Task<ActionResult> ObtenerTodos()
        {
            var relaciones = await _context.ProductoProveedorAlmacenes
                .Where(r => r.Estado != "Inactivo")
                .Join(_context.Productos, r => r.producto_id, p => p.id_Producto, (r, p) => new { r, p })
                .Join(_context.Proveedores, rp => rp.r.proveedor_id, pr => pr.id_proveedor, (rp, pr) => new { rp.r, rp.p, pr })
                .Join(_context.Almacenes, rpp => rpp.r.almacen_id, a => a.almacen_id, (rpp, a) => new
                {
                    Producto = rpp.p.Nombre,
                    Proveedor = rpp.pr.nombre,
                    Almacen = a.nombre
                })
                .ToListAsync();
            return Ok(relaciones);
        }

        [HttpGet("ListaReal")]
        public async Task<ActionResult> ObtenerListaReal()
        {
            var relaciones = await _context.ProductoProveedorAlmacenes
                .Where(r => r.Estado != "Inactivo")
                .Join(_context.Productos, r => r.producto_id, p => p.id_Producto, (r, p) => new { r, p })
                .Join(_context.Proveedores, rp => rp.r.proveedor_id, pr => pr.id_proveedor, (rp, pr) => new { rp.r, rp.p, pr })
                .Join(_context.Almacenes, rpp => rpp.r.almacen_id, a => a.almacen_id, (rpp, a) => new
                {
                    Producto = rpp.p.Nombre,
                    Proveedor = rpp.pr.nombre,
                    Almacen = a.nombre,
                    rpp.r.Estado
                })
                .ToListAsync();
            return Ok(relaciones);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            var relacion = await _context.ProductoProveedorAlmacenes
                .Where(r => r.id == id)
                .Join(_context.Productos, r => r.producto_id, p => p.id_Producto, (r, p) => new { r, p })
                .Join(_context.Proveedores, rp => rp.r.proveedor_id, pr => pr.id_proveedor, (rp, pr) => new { rp.r, rp.p, pr })
                .Join(_context.Almacenes, rpp => rpp.r.almacen_id, a => a.almacen_id, (rpp, a) => new
                {
                    Producto = rpp.p.Nombre,
                    Proveedor = rpp.pr.nombre,
                    Almacen = a.nombre
                })
                .FirstOrDefaultAsync();
            if (relacion is null)
            {
                return NotFound($"No se encontro la relacion producto-proveedor-almacen con id {id}.");
            }

            return Ok(relacion);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoProveedorAlmacen>> Crear([FromBody] ProductoProveedorAlmacen nuevaRelacion)
        {
            nuevaRelacion.Estado = "Activo";
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

            relacion.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
