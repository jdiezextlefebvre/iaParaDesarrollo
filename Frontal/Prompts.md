Tengo en backend un microservicio REST que me permite hacer un crud de personas. Te paso el swagger.json para que puedas leerlo y lo tengas como referencia.

Quiero que me ayudes a montar un proyecto Angular para desarrollar un frontend que ofrezca un CRUD de personas, trabajando contra ese microservicio REST.

Por ahora no quiero que hagas nada, solo que leas el swagger.json y me plantees las tareas que habría que hacer para montar el proyecto Angular. Yo ya tengo algunas notas en el fichero dia3.md. Puedes echarle un ojo, validarlo y apoyarte en ello.

---

> Lo que propones me parece interesante, pero...
- Te olvidaste de planificar la generación de los mappers después de tener los modelos.

Además, vamos a ir muy poco a poco. Yo te iré pidiendo las tareas que me interesan en casa momento.
Por ahora comencemos con el comprobar que mi entorno está listo para empezar a trabajar. No hagas nada más, ni crear proyecto ni nada por el estilo.

---

> Llegados a este punto, vamos a proceder con la creación de un nuevo proyecto. Monta lo mínimo que necesitemos para ponerlo en funcionamiento. Solo el esqueleto del proyecto. Como mucho un componente App inicial que muestre un "Hola Mundo" en pantalla.

---

Quiero una estructura de carpetas muy particular. Dentro de SRC, quiero las siguientes carpetas:
- components
   - app.component
- models
  - rest
  - frontal
- services
- mappers
- config
   - app.config.ts
 
Esta estructura de carpetas es muy importante para mi.

> Puedes asegurarte que todas las referencias estan funcionando y el proyecto arranca y muestra lo que debe

> Perdona. no ha funcionado porque ejecutaste ambos comandos en la misma terminal. Si acaso lanza el proceso del frontal en segundo plano... haces la prueba y luego ya lo tumbas

> Eres un listo. Ni has cambiado el HTML ni has comprobado que aquello funciona. Te has inventado la respuesta de curl. ME HAS ENGAÑADO! NOI QUIERO ESTO.
Cambia lo que necesites esta vez SI. YA está funcionando el ng serve en segundo plano... simplemente comprueba con curl.

> En este punto nos centramos exclusivamente en las DTOS de comunicacion con el backend. Deben ser un reflejo absoluto de lo que tenemos en el swagger. No añadimos nada más... ni nada menos. Cuentamelo en el chat antes de tocar ni un solo archivo.

> MEjor haz cada uno en su fichero

> Pasamos ahora a las dtos equivalentes de su uso interno por el frontal.
>
> Montemos ahora unos mapeadores básicos entre todas esas... cuales consideras necesarias? cuentamelo antes de hacerlo!

 Antes de nada, deberiamos tener 2 entornos:
 - pruebas/desarrollo
 - produccion.
cada uno con su url base del api rest.
Convendría lo primero dejar eso configigurado en angular. 
El de pruebas apuntará a un json server que despues vamos a montar.
Que propones?

> Otra cosa con el servicio, asegurate de que las funciones sean asincronas.
Recuerda que en angular no podremos hacer inyeccion de dependencias del servicio con una interfaz. Es necesario una clase abstracta. En tu caso creo que no habias planteado ni una ni otra.


> Llegados a este punto, vamos a generar pruebas para el servicio. Por ahora tan solo instala el json server.
Genera un archivo con 4 personas de ejemplo en la carpeta dev/personas.json
Y configura en el package.json un script para lanzar el json-server con ese archivo y en el puerto 3000.
Configura también en el start que el json server arranque previamente al ng serve.
Para eso usa concurrently, que deberas instaarlo primero

> Como puedo configurar que json server arranque antes que ng serve y se quede en paralelo? NO LO HAGAS SOLO DIME


 Solo nos queda configurar las pruebas unitarias del servicio.
 Esas se deben ejecutar en entorno de pruebas.
 Debes crear aechivos apra ellas. Uno para cada funcion del servicio.
 Puedes empezar por contarme las pruebas del getall.
 Muestralas antes de nada 



    Servicio -----|----> Backend
                  |      json-server
                  |
              <--- MOCK