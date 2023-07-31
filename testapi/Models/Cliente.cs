namespace testapi.Models;

public partial class Cliente
{
    public long CodCliente { get; set; }

    public string NombreApellido { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;
}
