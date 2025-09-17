# Qué queremos generar?

- Objetivo... para qué sirve
- Casos de uso (UML<<- plantuml>>)-> Requisitos funcionales
- Arquitectura
  - Diagrama de paquetes/módulos <-- mermaid
  - Diagrama clases <-- mermaid
  - Diagrama de entidad - relación <-- mermaid
- Cómo le meto mano al proyecto:
  - Como lo compilo después de hacer cambios
  - Cómo lo pruebo después de hacer cambios
    - Si solo ejecutar un único fichero de test
  - Cómo generar la documentación de las clases (si aplica) / API C# (interfaces)
- Política en cuanto al trabajo con el repo (branches)
- Pruebas
  - Swagger iría en el controller REST 

Un buen sitio para meterlo, por lo menos muchas de estas cosas es un README.md


---
Qué hacemos:
1. Generamos diagrama de casos de uso (plantuml)

    > Necesito generar el diagrama de casos de uso en plantuml de el proyecto PrsonasCrud.Domain.API. Mételo en el chat

    > No va mal.. pero:
    El actor humano nos sobra... Nuestro api solo es consumido por una app cliente.

    En la leyenda, las validaciones no las metemos.. ya están en las notas. Las DTOs tampoco las queremos en este punto

    Generalo en vertical y no en horizontal:
    - Pon los actores a la izquierda
    - Pon los casos de uso a la derecha
    

2. Generamos diagrama de paquetes/módulos (mermaid)
3. Generamos diagrama de clases (mermaid)
 > VAmos a usar ahora mermaid para generar el diagrama de clases del proyecto PersonasCrud.Domain.API
 > Vamos a representar las dtos y la interfaz del repositorio


 > VAmos a usar ahora mermaid para generar otro diagrama de clases del proyecto PersonasCrud.Domain.API
 > Vamos a representar solamente la interfaz del repositorio y las excepciones que puede lanzar


4. Generamos diagrama de entidad-relación (mermaid)
5. Generamos documentación de la API C# (interfaces)
6. Integrarlo en un documento de arquitectura y documentación técnica (markdown)
  > GEnerame un archivo markdown con toda la documentación técnica del proyecto PersonasCrud.Domain.API, incluyendo estos diagramas que tengo en el archivo documentacion.md
  Añade el resto de información que consideres pertinente... y que aporte.. no rollo ni texto por engordar aquello innecesariamente
7. Crear el documento README.md con una intro al proyecto, las instrucciones de compilación, pruebas, generación de documentación y política de branches. Añade un enlace al documento de arquitectura y documentación técnica
  > GEnerame un archivo README.md con una intro al proyecto PersonasCrud.Domain.API, las instrucciones de compilación, pruebas, generación de documentación y política de branches. Añade un enlace al documento de arquitectura y documentación técnica que tenemos en documentacion.md


---

```plantuml

@startuml PersonasCrud-MainDetailedUseCases
!theme aws-orange
title Sistema de Gestión de Personas - Casos de Uso Principales (Detallado)\nPersonasCrud.Domain.API

left to right direction

' Actor principal
actor "Aplicación Cliente" as Cliente

' Paquete del sistema
package "Sistema PersonasCrud" {
  usecase "Crear Nueva Persona" as UC001
  usecase "Consultar Persona por ID" as UC002
  usecase "Actualizar Datos de Persona" as UC003
  usecase "Eliminar Persona" as UC004
  usecase "Listar Todas las Personas" as UC005
}

' Relaciones
Cliente --> UC001
Cliente --> UC002
Cliente --> UC003
Cliente --> UC004
Cliente --> UC005

' Notas descriptivas
note right of UC001
  **Crear Nueva Persona**
  - Entrada: DatosNuevaPersonaDTO
  - Salida: PersonaDTO con UUID asignado
  - Validaciones: Nombre, DNI, Email, Edad
  - Restricciones: DNI y Email únicos
end note

note right of UC002
  **Consultar Persona por ID**
  - Entrada: UUID string
  - Salida: PersonaDTO o null
  - Validación: Formato UUID válido
end note

note right of UC003
  **Actualizar Datos de Persona**
  - Entrada: DatosModificarPersonaDTO
  - Salida: PersonaDTO actualizado o null
  - Validaciones: Todos los campos + UUID
  - Restricciones: DNI y Email únicos
end note

note right of UC004
  **Eliminar Persona**
  - Entrada: UUID string
  - Salida: PersonaDTO eliminado o null
  - Validación: Formato UUID válido
end note

note right of UC005
  **Listar Todas las Personas**
  - Entrada: Ninguna
  - Salida: IList<PersonaDTO>
  - Ordenamiento: Por nombre alfabético
end note

@enduml
@enduml
```

