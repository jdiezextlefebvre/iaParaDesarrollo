
- SonarQube (smell code)
  Pruebas de calidad de código (pruebas estáticas) : REVISIONES

  Complejidad ciclomática: Número de caminos por el que un código puede ejecutarse de forma distinta. -> # de pruebas mínimas a realizar.




  UpdatePersonaAsync            HAPPY_PATH



  // Given.   Estado de partida
      Dado que... tengo una objeto NuevaPersonaDTO,
      con nombre FELIPE, DNI = ALEATORIO, EMAIL ALEATORIO, edad =33
      Y que llamo previamente a la función: AddPersonaAsync
      Y obtengo su ID
      Y creo un objeto ModificarPersonaDTO con
      ese ID, y el resto de datos modificados a algo concreto
  // When     La accion que quiero probar
      Cuando llamo a la función UpdatePersonaAsync
      y le paso esos datos modificados...

  // Then     El resultado esperado
        Entonces el resultado es OK
        Y si llamo a GetPersonaAsync con ese ID
        Entonces obtengo un PersonaDTO
        con todos esos datos modificados y el mismo ID que el original

FIRST
  - Fast
  **- Independent**
  **- Repeatable**
  - Self-validating
  - Timely

---

Plan de trabajo
1. Comunicar mi intención a la IA... pidíendole que no haga NADA! CONTEXTO!

    > Quiero montar ahora un nuevo proyecto de pruebas unitarias para comprobar el funcionamiento de las funciones del proyecto Domain.Impl... aunque la comunicación con esas funciones la haré usando las interfaces del proyecto Domain.Api. El proyecto lo voy a montar con XUnit.
    Como lo ves? NO HAGAS NADA POR AHORA !

2. PASO A PASO:
   √ Crear un proyecto de pruebas unitarias del proyecto Domain.Impl
   Sn pruebas... solo el esqueleto con las dependencias (con el proyecto Domain, xUnit)

> Por ahora SOLO quiero que crees el nuevo proyecto de pruebas unitarias... NO CREES AUN NI UNA SOLA PRUEBA DENTRO. Eso si, configura todas las dependencias que nos harán falta. El proyecto creo que podríamos llamarlo Domain.Impl.Test. Como lo ves?

3. √ Un listado de las funciones que considera que hay que probar.
4. Le pediré que me detalle a modo de ejemplo cómo plantea hacer las pruebas de una función... una prueba..

> Vamos bien. Lo siguiente que quiero es que me des un ejemplo de como planteas implementar el happy path de la funcion de modificar una persona... UpdatePersonaAsync... NO CREES NI ARCHIVOS NI NADA... solo ponlo en el chat

5. Le corregiré, si es necesario. Le confirmaré

> En general veo la prueba muy bien. Me gusta la segunda comprobación de la persistencia que has hecho, nos garantiza que no tenemos una implementación de tipo echo, pero... no me gusta que esa comprobación se haga a través del api, ya que asumimos que la función update debe funcionar solo si la funcion get funciona. Prefiero que esa comprobación se haga a través deL CONTEXT de la BBDD . podríamos hacerlo? Que piensas?

> Eso me gusta mucho más. IMPORTANTE: Ten este tipo de cosas en cuenta para todas las pruebas que vamos a desarrollar. De momento solo quiero que saques un listado de las pruebas unitarias que consideras necesarias para la funcion de update. NO LAS IMPLEMENTES. Muestralas

1. Luego le diré que me de el listado completo de pruebas que plantea relaizar sobre una función

> Es un listado extraordinario. Quiero que procedas a implementar esas 21 pruebas. En la documentación, ve numerándolas, para asegurar que no nos olvidamos de ninguna. Crea un fichero dentro del proyecto de pruebas SOLO PARA LAS PRUEBAS DEL update.
Por ahora no podremos ejecutar esas pruebas... ya que algunas dependen otras funciones como ADD, que aún no estan implementadas. Asegurate que compila y más adelante ejecutamos.


2. Lo corregiré
3. Le pediré que me genere el código de todas las pruebas unitarias ... poco a poco

 > Esto ha ido bien. Vamos a continuar con el resto . Por ahora no generes esas pruebas solo saca el listado completo de las pruebas de cada una del resto de las 4 funciones que nos faltan. NO LAS IMPLEMENTES TODAVÍA. SOLO EL LISTADO

 > Perfecto. Implementa todas esas pruebas, igual que antes, usando ficheros separados para cada una de las funciones. Recuerda NUMERARLAS igual que antes.


> Creo que ya es buen momento para ejecutar las pruebas. Crees que falta algo previo?
---


[Fact]
public async Task UpdatePersonaAsync_WhenPersonaExistsAndDataIsValid_ShouldUpdatePersonaSuccessfully()
{
    // === GIVEN (Arrange) ===
    // 1. Crear un DbContext en memoria independiente para esta prueba
    var options = new DbContextOptionsBuilder<PersonasDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // BD única por prueba
        .Options;
    
    using var context = new PersonasDbContext(options);
    var repository = new PersonasRepositoryEF(context);
    
    // 2. Crear datos iniciales para la persona
    var datosNuevaPersona = new DatosNuevaPersonaDTO
    {
        Nombre = "FELIPE",
        Dni = GenerarDniAleatorio(), // Método helper para generar DNI único
        Email = GenerarEmailAleatorio(), // Método helper para generar email único
        Edad = 33
    };
    
    // 3. Añadir la persona inicialmente
    var personaOriginal = await repository.AddPersonaAsync(datosNuevaPersona);
    var idPersona = personaOriginal.Id; // Guardar el UUID generado
    
    // 4. Crear datos modificados
    var datosModificados = new DatosModificarPersonaDTO
    {
        Id = idPersona,
        Nombre = "FELIPE MODIFICADO",
        Dni = GenerarDniAleatorio(), // Nuevo DNI válido
        Email = GenerarEmailAleatorio(), // Nuevo email válido
        Edad = 35
    };
    
    // === WHEN (Act) ===
    // 5. Ejecutar la función a probar
    var personaModificada = await repository.UpdatePersonaAsync(datosModificados);
    // === THEN (Assert) ===
    // 6. Verificar que la operación fue exitosa
    Assert.NotNull(personaModificada);
    Assert.Equal(idPersona, personaModificada.Id); // El ID no debe cambiar
    Assert.Equal("FELIPE MODIFICADO", personaModificada.Nombre);
    Assert.Equal(datosModificados.Dni, personaModificada.Dni);
    Assert.Equal(datosModificados.Email, personaModificada.Email);
    Assert.Equal(35, personaModificada.Edad);
    
    // 7. Verificar persistencia consultando DIRECTAMENTE en la BD
    var personaEnBD = await context.Personas
        .FirstOrDefaultAsync(p => p.Uuid == idPersona);

    Assert.NotNull(personaEnBD);
    Assert.Equal("FELIPE MODIFICADO", personaEnBD.Nombre);
    Assert.Equal(datosModificados.Dni, personaEnBD.Dni);
    Assert.Equal(datosModificados.Email, personaEnBD.Email);
    Assert.Equal(35, personaEnBD.Edad);
    // Verificar que el ID de BD no cambió (int autogenerado)
    Assert.True(personaEnBD.Id > 0);}
