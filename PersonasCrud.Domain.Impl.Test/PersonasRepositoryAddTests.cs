using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.API.DTOs;
using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Domain.Impl.Context;
using PersonasCrud.Domain.Impl.Repositories;

namespace PersonasCrud.Domain.Impl.Test;

/// <summary>
/// Pruebas unitarias para la función AddPersonaAsync del repositorio PersonasRepositoryEF.
/// Total: 18 pruebas unitarias que cubren todos los caminos de la función.
/// </summary>
public class PersonasRepositoryAddTests
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
    /// Prueba 1/18: Happy Path - Creación exitosa de persona con datos válidos.
    /// </summary>
    [Fact]
    public async Task Test01_AddPersonaAsync_WhenDataIsValid_ShouldCreatePersonaSuccessfully()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Juan Pérez",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        // === WHEN (Act) ===
        var personaCreada = await repository.AddPersonaAsync(datosNuevaPersona);

        // === THEN (Assert) ===
        // Verificar respuesta del método
        Assert.NotNull(personaCreada);
        Assert.NotNull(personaCreada.Id);
        Assert.NotEmpty(personaCreada.Id);
        Assert.Equal("Juan Pérez", personaCreada.Nombre);
        Assert.Equal(datosNuevaPersona.Dni, personaCreada.Dni);
        Assert.Equal(datosNuevaPersona.Email, personaCreada.Email);
        Assert.Equal(30, personaCreada.Edad);

        // Verificar que el UUID es un GUID válido
        Assert.True(Guid.TryParse(personaCreada.Id, out _));

        // Verificar persistencia consultando DIRECTAMENTE en la BD
        var personaEnBD = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == personaCreada.Id);

        Assert.NotNull(personaEnBD);
        Assert.Equal("Juan Pérez", personaEnBD.Nombre);
        Assert.Equal(datosNuevaPersona.Dni, personaEnBD.Dni);
        Assert.Equal(datosNuevaPersona.Email, personaEnBD.Email);
        Assert.Equal(30, personaEnBD.Edad);
        Assert.True(personaEnBD.Id > 0); // ID numérico autogenerado
    }

    #endregion

    #region 2. VALIDACIONES DE ENTRADA (ArgumentException)

    /// <summary>
    /// Prueba 2/18: Validación - Parámetro datosNuevaPersona es null.
    /// </summary>
    [Fact]
    public async Task Test02_AddPersonaAsync_WhenDatosNuevaPersonaIsNull_ShouldThrowArgumentNullException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => repository.AddPersonaAsync(null!));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
    }

    /// <summary>
    /// Prueba 3/18: Validación - Nombre es null.
    /// </summary>
    [Fact]
    public async Task Test03_AddPersonaAsync_WhenNombreIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = null!,
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("nombre es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 4/18: Validación - Nombre es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test04_AddPersonaAsync_WhenNombreIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = string.Empty,
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("nombre es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 5/18: Validación - Nombre es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test05_AddPersonaAsync_WhenNombreIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "   ",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("nombre es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 6/18: Validación - DNI es null.
    /// </summary>
    [Fact]
    public async Task Test06_AddPersonaAsync_WhenDniIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = null!,
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("DNI es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 7/18: Validación - DNI es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test07_AddPersonaAsync_WhenDniIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = string.Empty,
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("DNI es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 8/18: Validación - DNI es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test08_AddPersonaAsync_WhenDniIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = "   ",
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("DNI es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 9/18: Validación - Email es null.
    /// </summary>
    [Fact]
    public async Task Test09_AddPersonaAsync_WhenEmailIsNull_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = GenerarDniAleatorio(),
            Email = null!,
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("email es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 10/18: Validación - Email es cadena vacía.
    /// </summary>
    [Fact]
    public async Task Test10_AddPersonaAsync_WhenEmailIsEmpty_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = GenerarDniAleatorio(),
            Email = string.Empty,
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("email es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 11/18: Validación - Email es solo espacios en blanco.
    /// </summary>
    [Fact]
    public async Task Test11_AddPersonaAsync_WhenEmailIsWhitespace_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = GenerarDniAleatorio(),
            Email = "   ",
            Edad = 30
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("email es obligatorio", exception.Message);
    }

    /// <summary>
    /// Prueba 12/18: Validación - Edad es negativa.
    /// </summary>
    [Fact]
    public async Task Test12_AddPersonaAsync_WhenEdadIsNegative_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = -1
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("edad debe estar entre 0 y 150", exception.Message);
    }

    /// <summary>
    /// Prueba 13/18: Validación - Edad es mayor a 150.
    /// </summary>
    [Fact]
    public async Task Test13_AddPersonaAsync_WhenEdadIsGreaterThan150_ShouldThrowArgumentException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Test",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 151
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => repository.AddPersonaAsync(datosNuevaPersona));
        
        Assert.Equal("datosNuevaPersona", exception.ParamName);
        Assert.Contains("edad debe estar entre 0 y 150", exception.Message);
    }

    #endregion

    #region 3. CASOS DE NEGOCIO (InvalidOperationException)

    /// <summary>
    /// Prueba 14/18: Caso de negocio - DNI ya existe.
    /// </summary>
    [Fact]
    public async Task Test14_AddPersonaAsync_WhenDniAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var dniExistente = "12345678A";

        // Crear primera persona
        var primeraPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Primera Persona",
            Dni = dniExistente,
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };
        await repository.AddPersonaAsync(primeraPersona);

        // Intentar crear segunda persona con el mismo DNI
        var segundaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Segunda Persona",
            Dni = dniExistente, // Mismo DNI
            Email = GenerarEmailAleatorio(),
            Edad = 25
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.AddPersonaAsync(segundaPersona));
        
        Assert.Contains($"Ya existe una persona con el DNI {dniExistente}", exception.Message);
    }

    /// <summary>
    /// Prueba 15/18: Caso de negocio - Email ya existe.
    /// </summary>
    [Fact]
    public async Task Test15_AddPersonaAsync_WhenEmailAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var emailExistente = "test@example.com";

        // Crear primera persona
        var primeraPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Primera Persona",
            Dni = GenerarDniAleatorio(),
            Email = emailExistente,
            Edad = 30
        };
        await repository.AddPersonaAsync(primeraPersona);

        // Intentar crear segunda persona con el mismo email
        var segundaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Segunda Persona",
            Dni = GenerarDniAleatorio(),
            Email = emailExistente, // Mismo email
            Edad = 25
        };

        // === WHEN & THEN ===
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.AddPersonaAsync(segundaPersona));
        
        Assert.Contains($"Ya existe una persona con el email {emailExistente}", exception.Message);
    }

    #endregion

    #region 4. CASOS LÍMITE

    /// <summary>
    /// Prueba 16/18: Caso límite - Edad es 0.
    /// </summary>
    [Fact]
    public async Task Test16_AddPersonaAsync_WhenEdadIsZero_ShouldCreatePersonaSuccessfully()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Bebé",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 0
        };

        // === WHEN (Act) ===
        var personaCreada = await repository.AddPersonaAsync(datosNuevaPersona);

        // === THEN (Assert) ===
        Assert.NotNull(personaCreada);
        Assert.Equal(0, personaCreada.Edad);

        // Verificar en BD
        var personaEnBD = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == personaCreada.Id);
        Assert.NotNull(personaEnBD);
        Assert.Equal(0, personaEnBD.Edad);
    }

    /// <summary>
    /// Prueba 17/18: Caso límite - Edad es 150.
    /// </summary>
    [Fact]
    public async Task Test17_AddPersonaAsync_WhenEdadIs150_ShouldCreatePersonaSuccessfully()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var datosNuevaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Persona Muy Mayor",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 150
        };

        // === WHEN (Act) ===
        var personaCreada = await repository.AddPersonaAsync(datosNuevaPersona);

        // === THEN (Assert) ===
        Assert.NotNull(personaCreada);
        Assert.Equal(150, personaCreada.Edad);

        // Verificar en BD
        var personaEnBD = await context.Personas
            .FirstOrDefaultAsync(p => p.Uuid == personaCreada.Id);
        Assert.NotNull(personaEnBD);
        Assert.Equal(150, personaEnBD.Edad);
    }

    /// <summary>
    /// Prueba 18/18: Caso límite - Verificar que cada persona tiene UUID único.
    /// </summary>
    [Fact]
    public async Task Test18_AddPersonaAsync_ShouldGenerateUniqueUuidForEachPersona()
    {
        // === GIVEN (Arrange) ===
        using var context = CreateInMemoryContext();
        var repository = new PersonasRepositoryEF(context);

        var primeraPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Primera",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 30
        };

        var segundaPersona = new DatosNuevaPersonaDTO
        {
            Nombre = "Segunda",
            Dni = GenerarDniAleatorio(),
            Email = GenerarEmailAleatorio(),
            Edad = 25
        };

        // === WHEN (Act) ===
        var persona1 = await repository.AddPersonaAsync(primeraPersona);
        var persona2 = await repository.AddPersonaAsync(segundaPersona);

        // === THEN (Assert) ===
        Assert.NotNull(persona1.Id);
        Assert.NotNull(persona2.Id);
        Assert.NotEqual(persona1.Id, persona2.Id); // UUIDs deben ser diferentes

        // Verificar en BD que ambas personas existen con UUIDs diferentes
        var personasEnBD = await context.Personas.ToListAsync();
        Assert.Equal(2, personasEnBD.Count);
        Assert.NotEqual(personasEnBD[0].Uuid, personasEnBD[1].Uuid);
    }

    #endregion
}
