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
    public class ProductoController : ControllerBase
    {
        private readonly JoyeriaMsBdContext _context;

        public ProductoController(JoyeriaMsBdContext context)
        {
            _context = context;
        }

        // GET: api/producto/lista
        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<Producto>>> ListaProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria) // Incluir la categoría en la respuesta
                .ToListAsync();
            return Ok(productos);
        }

        // GET: api/producto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> ObtenerProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria) // Incluir la categoría en la respuesta
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado" });
            }

            return Ok(producto);
        }

        // POST: api/producto
        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.Id }, producto);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = "Error al crear el producto", error = ex.Message });
            }
        }

        // PUT: api/producto/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest(new { mensaje = "El ID del producto no coincide" });
            }

            var productoExistente = await _context.Productos.FindAsync(id);
            if (productoExistente == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado" });
            }

            // Actualizar solo los campos permitidos
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripción = producto.Descripción;
            productoExistente.Codigo = producto.Codigo;
            productoExistente.CategoriaId = producto.CategoriaId;
            productoExistente.Color = producto.Color;
            productoExistente.Material = producto.Material;
            productoExistente.Peso = producto.Peso;
            productoExistente.PrecioVenta = producto.PrecioVenta;
            productoExistente.Estado = producto.Estado;
            productoExistente.ImagenURL = producto.ImagenURL;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el producto" });
            }
        }

        // DELETE: api/producto/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado" });
            }

            try
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar el producto", error = ex.Message });
            }
        }
    }
}
