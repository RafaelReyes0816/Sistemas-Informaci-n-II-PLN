using AlmacenMis.Data;
using AlmacenMis.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenMis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly AlmacenMisDbContext _context;

        public StockController(AlmacenMisDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            var stocks = await _context.Stocks
                .Join(_context.Productos, s => s.producto_id, p => p.id_Producto, (s, p) => new { s, p })
                .Join(_context.Almacenes, sp => sp.s.almacen_id, a => a.almacen_id, (sp, a) => new
                {
                    Producto = sp.p.Nombre,
                    Almacen = a.nombre,
                    sp.s.cantidad
                })
                .ToListAsync();
            return Ok(stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            var stock = await _context.Stocks
                .Where(s => s.id == id)
                .Join(_context.Productos, s => s.producto_id, p => p.id_Producto, (s, p) => new { s, p })
                .Join(_context.Almacenes, sp => sp.s.almacen_id, a => a.almacen_id, (sp, a) => new
                {
                    Producto = sp.p.Nombre,
                    Almacen = a.nombre,
                    sp.s.cantidad
                })
                .FirstOrDefaultAsync();
            if (stock is null)
            {
                return NotFound($"No se encontro el stock con id {id}.");
            }

            return Ok(stock);
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> Crear([FromBody] Stock nuevoStock)
        {
            _context.Stocks.Add(nuevoStock);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoStock.id }, nuevoStock);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Stock>> Actualizar(int id, [FromBody] Stock stockActualizado)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.id == id);
            if (stock is null)
            {
                return NotFound($"No se encontro el stock con id {id}.");
            }

            stock.producto_id = stockActualizado.producto_id;
            stock.almacen_id = stockActualizado.almacen_id;
            stock.cantidad = stockActualizado.cantidad;

            await _context.SaveChangesAsync();
            return Ok(stock);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.id == id);
            if (stock is null)
            {
                return NotFound($"No se encontro el stock con id {id}.");
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
