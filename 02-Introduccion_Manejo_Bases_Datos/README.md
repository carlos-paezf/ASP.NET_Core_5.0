# Sección 02: Introducción al Manejo de Bases de Datos

## Instalación SQL Server Express 2022

Vamos a descargar SQL Server Express 2022 desde el siguiente enlace: [SQL Server 2022 - Microsoft](https://www.microsoft.com/en-us/sql-server/sql-server-downloads). Los pasos a seguir con el instalador son muy simples (a continuación un breve resumen de lo que se enseña en [Instalar SQL Server 2022 gratis desde cero (y bien) [TUTORIAL]](https://www.youtube.com/watch?v=_fFz-_O2yvI)):

1. Seleccionar el tipo de instalación *Custom*
2. En algunos casos es importante establecer que el idioma de Windows sea *Español (España)* o inglés
3. Definir la ubicación de la instalación
4. Permitir cambios que llegue a realizar la aplicación.
5. Esperar descarga e instalación de los paquetes
6. En el *Centro de instalación de SQL Server*, seleccionamos *Nueva instalación independiente de SQL Server o agregar características a una instalación existente*.
7. Aceptar términos y condiciones de la licencia.
8. En la sección de *Extensión de Azure para SQL Server* desmarcamos la extensión.
9. En la *Selección de Características*, desmarcamos las sub-opciones de *Servicios de Motor de Base de Datos* y solo dejamos dicha opción seleccionada.
10. En la *Configuración de la Instancia* podemos personalizar el nombre de la misma, o dejar un nombre por defecto. En este caso usaré la opción por defecto (`MSSQLSERVER`)
11. En la sección de *Configuración de Motor de base de datos*, en la pestaña de *Configuración del Servidor*, seleccionamos el modo de autenticación mixto para personalizar los usuarios y sus permisos.
12. Se establece la contraseña del administrador del sistema
13. En la misma sección, en la pestaña de *Directorios de datos*, cambiamos las rutas por defecto, por rutas personalizadas que eviten espacios o problemas con las mismas (Ejm: `C:\SQLServerData`).
14. En la misma sección, en la pestaña de *Memoria* podemos seleccionar la cantidad de memoria recomendada.
15. Terminada la instalación independiente, vamos al *Centro de instalación de SQL Server* y seleccionamos *Instalar las herramientas de administración de SQL Server* o por sus siglas en inglés SSMS.
16. En la instalación del SSMS definimos una ruta para la aplicación que sea fácil de administrar, por ejemplo `C:\Program Files (x86)\SSMS19`.
17. Terminada la instalación del SSMS, procedemos a ejecutarla.

Por defecto, el server que nos muestra es el que definimos en el paso 10 de la anterior lista. Para la autenticación podemos usar el usuario windows, o usar el usuario `sa` con la contraseña definida en el paso 12.

## Microsoft SQL Server en Docker

Una opción viable para hacer uso de bases de datos con SQL Server, es mediante un contenedor de Docker con una imagen de SQL Server. Para ello primero debemos descargar la imagen de Docker Hub [Microsoft SQL Server - Ubuntu based images](https://hub.docker.com/_/microsoft-mssql-server):

```txt
$: docker pull mcr.microsoft.com/mssql/server
```

Podemos verificar la versión de la imagen instalada usando el comando:

```txt
$: docker images
```

Creamos el contenedor con la imagen descargada de la siguiente manera ([Inicio rápido: Ejecución de imágenes de contenedor de SQL Server para Linux con Docker](https://learn.microsoft.com/es-mx/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash)):

```txt
$: docker run -d -p 1433:1433 \
    --name SQLServer \
    --hostname SQLServer \
    -e SA_PASSWORD=P@ssw0rd \
    -e ACCEPT_EULA=Y \
    mcr.microsoft.com/mssql/server
```

Podemos comprobar que el contenedor está corriendo usando el comando:

```txt
$: docker container ls -a
```

Si queremos interactuar con la terminal del contenedor podemos usar el comando:

```txt
$: docker exec -it SQLServer bash
```

Desde la consola interactiva podemos conectarnos al servidor usando el siguiente comando:

```txt
$: /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P P@ssw0rd
```

Mediante Microsoft SQL Server Management Studio, podemos hacer la conexión al container usando como server name `localhost, 1433` y la autenticación de SQL Server Authentication con el usuario `SA` y la contraseña definida al crear el contenedor. En caso de que aparezca el error `MSSQLSERVER_18456`, se puede intentar ingresar con la autenticación de windows.

## Restaurar una base de datos

Para esta sección vamos a usar una base de datos existente, por lo cual tendremos que recuperar el archivo [BDHospital.bak](./BDHospital.bak) dentro del servidor de SQL Server. Dentro del MSSMS seleccionamos el servidor, en la carpeta de `Databases` pulsamos click izquierdo y seleccionamos la opción ***Restore Database***. En la nueva ventana seleccionamos como fuente `Device` y buscamos el archivo arriba mencionado.

Cuando la base de datos ha sido restaura, podremos manipularla o navegar a través de ella. En la siguiente lección nos conectaremos desde .NET a la base de datos restaurada.

## Entity Framework para conectar a una base de datos

En esta lección usaremos la consola para mapear las entidades de la base de datos dentro de nuestro proyecto. Lo primero será la creación de nuevo proyecto para esta sección, y en esta ocasión lo crearemos con el comando:

```txt
$: dotnet new mvc --language "C#" --name "Section02"
```

Lo siguiente es la instalación del proveedor de Entity Framework con el siguiente comando:

```txt
$: dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Posteriormente obtenemos las herramientas del Entity Framework mediante el siguiente comando de la CLI de .NET:

```txt
$: dotnet tool install --global dotnet-ef

$: dotnet add package Microsoft.EntityFrameworkCore.Tools

$: dotnet add package Microsoft.EntityFrameworkCore.Design
```

Para el mapeo o Scaffold necesitamos la cadena de conexión a la base de datos, y la podemos guardar en 2 lugares: En `appsettings.json` (no es muy recomendado, pero la usaremos en esta oportunidad), o en los secretos de usuario ([Scaffolding (utilización de técnicas de ingeniería inversa)](https://learn.microsoft.com/es-es/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli)).

Dentro del archivo `appsettings.json` vamos a añadir una nueva pareja de clave valor con la cadena de conexión:

```json
{
    ...,
    "ConnectionStrings": {
        "BDHospital": "Data Source=localhost, 1433;Initial Catalog=BDHospital;Integrated Security=True;TrustServerCertificate=True"
    }
}
```

En el `Data Source` definimos el servidor, en `Initial Catalog` definimos la base de datos a la cual nos conectamos, en `Integrated Security` definimos que usaremos la autenticación combinada de Windows y SQL Server, y en `TrustServerCertificate` generamos un certificado de conexión para que el contenedor de docker permita la conexión a nuestro server.

Posteriormente realizamos el mapeo con el siguiente comando:

```txt
$: dotnet ef dbcontext scaffold "Name=ConnectionStrings:BDHospital" Microsoft.EntityFrameworkCore.SqlServer -o Models
```

> Nota: Durante la realización del curso se presentaron inconvenientes en cuanto a la cadena de conexión, por lo tanto cambié el comando al siguiente:
>
> ```txt
> $: dotnet ef dbcontext scaffold "Data Source=localhost, 1433;Initial Catalog=BDHospital;Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models
> ```
>
> Lo anterior hace que el archivo de contexto (`BDHospitalContext`) tenga la siguiente configuración:
>
> ```c#
> ...
> public partial class BDHospitalContext : DbContext
> {
>     ...
>     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
> #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
>         => optionsBuilder.UseSqlServer("Data Source=localhost, 1433;Initial Catalog=BDHospital;Integrated Security=True;TrustServerCertificate=True");
> }
> ```

## Creando listado de especialidades de la DB

Crearemos un listado con la tabla `Especialidad`, por lo que primero crearemos un controlador dedicado a tal modelo llamado `Controllers/EspecialidadController.cs`. También debemos crear la vista `Views/Especialidad/Index.cshtml` y una clase `Classes/EspecialidadClass.cs` para estandarizar la data y tendrá la siguiente información:

```c#
...
namespace Section02.Classes
{
    public class EspecialidadClass
    {
        public int iidespecialidad { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }
}
```

En el controlador configuraremos la acción para usar el contexto de la base de datos y realizar una consulta que nos traiga todas las especialidades activas:

```c#
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class EspecialidadController : Controller
    {
        public IActionResult Index()
        {
            List<EspecialidadClass> especialidadList = new List<EspecialidadClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                especialidadList = (
                    from especialidad in db.Especialidades
                    where especialidad.Bhabilitado == 1
                    select new EspecialidadClass
                    {
                        iidespecialidad = especialidad.Iidespecialidad,
                        nombre = especialidad.Nombre,
                        descripcion = especialidad.Descripcion
                    }
                ).ToList();
            }

            return View(especialidadList);
        }
    }
}
```

## Renderizar listado de especialidades

En la vista que creamos para renderizar la información de las especialidades, vamos a estructurar una tabla de la siguiente manera:

```cshtml
@using Section02.Classes; 
@model IEnumerable<EspecialidadClass>;

@{
    ViewData["Title"] = "Especialidad";
}

<h1>Especialidades</h1>

<table class="table">
    <thead class="thead-dark">
        <tr>
            <th>Id Especialidad</th>
            <th>Nombre</th>
            <th>Descripción</th>
        </tr>
    </thead>

    <tbody>
        @foreach (EspecialidadClass e in Model) 
        {
            <tr>
                <td>@e.iidespecialidad</td>
                <td>@e.nombre</td>
                <td>@e.descripcion</td>
            </tr>
        }
    </tbody>
</table>
```

Además, vamos a configurar que por defecto la aplicación ingrese al controlador de especialidades, y esto lo hacemos desde `Program.cs`:

```c#
...
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Especialidad}/{action=Index}/{id?}");
...
```

## Uso de JOIN en ASP.NET

Crearemos un nuevo controller llamado `PersonaController.cs`, dentro del cual creamos un arreglo en la acción `Index` para traer a las personas y consultar el valor de su sexo, el cual se encuentra definido en otra taba de la DB.

Antes, vamos a crear una clase para el tipado de las personas, y esta clase llamada `Classes/PersonaClass.cs` tendrá la siguiente información:

```c#
using System;

namespace Section02.Classes
{
    public class PersonaClass
    {
        public int iidPersona { get; set; }
        public string nombreCompleto { get; set; }
        public string email { get; set; }
        public string nombreSexo { get; set; }
    }
}
```

Dentro del controlador creamos la lista del tipo `PersonaClass`:

```c#
using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;

namespace Section02.Controllers
{
    public class PersonaController : Controller
    {
        public IActionResult Index()
        {
            List<PersonaClass> personasList = new List<PersonaClass>();

            return View();
        }
    }
}
```

Lo interesante en esta lección es la aplicación de un JOIN con el fin de consultar información en otra tabla a partir de un resultado originado por x tabla. Usando .NET tendremos la siguiente sentencia:

```c#
using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class PersonaController : Controller
    {
        public IActionResult Index()
        {
            List<PersonaClass> personasList = new List<PersonaClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                personasList = (
                    from persona in db.Personas
                    join sexo in db.Sexos
                    on persona.Iidsexo equals sexo.Iidsexo
                    where persona.Bhabilitado == 1
                    select new PersonaClass
                    {
                        iidPersona = persona.Iidpersona,
                        nombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
                        email = persona.Email,
                        nombreSexo = sexo.Nombre
                    }
                ).ToList();
            }

            return View(personasList);
        }
    }
}
```

Lo siguiente es crear la vista para el controlador, pero en la siguiente sección vamos a renderizar los datos, por el momento solo creamos la vista y la enlazamos en el navbar del archivo `Views/Shared/_Layout.cshtml`:

```cshtml
...
<ul class="navbar-nav flex-grow-1">
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Persona" asp-action="Index">Personas</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Especialidad" asp-action="Index">Especialidades</a>
    </li>
