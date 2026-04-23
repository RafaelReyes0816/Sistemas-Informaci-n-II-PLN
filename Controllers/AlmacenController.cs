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

        [HttpGet]
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            var almacen = await _context.Almacenes
                .Where(a => a.almacen_id == id)
                .Select(a => new
                {
                    a.Código,
                    a.nombre,
                    a.Estado
                })
                .FirstOrDefaultAsync();
            if (almacen is null)
            {
                return NotFound($"No se encontro el almacen con id {id}.");
            }

            return Ok(almacen);
        }

        [HttpPost]
        public async Task<ActionResult<Almacen>> Crear([FromBody] Almacen nuevoAlmacen)
        {
            _context.Almacenes.Add(nuevoAlmacen);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoAlmacen.almacen_id }, nuevoAlmacen);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var almacen = await _context.Almacenes.FirstOrDefaultAsync(a => a.almacen_id == id);
            if (almacen is null)
            {
                return NotFound($"No se encontro el almacen con id {id}.");
            }

            _context.Almacenes.Remove(almacen);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
