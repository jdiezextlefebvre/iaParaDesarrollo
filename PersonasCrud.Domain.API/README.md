# PersonasCrud.Domain.API

## 📋 Descripción

API de dominio para la gestión de personas que define los contratos e interfaces para operaciones CRUD. Este proyecto forma parte de una arquitectura limpia y actúa como la capa de abstracción entre las capas superiores y la implementación del dominio.

## 🎯 Objetivos

- **Separación de responsabilidades**: Define contratos sin implementación
- **Inversión de dependencias**: Las capas superiores dependen de abstracciones
- **Transferencia de datos**: DTOs optimizados para comunicación entre capas
- **Validación de contratos**: Especifica excepciones y comportamientos esperados

## 🏗️ Estructura del Proyecto

```
PersonasCrud.Domain.API/
├── DTOs/
│   ├── PersonaDTO.cs
│   ├── DatosNuevaPersonaDTO.cs
│   └── DatosModificarPersonaDTO.cs
└── Repositories/
    └── IPersonasRepository.cs
```

## 📚 Documentación

- **[Arquitectura y Diagramas](ARCHITECTURE.md)** - Casos de uso, diagramas de clases y principios arquitectónicos

## 🛠️ Desarrollo

### Requisitos Previos
- **.NET 9.0 SDK** o superior
- **IDE compatible**: Visual Studio 2022, VS Code, JetBrains Rider

### Compilación

```bash
# Compilar el proyecto
dotnet build

# Compilar en modo Release
dotnet build --configuration Release

# Limpiar y compilar
dotnet clean && dotnet build
```

### Verificación

```bash
# Verificar que no hay errores de compilación
dotnet build --verbosity normal

# Verificar warnings y errores
dotnet build --verbosity detailed
```

### Análisis de Código

```bash
# Análisis estático con .NET analyzers
dotnet build --verbosity normal

# Formateo de código
dotnet format

# Verificar formato sin aplicar cambios
dotnet format --verify-no-changes
```

### Generación de Documentación

```bash
# Generar documentación XML (configurado en .csproj)
dotnet build --configuration Release

# La documentación XML se genera automáticamente en:
# bin/Release/net9.0/PersonasCrud.Domain.API.xml
```

### Comandos Útiles

```bash
# Información del proyecto
dotnet list package

# Verificar versiones de .NET disponibles
dotnet --list-sdks

# Verificar información del proyecto
dotnet --info

# Limpiar artefactos de compilación
dotnet clean
```

## 🔧 Configuración del Proyecto

### Características Habilitadas
- **ImplicitUsings**: Referencias using implícitas
- **Nullable**: Tipos de referencia nullable habilitados
- **TreatWarningsAsErrors**: Warnings tratados como errores
- **DocumentationFile**: Generación automática de documentación XML

### Estructura de Directorios
```
PersonasCrud.Domain.API/
├── bin/                    # Binarios compilados
├── obj/                    # Archivos temporales de compilación
├── DTOs/                   # Objetos de transferencia de datos
├── Repositories/           # Interfaces del repositorio
├── PersonasCrud.Domain.API.csproj    # Archivo del proyecto
├── README.md              # Este archivo
└── ARCHITECTURE.md        # Documentación arquitectónica
```

## 🧪 Pruebas

### Ejecución de Pruebas
```bash
# Ejecutar todas las pruebas del dominio
cd ..
dotnet test PersonasCrud.Domain.Impl.Test

# Ejecutar pruebas con detalle
dotnet test PersonasCrud.Domain.Impl.Test --logger "console;verbosity=detailed"

# Ejecutar solo un archivo de pruebas específico
dotnet test PersonasCrud.Domain.Impl.Test --filter "PersonasRepositoryUpdateTests"

# Ejecutar una prueba específica
dotnet test PersonasCrud.Domain.Impl.Test --filter "Test01_UpdatePersonaAsync_WhenPersonaExistsAndDataIsValid_ShouldUpdatePersonaSuccessfully"
```

### Cobertura de Pruebas
```bash
# Instalar herramienta de cobertura (solo una vez)
dotnet tool install --global coverlet.console

# Ejecutar pruebas con cobertura
dotnet test PersonasCrud.Domain.Impl.Test --collect:"XPlat Code Coverage"

# Generar reporte HTML de cobertura
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```

### Tipos de Pruebas Disponibles
- **Pruebas de Repositorio**: 60 pruebas unitarias para `PersonasRepositoryEF`
  - `PersonasRepositoryUpdateTests`: 21 pruebas
  - `PersonasRepositoryAddTests`: 18 pruebas
  - `PersonasRepositoryGetTests`: 7 pruebas
  - `PersonasRepositoryDeleteTests`: 8 pruebas
  - `PersonasRepositoryListTests`: 6 pruebas

## 🌿 Política de Branches

### Estructura de Branches
```
main                    # Rama principal (estable)
├── develop            # Integración de nuevas características
├── feature/*          # Nuevas funcionalidades
├── bugfix/*          # Corrección de errores
├── hotfix/*          # Correcciones urgentes en producción
└── release/*         # Preparación de releases
```

### Workflow de Desarrollo
1. **Feature Development**:
   ```bash
   git checkout develop
   git pull origin develop
   git checkout -b feature/nueva-funcionalidad
   # ... desarrollar ...
   git push origin feature/nueva-funcionalidad
   # Crear Pull Request a develop
   ```

2. **Bug Fixes**:
   ```bash
   git checkout develop
   git checkout -b bugfix/correccion-error
   # ... corregir ...
   git push origin bugfix/correccion-error
   # Crear Pull Request a develop
   ```

