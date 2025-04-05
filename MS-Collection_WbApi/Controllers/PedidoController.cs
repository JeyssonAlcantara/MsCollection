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
    public class PedidoController : ControllerBase
    {
        private readonly JoyeriaMsBdContext _context;

        public PedidoController(JoyeriaMsBdContext context)
        {
            _context = context;
        }

        // GET: api/pedido/lista
        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<Pedido>>> ListaPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario) // Incluir la información del cliente
                .ToListAsync();
            return Ok(pedidos);
        }

        // GET: api/pedido/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> ObtenerPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario) // Incluir la información del cliente
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        // POST: api/pedido
        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerPedido), new { id = pedido.Id }, pedido);
        }

        // PUT: api/pedido/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPedido(int id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pedidos.Any(e => e.Id == id))
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

        // DELETE: api/pedido/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
