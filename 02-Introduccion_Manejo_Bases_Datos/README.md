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
        "DBHospital": "Data Source=localhost, 1433;Initial Catalog=BDHospital;Integrated Security=True;TrustServerCertificate=True"
    }
}
```

En el `Data Source` definimos el servidor, en `Initial Catalog` definimos la base de datos a la cual nos conectamos, en `Integrated Security` definimos que usaremos la autenticación combinada de Windows y SQL Server, y en `TrustServerCertificate` generamos un certificado de conexión para que el contenedor de docker permita la conexión a nuestro server.

Posteriormente realizamos el mapeo con el siguiente comando:

```txt
$: dotnet ef dbcontext scaffold "Name=ConnectionStrings:DBHospital" Microsoft.EntityFrameworkCore.SqlServer -o Models
```

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
