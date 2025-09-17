// NOTA: Este archivo NO debe configurar el servidor web.
// La configuración del servidor (builder.Services, app.Run(), etc.) 
// debe hacerse en un proyecto Host separado.
//
// Este proyecto (PersonasCrud.RestV1.Impl) contiene únicamente:
// - Controladores REST puros
// - Middleware de manejo de excepciones  
// - Mappers entre DTOs
//
// Para usar estos componentes en un servidor web, el proyecto Host debe:
// 1. Referenciar este proyecto
// 2. Registrar las dependencias necesarias
// 3. Configurar el pipeline con el middleware
// 4. Ejecutar el servidor