```plantuml
@startuml PersonasCrud-AllUseCases
!theme aws-orange
title Sistema de Gestión de Personas - Casos de Uso Completos\nPersonasCrud.Domain.API

left to right direction

' Actor principal
actor "Aplicación Cliente" as Cliente

' Paquete del sistema
package "Sistema PersonasCrud" {

  ' Casos de uso principales (CRUD)
  usecase "Crear Nueva Persona" as UC001
  usecase "Consultar Persona por ID" as UC002
  usecase "Actualizar Datos de Persona" as UC003
  usecase "Eliminar Persona" as UC004
  usecase "Listar Todas las Personas" as UC005

  ' Casos de uso de validación (incluidos)
  usecase "Validar Datos de Entrada" as UC101
  usecase "Verificar Unicidad de DNI" as UC102
  usecase "Verificar Unicidad de Email" as UC103
  usecase "Validar Formato de UUID" as UC104
  usecase "Validar Rango de Edad" as UC105

}

' Relaciones Actor-Casos de Uso
Cliente --> UC001
Cliente --> UC002
Cliente --> UC003
Cliente --> UC004
Cliente --> UC005

' Relaciones de inclusión (<<include>>)
UC001 ..> UC101 : <<include>>
UC001 ..> UC102 : <<include>>
UC001 ..> UC103 : <<include>>
UC001 ..> UC105 : <<include>>

UC002 ..> UC104 : <<include>>

UC003 ..> UC101 : <<include>>
UC003 ..> UC104 : <<include>>
UC003 ..> UC102 : <<include>>
UC003 ..> UC103 : <<include>>
UC003 ..> UC105 : <<include>>

UC004 ..> UC104 : <<include>>

@enduml


```
---

```mermaid

classDiagram

    direction LR


    title PersonasCrud.Domain.API - Diagrama de Clases
    
    %% Namespace para DTOs
    namespace PersonasCrud-Domain-API-DTOs {
        class PersonaDTO {
            +string Id
            +string Nombre
            +string Dni
            +string Email
            +int Edad
        }
        
        class DatosNuevaPersonaDTO {
            +string Nombre
            +string Dni
            +string Email
            +int Edad
        }
        
        class DatosModificarPersonaDTO {
            +string Id
            +string Nombre
            +string Dni
            +string Email
            +int Edad
        }
    }
    
    %% Namespace para Repositories
    namespace PersonasCrud-Domain-API-Repositories {
        class IPersonasRepository {
            <<interface>>
            +AddPersonaAsync(DatosNuevaPersonaDTO datosNuevaPersona) Task~PersonaDTO~
            +GetPersonaAsync(string id) Task~PersonaDTO?~
            +DeletePersonaAsync(string id) Task~PersonaDTO?~
            +UpdatePersonaAsync(DatosModificarPersonaDTO datosModificarPersona) Task~PersonaDTO?~
            +ListPersonasAsync() Task~IList~PersonaDTO~~
        }
    }
    
    %% Relaciones
    IPersonasRepository ..> PersonaDTO : "retorna"
    IPersonasRepository ..> DatosNuevaPersonaDTO : "utiliza"
    IPersonasRepository ..> DatosModificarPersonaDTO : "utiliza"
    
    %% Notas informativas
    note for PersonaDTO "DTO principal que representa<BR>una persona completa con UUID"
    note for DatosNuevaPersonaDTO "DTO para crear personas<BR>(sin ID, se genera automáticamente)"
    note for DatosModificarPersonaDTO "DTO para actualizar personas<BR>(incluye ID para identificación)"
    note for IPersonasRepository "Interfaz del repositorio<BR>define operaciones CRUD"



```

```mermaid

classDiagram
    direction TB
    title PersonasCrud.Domain.API - Interfaz del Repositorio y Excepciones
    
    %% Interfaz del repositorio
    class IPersonasRepository {
        <<interface>>
        +AddPersonaAsync(DatosNuevaPersonaDTO) Task~PersonaDTO~
        +GetPersonaAsync(string id) Task~PersonaDTO?~
        +DeletePersonaAsync(string id) Task~PersonaDTO?~
        +UpdatePersonaAsync(DatosModificarPersonaDTO) Task~PersonaDTO?~
        +ListPersonasAsync() Task~IList~PersonaDTO~~
    }
    
    %% Excepciones del sistema .NET
    class ArgumentException {
        <<exception>>
        +ParamName string
        +Message string
    }
    
    class ArgumentNullException {
        <<exception>>
        +ParamName string
        +Message string
    }
    
    class InvalidOperationException {
        <<exception>>
        +Message string
    }
    
    %% Relaciones de excepciones
    IPersonasRepository ..> ArgumentException : throws
    IPersonasRepository ..> ArgumentNullException : throws
    IPersonasRepository ..> InvalidOperationException : throws
    
    %% Herencia de excepciones
    ArgumentNullException --|> ArgumentException : extends
    
    %% Notas explicativas
    note for IPersonasRepository "Repositorio principal del dominio"
    note for ArgumentException "Datos de entrada inválidos"
    note for InvalidOperationException "Violaciones de reglas de negocio"

```