</ul>
...
```

## Renderizar listado de JOIN

Dentro de la vista que creamos para el controlador de personas vamos a realizar la siguiente configuración para listar los resultados:

```cshtml
@using Section02.Classes;
@model IEnumerable<PersonaClass>

@{
    ViewData["Title"] = "Personas";
}

<h1>Listado de Personas</h1>


<table class="table">
    <thead class="thead-dark">
        <tr>
            <th>Id Persona</th>
            <th>Nombre Completo</th>
            <th>Email</th>
            <th>Sexo</th>
        </tr>
    </thead>

    <tbody>
        @foreach (PersonaClass p in Model)
        {
            <tr>
                <td>@p.iidPersona</td>
                <td>@p.nombreCompleto</td>
                <td>@p.email</td>
                <td>@p.nombreSexo</td>
            </tr>
        }
    </tbody>
</table>
```

## Tag Display en Entity Framework

En estos momentos estamos creando las tablas y poniendo manualmente el nombre de cada columna, Entity Framework nos permite realizar esta labor de una manera más sencilla usando un tag sobre las propiedades de la clase que usamos para tipar los listados, por ejemplo con la clase de personas:

```c#
using System;
using System.ComponentModel.DataAnnotations;

namespace Section02.Classes
{
    public class PersonaClass
    {
        [Display(Name = "Id Persona")]
        public int iidPersona { get; set; }

