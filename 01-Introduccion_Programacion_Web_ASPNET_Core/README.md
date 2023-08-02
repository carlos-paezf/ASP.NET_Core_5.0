# Sección 01: Introducción a Programación web en ASP.NET Core

## Crear un proyecto en ASP.NET Core 6.0

Vamos a crear un nuevo proyecto usando Visual Studio 2022, para lo cual seguimos los siguientes pasos:

1. Seleccionamos que el proyecto sea de tipo `Aplicación web de ASP.NET Core (Modelo-Vista-Controlador)`.
2. El nombre que le pongamos a la aplicación será el mismo nombre de la solución, además de que debemos definir si queremos que la solución y el proyecto estén ubicados en el mismo folder.
3. Posteriormente, definimos el framework a usar (en esta oportunidad usaremos `.NET 6.0`), seguido de que habilitamos el check para configurar HTTPS. Con lo anterior, ya tendríamos un nuevo proyecto creado.
4. Para correr la aplicación tenemos 2 opciones:
   1. Realizar la ejecución mediante el IDE de Visual Studio, en cuyo caso se abre una nueva pestaña en el navegador predeterminado con la dirección `https://localhost:7285`, la cual en algunos navegadores es bloqueada por el error `NET::ERR_CERT_INVALID`, en cuyo caso es recomendable usar Chrome para abrir el proyecto
   2. Ejecutar el siguiente comando en una terminal ubicada en el folder del proyecto:

       ```txt
       $: dotnet run
       ```

       Cuando se compila y ejecuta el proyecto veremos una impresión en consola, en la cual se nos indica por cuales rutas podemos acceder a la vista web del proyecto:

       ```txt
       Compilando...

       info: Microsoft.Hosting.Lifetime[14]
             Now listening on: <https://localhost:7285>
       info: Microsoft.Hosting.Lifetime[14]
             Now listening on: <http://localhost:5298>
       info: Microsoft.Hosting.Lifetime[0]
             Application started. Press Ctrl+C to shut down.
       info: Microsoft.Hosting.Lifetime[0]
             Hosting environment: Development
       info: Microsoft.Hosting.Lifetime[0]
             Content root path: C:\Users\carlo\Documents\Cursos\Curso_ASP.NET_Core_5.0\01-Introduccion_Programacion_Web_ASPNET_Core\Seccion01\
       ```
