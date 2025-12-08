using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

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

    // NUEVO: Campo para la ruta de la imagen
    public string? RutaImagen { get; set; }

    // Propiedad que no se mapea a la base de datos (para subir archivos)
    [NotMapped]
    public IFormFile? ImagenFile { get; set; }

    public virtual ICollection<DetalleVentas> DetalleVentas { get; set; } = new List<DetalleVentas>();
    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;
}