        [Display(Name = "Nombre Completo")]
        public string nombreCompleto { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Sexo")]
        public string nombreSexo { get; set; }
    }
}
```

Luego, en la vista debemos realizar el siguiente llamado:

```cshtml
@using Section02.Classes;
...
<table class="table">
    <thead class="thead-dark">
        <tr>
            <th>@Html.DisplayNameFor(model => model.iidPersona)</th>
            <th>@Html.DisplayNameFor(model => model.nombreCompleto)</th>
            <th>@Html.DisplayNameFor(model => model.email)</th>
            <th>@Html.DisplayNameFor(model => model.nombreSexo)</th>
        </tr>
    </thead>
    ...
</table>
```

## Uso del Scaffolding List

Vamos a crear una nueva clase llamada `SedeClass.cs` y en ella tendremos la siguiente estructura:

```c#
using System;
using System.ComponentModel.DataAnnotations;

namespace Section02.Classes
{
    public class SedeClass
    {
        [Display(Name = "Id Sede")]
        public int iidSede { get; set; }

        [Display(Name = "Nombre de la Sede")]
        public string nombreSede { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }
    }
}
```

En seguida, creamos el controlador para las sedes y creamos un listado como los anteriores y lo retornamos a la vista:

```c#
using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class SedeController : Controller
    {
        public IActionResult Index()
        {
            List<SedeClass> sedesList = new List<SedeClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                sedesList = (
                    from sede in db.Sedes
                    where sede.Bhabilitado == 1
                    select new SedeClass
                    {
                        iidSede = sede.Iidsede,
                        nombreSede = sede.Nombre,
                        direccion = sede.Direccion
                    }
                ).ToList();
            }

            return View(sedesList);
        }
    }
