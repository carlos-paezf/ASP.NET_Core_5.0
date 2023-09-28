# Sección 03: Uso de filtros

## Primeros pasos

Para esta sección vamos a crear un nuevo proyecto, pero nos conectaremos a la misma base de datos de la sección anterior. Con esto en mente, usaremos el siguiente comando de dotnet CLI para la creación del proyecto:

```txt
$: dotnet new mvc --language "C#" --name "Section03"
```

Luego añadimos los paquetes de Entity Framework para realizar el trabajo con entidades y la base de datos:

```txt
$: dotnet add package Microsoft.EntityFrameworkCore.SqlServer

$: dotnet add package Microsoft.EntityFrameworkCore.Tools

$: dotnet add package Microsoft.EntityFrameworkCore.Design
```

Hacemos el mapeo con la base de datos usando el siguiente comando:

```txt
$: dotnet ef dbcontext scaffold "Data Source=LAPTOP-FERRER\SQLEXPRESS;Initial Catalog=BDHospital;Persist Security Info=True;User ID=SA;Password=P@ssw0rd;Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models
```

## Filtrado por nombre de especialidad

En esta lección vamos a usar la tabla de especialidades, por lo que podríamos copiar y pegar lo relacionado a la entidad, controlador y vista de la sección anterior, exceptuando que tendremos un nuevo cambio de manera inicial en la vista:

```cshtml
...
<form method="POST" aps-controller="Especialidad" asp-action="Index" class="my-3"
    style="display: grid; grid-template-columns: 1fr 2fr 1fr; gap: 1rem;">
    <label for="name">Ingrese nombre de la especialid</label>
    <input type="text" name="Nombre" id="name" class="form form-control">
    <button type="submit" class="btn btn-info">Enviar</button>
</form>
...
```

En el controlador vamos a modificar los parámetros que puede recibir la acción `Index` de la siguiente manera:

```c#
...
namespace Section03.Controllers
{
    public class EspecialidadController : Controller
    {
        public IActionResult Index(EspecialidadClass objEspecialidad)
        {
            ...
        }
    }
}
```

Como queremos filtrar por nombre, vamos a definir que si el valor del campo es nulo o vacío muestre la lista que tenemos actualmente, pero en caso contrario, mostramos el resultado de buscar el nombre en la base de datos:

```c#
...
namespace Section03.Controllers
{
    public class EspecialidadController : Controller
    {
        public IActionResult Index(EspecialidadClass objEspecialidad)
        {
            ...
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if (objEspecialidad.Nombre == null || objEspecialidad.Nombre == "")
                {
                    ...
                }
                else
                {
                    especialidadList = (
                        ...
                        where especialidad.Bhabilitado == 1 && especialidad.Nombre.Contains(objEspecialidad.Nombre)
                        ...
                    ).ToList();
                }
            }
            ...
        }
    }
}
```

## Botón de limpiar formulario

Vamos a añadir una funcionalidad que nos permita limpiar el término de búsqueda y de tal manera tener de nuevo todo el listado de los datos. Dentro del controlador `EspecialidadController.cs` hacemos uso de un `ViewBag`, con el objetivo de tener un tipo de historial de lo que escribió el usuario y poderlo mostrar en el input:

```c#
...
namespace Section03.Controllers
{
    public class EspecialidadController : Controller
    {
        public IActionResult Index(EspecialidadClass objEspecialidad)
        {
            ...
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if (objEspecialidad.Nombre == null || objEspecialidad.Nombre == "")
                {
                    ...
                    ViewBag.NombreEspecialidad = "";
                }
                else
                {
                    ...
                    ViewBag.NombreEspecialidad = objEspecialidad.Nombre;
                }
            }
            ...
        }
    }
}
```

En la vista vamos a usar el siguiente código:

```cshtml
...
@{
    ...
    string nombreEspecialidad = ViewBag.NombreEspecialidad;
}
...
<form ...>
    ...
    <input ... value="@nombreEspecialidad">
    ...
</form>
...
```

Para el botón de limpiar vamos a añadir el siguiente fragmento en la vista:

