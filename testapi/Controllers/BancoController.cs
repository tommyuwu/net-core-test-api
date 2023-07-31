using Microsoft.AspNetCore.Mvc;
using testapi.Models;
using Microsoft.EntityFrameworkCore;

namespace testapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BancoController : Controller
    {
        private readonly PostgresContext _context;
        public BancoController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Banco>>> GetBancos()
        {
            return await _context.Bancos.ToListAsync();
        }

        [HttpPost("post")]
        public async Task<ActionResult<Banco>> AddBanco([FromBody] Banco banco)
        {
            if (string.IsNullOrEmpty(banco.NombreBco))
            {
                return BadRequest("El nombre del banco no puede estar vacio.");
            }
            else
            {
                _context.Bancos.Add(banco);
                if (await _context.Bancos.FirstOrDefaultAsync(b => b.NombreBco == banco.NombreBco) != null)
                {
                    return BadRequest("El banco ya está registrado.");
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpPut("put/{codBanco}")]
        public async Task<ActionResult<Banco>> UpdateBanco(long codBanco, [FromBody] Banco banco)
        {
            var result = await _context.Bancos.FirstOrDefaultAsync(b => b.CodBanco == codBanco);
            if (result != null)
            {
                if (result.NombreBco == banco.NombreBco)
                {
                    return BadRequest("El nombre no puede ser igual al anterior.");
                }
                else
                {
                    result.NombreBco = banco.NombreBco;

                    _context.Bancos.Update(result);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            else
            {
                return NotFound("El banco no existe.");
            }
        }

        [HttpDelete("delete/{codBanco}")]
        public async Task<ActionResult<Banco>> DeleteBanco(long codBanco)
        {
            var result = await _context.Bancos.FirstOrDefaultAsync(b => b.CodBanco == codBanco);
            if (result != null)
            {
                _context.Bancos.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound("El banco no existe.");
            }
        }
    }
}