3. **Hotfixes**:
   ```bash
   git checkout main
   git checkout -b hotfix/correccion-urgente
   # ... corregir ...
   git push origin hotfix/correccion-urgente
   # Crear Pull Request a main Y develop
   ```

### Convenciones de Commit
```bash
# Formato: tipo(scope): descripción

feat(dto): añadir validación de email en PersonaDTO
fix(repository): corregir validación de UUID en GetPersonaAsync
docs(readme): actualizar instrucciones de compilación
test(repository): añadir pruebas para casos límite de edad
refactor(interface): simplificar firma de métodos async
```

### Validaciones Pre-commit
```bash
# Ejecutar antes de cada commit
dotnet build                    # Compilación exitosa
dotnet test                     # Todas las pruebas pasan
dotnet format --verify-no-changes  # Código formateado correctamente
```

## 🤝 Contribución

### Proceso de Contribución
1. **Fork** el repositorio
2. **Crear** una rama desde `develop`
3. **Desarrollar** siguiendo las convenciones del proyecto
4. **Ejecutar** todas las validaciones pre-commit
5. **Crear** Pull Request con descripción detallada
6. **Revisar** feedback y aplicar cambios si es necesario

### Checklist de Pull Request
- [ ] Código compila sin warnings
- [ ] Todas las pruebas pasan
- [ ] Código formateado correctamente
- [ ] Documentación actualizada si es necesario
- [ ] Commit messages siguen las convenciones
- [ ] No hay conflictos con la rama base

## 📚 Enlaces Útiles

### Documentación del Proyecto
- [Arquitectura y Diagramas](ARCHITECTURE.md)
- [Casos de Uso Detallados](ARCHITECTURE.md#casos-de-uso)
- [Modelo de Datos](ARCHITECTURE.md#modelo-de-datos)

### Tecnologías Utilizadas
- [.NET 9.0](https://docs.microsoft.com/en-us/dotnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [XUnit](https://xunit.net/) - Framework de pruebas
- [PlantUML](https://plantuml.com/) - Diagramas de casos de uso
- [Mermaid](https://mermaid-js.github.io/) - Diagramas de clases

### Recursos de Clean Architecture
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [Domain-Driven Design](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

---

**📄 Licencia**: Este proyecto es parte de un ejercicio de formación en desarrollo con IA.

**👥 Equipo**: Desarrollado como ejemplo de arquitectura limpia y buenas prácticas en .NET.

## 🔍 DTOs

### PersonaDTO
DTO principal que representa una persona completa:
- `Id`: UUID string (único)
- `Nombre`: Nombre completo
- `Dni`: Documento de identidad (único)
- `Email`: Correo electrónico (único)  
- `Edad`: Edad en años (0-150)

### DatosNuevaPersonaDTO
DTO para crear nuevas personas (sin ID):
- `Nombre`: Nombre completo (requerido)
- `Dni`: Documento de identidad (requerido, único)
- `Email`: Correo electrónico (requerido, único)
- `Edad`: Edad en años (0-150)

### DatosModificarPersonaDTO
DTO para actualizar personas existentes:
- `Id`: UUID de la persona a modificar (requerido)
- `Nombre`: Nuevo nombre completo
- `Dni`: Nuevo documento de identidad (único)
- `Email`: Nuevo correo electrónico (único)
- `Edad`: Nueva edad en años (0-150)

## 📋 API Reference

### IPersonasRepository

| Método | Propósito | Entrada | Salida | Excepciones |
|--------|-----------|---------|---------|-------------|
| `AddPersonaAsync` | Crea nueva persona | `DatosNuevaPersonaDTO` | `PersonaDTO` | `ArgumentException`, `InvalidOperationException` |
| `GetPersonaAsync` | Obtiene persona por ID | `string id` | `PersonaDTO?` | `ArgumentException` |
| `UpdatePersonaAsync` | Actualiza persona | `DatosModificarPersonaDTO` | `PersonaDTO?` | `ArgumentException`, `InvalidOperationException` |
| `DeletePersonaAsync` | Elimina persona | `string id` | `PersonaDTO?` | `ArgumentException` |
| `ListPersonasAsync` | Lista todas las personas | - | `IList<PersonaDTO>` | - |

## ⚡ Reglas de Negocio

### Validaciones de Entrada
- **Campos obligatorios**: No pueden ser `null` o vacíos
- **Edad**: Debe estar entre 0 y 150 años
- **UUID**: Debe tener formato válido para identificadores

### Restricciones de Unicidad
- **DNI**: Único en todo el sistema
- **Email**: Único en todo el sistema
- **ID**: UUID único generado automáticamente

### Comportamiento de Excepciones
- **ArgumentException**: Validaciones de formato y rango
- **ArgumentNullException**: Parámetros obligatorios nulos
- **InvalidOperationException**: Violaciones de reglas de negocio

## 🎯 Principios de Diseño

- **Inversión de Dependencias**: Solo define contratos, no implementaciones
- **Responsabilidad Única**: Cada DTO tiene un propósito específico
- **Inmutabilidad de Contratos**: Los DTOs son estables para comunicación entre capas
- **Fail Fast**: Validaciones tempranas con excepciones específicas

## 📝 Notas de Implementación

- Este proyecto **NO contiene implementaciones**, solo definiciones
- Los DTOs usan propiedades públicas para facilitar serialización
- Las interfaces son asíncronas (`Task<T>`) para escalabilidad
- Se utiliza `nullable reference types` (.NET 9.0)
