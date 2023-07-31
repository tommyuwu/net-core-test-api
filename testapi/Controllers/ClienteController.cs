using Microsoft.AspNetCore.Mvc;
using testapi.Models;
using Microsoft.EntityFrameworkCore;

namespace testapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        private readonly PostgresContext _context;
        public ClienteController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpPost("post")]
        public async Task<ActionResult<Cliente>> AddCliente([FromBody] Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.NombreApellido) || string.IsNullOrEmpty(cliente.NumeroDocumento) || string.IsNullOrEmpty(cliente.TipoDocumento))
            {
                return BadRequest("Los datos no pueden estar vacios.");
            }
            else
            {
                _context.Clientes.Add(cliente);
                if (await _context.Clientes.FirstOrDefaultAsync(c => c.NumeroDocumento == cliente.NumeroDocumento) != null)
                {
                    return BadRequest("El documento ya está en uso.");
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpPut("put/{codCliente}")]
        public async Task<ActionResult<Cliente>> UpdateCliente(long codCliente, [FromBody] Cliente cliente)
        {
            var result = await _context.Clientes.FirstOrDefaultAsync(c => c.CodCliente == codCliente);
            if (result != null)
            {
                if (result.NumeroDocumento == cliente.NumeroDocumento && result.NombreApellido == cliente.NombreApellido)
                {
                    return BadRequest("Los datos no pueden ser iguales.");
                }
                else
                {
                    result.NombreApellido = cliente.NombreApellido;
                    result.TipoDocumento = cliente.TipoDocumento;
                    result.NumeroDocumento = cliente.NumeroDocumento;

                    _context.Clientes.Update(result);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            else
            {
                return NotFound("El cliente no existe.");
            }
        }

        [HttpDelete("delete/{codCliente}")]
        public async Task<ActionResult<Cliente>> DeleteCliente(long codCliente)
        {
            var result = await _context.Clientes.FirstOrDefaultAsync(c => c.CodCliente == codCliente);
            if (result != null)
            {
                _context.Clientes.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound("El cliente no existe.");
            }
        }
    }
}