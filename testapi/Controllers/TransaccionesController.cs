using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using NuGet.Versioning;
using System.Net;
using System.Net.Http.Headers;
using testapi.Models;

namespace testapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionesController : Controller
    {
        private readonly PostgresContext _context;
        public TransaccionesController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TransaccionesTarjeta>>> GetTransacciones()
        {
            return await _context.TransaccionesTarjetas.ToListAsync();
        }

        [HttpPost("generar")]
        public void GenerarTransacciones()
        {
            var tarjetas = _context.TarjetaMaestros.ToList();
            foreach (var t in tarjetas)
            {
                if (_context.TransaccionesTarjetas.FirstOrDefault(tr => tr.IdTarjeta == t.Id) == null)
                {
                    Random random = new();
                    var monto = random.Next(10000, 2500000);
                    TransaccionesTarjeta transaccion = new(t.Id, monto, DateTime.Now, "Pendiente");

                    _context.TransaccionesTarjetas.Add(transaccion);
                    _context.SaveChanges();
                }
            }
        }

        [HttpPost("autorizar")]
        [Authorize("Bearer"+token)]
        public void AutorizarTransacciones()
        {
            /*var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);*/


            var tarjetas = _context.TarjetaMaestros.ToList();
            foreach (var t in tarjetas)
            {
                var result = _context.TransaccionesTarjetas.FirstOrDefault(tr => tr.IdTarjeta == t.Id);
                if (result != null && result.Estado.Equals("Pendiente"))
                {

                    if ((t.Saldo+result.MontoTransacccion) <= t.MontoLinea)
                    {
                        t.Saldo += result.MontoTransacccion;
                        result.Estado = "Aprobada";

                        _context.TransaccionesTarjetas.Update(result);
                        _context.TarjetaMaestros.Update(t);
                        _context.SaveChanges();
                    }
                    else
                    {
                        result.Estado = "Denegada";
                        _context.TransaccionesTarjetas.Update(result);
                        _context.TarjetaMaestros.Update(t);
                        _context.SaveChanges();
                    }
                    
                }
            }
        }
    }
}
