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

        [HttpGet("ListaReal")]
        public async Task<ActionResult> ObtenerListaReal()
        {
            var producto = await _context.Productos
                .Where(p => p.Estado != "Inactivo")
                .Select(p => new
                {
                    p.Código,
                    p.Nombre,
                    p.Estado
                })
                .ToListAsync();

            return Ok(producto);
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult> ObtenerPorCodigo(string codigo)
        {
            var producto = await _context.Productos
                .Where(p => p.Código == codigo)
                .Select(p => new
                {
                    p.Código,
                    p.Nombre,
                    p.Estado
                })
                .FirstOrDefaultAsync();
            if (producto is null)
            {
                return NotFound($"No se encontro el producto con codigo {codigo}.");
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
            return CreatedAtAction(nameof(ObtenerPorCodigo), new { codigo = nuevoProducto.Código }, nuevoProducto);
        }

        [HttpPut("{codigo}")]
        public async Task<ActionResult<Producto>> Actualizar(string codigo, [FromBody] Producto productoActualizado)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Código == codigo);
            if (producto is null)
            {
                return NotFound($"No se encontro el producto con codigo {codigo}.");
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

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Eliminar(string codigo)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Código == codigo);
            if (producto is null)
            {
                return NotFound($"No se encontro el producto con codigo {codigo}.");
            }

            producto.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}