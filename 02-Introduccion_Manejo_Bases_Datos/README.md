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
