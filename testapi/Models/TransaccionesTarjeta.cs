namespace testapi.Models;

public partial class TransaccionesTarjeta
{
    public long CodTransacc { get; set; }

    public long IdTarjeta { get; set; }

    public double MontoTransacccion { get; set; }

    public DateTime FechaHoraTransacc { get; set; }

    public string Estado { get; set; } = null!;

    public TransaccionesTarjeta(long idTarjeta, double montoTransacccion, DateTime fechaHoraTransacc, string estado)
    {
        IdTarjeta = idTarjeta;
        MontoTransacccion = montoTransacccion;
        FechaHoraTransacc = fechaHoraTransacc;
        Estado = estado;
    }
}
