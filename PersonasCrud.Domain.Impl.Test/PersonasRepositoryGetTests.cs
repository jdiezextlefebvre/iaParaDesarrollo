using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.API.DTOs;
using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Domain.Impl.Context;
using PersonasCrud.Domain.Impl.Repositories;

namespace PersonasCrud.Domain.Impl.Test;

/// <summary>
/// Pruebas unitarias para la función GetPersonaAsync del repositorio PersonasRepositoryEF.
/// Total: 7 pruebas unitarias que cubren todos los caminos de la función.
/// </summary>
public class PersonasRepositoryGetTests
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
    /// Prueba 1/7: Happy Path - Obtener persona existente.
    /// </summary>
    [Fact]
    public async Task Test01_GetPersonaAsync_WhenPersonaExists_ShouldReturnPersonaDTO()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear persona inicial
        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "María García",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 28
        };
        var personaCreada = await repository.AddPersonaAsync(datosNuevaPersona);

        // === WHEN (Act) ===
        var personaObtenida = await repository.GetPersonaAsync(personaCreada.Id);

        // === THEN (Assert) ===
        Assert.NotNull(personaObtenida);
        Assert.Equal(personaCreada.Id, personaObtenida.Id);
        Assert.Equal("María García", personaObtenida.Nombre);
        Assert.Equal(datosNuevaPersona.Dni, personaObtenida.Dni);
        Assert.Equal(datosNuevaPersona.Email, personaObtenida.Email);
        Assert.Equal(28, personaObtenida.Edad);
    }

    #endregion

    #region 2. VALIDACIONES DE ENTRADA (ArgumentException)

    /// <summary>
    /// Prueba 2/7: Validación - ID es null.
    /// </summary>
    [Fact]
    public async Task Test02_GetPersonaAsync_WhenIdIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.GetPersonaAsync(null!));
        
        Assert.Equal("id", exception.ParamName);
        Assert.Contains("ID no puede ser nulo o vacío", exception.Message);
    }

    /// <summary>
    /// Prueba 3/7: Validación - ID es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test03_GetPersonaAsync_WhenIdIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.GetPersonaAsync(string.Empty));
        
        Assert.Equal("id", exception.ParamName);
        Assert.Contains("ID no puede ser nulo o vacío", exception.Message);
    }

    /// <summary>
    /// Prueba 4/7: Validación - ID es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test04_GetPersonaAsync_WhenIdIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.GetPersonaAsync("   "));
        
        Assert.Equal("id", exception.ParamName);
        Assert.Contains("ID no puede ser nulo o vacío", exception.Message);
    }

    #endregion

    #region 3. CASOS DE NEGOCIO

    /// <summary>
    /// Prueba 5/7: Caso de negocio - Persona no existe.
    /// </summary>
    [Fact]
    public async Task Test05_GetPersonaAsync_WhenPersonaDoesNotExist_ShouldReturnNull()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var idInexistente = Guid.NewGuid().ToString();

        // === WHEN (Act) ===
        var resultado = await repository.GetPersonaAsync(idInexistente);

        // === THEN (Assert) ===
        Assert.Null(resultado);
    }

    /// <summary>
    /// Prueba 6/7: Caso de negocio - ID con formato UUID inválido.
    /// </summary>
    [Fact]
    public async Task Test06_GetPersonaAsync_WhenIdIsInvalidUuid_ShouldReturnNull()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var idInvalido = "esto-no-es-un-uuid-valido";

        // === WHEN (Act) ===
        var resultado = await repository.GetPersonaAsync(idInvalido);

        // === THEN (Assert) ===
        Assert.Null(resultado);
    }

    #endregion

    #region 4. CASOS LÍMITE

    /// <summary>
    /// Prueba 7/7: Caso límite - Verificar que retorna la persona correcta cuando existen múltiples.
    /// </summary>
    [Fact]
    public async Task Test07_GetPersonaAsync_WhenMultiplePersonasExist_ShouldReturnCorrectOne()
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
        var personaObtenida = await repository.GetPersonaAsync(persona2.Id);

        // === THEN (Assert) ===
        Assert.NotNull(personaObtenida);
        Assert.Equal(persona2.Id, personaObtenida.Id);
        Assert.Equal("Segunda Persona", personaObtenida.Nombre);
        Assert.Equal(30, personaObtenida.Edad);

        // Verificar que no es ninguna de las otras personas
        Assert.NotEqual(persona1.Id, personaObtenida.Id);
        Assert.NotEqual(persona3.Id, personaObtenida.Id);
    }

    #endregion
}
