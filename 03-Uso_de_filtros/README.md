# Sección 03: Uso de filtros

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
$: dotnet ef dbcontext scaffold "Data Source=localhost, 1433;Initial Catalog=BDHospital;Integrated Security=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models
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