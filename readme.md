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
7. [Inicio](#inicio)
## Características
- Implementación de variables de entorno para el acceso a los servicios y otros usos
- Lectura y procesamiento de archivos incluyendo validación de datos y, generador de reportes por solicitud
- Registros de Logs con implementación de patrón Rate limit, para monitorear la actividad en el sistema y, limitar la cantidad de solicitudes para prevenir sobrecarga en el servidor
- Autenticación mediante tokens con formato JWT, para proteger la comunicación entre servicios
  
Ejemplo de reporte generado
```json
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
        "messageError":"la propiedad help deskS no esta en un formato valido"
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
```
Tests/
│
├── EmployeeControllerTest.cs   # test del controlador que de procesa el archivo y lo prepara, para crear nuevos registros en la base de datos
```
- Iniciar test: con el siguiente comando ejecuta las pruebas unitarias
```bash
dotnet test
```
## Monitoreo de rutas
- Monitoreo: se realiza mediante un middleware y la implementación de la clase que contiene la lógica para crear logs personalizados
  
Ejemplo de salida de logs
```javascript
  warn: app.Logs.Loggin[0]
      Date: 08/01/2025 13:47:33, Method: POST /api/employee, StatusCode: 401, in 100ms IP address 153.151.222.39 countRequest 1
  info: app.Logs.Loggin[0]
      Date: 08/01/2025 13:52:56, Method: POST /api/employee, StatusCode: 200 in 855ms IP address 153.151.222.39 countRequest 2
  warn: app.Logs.Loggin[0]
      Date: 08/01/2025 13:57:26, Method: POST /api/employee, StatusCode: 429, in 12ms IP address 153.151.222.39 countRequest 16
```
## Inicio
- Inicio de la aplicación: una vez clonado el repositorio se debe escribir el siguiente comando en la terminal
```bash
dotnet run
```
