
# IAs para desarrolladores

- Desarrollo
- Pruebas
- Documentación

---

Programa de chatbot:
- Plugin de Chatgpt para VisualStudioCode

Modelo de IA:
- GPTs de OpenAI
- Gemini de Google
- Claude de Anthropic
- ...

Usos:
- Preguntas... dudas a chatbots: CHATGPT
  - General: APIs. librerías
  - Código error
  - Código ejemplo

Autocompletados (TAB) (plugins dentros de los IDEs):
- VisualStudioCode
- VisualStudio
- IntelliJ, PhpStorm, PyCharm

Hoy en día tiramos mucho de los modos AGENTE!
En estos modos, se instalan con plugins en los entornos de desarrollo y me permiten directamente modificar mi base de código. Incluso tienen capacidad para integrarse en la terminal y ejecutar comandos.

Esos agentes... no están muy finos! Cada día mejor... pero les falta!
- Problema con los prompts (ESTO ES COMPLICADO DE RESOLVER A CORTO PLAZO... por suerte!)
- Capacidad muy MUY limitada de memoria (capacidad de retener todo el proyecto en la cabeza), tamaño del prompt!
  - Si les pido muchas tareas de golpe
  - Si les pido tareas muy complejas
  - Si les pido que arreglen mucho código de una
  FALLAN MUCHO! MUCHISIMO!

  Con mucha frecuencia nos hacen destrozos en el código.
  Se hace indispensable tener un buen control de versiones (GIT) y hacer commits frecuentes.
  - commits en modo amend

Dicho esto... parece que no merece la pena usar estas herramientas!
SI LA MERECE... solo hay que tener en cuenta sus limitaciones y usarlas con cabeza.
Podemos tener un ahorro de tiempo de un 70%.
Y puede ser a la vez a muy frustrante. De ese 30% del tiempo que nos queda... y que trabajamos. El 90% del 30% nos la pasamos pidiéndole 17 veces a la IA que genera la misma mierda que no está siendo capaz de generar de una.

Poco a poco con estas herramientas... conseguimos mucho.
Y .. hay que ser muy muy específico con los prompts.
---

> Un producto de software por definición es un producto sujeto a cambios y mantenimiento.

Que un programa funciones... eso se da por hecho! 
Si no funciona... por definición, NO ES UN PRODUCTO DE SOFTWARE!

Un Coche por definición es un producto sujeto a (cambios y) mantenimientos.

    ESCRIBIMOS CÓDIGO <> PRUEBAS -> OK -> REFACTORIZACIÓN <> PRUEBAS -> OK (A falta de documentar)
    <-------------------------------->    <------------------------------>
            50% del trabajo                     50% del trabajo
            8 horas/día                         8 horas/día  

---

El diseñar un software es complejo.. y especificarlo mucho más.
Con un prompt olvidaros de crear un buen programa!

Una ventaja que tienen estas herramientas es que escriben mucho código y muy rápido...
Y tenemos que aprovecharnos de ello.

---

C#. Servicio REST (CRUD) Personas

GET http://app.com/api/v1/personas
GET http://app.com/api/v1/personas/{id}
DELETE http://app.com/api/v1/personas/{id}
POST http://app.com/api/v1/personas
PUT http://app.com/api/v1/personas/{id}
PATCH http://app.com/api/v1/personas/{id}

En paralelo haremos pruebas unitarias... e iremos montando documentación.

UML? Lenguaje para pintar DIAGRAMAS.
Lenguajes para ESCRIBIR DIAGRAMAS.
- mermaid
- plantuml

---

Solución -> App que quiero montar
   Proyectos

Arquitectura Limpia: CleanArchitecture
- Capa Dominio
  - Repositorios                            Interfaz | Implementación
- Capa Negocio
  - Servicio                                Interfaz | Implementación
- Capa de Exposición    
  - Controlador HTTP REST v1/v2, SOAP       Interfaz | Implementación

DTO: Data Transfer Object: PersonaDTO
Entity: PersonaEntity -> Esto es lo que se persiste en un repositorio

---

# Principio de Inversión de dependencias


    IPersonasRepository
        addPersona(DatosAltaPersonaDTO):: Task<PersonaDTO>
        getPersona(id...uuid):: Task<PersonaDTO>
        deletePersona(id...uuid):: Task<PersonaDTO>
        updatePersona(DatosAModificarPersonaDTO):: Task<PersonaDTO>
        listPersonas():: Task<List<PersonaDTO>>

    Persona (Objeto persistible en BBDD)
        id (BBDD debe ser autogenerado... y secuencial (numérico))
        uuid (Identificador universal único... cadena alfanumérica)
          ASDJ-1234-5678-ABCD
        nombre
        dni
        email
        edad

    PersonaDTO (Objeto que se transfiere entre capas)
        id (uuid)
        nombre
        dni
        email
        edad

    Las excepciones son para controlar casos EXCEPCIONALES.. cosas que a priori no se si se van a poder realizar:
    - No se si cuando vaya a la BBDD la BBDD va a estar ahí!
        try {
            // Código que puede lanzar una excepción
        } catch (Exception e) {
            // Manejo de la excepción
        }

