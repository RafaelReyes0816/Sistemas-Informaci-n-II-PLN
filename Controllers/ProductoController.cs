using AlmacenMis.Data;
using AlmacenMis.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenMis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly AlmacenMisDbContext _context;

        public ProductoController(AlmacenMisDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            var productos = await _context.Productos
                .Select(p => new
                {
                    p.Código,
                    p.Nombre,
                    p.Estado
                })
                .ToListAsync();
            return Ok(productos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            var producto = await _context.Productos
                .Where(p => p.id_Producto == id)
                .Select(p => new
                {
                    p.Código,
                    p.Nombre,
                    p.Estado
                })
                .FirstOrDefaultAsync();
            if (producto is null)
            {
                return NotFound($"No se encontro el producto con id {id}.");
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> Crear([FromBody] Producto nuevoProducto)
        {
            if (string.IsNullOrWhiteSpace(nuevoProducto.Nombre) ||
                string.IsNullOrWhiteSpace(nuevoProducto.Código) ||
                string.IsNullOrWhiteSpace(nuevoProducto.Estado))
            {
                return BadRequest("Nombre, Código y Estado son obligatorios.");
            }

            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoProducto.id_Producto }, nuevoProducto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Producto>> Actualizar(int id, [FromBody] Producto productoActualizado)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.id_Producto == id);
            if (producto is null)
            {
                return NotFound($"No se encontro el producto con id {id}.");
            }

            if (string.IsNullOrWhiteSpace(productoActualizado.Nombre) ||
                string.IsNullOrWhiteSpace(productoActualizado.Código) ||
                string.IsNullOrWhiteSpace(productoActualizado.Estado))
            {
                return BadRequest("Nombre, Código y Estado son obligatorios.");
            }

            producto.Nombre = productoActualizado.Nombre;
            producto.Código = productoActualizado.Código;
            producto.Estado = productoActualizado.Estado;
            await _context.SaveChangesAsync();

            return Ok(producto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.id_Producto == id);
            if (producto is null)
            {
                return NotFound($"No se encontro el producto con id {id}.");
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}