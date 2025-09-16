using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.API.DTOs;
using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Domain.Impl.Context;
using PersonasCrud.Domain.Impl.Entities;
using PersonasCrud.Domain.Impl.Repositories;

namespace PersonasCrud.Domain.Impl.Test;

/// <summary>
/// Pruebas unitarias para la función DeletePersonaAsync del repositorio PersonasRepositoryEF.
/// Total: 8 pruebas unitarias que cubren todos los caminos de la función.
/// </summary>
public class PersonasRepositoryDeleteTests
{
    #region Helper Methods

    /// <summary>
    /// Crea un DbContext en memoria independiente para cada prueba.
    /// </summary>
    private static PersonasDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PersonasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new PersonasDbContext(options);
    }

    /// <summary>
    /// Genera un DNI aleatorio único para las pruebas.
    /// </summary>
    private static string GenerarDniAleatorio()
    {
        var random = new Random();
        return $"{random.Next(10000000, 99999999)}{(char)('A' + random.Next(0, 26))}";
    }

    /// <summary>
    /// Genera un email aleatorio único para las pruebas.
    /// </summary>
    private static string GenerarEmailAleatorio()
    {
        return $"test{Guid.NewGuid().ToString()[..8]}@example.com";
    }

    #endregion

    #region 1. HAPPY PATH

    /// <summary>
    /// Prueba 1/8: Happy Path - Eliminar persona existente exitosamente.
    /// </summary>
    [Fact]
    public async Task Test01_DeletePersonaAsync_WhenPersonaExists_ShouldReturnTrueAndDeleteFromDatabase()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear persona inicial
        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Ana López",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 32
        };
        var personaCreada = await repository.AddPersonaAsync(datosNuevaPersona);

        // Verificar que la persona existe antes de eliminar
        var personaAntesDeEliminar = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == personaCreada.Id);
        Assert.NotNull(personaAntesDeEliminar);

        // === WHEN (Act) ===
        var personaEliminada = await repository.DeletePersonaAsync(personaCreada.Id);

        // === THEN (Assert) ===
        Assert.NotNull(personaEliminada);
        Assert.Equal(personaCreada.Id, personaEliminada.Id);
        Assert.Equal("Ana López", personaEliminada.Nombre);

        // Verificar directamente en la base de datos que la persona fue eliminada
        var personaDespuesDeEliminar = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == personaCreada.Id);
        Assert.Null(personaDespuesDeEliminar);
    }

    #endregion

    #region 2. VALIDACIONES DE ENTRADA (ArgumentException)

    /// <summary>
    /// Prueba 2/8: Validación - ID es null.
    /// </summary>
    [Fact]
    public async Task Test02_DeletePersonaAsync_WhenIdIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.DeletePersonaAsync(null!));
        
        Assert.Equal("id", exception.ParamName);
        Assert.Contains("ID no puede ser nulo o vacío", exception.Message);
    }

    /// <summary>
    /// Prueba 3/8: Validación - ID es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test03_DeletePersonaAsync_WhenIdIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.DeletePersonaAsync(string.Empty));
        
        Assert.Equal("id", exception.ParamName);
        Assert.Contains("ID no puede ser nulo o vacío", exception.Message);
    }

    /// <summary>
    /// Prueba 4/8: Validación - ID es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test04_DeletePersonaAsync_WhenIdIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.DeletePersonaAsync("   "));
        
        Assert.Equal("id", exception.ParamName);
        Assert.Contains("ID no puede ser nulo o vacío", exception.Message);
    }

    #endregion

    #region 3. CASOS DE NEGOCIO

    /// <summary>
    /// Prueba 5/8: Caso de negocio - Persona no existe.
    /// </summary>
    [Fact]
    public async Task Test05_DeletePersonaAsync_WhenPersonaDoesNotExist_ShouldReturnFalse()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var idInexistente = Guid.NewGuid().ToString();

        // === WHEN (Act) ===
        var resultado = await repository.DeletePersonaAsync(idInexistente);

        // === THEN (Assert) ===
        Assert.Null(resultado);
    }

    /// <summary>
    /// Prueba 6/8: Caso de negocio - ID con formato UUID inválido.
    /// </summary>
    [Fact]
    public async Task Test06_DeletePersonaAsync_WhenIdIsInvalidUuid_ShouldReturnFalse()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var idInvalido = "esto-no-es-un-uuid-valido";

        // === WHEN (Act) ===
        var resultado = await repository.DeletePersonaAsync(idInvalido);

        // === THEN (Assert) ===
        Assert.Null(resultado);
    }

    #endregion

    #region 4. CASOS LÍMITE

    /// <summary>
    /// Prueba 7/8: Caso límite - Eliminar una persona específica entre múltiples.
    /// </summary>
    [Fact]
    public async Task Test07_DeletePersonaAsync_WhenMultiplePersonasExist_ShouldDeleteOnlySpecifiedOne()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear múltiples personas
        var persona1 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Primera Persona",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 25
        });

        var persona2 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Segunda Persona",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        });

        var persona3 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Tercera Persona",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 35
        });

        // === WHEN (Act) ===
        var personaEliminada = await repository.DeletePersonaAsync(persona2.Id);

        // === THEN (Assert) ===
        Assert.NotNull(personaEliminada);
        Assert.Equal(persona2.Id, personaEliminada.Id);
        Assert.Equal("Segunda Persona", personaEliminada.Nombre);

        // Verificar que la persona2 fue eliminada
        var persona2Eliminada = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == persona2.Id);
        Assert.Null(persona2Eliminada);

        // Verificar que las otras personas siguen existiendo
        var persona1Existe = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == persona1.Id);
        Assert.NotNull(persona1Existe);
        Assert.Equal("Primera Persona", persona1Existe.Nombre);

        var persona3Existe = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == persona3.Id);
        Assert.NotNull(persona3Existe);
        Assert.Equal("Tercera Persona", persona3Existe.Nombre);
    }

    /// <summary>
    /// Prueba 8/8: Caso límite - Intentar eliminar la misma persona dos veces.
    /// </summary>
    [Fact]
    public async Task Test08_DeletePersonaAsync_WhenPersonaAlreadyDeleted_ShouldReturnFalse()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear persona inicial
        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Carlos Ruiz",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 40
        };
        var personaCreada = await repository.AddPersonaAsync(datosNuevaPersona);

        // Eliminar la persona por primera vez
        var personaEliminadaUnaVez = await repository.DeletePersonaAsync(personaCreada.Id);
        Assert.NotNull(personaEliminadaUnaVez);
        Assert.Equal(personaCreada.Id, personaEliminadaUnaVez.Id);

        // === WHEN (Act) ===
        var segundoDelete = await repository.DeletePersonaAsync(personaCreada.Id);

        // === THEN (Assert) ===
        Assert.Null(segundoDelete);

        // Verificar que la persona sigue sin existir en la base de datos
        var personaNoExiste = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == personaCreada.Id);
        Assert.Null(personaNoExiste);
    }

    #endregion
}
