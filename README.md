# Game Changer
Game Changer es un sistema que permite la administración del inventario de una tienda de videojuegos mediante una aplicación web. La aplicación web permite guardar, ver, borrar o editar objetos en el inventario del sistema. La aplicación también cuenta con un mecanismo de autenticación para el acceso seguro de los administradores del sistema.

Para Game Changer se utilizó el framework de Blazor Server sobre .NET 10 para representar la interfaz web junto con C#. En la parte del almacenamiento de la información se utilizó una base ORM con Entity Framework Core y SQL Server, para eliminar la necesidad de escribir sentencias SQL manualmente en la gestión de la información.

En el programa se implementó el uso de Selenium junto con el framework NUnit para la automatización de las pruebas, y además se utilizó ExtentReports para generar reportes HTML con evidencias de cada ejecución de los procesos automatizados. Por último, para el control de versiones del proyecto se utilizó Git y GitHub con la metodología de GitFlow aprendida en clase, mientras que la planificación y seguimiento de las historias y épicas se realizó a través de Jira.
