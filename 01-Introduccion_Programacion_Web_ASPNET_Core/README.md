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

Cuando usamos la terminal, podemos aprovechar el Hot Reload que viene integrado a partir de .NET Core 6.0, para lo cual ejecutamos el siguiente comando:

```txt
$: dotnet watch
```

## Creando vistas y poniendo opciones al menú

Al crear el proyecto de la forma que lo hicimos en la sección anterior, tendremos un frontend básico que viene integrado a modo de plantilla. Lo que vamos a hacer en estos momentos, es modificar el navbar, el cual se encuentra en `Views/Shared/_Layout.cshtml`.

Para crear una nueva vista iremos al archivo `Controllers/HomeController.cs`, en donde encontramos las vistas que implementan la interfaz `IActionResult` y que retornan una `View()`. Como las acciones que inicialmente encontramos, están en la clase `HomeController`, podremos ver sus vistas dentro del directorio `Views/Home`.

Crearemos una nueva vista de ejemplo, para lo cual dentro de la clase `HomeController` definimos una nueva acción:

```c#
using Microsoft.AspNetCore.Mvc;
using Seccion01.Models;
using System.Diagnostics;

namespace Seccion01.Controllers
{
    public class HomeController : Controller
    {
        ...
        public IActionResult ExampleView()
        {
            return View();
        }
        ...
    }
}
```

Si nos apoyamos en Visual Studio 2022, podemos seleccionar `ExampleView()`, pulsar click derecho, seleccionar *Agregar vista...*, elegir *Vista de Razor*, pulsar el botón de *Agregar*, y de ser necesario seleccionar algunas configuraciones extras en la pantalla final.

Si estamos en un editor de código como VSCode, podemos usar las extensiones relacionadas a .NET y generar el archivo mediante un comando. Pero en esta ocasión vamos a crear el archivo de manera manual dentro de `Views/Home` con el nombre `ExampleView.cshtml` y tendrá el siguiente contenido:

```cshtml
@{
    ViewData["Title"] = "ExampleView";
}

<h1>Example View</h1>
```

Ahora, vamos a enlazar la página en el menú, para lo cuál vamos a `Views/Shared/_Layout.cshtml` y añadimos lo siguiente en el nav:

```html
...
<nav class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
    <ul class="navbar-nav flex-grow-1">
        ...
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="ExampleView">Example View</a>
        </li>
    </ul>
</nav>
...
```

Como nos damos cuenta, en el atributo `asp-controller` ponemos el nombre del controlador excluyendo la palabra `Controller`, y en el atributo `asp-action` apuntamos a la acción que acabamos de crear.

Para visualizar los cambios, volvemos a lanzar el proyecto, ya sea usando Visual Studio 2022 o la terminal.

## Creando un nuevo controlador

En esta ocasión vamos a crear nuestro propio controlador, y de nuevo podemos apoyarnos en las opciones de Visual Code 2022 para añadir una plantilla de controlador, o podemos usar las extensiones de VSCode para realizar una acción similar. En esta ocasión creamos un controlador llamado `CountryController`. El contenido con el que vamos a partir será el siguiente:

```c#
using Microsoft.AspNetCore.Mvc;

namespace Seccion01.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
```

Creamos una nueva vista llamada `Views/Country/Index.cshtml` (Visual Studio 2022 simplifica la creación y ubicación de las vistas a partir de un controlador, pero en esta ocasión haré el proceso de forma manual).

```cshtml
@{
    ViewData["Title"] = "Country";
}

<h1>Countries</h1>
```

Ahora, añadimos la nueva vista al menú dentro de `_Layout.cshtml`:

```html
...
<nav class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
    <ul class="navbar-nav flex-grow-1">
        ...
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Country" asp-action="Index">Countries</a>
        </li>
    </ul>
</nav>
...
```

## Pasar información del Controller a la Vista

Vamos compartir información desde el controlador hacia la vista. Para esto, vamos a crear unos métodos dentro del controlador con el fin de retornar algunos tipos de datos diferentes:

```c#
using Microsoft.AspNetCore.Mvc;

namespace Seccion01.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string Country()
        {
            return "Colombia";
        }

        public char InitialLetter()
        {
            return 'C';
        }

        public double CountrySurfaceKM()
        {
            return 1.142;
        }

        public bool HasFourSeasons()
        {
            return false;
        }
    }
}
```

Ahora, cuando vamos al navegador y escribimos el nombre del método después de `https://localhost:7285/Country/`, vamos a observar la data que retornamos en cada uno.

## Recibir parámetros desde la URL

Para recibir el valor de un parámetro dentro del controlador, solo debemos establecer un argumento dentro de la función que controla el endpoint. Por ejemplo, si queremos que desde el controlador de países se salude a una persona con el nombre que ingreso por parámetro, debemos escribir el siguiente código:

```c#
...
namespace Seccion01.Controllers
{
    public class CountryController : Controller
    {
        ...
        public string SayWelcome(string name)
        {
            return "Hi " + name + ", welcome to Colombia";
        }
    }
}
```

En la url tendríamos la siguiente estructura de acuerdo al objetivo planteado: `https://localhost:7285/Country/SayWelcome/?name=Carlos`, y como respuesta obtendremos el mensaje `Hi Carlos, welcome to Colombia`. Podemos añadir más argumentos a la función para recibir más parámetros, por ejemplo, para recibir como parámetro adicional el país y usar la siguiente url `https://localhost:7285/Country/SayWelcome/?name=Carlos&country=Colombia`, actualizaríamos el controlador de la siguiente manera y adicionalmente podemos hacer que tenga un valor por defecto:

```c#
...
namespace Seccion01.Controllers
{
    public class CountryController : Controller
    {
        ...
        public string SayWelcome(string name, string country = "Colombia")
        {
            return "Hi " + name + ", welcome to " + country;
        }
    }
}
```

## Pasar como parámetro una clase

¿Que pasaría si tuviéramos que recibir múltiples parámetros desde la URL? Nuestro código se vería muy sucio y tosco si lo trabajamos como lo venimos haciendo. Para evitar esto podemos crear una clase cuyos atributos sean los parámetros que esperamos.

Vamos a crear una nueva carpeta dentro del paquete del proyecto que estamos desarrollando, es decir, nuestro nuevo folder debe estar al nivel de los controladores, modelos y otros. El nuevo directorio se llamará `Classes` y tendrá un archivo llamado `AmbassadorClass.cs` (de nuevo, podemos apoyarnos en las herramientas de Visual Studio 2022 o en las extensiones que tenemos en VSCode, con el fin de agilizar el proceso de creación de clases).

La nueva clase tendrá los siguientes atributos, los cuales podemos inicializar para que tengan un valor en caso de que el usuario no lo provea:

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seccion01.Classes
{
    public class AmbassadorClass
    {
        public string name { get; set; }
        public string country { get; set; } = "Colombia"; 
    }
}
```

Para hacer uso de nuestra clase dentro del controlador de países, debemos realizar la importación y llamar un parámetros del tipo embajador dentro de la acción, para luego llamar sus atributos:

```c#
...
using Seccion01.Classes;

namespace Seccion01.Controllers
{
    public class CountryController : Controller
    {
        ...
        public string SayWelcome(AmbassadorClass ambassador)
        {
            return "Hi " + ambassador.name + ", welcome to " + ambassador.country;
        }
    }
}
```
