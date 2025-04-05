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
    public class UsuarioController : ControllerBase
    {
        private readonly JoyeriaMsBdContext _context;

        public UsuarioController(JoyeriaMsBdContext context)
        {
            _context = context;
        }

        // 🔹 Obtener todos los usuarios
        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<Usuario>>> ListaUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new {
                    u.Id,
                    u.Nombre,
                    u.Email,
                    u.User,
                    u.Rol,
                    u.Estado

                }) // Evita exponer la contraseña
                .ToListAsync();
            return Ok(usuarios);
        }

        // 🔹 Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObtenerUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new {
                    u.Id,
                    u.Nombre,
                    u.Email,
                    u.User,
                    u.Rol,
                    u.Estado
                }) // Evita exponer la contraseña
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }
            return Ok(usuario);
        }

        // 🔹 Crear un nuevo usuario
        [HttpPost("crear")]
        public async Task<ActionResult<Usuario>> CrearUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.User))
            {
                return BadRequest(new { mensaje = "Datos inválidos. Nombre, Email y User son obligatorios." });
            }

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuario.Id }, new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Email,
                    usuario.User,
                    usuario.Rol,
                    usuario.Estado
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = "Error al guardar el usuario", error = ex.Message });
            }
        }

        // 🔹 Actualizar un usuario
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuario usuarioActualizado)
        {
            if (id != usuarioActualizado.Id)
            {
                return BadRequest(new { mensaje = "El ID proporcionado no coincide con el usuario" });
            }

            try
            {
                var usuarioExistente = await _context.Usuarios.FindAsync(id);
                if (usuarioExistente == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                }

                // Actualización segura (evita cambiar la contraseña accidentalmente)
                usuarioExistente.Nombre = usuarioActualizado.Nombre;
                usuarioExistente.Email = usuarioActualizado.Email;
                usuarioExistente.User = usuarioActualizado.User;
                usuarioExistente.Rol = usuarioActualizado.Rol;
                usuarioExistente.Estado = usuarioActualizado.Estado;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensaje = "Error de concurrencia al actualizar el usuario" });
            }
        }

        // 🔹 Eliminar un usuario
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Usuario eliminado correctamente" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar el usuario", error = ex.Message });
            }
        }
    }
}
