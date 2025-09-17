

Capa Dominio API  - Persistencia a BBDD
Capa Dominio Impl - Trabajar con Entity Framework BBDD Relacional.. incluye implementación para InMemory
Capa Servicio API - Contratos de servicio
Capa Servicio Impl - Implementación de los servicios
Capa RESTv1 API -   Contratos REST (DTOs, controladores)
Capa RESTv1 Impl -  Implementación de los controladores REST (mapeo, llamadas a servicios, etc)
                    Documentación XML para Swagger
Aplicación      - Configurar un entorno (Servidor web) donde alojar el Servicio Rest 
                    y Swagger
                  Esto nos permite en el futuro definir nuevos servicios que podamos alojar en el mismo entorno

En cada capa tenemos sus propias DTOs con mapeadores para convertir entre capas.

He montado 5 proyectos:
- Servicio x2
- RESTv1 x2
- Aplicación x1

---

# Angular Frontend

Typescript -> transpilación -> JS

Está implementado para ayudarnos a cumplir con ciertos principios de diseño y arquitectura:
SOLID

Angular es un contenedor de inversión de control.

> ng: Cliente que nos permite automatizar muchas tareas de proyectos Angular

JS ya tiene una herramienta similar a dotnet+nuget... para automatizar tareas: npm + yarn
ng trabaja sobre ellas.

# Componentes Web que vamos a montar:

CRUD de personas:
- Componente Listado de personas
  - Componente Item del listado de personas
- Componente Formulario Edición Persona
- Componente Formulario Nueva Persona
- Componente Detalle Persona
- Componente App (componente raíz)
- Servicio para la gestión de personas (llamadas al API REST)
- Modelos (DTOs, para albergar la información que necesitan manejar nuestros componentes)
Para hacer el desarrollo, y facilitar un poco algunas pruebas simples, vamos a usar json-server (FAKE).
Para pruebas automatizadas usaremos Mocks/Dummies/Spies/Stubs
- Modelos para recoger la información que nos devuelve el API REST
- Mapeadores para convertir entre los modelos del API REST y los modelos que usan nuestros componentes

    Componente Listado de personas -----------------------> ServicioFrontal ------------> API REST
    ├── Modelo: PersonasList (colección de Persona) <------ :: getAll()      <----json--- IEnumerable[PersonaRestDTO]
    ├── Componente Item del listado de personas          <---mapper---PersonasListRest
        └── Modelo: Persona (id, nombre, dni, email, edad)

---
Angular cambió hace tiempo... No tanto.. y de hecho sigue habiendo mucho ejemplo con una forma hoy en día considerada DESACTUALIZADA.
Antes montábamos módulos. Hoy en día trabajamos con componentes standalone.
---

Test-Doubles:
- Dummy
- Mock
- Spy
- Fake
- Stub