```cshtml
...
<form style="display: grid; grid-template-columns: 1fr 2fr 1fr 1fr; gap: 1rem;" ...>
    ...
    <button type="button" class="btn btn-info" onclick="clean()">Limpiar</button>
</form>
...
```

La función que se menciona en el botón la definimos como script dentro de la propia vista, recordando la importancia de tener marcado con ids los elementos con los que vamos a interactuar:

```cshtml
...
<form id="form" ...>
    ...
    <input ... id="name">
    ...
</form>
...
<script>
    function clean () {
        document.getElementById("name").value = "";
        document.getElementById("form").submit()
    }
</script>
```

## Filtro sensitivo por TextBox

En esta lección vamos a modificar el formulario con el objetivo de que no tengamos que usar botones, sino que en cada ocasión que modificamos la entrada se realice la búsqueda a la base de datos.

Lo primero que haremos será modificar la vista de la siguiente manera:

```cshtml
...
<form id="form" method="POST" aps-controller="Especialidad" asp-action="Index" class="my-3"
    style="display: grid; grid-template-columns: 1fr 2fr 1fr; gap: 1rem;">
    <label for="name">Ingrese nombre de la especialid</label>
    <input type="text" name="Nombre" id="name" value="@nombreEspecialidad" class="form form-control" onkeyup="search()">
    <button type="button" class="btn btn-info" onclick="clean()">Limpiar</button>
</form>
...
```

Luego, en el script creamos la función para el envío del formulario y de tal manera hacer uso de la acción definida en el controlador:

```cshtml
...
<script>
    ...
    function search() {
        document.getElementById("form").submit()
    }
</script>
```

## Datos de ComboBox desde la DB

Para esta ocasión vamos a usar la tabla `Personas`, por lo que vamos a crear la clase que nos ayuda a definir un objeto de dicha tabla:

```c#
using System;
using System.ComponentModel.DataAnnotations;

namespace Section03.Classes
{
    public class PersonaClass
    {
        [Display(Name = "Id Persona")]
        public int IidPersona { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Sexo")]
        public string NombreSexo { get; set; }

        [Display(Name = "Id Sexo")]
        public int Iidsexo { get; set; }
    }
}
```

También creamos el controlador para personas, con un método de listado con consulta JOIN que pronto alteraremos:

```c#
using Microsoft.AspNetCore.Mvc;
using Section03.Classes;
using Section03.Models;

namespace Section03.Controllers
{
    public class PersonaController : Controller
    {
        public IActionResult Index()
        {
            List<PersonaClass> personaList = new List<PersonaClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                personaList = (
                    from persona in db.Personas
                    join sexo in db.Sexos
                    on persona.Iidsexo equals sexo.Iidsexo
                    where persona.Bhabilitado == 1
                    select new PersonaClass
                    {
                        IidPersona = persona.Iidpersona,
                        NombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
                        Email = persona.Email,
                        NombreSexo = sexo.Nombre
                    }
                ).ToList();
            }

            return View(personaList);
        }

    }
}
```

En el controlador vamos a crear un método para llenar el combo box de sexo, o lo que en este caso será una instancia de la clase `SelectListItem`, con los valores que se registran en la base de datos. Posteriormente lo llamamos en la acción `Index`:

```c#
...
using Microsoft.AspNetCore.Mvc.Rendering;
...
namespace Section03.Controllers
{
    public class PersonaController : Controller
    {
        public void FillSexComboBox()
        {
            List<SelectListItem> sexList = new List<SelectListItem>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                sexList = (
                    from sexo in db.Sexos
                    where sexo.Bhabilitado == 1
                    select new SelectListItem
                    {
                        Text = sexo.Nombre,
                        Value = sexo.Iidsexo.ToString()
                    }
                ).ToList();
            }

            ViewBag.SexList = sexList;
        }
        
        public IActionResult Index()
        {
            ...
            FillSexComboBox();
            ...
        }

    }
}
```

