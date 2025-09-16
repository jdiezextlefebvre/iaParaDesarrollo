using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.API.DTOs;
using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Domain.Impl.Context;
using PersonasCrud.Domain.Impl.Repositories;

namespace PersonasCrud.Domain.Impl.Test;

/// <summary>
/// Pruebas unitarias para la función UpdatePersonaAsync del repositorio PersonasRepositoryEF.
/// Total: 21 pruebas unitarias que cubren todos los caminos de la función.
/// </summary>
public class PersonasRepositoryUpdateTests
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
    /// Prueba 1/21: Happy Path - Actualización exitosa de persona existente con datos válidos.
    /// </summary>
    [Fact]
    public async Task Test01_UpdatePersonaAsync_WhenPersonaExistsAndDataIsValid_ShouldUpdatePersonaSuccessfully()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear datos iniciales para la persona
        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "FELIPE",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 33
        };

        // Añadir la persona inicialmente
        var personaOriginal = await repository.AddPersonaAsync(datosNuevaPersona);
        var idPersona = personaOriginal.Id;

        // Crear datos modificados
        var datosModificados = new DatosModificarPersonaDTO
        {
            Id = idPersona,
            Nombre = "FELIPE MODIFICADO",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 35
        };

        // === WHEN (Act) ===
        var personaModificada = await repository.UpdatePersonaAsync(datosModificados);

        // === THEN (Assert) ===
        // Verificar respuesta del método
        Assert.NotNull(personaModificada);
        Assert.Equal(idPersona, personaModificada.Id);
        Assert.Equal("FELIPE MODIFICADO", personaModificada.Nombre);
        Assert.Equal(datosModificados.Dni, personaModificada.Dni);
        Assert.Equal(datosModificados.Email, personaModificada.Email);
        Assert.Equal(35, personaModificada.Edad);

        // Verificar persistencia consultando DIRECTAMENTE en la BD
        var personaEnBD = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == idPersona);

        Assert.NotNull(personaEnBD);
        Assert.Equal("FELIPE MODIFICADO", personaEnBD.Nombre);
        Assert.Equal(datosModificados.Dni, personaEnBD.Dni);
        Assert.Equal(datosModificados.Email, personaEnBD.Email);
        Assert.Equal(35, personaEnBD.Edad);
        Assert.True(personaEnBD.Id > 0);

        // Verificar que solo existe UNA persona con ese UUID
        var cantidadPersonasConEseUuid = await context.Personas
            .CountAsync(p => p.Uuid == idPersona);
        Assert.Equal(1, cantidadPersonasConEseUuid);
    }

    #endregion

    #region 2. VALIDACIONES DE ENTRADA (ArgumentException)

    /// <summary>
    /// Prueba 2/21: Validación - Parámetro datosModificarPersona es null.
    /// </summary>
    [Fact]
    public async Task Test02_UpdatePersonaAsync_WhenDatosModificarPersonaIsNull_ShouldThrowArgumentNullException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => repository.UpdatePersonaAsync(null!));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
    }

    /// <summary>
    /// Prueba 3/21: Validación - ID es null.
    /// </summary>
    [Fact]
    public async Task Test03_UpdatePersonaAsync_WhenIdIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = null!,
            Nombre = "Test",
            Dni = "12345678A",
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("ID es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 4/21: Validación - ID es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test04_UpdatePersonaAsync_WhenIdIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = string.Empty,
            Nombre = "Test",
            Dni = "12345678A",
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("ID es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 5/21: Validación - ID es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test05_UpdatePersonaAsync_WhenIdIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = "   ",
            Nombre = "Test",
            Dni = "12345678A",
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("ID es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 6/21: Validación - Nombre es null.
    /// </summary>
    [Fact]
    public async Task Test06_UpdatePersonaAsync_WhenNombreIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = null!,
            Dni = "12345678A",
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("nombre es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 7/21: Validación - Nombre es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test07_UpdatePersonaAsync_WhenNombreIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = string.Empty,
            Dni = "12345678A",
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("nombre es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 8/21: Validación - Nombre es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test08_UpdatePersonaAsync_WhenNombreIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "   ",
            Dni = "12345678A",
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("nombre es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 9/21: Validación - DNI es null.
    /// </summary>
    [Fact]
    public async Task Test09_UpdatePersonaAsync_WhenDniIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "Test",
            Dni = null!,
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("DNI es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 10/21: Validación - DNI es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test10_UpdatePersonaAsync_WhenDniIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "Test",
            Dni = string.Empty,
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("DNI es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 11/21: Validación - DNI es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test11_UpdatePersonaAsync_WhenDniIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "Test",
            Dni = "   ",
            Email = "test@example.com",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("DNI es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 12/21: Validación - Email es null.
    /// </summary>
    [Fact]
    public async Task Test12_UpdatePersonaAsync_WhenEmailIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "Test",
            Dni = "12345678A",
            Email = null!,
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("email es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 13/21: Validación - Email es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test13_UpdatePersonaAsync_WhenEmailIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "Test",
            Dni = "12345678A",
            Email = string.Empty,
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("email es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 14/21: Validación - Email es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test14_UpdatePersonaAsync_WhenEmailIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "Test",
            Dni = "12345678A",
            Email = "   ",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("email es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 15/21: Validación - Edad es negativa.
    /// </summary>
    [Fact]
    public async Task Test15_UpdatePersonaAsync_WhenEdadIsNegative_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "Test",
            Dni = "12345678A",
            Email = "test@example.com",
            Edad = -1
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("edad debe estar entre 0 y 150", exception.Message);
    }

    /// <summary>
    /// Prueba 16/21: Validación - Edad es mayor a 150.
    /// </summary>
    [Fact]
    public async Task Test16_UpdatePersonaAsync_WhenEdadIsGreaterThan150_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = "Test",
            Dni = "12345678A",
            Email = "test@example.com",
            Edad = 151
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Equal("datosModificarPersona", exception.ParamName);
        Assert.Contains("edad debe estar entre 0 y 150", exception.Message);
    }

    #endregion

    #region 3. CASOS DE NEGOCIO

    /// <summary>
    /// Prueba 17/21: Caso de negocio - Persona no encontrada.
    /// </summary>
    [Fact]
    public async Task Test17_UpdatePersonaAsync_WhenPersonaDoesNotExist_ShouldReturnNull()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = Guid.NewGuid().ToString(), // UUID que no existe
            Nombre = "Test",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        // === WHEN (Act) ===
        var resultado = await repository.UpdatePersonaAsync(datosModificar);

        // === THEN (Assert) ===
        Assert.Null(resultado);
    }

    /// <summary>
    /// Prueba 18/21: Caso de negocio - DNI ya existe en otra persona.
    /// </summary>
    [Fact]
    public async Task Test18_UpdatePersonaAsync_WhenDniAlreadyExistsInAnotherPersona_ShouldThrowInvalidOperationException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear primera persona
        var primeraPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Primera",
            Dni = "11111111A",
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };
        await repository.AddPersonaAsync(primeraPersona);

        // Crear segunda persona
        var segundaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Segunda",
            Dni = "22222222B",
            Email = GenerarEmailAleatorio(),
            Edad = 25
        };
        var personaAModificar = await repository.AddPersonaAsync(segundaPersona);

        // Intentar modificar la segunda persona con el DNI de la primera
        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = personaAModificar.Id,
            Nombre = "Segunda Modificada",
            Dni = "11111111A", // DNI que ya existe en otra persona
            Email = GenerarEmailAleatorio(),
            Edad = 26
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Contains("Ya existe otra persona con el DNI 11111111A", exception.Message);
    }

    /// <summary>
    /// Prueba 19/21: Caso de negocio - Email ya existe en otra persona.
    /// </summary>
    [Fact]
    public async Task Test19_UpdatePersonaAsync_WhenEmailAlreadyExistsInAnotherPersona_ShouldThrowInvalidOperationException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear primera persona
        var primeraPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Primera",
            Dni = GenerarDniAleatorio(),
            Email = "primera@example.com",
            Edad = 30
        };
        await repository.AddPersonaAsync(primeraPersona);

        // Crear segunda persona
        var segundaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Segunda",
            Dni = GenerarDniAleatorio(),
            Email = "segunda@example.com",
            Edad = 25
        };
        var personaAModificar = await repository.AddPersonaAsync(segundaPersona);

        // Intentar modificar la segunda persona con el email de la primera
        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = personaAModificar.Id,
            Nombre = "Segunda Modificada",
            Dni = GenerarDniAleatorio(),
            Email = "primera@example.com", // Email que ya existe en otra persona
            Edad = 26
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.UpdatePersonaAsync(datosModificar));
        
        Assert.Contains("Ya existe otra persona con el email primera@example.com", exception.Message);
    }

    /// <summary>
    /// Prueba 20/21: Caso límite - Actualizar persona manteniendo su propio DNI.
    /// </summary>
    [Fact]
    public async Task Test20_UpdatePersonaAsync_WhenUpdatingPersonaWithSameDni_ShouldUpdateSuccessfully()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear persona inicial
        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = "12345678A",
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };
        var personaOriginal = await repository.AddPersonaAsync(datosNuevaPersona);

        // Modificar persona manteniendo el mismo DNI
        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = personaOriginal.Id,
            Nombre = "Test Modificado",
            Dni = "12345678A", // Mismo DNI
            Email = GenerarEmailAleatorio(),
            Edad = 31
        };

        // === WHEN (Act) ===
        var personaModificada = await repository.UpdatePersonaAsync(datosModificar);

        // === THEN (Assert) ===
        Assert.NotNull(personaModificada);
        Assert.Equal("Test Modificado", personaModificada.Nombre);
        Assert.Equal("12345678A", personaModificada.Dni);
        Assert.Equal(31, personaModificada.Edad);

        // Verificar en BD
        var personaEnBD = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == personaOriginal.Id);
        Assert.NotNull(personaEnBD);
        Assert.Equal("Test Modificado", personaEnBD.Nombre);
    }

    /// <summary>
    /// Prueba 21/21: Caso límite - Actualizar persona manteniendo su propio email.
    /// </summary>
    [Fact]
    public async Task Test21_UpdatePersonaAsync_WhenUpdatingPersonaWithSameEmail_ShouldUpdateSuccessfully()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // Crear persona inicial
        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = GenerarDniAleatorio(),
            Email = "test@example.com",
            Edad = 30
        };
        var personaOriginal = await repository.AddPersonaAsync(datosNuevaPersona);

        // Modificar persona manteniendo el mismo email
        var datosModificar = new DatosModificarPersonaDTO
        {
            Id = personaOriginal.Id,
            Nombre = "Test Modificado",
            Dni = GenerarDniAleatorio(),
            Email = "test@example.com", // Mismo email
            Edad = 31
        };

        // === WHEN (Act) ===
        var personaModificada = await repository.UpdatePersonaAsync(datosModificar);

        // === THEN (Assert) ===
        Assert.NotNull(personaModificada);
        Assert.Equal("Test Modificado", personaModificada.Nombre);
        Assert.Equal("test@example.com", personaModificada.Email);
        Assert.Equal(31, personaModificada.Edad);

        // Verificar en BD
        var personaEnBD = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == personaOriginal.Id);
        Assert.NotNull(personaEnBD);
        Assert.Equal("Test Modificado", personaEnBD.Nombre);
    }

    #endregion
}
