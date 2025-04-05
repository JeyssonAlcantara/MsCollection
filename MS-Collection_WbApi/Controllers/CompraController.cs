using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MS_Collection_WbApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS_Collection_WbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly JoyeriaMsBdContext _context;

        public CompraController(JoyeriaMsBdContext context)
        {
            _context = context;
        }

        // GET: api/compra/lista
        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<Compra>>> ListaCompras()
        {
            var compras = await _context.Compras
                .Include(c => c.DetalleCompras)  // Incluir los detalles de la compra
                .Include(c => c.Proveedor)       // Incluir el proveedor
                .ToListAsync();
            return Ok(compras);
        }

        // GET: api/compra/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> ObtenerCompra(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.DetalleCompras)  // Incluir los detalles de la compra
                .Include(c => c.Proveedor)       // Incluir el proveedor
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra == null)
            {
                return NotFound();
            }

            return Ok(compra);
        }

        // POST: api/compra
        [HttpPost]
        public async Task<ActionResult<Compra>> CrearCompra(Compra compra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerCompra), new { id = compra.Id }, compra);
        }

        // PUT: api/compra/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCompra(int id, Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest();
            }

            _context.Entry(compra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Compras.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/compra/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCompra(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
