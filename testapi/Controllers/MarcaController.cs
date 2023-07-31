using Microsoft.AspNetCore.Mvc;
using testapi.Models;
using Microsoft.EntityFrameworkCore;

namespace testapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcaController : Controller 
    {
        private readonly PostgresContext _context;
        public MarcaController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Marca>>> GetMarcas()
        {
            return await _context.Marcas.ToListAsync();
        }

        [HttpPost("post")]
        public async Task<ActionResult<Marca>> AddMarca([FromBody] Marca marca)
        {
            if (string.IsNullOrEmpty(marca.Descripcion))
            {
                return BadRequest("El nombre de la marca no puede estar vacia.");
            }
            else
            {
                _context.Marcas.Add(marca);
                if (await _context.Marcas.FirstOrDefaultAsync(m => m.Descripcion == marca.Descripcion) != null)
                {
                    return BadRequest("La marca ya está registrada.");
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpPut("put/{codMarca}")]
        public async Task<ActionResult<Marca>> UpdateMarca(long codMarca, [FromBody] Marca marca)
        {
            var result = await _context.Marcas.FirstOrDefaultAsync(m => m.CodMarca == codMarca);
            if (result != null)
            {
                if (result.Descripcion == marca.Descripcion)
                {
                    return BadRequest("El nombre no puede ser igual al anterior.");
                }
                else
                {
                    result.Descripcion = marca.Descripcion;

                    _context.Marcas.Update(result);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            else
            {
                return NotFound("La marca no existe.");
            }
        }

        [HttpDelete("delete/{codMarca}")]
        public async Task<ActionResult<Marca>> DeleteMarca(long codMarca)
        {
            var result = await _context.Marcas.FirstOrDefaultAsync(m => m.CodMarca == codMarca);
            if (result != null)
            {
                _context.Marcas.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound("La marca no existe.");
            }
        }
    }
}
