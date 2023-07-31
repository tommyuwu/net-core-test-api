using Microsoft.AspNetCore.Mvc;
using testapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace testapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarjetaController : Controller
    {
        private readonly PostgresContext _context;
        public TarjetaController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TarjetaMaestro>>> GetTarjetas()
        {
            return await _context.TarjetaMaestros.ToListAsync();
        }

        [HttpPost("post")]
        public async Task<ActionResult<TarjetaMaestro>> AddTarjeta([FromBody] AuxTarjeta tarjetaAux)
        {
            TarjetaMaestro tarjeta = new();
            if (_context.Marcas.FirstOrDefault(m => m.CodMarca == tarjetaAux.CodMarca) != null && _context.Bancos.FirstOrDefault(b => b.CodBanco == tarjetaAux.CodBanco) != null && _context.Clientes.FirstOrDefault(c => c.CodCliente == tarjetaAux.CodCliente) != null)
            {
                tarjeta.Saldo = 0;
                tarjeta.MontoLinea = tarjetaAux.MontoLinea;
                tarjeta.CodCliente = tarjetaAux.CodCliente;
                tarjeta.CodBanco = tarjetaAux.CodBanco;
                tarjeta.CodMarca = tarjetaAux.CodMarca;
                do
                {
                    tarjeta.NumTarjeta = Create16DigitString();
                } while (_context.TarjetaMaestros.FirstOrDefault(t => t.NumTarjeta == tarjeta.NumTarjeta) != null);

                _context.TarjetaMaestros.Add(tarjeta);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Datos invalidos.");
            }

        }

        [HttpDelete("delete/{numTarjeta}")]
        public async Task<ActionResult<TarjetaMaestro>> DeleteTarjeta(string numTarjeta)
        {
            var result = await _context.TarjetaMaestros.FirstOrDefaultAsync(t => t.NumTarjeta == numTarjeta);
            if (result != null)
            {
                _context.TarjetaMaestros.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound("No existe la tarjeta.");
            }
        }

        public class AuxTarjeta
        {
            public long CodCliente { get; set; }
            public long CodMarca { get; set; }
            public long CodBanco { get; set; }
            public double MontoLinea { get; set; }

            public AuxTarjeta()
            {
                this.MontoLinea = MontoLinea;
                this.CodCliente = CodCliente;
                this.CodMarca = CodMarca;
                this.CodBanco = CodBanco;
            }
        }

        private static Random RNG = new();
        public string Create16DigitString()
        {
            var builder = new StringBuilder("5");
            while (builder.Length < 16)
            {
                builder.Append(RNG.Next(10));
            }
            return builder.ToString();
        }
    }
}