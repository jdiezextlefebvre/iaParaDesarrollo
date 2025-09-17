# Arquitectura - PersonasCrud.Domain.API

## 🏗️ Vista Arquitectónica del Dominio

Esta documentación describe la arquitectura y diseño del API de dominio para la gestión de personas, incluyendo casos de uso, modelo de datos y contratos de la interfaz.

## 📋 Casos de Uso

### Vista General

```plantuml
@startuml PersonasCrud-Overview
!theme aws-orange
title Sistema de Gestión de Personas - Vista General\nPersonasCrud.Domain.API

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

' Relaciones simples
Cliente --> UC001
Cliente --> UC002
Cliente --> UC003
Cliente --> UC004
Cliente --> UC005

@enduml
```

### Vista Detallada

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
```

### Vista Completa con Validaciones

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

## 📦 Modelo de Datos

### Diagrama de Clases - DTOs e Interfaz

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

### Diagrama de Excepciones

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

## 🎯 Principios Arquitectónicos

### Separación de Responsabilidades
- **API Layer**: Define contratos e interfaces
- **DTO Layer**: Objetos de transferencia optimizados
- **Repository Pattern**: Abstracción de persistencia

### Inversión de Dependencias
- Las capas superiores dependen de abstracciones
- No hay referencias a implementaciones concretas
- Facilita testing y intercambio de implementaciones

### Domain-Driven Design
- Enfoque en el dominio de negocio (Personas)
- Contratos claros y expresivos
- Validaciones de dominio explícitas

## 📝 Patrones Implementados

- **Repository Pattern**: `IPersonasRepository`
- **DTO Pattern**: Separación de objetos de entrada/salida
- **Fail Fast**: Validaciones tempranas con excepciones
- **Async/Await**: Operaciones no bloqueantes
