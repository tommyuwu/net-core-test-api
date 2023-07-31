namespace testapi.Models;

public partial class TarjetaMaestro
{
    public long Id { get; set; }

    public string NumTarjeta { get; set; } = null!;

    public double Saldo { get; set; }

    public double MontoLinea { get; set; }

    public long CodCliente { get; set; }

    public long CodBanco { get; set; }

    public long CodMarca { get; set; }
}