---
    Capa Dominio - (API)
        PersonasRepository : IPersonasRepository
            addPersona(DatosAltaPersonaDTO):: Task<PersonaDTO>
            getPersona(id...uuid):: Task<PersonaDTO>
            deletePersona(id...uuid):: Task<PersonaDTO>
            updatePersona(DatosAModificarPersonaDTO):: Task<PersonaDTO>
            listPersonas():: Task<List<PersonaDTO>>
        PersonaDTO
            id (uuid)
            nombre
            dni
            email
            edad
---
    Implementación de la Capa de Dominio mediante el EntityFramework de .net
        PersonaEntity
            id (BBDD debe ser autogenerado... y secuencial (numérico))
            uuid (Identificador universal único... cadena alfanumérica)
              ASDJ-1234-5678-ABCD
            nombre
            dni
            email
            edad
        PersonasRepositoryEF : IPersonasRepository
            addPersona(DatosAltaPersonaDTO):: Task<PersonaDTO>
            getPersona(id...uuid):: Task<PersonaDTO>
            deletePersona(id...uuid):: Task<PersonaDTO>
            updatePersona(DatosAModificarPersonaDTO):: Task<PersonaDTO>
            listPersonas():: Task<List<PersonaDTO>>
        PersonasMapper
            PersonaDTO -> PersonaEntity
            PersonaEntity -> PersonaDTO
---
    Capa Negocio - (API)
        IPersonasService
            addPersona(DatosAltaPersonaDTO):: Task<PersonaDTO>
                // Persistir la persona en el repositorio
                repositorio.addPersona(DatosAltaPersonaDTO)
                // Enviar email
                emailService.sendEmail(DatosAltaPersonaDTO.email)
            getPersona(id...uuid):: Task<PersonaDTO>
            deletePersona(id...uuid):: Task<PersonaDTO>
            updatePersona(DatosAModificarPersonaDTO):: Task<PersonaDTO>
            listPersonas():: Task<List<PersonaDTO>>
        PersonaServiceDTO
            id (uuid)
            nombre
            dni
            email
            edad
        DatosNuevaPersonaServiceDTO
            nombre
            dni
            email
            edad
        DatosModificarPersonaServiceDTO
            id (uuid)
            nombre
            dni
            email
            edad
---
    Capa Negocio - (Implementación)
        PersonasService: IPersonasService
            addPersona(DatosAltaPersonaDTO):: Task<PersonaDTO> throw PersonaAlreadyExistsException
                // Persistir la persona en el repositorio
                repositorio.addPersona(DatosAltaPersonaDTO)
                // Enviar email
                emailService.sendEmail(DatosAltaPersonaDTO.email)
            getPersona(id...uuid):: Task<PersonaDTO>
            deletePersona(id...uuid):: Task<PersonaDTO>
            updatePersona(DatosAModificarPersonaDTO):: Task<PersonaDTO>
            listPersonas():: Task<List<PersonaDTO>>
        Mapper
            PersonaDTO -> PersonaServiceDTO
            PersonaServiceDTO -> PersonaDTO
        PersonaAlreadyExistsException : Exception
--- 
    IPersonasControllerRestv1
        addPersona(DatosAltaPersonaRestDTO):: Task<PersonaRestDTO>
            PersonaAlreadyExistsException -> HTTP 409 Conflict
        getPersona(id...uuid):: Task<PersonaRestDTO>
        deletePersona(id...uuid):: Task<PersonaRestDTO>
        updatePersona(DatosAModificarPersonaRestDTO):: Task<PersonaRestDTO>
        listPersonas():: Task<List<PersonaRestDTO>>
    PersonaRestDTO
        id (uuid)
        nombre
        dni
        email
        edad    
    DatosNuevaPersonaRestDTO
        nombre
        dni
        email
        edad
    DatosModificarPersonaRestDTO
        id (uuid)
        nombre
        dni
        email
        edad
