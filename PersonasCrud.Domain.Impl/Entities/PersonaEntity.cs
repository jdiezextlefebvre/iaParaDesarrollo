using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonasCrud.Domain.Impl.Entities;

/// <summary>
/// Entidad que representa una persona en la base de datos.
/// </summary>
[Table("Personas")]
public class PersonaEntity
{
    /// <summary>
    /// Identificador único autogenerado de la persona en la base de datos (clave primaria).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Identificador único universal de la persona (UUID).
    /// Se utiliza para la comunicación entre capas.
    /// </summary>
    [Required]
    [StringLength(36)]
    [Column("UUID")]
    public string Uuid { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo de la persona.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Documento Nacional de Identidad de la persona.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string Dni { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de correo electrónico de la persona.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Edad de la persona en años.
    /// </summary>
    [Required]
    [Range(0, 150)]
    public int Edad { get; set; }
}