```

La vista `Views/Sede/Index.cshtml` la vamos a generar un Visual Studio Code 2022, con el fin de que no sea un template vació, sino que sea un template `list`. Lo anterior nos genera el código que hemos venido trabajando en las últimas vistas, pero con algunas pequeñas modificaciones, se ve algo cómo lo siguiente (He borrado luego las acciones CRUD que se generan dentro del template, y cambie algunos nombres):

```cshtml
@model IEnumerable<Section02.Classes.SedeClass>

@{
    ViewData["Title"] = "Index";
}

<h1>Index</h1>

<p>
    <a asp-action="Create">Create New</a>
</p>

<table class="table">
    <thead>
        <tr>
            <th>@Html.DisplayNameFor(model => model.iidSede)</th>
            <th>@Html.DisplayNameFor(model => model.nombreSede)</th>
            <th>@Html.DisplayNameFor(model => model.direccion)</th>
            <th></th>
        </tr>
    </thead>

    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>@Html.DisplayFor(model => item.iidSede)</td>
                <td>@Html.DisplayFor(model => item.nombreSede)</td>
                <td>@Html.DisplayFor(model => item.direccion)</td>
                <td>
                    @Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) |
                    @Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ }) |
                    @Html.ActionLink("Delete", "Delete", new { /* id=item.PrimaryKey */ })
                </td>
            </tr>
        }
    </tbody>
</table>
```

## Tarea: Listado de `pagina` y `medicamento`

### Preguntas de esta tarea

1. Listar la tabla `pagina` con las propiedades `iidpagina`, `mensaje`, `accion` y `controller`
2. Listar la tabla `medicamento` con las propiedades `iidmedicamento`, `nombre`, `precio`, `stock`, `nombreFormaFarmaceutica`.
3. En ambos casos `bhabilitado` debe ser `1`

### Solución

#### Clases

`PaginaClass`:

```c#
using System;
using System.ComponentModel.DataAnnotations;

namespace Section02.Classes
{
    public class PaginaClass
    {
        [Display(Name = "Id Página")]
        public int iidPagina { get; set; }

        [Display(Name = "Mensaje")]
        public string mensaje { get; set; }

        [Display(Name = "Acción")]
        public string accion { get; set; }

        [Display(Name = "Controlador")]
        public string controlador { get; set; }
    }
}
```

`MedicamentoClass`:

```c#
using System;
using System.ComponentModel.DataAnnotations;

