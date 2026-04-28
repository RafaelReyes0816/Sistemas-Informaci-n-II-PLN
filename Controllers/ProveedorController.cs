using AlmacenMis.Data;
using AlmacenMis.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenMis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly AlmacenMisDbContext _context;

        public ProveedorController(AlmacenMisDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            var proveedores = await _context.Proveedores
                .Select(p => new
                {
                    p.Código,
                    p.nombre,
                    p.Estado
                })
                .ToListAsync();
            return Ok(proveedores);
        }

        [HttpGet("ListaReal")]
        public async Task<ActionResult> ObtenerListaReal()
        {
            var proveedor = await _context.Proveedores
                .Where(p => p.Estado != "Inactivo")
                .Select(p => new
                {
                    p.Código,
                    p.nombre,
                    p.Estado
                })
                .ToListAsync();

            return Ok(proveedor);
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult> ObtenerPorCodigo(string codigo)
        {
            var proveedor = await _context.Proveedores
                .Where(p => p.Código == codigo)
                .Select(p => new
                {
                    p.Código,
                    p.nombre,
                    p.Estado
                })
                .FirstOrDefaultAsync();
            if (proveedor is null)
            {
                return NotFound($"No se encontro el proveedor con codigo {codigo}.");
            }

            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<ActionResult<Proveedor>> Crear([FromBody] Proveedor nuevoProveedor)
        {
            _context.Proveedores.Add(nuevoProveedor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorCodigo), new { codigo = nuevoProveedor.Código }, nuevoProveedor);
        }

        [HttpPut("{codigo}")]
        public async Task<ActionResult<Proveedor>> Actualizar(string codigo, [FromBody] Proveedor proveedorActualizado)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.Código == codigo);
            if (proveedor is null)
            {
                return NotFound($"No se encontro el proveedor con codigo {codigo}.");
            }

            proveedor.Código = proveedorActualizado.Código;
            proveedor.nombre = proveedorActualizado.nombre;
            proveedor.Estado = proveedorActualizado.Estado;

            await _context.SaveChangesAsync();
            return Ok(proveedor);
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Eliminar(string codigo)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.Código == codigo);
            if (proveedor is null)
            {
                return NotFound($"No se encontro el proveedor con codigo {codigo}.");
            }

            proveedor.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}