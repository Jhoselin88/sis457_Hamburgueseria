using System;
using System.Collections.Generic;

namespace WebHamburgueseria.Models;

public partial class DetalleVentas
{
    public int Id { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? Total { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Ventas IdVentaNavigation { get; set; } = null!;
}