En la vista recibimos el valor de `ViewBag.SexList` y lo asignamos a una variable, además de crear una tabla para listar los resultados de las personas junto a un formulario:

```cshtml
@using Section03.Classes;
@model IEnumerable<PersonaClass>

@{
    ViewData["Title"] = "Listado de Personas";
    List<SelectListItem> sexList = ViewBag.SexList;
}


<h1>Personas</h1>


<form id="form" class="my-3"
    style="display: grid; grid-template-columns: 1fr 2fr; gap: 1rem;">
    <label for="name">Seleccione el sexo:</label>
    @Html.DropDownList("IidSexo", sexList, new { @class="form form-control" })
</form>


<table class="table">
    <thead class="thead-dark">
        <tr>
            <th>@Html.DisplayNameFor(model => model.IidPersona)</th>
            <th>@Html.DisplayNameFor(model => model.NombreCompleto)</th>
            <th>@Html.DisplayNameFor(model => model.Email)</th>
            <th>@Html.DisplayNameFor(model => model.NombreSexo)</th>
        </tr>
    </thead>

    <tbody>
        @foreach (PersonaClass item in Model)
        {
            <tr>
                <td>@Html.DisplayFor(model => item.IidPersona)</td>
                <td>@Html.DisplayFor(model => item.NombreCompleto)</td>
                <td>@Html.DisplayFor(model => item.Email)</td>
                <td>@Html.DisplayFor(model => item.NombreSexo)</td>
            </tr>
        }
    </tbody>
</table>
```

Queremos añadir una opción por defecto al selector, con el objetivo de que aparezca antes que las opciones de la base de datos, es decir aparezca en la posición `0` del arreglo de items, para lo cual modificamos el método del controlador de la siguiente manera:

```c#
...
namespace Section03.Controllers
{
    public class PersonaController : Controller
    {
        public void FillSexComboBox()
        {
            ...
            using (BDHospitalContext db = new BDHospitalContext())
            {
                ...
                sexList.Insert(
                    0,
                    new SelectListItem
                    {
                        Text = "--Seleccione--",
                        Value = ""
                    }
                );
            }
            ...
        }
        ...
    }
}
```

En la próxima lección veremos la manera para que el valor de la selección en el combo box, afecte el resultado de la consulta en la acción del controlador.

## Filtrar resultados por selección de ComboBox

En esta sección vamos a intentar que el filtro de sexo afecte los resultados de las personas listadas. Dentro del controlador vamos a modificar la acción `Index`, para que si la selección sea la opción por defecto o 0, nos traiga todos los resultados, pero si hay una selección especifica pueda filtrar los valores:

```c#
...
namespace Section03.Controllers
{
    public class PersonaController : Controller
    {
        ...
        public IActionResult Index(PersonaClass objPersona)
        {
            ...
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if (objPersona.Iidsexo == 0)
                {
                    ...
                }
                else
                {
                    personaList = (
                        from persona in db.Personas
                        join sexo in db.Sexos
                        on persona.Iidsexo equals sexo.Iidsexo
                        where persona.Bhabilitado == 1
                        && persona.Iidsexo == objPersona.Iidsexo
                        select new PersonaClass
                        {
                            IidPersona = persona.Iidpersona,
                            NombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
                            Email = persona.Email,
                            NombreSexo = sexo.Nombre
                        }
                    ).ToList();
                }
            }

            return View(personaList);
        }
    }
}
```

Por último, dentro la vista debemos aplicar la siguiente modificación:

```cshtml
...
<form id="form" class="my-3" method="POST" aps-controller="Especialidad" asp-action="Index" class="my-3"
    style="display: grid; grid-template-columns: 1fr 2fr 1fr 1fr; gap: 1rem;">
    ...
    <button type="button" class="btn btn-success" onclick="submit()">Enviar</button>
    <button type="button" class="btn btn-info" onclick="clean()">Limpiar</button>
</form>
...
<script>
    function clean () {
        document.getElementById("IidSexo").value = 0;
        document.getElementById("form").submit()
    }

    function search() {
        document.getElementById("form").submit()
    }
</script>
```
