using System;
using System.Collections.Generic;

namespace WebHamburgueseria.Models;

public partial class Ventas
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdCliente { get; set; }

    public string? NumeroTransaccion { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual ICollection<DetalleVentas> DetalleVentas { get; set; } = new List<DetalleVentas>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
