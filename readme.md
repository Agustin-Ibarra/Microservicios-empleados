# Microservicios Empleados
### Descripción del funcionamiento: 
Este microservicio recibe un archivo con información de los nuevos empleados contratados. Procesa el archivo, analiza los datos y prepara la información para su ingreso en la base de datos de la empresa. Al finalizar, genera un resumen del proceso, donde incluye, cantidad total de registros leídos, cantidad de errores ocurridos, y un listado detallado de los errores, en caso de que se hayan producido.
## Tabla de contenido
1. [Características](#características)
2. [BackEnd](#backend)
3. [Base de datos](#base-de-datos)
4. [Documentacion](#documentacion)
5. [Pruebas unitarias](#pruebas-unitarias)
6. [Monitoreo de rutas](#monitoreo-de-rutas)
## Características
- Implementación de variables de entorno para el acceso a los servicios y otros usos
- Lectura y procesamiento de archivos incluyendo validación de datos y, generador de reportes por solicitud
  
Ejemplo de reporte generado
```javascript
  {
    "totalRows":42,
    "totalErrors":3,
    "errorsList":
    [
      {
        "row":2,
        "messageError":"El email: dpeartree0samsung.com no es valido"
      },
      {
        "row":13,
        "messageError":"la propiedad help desks no esta en un formato valido"
      },
      {
        "row":14,
        "messageError":"El numero de telefono 1494324315A#!* no tiene una longitud de 10 numeros, o no esta un formato valido, o contiene caracteres invalidos"
      }
    ]
  }
```
## BackEnd
- Tecnologías utilizadas: C# .NET Core ASP.NET dotnet 
```
App/
│
├── Controllers/   # Procesan las peticiones y contienen la lógica de las respuestas
├── Data/          # Configuración del constructor de AppDbContext y definición de entidades
├── Logs/          # Configuración de logs y monitoreo de la aplicación
├── Models/        # Clases que representan las entidades de la base de datos
├── Repository/    # Clases que interactúan con la base de datos mediante Entity Framework
├── Services/      # Capa de servicios
└── Program.cs     # Punto de entrada de la aplicación (configuración del servidor)
```
## Base de datos
La persistencia de información se realiza a través de una base de datos relacional, el BackEnd utiliza Entity Framework para la interacción con la base de datos, a través de un ORM
- Base de datos relacional
- Gestor de base de datos: MySql
## Documentacion
- Documentación de APIs: la documentación de los endpoint y APIs esta creada con Swagger Open.io
- Enlace: documentación disponible en [docs](http://localhost:5297/swagger)
## Pruebas unitarias
- Librerias: las pruebas unitarias estan creadas con la libreria de Xunit y Mock
<!-- ```
Tests/
│
├── CartControllerTests.cs   # test del controlador que de procesa las peticiones del carrito de compras
├── ShopControllerTasts.cs   # test del controlador que porecesa las solicitudes de la seccion shop
``` -->
- Iniciar test: con el siguiente comando ejecuta las pruebas unitarias
```bash
dotnet test
```
## Monitoreo de rutas
- Monitoreo: se realiza mediante un middleware y la implementación de la clase que contiene la lógica para crear logs personalizados
  
Ejemplo de salida de logs
```javascript
  info: app.Logs.Loggin[0]
      GET /api/item code 200 in 855ms ip address 153.151.222.39

  warn: app.Logs.Loggin[0]
      GET /api/itema code 404 in 1ms ip address 153.151.222.39
```
## Inicio
- Inicio de la aplicación: una vez clonado el repositorio se debe escribir el siguiente comando en la terminal
```bash
dotnet test
```
