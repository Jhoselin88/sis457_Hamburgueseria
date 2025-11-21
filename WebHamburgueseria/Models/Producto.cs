using System;
using System.Collections.Generic;

namespace WebHamburgueseria.Models;

public partial class Producto
{
    public int Id { get; set; }

    public int IdCategoria { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Saldo { get; set; }

    public decimal PrecioVenta { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual ICollection<DetalleVentas> DetalleVentas { get; set; } = new List<DetalleVentas>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;
}