namespace Section02.Classes
{
    public class MedicamentoClass
    {
        [Display(Name = "Id Medicamento")]
        public int iidMedicamento { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Precio")]
        public decimal? precio { get; set; }

        [Display(Name = "Stock")]
        public int? stock { get; set; }

        [Display(Name = "Nombre Forma Farmacéutica")]
        public string nombreFormaFarmaceutica { get; set; }
    }
}
```

#### Controllers

`PaginaController`:

```c#
using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class PaginaController : Controller
    {
        public IActionResult Index()
        {
            List<PaginaClass> paginasList = new List<PaginaClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                paginasList = (
                    from pagina in db.Paginas
                    where pagina.Bhabilitado == 1
                    select new PaginaClass
                    {
                        iidPagina = pagina.Iidpagina,
                        mensaje = pagina.Mensaje,
                        accion = pagina.Accion,
                        controlador = pagina.Controlador
                    }
                ).ToList();
            }

            return View(paginasList);
        }
    }
}
```

`MedicamentoController`:

```c#
using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class MedicamentoController : Controller
    {
        public IActionResult Index()
        {
            List<MedicamentoClass> medicamentosList = new List<MedicamentoClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                medicamentosList = (
                    from medicamento in db.Medicamentos
                    join formaFarmaceutica in db.FormaFarmaceuticas
                    on medicamento.Iidformafarmaceutica equals formaFarmaceutica.Iidformafarmaceutica
                    where medicamento.Bhabilitado == 1
                    select new MedicamentoClass
                    {
                        iidMedicamento = medicamento.Iidmedicamento,
                        nombre = medicamento.Nombre,
                        precio = medicamento.Precio,
                        stock = medicamento.Stock,
                        nombreFormaFarmaceutica = formaFarmaceutica.Nombre
                    }
                ).ToList();
            }

            return View(medicamentosList);
        }
    }
}
```

#### Vistas

`Pagina/Index.cshtml`:

```cshtml
@using Section02.Classes;
@model IEnumerable<PaginaClass>

@{
    ViewData["Title"] = "Pagina";
}

<h1>Lisado de Páginas</h1>

<table class="table">
    <thead class="thead-dark">
        <tr>
            <th>@Html.DisplayNameFor(model => model.iidPagina)</th>
            <th>@Html.DisplayNameFor(model => model.mensaje)</th>
            <th>@Html.DisplayNameFor(model => model.accion)</th>
            <th>@Html.DisplayNameFor(model => model.controlador)</th>
            <th></th>
        </tr>
    </thead>

    <tbody>
        @foreach (PaginaClass item in Model)
        {
            <tr>
                <td>@Html.DisplayFor(model => item.iidPagina)</td>
                <td>@Html.DisplayFor(model => item.mensaje)</td>
                <td>@Html.DisplayFor(model => item.accion)</td>
                <td>@Html.DisplayFor(model => item.controlador)</td>
            </tr>
        }
    </tbody>
</table>
```

`Medicamento/Index.cshtml`:

```cshtml
@using Section02.Classes;
@model IEnumerable<MedicamentoClass>

@{
    ViewData["Title"] = "Medicamentos";
}

<h1>Lisado de Medicamentos</h1>

<table class="table">
    <thead class="thead-dark">
        <tr>
            <th>@Html.DisplayNameFor(model => model.iidMedicamento)</th>
            <th>@Html.DisplayNameFor(model => model.nombre)</th>
            <th>@Html.DisplayNameFor(model => model.precio)</th>
            <th>@Html.DisplayNameFor(model => model.stock)</th>
            <th>@Html.DisplayNameFor(model => model.nombreFormaFarmaceutica)</th>
            <th></th>
        </tr>
    </thead>

    <tbody>
        @foreach (MedicamentoClass item in Model)
        {
            <tr>
                <td>@Html.DisplayFor(model => item.iidMedicamento)</td>
                <td>@Html.DisplayFor(model => item.nombre)</td>
                <td>@Html.DisplayFor(model => item.precio)</td>
                <td>@Html.DisplayFor(model => item.stock)</td>
                <td>@Html.DisplayFor(model => item.nombreFormaFarmaceutica)</td>
            </tr>
        }
    </tbody>
</table>
```

## ViewBag

Otra manera de pasar la información desde el controlador a la vista es usando `ViewBag`, dentro de la cuál enviamos las variables que necesitamos, por ejemplo: Queremos pasar el titulo de la vista medicamentos, en el controlador podemos hacer lo siguiente:

```c#
...
namespace Section02.Controllers
{
    public class MedicamentoController : Controller
    {
        public IActionResult Index()
        {
            ...
            ViewBag.Title = "Lista de medicamentos";
            ...
        }
    }
}
```

Luego en la vista debemos crear una variable que reciba este valor y lo muestre:

```cshtml
@{
    string title = ViewBag.Title;
    ViewData["Title"] = title;
}

<h1>@title</h1>
```
