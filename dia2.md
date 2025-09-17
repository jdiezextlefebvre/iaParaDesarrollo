
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
---

> Nos tocaría ahora implementar la capa de servicio. Comencemos solo por el API. ME muestras lo que planteas hacer antes de hacerlo?

>En la DTO estas planteando datos que no tenemos en capa de dominio. Revisalo

> Respondiendo a tus preguntas:
1. Están ahí por cumplimiento con una arquitectura limpia... ahora mismo son iguales... en el futuro dejarán de serlo. Los mapeadores actuaran de muralla de contención ante cambios.
2. NO
3. Deberíamos crearlas. Aislan del repositorio, por si el día de mañana cambiamos el motor de BBDD. Pero por ahora NO LO HAREMOS...
4. NO queremos paginación.
A ver cómo lo planteas entonces...


> Algunas de las operaciones del servicio no están soportadas por el repo. Quitalas por ahora. Dejame ver

> SI. procede según hemos acordado. Procede también no solo con el api, sino con la implementación de ese api. Recuerda que tenemos un proyecto independiente para esto que ya está creado

---

> Nos toca ahora la capa de controlador rest.
  Ten en cuenta que esta capa no configura el servidor web. Eso se hará en otro proyecto después, donde exista la posibilidad de integrar no solo este microservicio, sino otros más. Le ves sentido?
  Para el tratamiento de excepcionesque provengan de capa de servicio, o que aparezcan aqui, usareemos un Middleware de Manejo de Excepciones.

  Me muestras lo que planteas hacer antes de hacerlo?
  

  > Eso lo tenemos definido por algun sitio?

> Quiero que me muestraes de nuevo todo lo que planteas hacer.... con detalle , pero en el chat

> Recuerda que esa configuracion de servidor irá en otro proyecto. No la crees aun. De acuerdo?

> Implementa todo esto de la capa del controlador. Prepara primero un plan de acción, para asegurar que no se escapa nada.

> Nos faltaría un proyecto nuevo, para poder ejecutar todo. De momento en ese proyecto solo pondremos este controlador rest... más adelante quizás añadamos nuevos... llama al proyecto Aplciacion. Crealo dentro de esta misma solucion. De momento solo crea el proyecto y configura dependencias. No añadas nada más.

> podemos probar que funciona y arranca?
> Que es ese endpoint??? Ese es el nuestro???
> Claro
---

> Vamos a documentar el controlador con swagger y a configurarlo en la app.
Me cuentas lo que planteas antes de hacerlo?

> Haz la implementación. Prepara primero un plan de trabajo para asegurar que nada se nos pasa


---

>No cree .gitignore... y se me ha metido en el repo local mucha basura. Lo arreglas? Me cuentas primero lo que vas a hacer?