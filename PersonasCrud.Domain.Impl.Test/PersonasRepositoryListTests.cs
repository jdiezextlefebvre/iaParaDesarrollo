using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.API.DTOs;
using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Domain.Impl.Context;
using PersonasCrud.Domain.Impl.Repositories;

namespace PersonasCrud.Domain.Impl.Test;

/// <summary>
/// Pruebas unitarias para la función ListPersonasAsync del repositorio PersonasRepositoryEF.
/// Total: 6 pruebas unitarias que cubren todos los caminos de la función.
/// </summary>
public class PersonasRepositoryListTests
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
    /// Prueba 1/6: Happy Path - Listar personas cuando existen múltiples registros.
    /// </summary>
    [Fact]
    public async Task Test01_ListPersonasAsync_WhenMultiplePersonasExist_ShouldReturnAllPersonasOrderedByNombre()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear personas en orden no alfabético para verificar ordenamiento
        var persona1 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Zelda García",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 25
        });

        var persona2 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Alberto López",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        });

        var persona3 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "María Fernández",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 35
        });

        // === WHEN (Act) ===
        var listaPersonas = await repository.ListPersonasAsync();

        // === THEN (Assert) ===
        Assert.NotNull(listaPersonas);
        Assert.Equal(3, listaPersonas.Count);

        // Verificar que están ordenadas por nombre (Alberto, María, Zelda)
        Assert.Equal("Alberto López", listaPersonas[0].Nombre);
        Assert.Equal(persona2.Id, listaPersonas[0].Id);
        
        Assert.Equal("María Fernández", listaPersonas[1].Nombre);
        Assert.Equal(persona3.Id, listaPersonas[1].Id);
        
        Assert.Equal("Zelda García", listaPersonas[2].Nombre);
        Assert.Equal(persona1.Id, listaPersonas[2].Id);
    }

    #endregion

    #region 2. CASOS DE NEGOCIO

    /// <summary>
    /// Prueba 2/6: Caso de negocio - Lista vacía cuando no hay personas.
    /// </summary>
    [Fact]
    public async Task Test02_ListPersonasAsync_WhenNoPersonasExist_ShouldReturnEmptyList()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // No agregar ninguna persona

        // === WHEN (Act) ===
        var listaPersonas = await repository.ListPersonasAsync();

        // === THEN (Assert) ===
        Assert.NotNull(listaPersonas);
        Assert.Empty(listaPersonas);
    }

    /// <summary>
    /// Prueba 3/6: Caso de negocio - Lista con una sola persona.
    /// </summary>
    [Fact]
    public async Task Test03_ListPersonasAsync_WhenOnlyOnePersonaExists_ShouldReturnListWithOneItem()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Juan Único",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 28
        };
        var personaCreada = await repository.AddPersonaAsync(datosNuevaPersona);

        // === WHEN (Act) ===
        var listaPersonas = await repository.ListPersonasAsync();

        // === THEN (Assert) ===
        Assert.NotNull(listaPersonas);
        Assert.Single(listaPersonas);
        
        var personaEnLista = listaPersonas[0];
        Assert.Equal(personaCreada.Id, personaEnLista.Id);
        Assert.Equal("Juan Único", personaEnLista.Nombre);
        Assert.Equal(datosNuevaPersona.Dni, personaEnLista.Dni);
        Assert.Equal(datosNuevaPersona.Email, personaEnLista.Email);
        Assert.Equal(28, personaEnLista.Edad);
    }

    #endregion

    #region 3. CASOS LÍMITE

    /// <summary>
    /// Prueba 4/6: Caso límite - Ordenamiento con nombres similares.
    /// </summary>
    [Fact]
    public async Task Test04_ListPersonasAsync_WhenPersonasHaveSimilarNames_ShouldOrderCorrectly()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear personas con nombres que prueben el ordenamiento
        await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Ana María",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 25
        });

        await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Ana",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        });

        await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Ana Belén",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 35
        });

        // === WHEN (Act) ===
        var listaPersonas = await repository.ListPersonasAsync();

        // === THEN (Assert) ===
        Assert.Equal(3, listaPersonas.Count);
        
        // Verificar orden alfabético: "Ana", "Ana Belén", "Ana María"
        Assert.Equal("Ana", listaPersonas[0].Nombre);
        Assert.Equal("Ana Belén", listaPersonas[1].Nombre);
        Assert.Equal("Ana María", listaPersonas[2].Nombre);
    }

    /// <summary>
    /// Prueba 5/6: Caso límite - Lista después de eliminar personas.
    /// </summary>
    [Fact]
    public async Task Test05_ListPersonasAsync_AfterDeletingPersonas_ShouldReturnOnlyRemainingPersonas()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear tres personas
        var persona1 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Persona A",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 25
        });

        var persona2 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Persona B",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        });

        var persona3 = await repository.AddPersonaAsync(new DatosNuevaPersonaDTO
        {
            Nombre = "Persona C",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 35
        });

        // Eliminar la persona del medio
        await repository.DeletePersonaAsync(persona2.Id);

        // === WHEN (Act) ===
        var listaPersonas = await repository.ListPersonasAsync();

        // === THEN (Assert) ===
        Assert.Equal(2, listaPersonas.Count);
        
        // Verificar que solo existen las personas que no fueron eliminadas
        var idsEnLista = listaPersonas.Select(p => p.Id).ToList();
        Assert.Contains(persona1.Id, idsEnLista);
        Assert.Contains(persona3.Id, idsEnLista);
        Assert.DoesNotContain(persona2.Id, idsEnLista);
        
        // Verificar orden alfabético
        Assert.Equal("Persona A", listaPersonas[0].Nombre);
        Assert.Equal("Persona C", listaPersonas[1].Nombre);
    }

    /// <summary>
    /// Prueba 6/6: Caso límite - Integridad de datos en la lista.
    /// </summary>
    [Fact]
    public async Task Test06_ListPersonasAsync_ShouldReturnCompletePersonaData()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Validación Completa",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 42
        };
        var personaCreada = await repository.AddPersonaAsync(datosPersona);

        // === WHEN (Act) ===
        var listaPersonas = await repository.ListPersonasAsync();

        // === THEN (Assert) ===
        Assert.Single(listaPersonas);
        
        var personaEnLista = listaPersonas[0];
        
        // Verificar que todos los campos están completos y correctos
        Assert.False(string.IsNullOrEmpty(personaEnLista.Id));
        Assert.Equal(personaCreada.Id, personaEnLista.Id);
        Assert.Equal("Validación Completa", personaEnLista.Nombre);
        Assert.Equal(datosPersona.Dni, personaEnLista.Dni);
        Assert.Equal(datosPersona.Email, personaEnLista.Email);
        Assert.Equal(42, personaEnLista.Edad);
        
        // Verificar que el ID es un UUID válido
        Assert.True(Guid.TryParse(personaEnLista.Id, out _));
    }

    #endregion
}