---
    PersonasControllerRestv1 : IPersonasControllerRestv1
        addPersona(DatosAltaPersonaRestDTO):: Task<PersonaRestDTO> throw PersonaAlreadyExistsException
            // Validar datos de entrada
            if (DatosAltaPersonaRestDTO.nombre is null or empty) throw new ArgumentException("El nombre es obligatorio")
            if (DatosAltaPersonaRestDTO.dni is null or empty) throw new ArgumentException("El dni es obligatorio")
            if (DatosAltaPersonaRestDTO.email is null or empty) throw new ArgumentException("El email es obligatorio")
            if (DatosAltaPersonaRestDTO.edad < 0 or DatosAltaPersonaRestDTO.edad > 150) throw new ArgumentException("La edad no es válida")
            // Mapear DTO de REST a DTO de Servicio
            DatosNuevaPersonaServiceDTO datosService = Mapper.Map<DatosNuevaPersonaServiceDTO>(DatosAltaPersonaRestDTO)
            // Llamar al servicio
            PersonaServiceDTO personaService = personasService.addPersona(datosService)
            // Mapear DTO de Servicio a DTO de REST
            PersonaRestDTO personaRest = Mapper.Map<PersonaRestDTO>(personaService)
            return personaRest
        getPersona(id...uuid):: Task<PersonaRestDTO>
        deletePersona(id...uuid):: Task<PersonaRestDTO>
        updatePersona(DatosAModificarPersonaRestDTO):: Task<PersonaRestDTO>
        listPersonas():: Task<List<PersonaRestDTO>>
        Mapper
            PersonaServiceDTO -> PersonaRestDTO
            PersonaRestDTO -> PersonaServiceDTO



---

Diagrama de paquetes:

                                        DomainAPI   <---  DomainImpl
                                            ^                ^ 
                                            |                |
                        ServiceAPI   <---  ServiceImpl       |
                            ^                   ^            |
                            |                   |            |
        RestV1API  <---  RestV1Impl             |            |
                            ^                   |            |
                            |                   |            |
                             Aplicación Web (con su servidor)


---

Bicicleta: BTWIN
- Ruedas.  (Especificación)             <- Mavic Crossmax
- Manillar. (Especificación)            <- FSA K-Force
- Pedales. (Especificación)
- Cadena.  (Especificación)
- Cuadros  (Especificación)
- Sillín (Especificación)
- Sistema de frenos (Especificación)    <- Shimano XT

Integrar esos componentes (Implementación)
Mi bicicleta no depende de los interfaces... depende de las implementaciones concretas.

---

Tenemos claro que es una prueba UNITARIA?
    ServicioImpl.addPersona(DatosAltaPersonaDTO)   (Prueba unitaria?)


---

Rueda <- La probaré primero
    Monto la rueda en 4 hierros mal soldados (bastidor)
    Le pego un viaje con la mano y veo que gira..
Sistema de frenos <- La probaré antes de montar la bicicleta
    Cómo pruebo el sistema de frenos?
    - Prueba unitaria del sistema de frenos: Una prueba unitaria es una prueba de un COMPONENTE AISLADO
      - Necesito 4 hierros mal soldados (bastidor) al que le atornillo el sistema de frenos... y la palanca.
    - Aprieto la palanca (SistemaFrenos.apretarPalanca())
      - Voy comprobar que la pinza de freno se cierra...
      - Pero... se cierra con la fuerza suficiente? Quizás me interese colocar entre medias de la pinza un sensor de presión.

Esas pruebas me garantizan que la bici va a funcionar? NO
    Pa' que las hago? Qué me dan?                          CONFIANZA +1         Voy bien


Si estas pruebas funcionan... me tengo que meter con el siguiente nivel de pruebas: Pruebas de Integración
En ellas pruebo la COMUNICACION entre 2 componentes.
    - Rueda + Sistema de frenos
      Monto el sistema de frenos en un bastidor... coloco en medio de las pinzas la rueda.
      Le pego un viaje a la rueda... y aprieto la palanca. 
      Tengo que asegurarme que la rueda frena! Y MIRA QUE NO!

Si todas estas pruebas funcionan,... eso me garantiza que la bici va a funcionar? NO
    Pa' que las hago? Qué me dan?                          CONFIANZA +1         Voy bien

Cuando estas pruebas funcionan... me meto con el siguiente nivel de pruebas: Pruebas de Sistema
Se centran en el comportamiento del sistema completo.
    Ya tengo bici... me subo y me voy a Toledo...

Si estas pruebas funcionan... eso me garantiza que la bici va a funcionar? SI
    Si estas pruebas me garantizan que la bici funciona.. para que quiero las otras?
    Si hago estas pruebas y han funcionado... necesito pruebas unitarias y de sistema? YA NO... pa' que?

    El truco es doble:
    - Y si no funcionan, qué falla? NPI
    - Cuando puedo hacer estas pruebas? Cuando tengo la bici (el sistema completo)... y mientras tanto? No se como voy?

Las pruebas deben ser OPORTUNAS... hacerse en el momento adecuado.

