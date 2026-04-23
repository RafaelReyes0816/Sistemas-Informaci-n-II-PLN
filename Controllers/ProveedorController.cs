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

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            var proveedor = await _context.Proveedores
                .Where(p => p.id_proveedor == id)
                .Select(p => new
                {
                    p.Código,
                    p.nombre,
                    p.Estado
                })
                .FirstOrDefaultAsync();
            if (proveedor is null)
            {
                return NotFound($"No se encontro el proveedor con id {id}.");
            }

            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<ActionResult<Proveedor>> Crear([FromBody] Proveedor nuevoProveedor)
        {
            _context.Proveedores.Add(nuevoProveedor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoProveedor.id_proveedor }, nuevoProveedor);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Proveedor>> Actualizar(int id, [FromBody] Proveedor proveedorActualizado)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.id_proveedor == id);
            if (proveedor is null)
            {
                return NotFound($"No se encontro el proveedor con id {id}.");
            }

            proveedor.Código = proveedorActualizado.Código;
            proveedor.nombre = proveedorActualizado.nombre;
            proveedor.Estado = proveedorActualizado.Estado;

            await _context.SaveChangesAsync();
            return Ok(proveedor);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.id_proveedor == id);
            if (proveedor is null)
            {
                return NotFound($"No se encontro el proveedor con id {id}.");
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}