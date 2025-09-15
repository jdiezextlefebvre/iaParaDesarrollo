using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.Impl.Entities;

namespace PersonasCrud.Domain.Impl.Context;

/// <summary>
/// Contexto de base de datos para las personas usando Entity Framework.
/// </summary>
public class PersonasDbContext : DbContext
{
    /// <summary>
    /// Constructor del contexto.
    /// </summary>
    /// <param name="options">Opciones de configuración del contexto.</param>
    public PersonasDbContext(DbContextOptions<PersonasDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet para las personas.
    /// </summary>
    public DbSet<PersonaEntity> Personas { get; set; }

    /// <summary>
    /// Configuración del modelo de datos.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la entidad PersonaEntity
        modelBuilder.Entity<PersonaEntity>(entity =>
        {
            // Configurar índice único para UUID
            entity.HasIndex(e => e.Uuid)
                  .IsUnique();

            // Configurar índice único para DNI
            entity.HasIndex(e => e.Dni)
                  .IsUnique();

            // Configurar índice único para Email
            entity.HasIndex(e => e.Email)
                  .IsUnique();
        });
    }
}