Eso mismo me va a pasar con nuestro servicioREST.

A la hora de diseñar pruebas tenemos que seguir unos principios, igual que en desarrollo.

# SOLID

Single Responsibility Principle (SRP)
Open/Closed Principle (OCP)
Liskov Substitution Principle (LSP)
Interface Segregation Principle (ISP)
Dependency Inversion Principle (DIP)


# FIRST

Fast
Independent
Repeatable
Self-Validating
Timely <- Oportunas

---

# Metodologías Tradicionales: Cascada

    TOMA DE REQUISITOS
        Análisis
            Diseño
                Implementación
                    Pruebas
                        Despliegue
                            Mantenimiento

# Hoy en día, trabajamos con metodologías ágiles: SCRUM, XP, Kanban, ...

    ENTREGAR EL PRODUCTO DE FORMA INCREMENTAL -> Feedback continuo del cliente


Metodologías ágiles:
- Corbatillas    IIIIIIII
- Desarrollador

Entrega 1: 10% de la funcionalidad -> PASO A PRODUCCIÓN
    Pruebas a nivel de producción 10%
Entrega 2: +15% de la funcionalidad -> PASO A PRODUCCIÓN
    Pruebas a nivel de producción 15%??? He tocao código... a ver si jodo algo?
        15% + 10% = 25%
Entrega n....


- Las pruebas me crecen como enanos!
  
De donde sale la pasta? Y los recursos ? y el tiempo? NO LA HAY. Ni TIEMPO NI PASTA NI RECURSOS!!!
Solución: Automatizar las pruebas.... las vamos a hacer de continuo.


> Extraído del manifiesto ágil:

El software funcionando es la **medida** principal de progreso => Define un indicador de un cuadro de mando

        NS
    ------
    La medida principal de progreso es el "software funcionando"
    ------------------------------- ---------------------------
    sujeto                                  predicado

La forma en la que voy a medir (indicador) cuánto avanza el proyecto es el mediante el concepto: "software funcionando"

"software funcionando"?? Software que funciona.

> Quién dice que el software funciona? 

    El cliente??? ABERRANTE!
    LAS PRUEBAS !!!!

---

# DEVOPS

Cultura, filosofía, un movimiento en pro de la AUTOMATIZACION. Dev---> Ops
               AUTOMATIZABLE?     HERRAMIENTAS
    PLAN              x
    CODE              x
    BUILD             √     .net: dotnet msbuild nuget
    --------------------------------> Desarrollo ágil
    TEST
        definición    x
        ejecución     √     .net: dotnet test  NUnit, xUnit, MSTest
    --------------------------------> Integración continua
    Tener continuamente la última versión del software instalada en el entorno de integración sometida a pruebas automáticas. ---> Lo hago con un script PIPELINE DE CI (un programa saca el código del repositorio, lo compila, lo despliega en un entorno de integración y ejecuta las pruebas automáticas) => INFORME DE PRUEBAS
    RELEASE           √
    --------------------------------> Entrega continua (Continuous Delivery)
    DEPLOY            √
    --------------------------------> Despliegue continuo (Continuous Deployment)
    OPERATE           √
    MONITOR           √
    -------------------------------> Devops completo

---

Quiero montar un servicio REST con C# .net para hacer un crud de personas.
Vamos a seguir una arquitectura limpia, separando capa de dominio, negocio y exposición. De cada capa montaremos 2 proyectos independientes, uno con las interfaces (API) y otro con las implementaciones (Impl).
Todo ello lo tendremos en una única solución de .net.
La persistencia la haremos con el EntityFramework de .net.

Por ahora SOLO vas a montar la estructura base de la solución... después iremos trabajando en cada proyecto de forma independiente. Te iré dando instrucciones poco a poco.

---

Vamos a comenzar con capa de dominio. SOLO EL API. Montemos una interfaz para el repositorio y DTOS para Persona, NuevaPersona y ModificarPersona.
El DTO De persona necesita llevar id(generado autonumerico secuencial por la BBDD) , publicid (guid,uuidv4), nombre, dni, email y edad.

---
Añade un identificador publico a la entidad. Se debe generar en automático con un uuidv4 al crear la entidad (en el mapper). Ese campo es el que se expone en la dto como campo id. El id de la entidad no se expone. es interno.Actualiza la
s funciones del respositorio (tanto interfaz como implementación) para que reflejen el cambio

---
Si... vamos a por la implementación de la capa de dominio. Vamos a usar el EntityFramework de .net para la persistencia. Por ahora creamos una implementación que use una bbdd inmemory para ir jugando. Más adelante la cambiaremos por una BBDD Real.
En esta capa necesitamos una entidad para Persona, una implementación del repositorio y un mapeador entre las DTOs y PersonaEntity.