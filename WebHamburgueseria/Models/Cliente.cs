using System;
using System.Collections.Generic;

namespace WebHamburgueseria.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string CedulaIdentidad { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
}
