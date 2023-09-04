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
