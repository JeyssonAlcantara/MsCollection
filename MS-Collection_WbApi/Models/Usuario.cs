﻿using MS_Collection_WbApi.Models;
using System.Text.Json.Serialization;

public partial class Usuario
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public string? Cedula { get; set; }
    public string? Email { get; set; }
    public string? User { get; set; }
    public string? Contraseña { get; set; }
    public string? Rol { get; set; }
    public bool? Estado { get; set; }

    [JsonIgnore]  // 🔹 Evita que esta lista se serialice
    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
