# EMI Group API 

La aplicación se desarrolla bajo la arquitectura de microservicios. Implementado arquitectura Hexagonal y DDD.
Ademas, el proyecto se desarrolló bajo el concepto de MinimalApis y el patrón arquitectonico de CQRS, se implementan EntitiyFramework cómo ORM y de la parte de la base de datos SQL Server.
Se aplican patrones de diseño cómo UnitOfWork, Patrón Factory, Patrón Repository, a su vez, se implementa código limpio.
Se crean pruebas unitarias para validar el correcto funcionamiento de algunos commados.
Por otra parte, se implementan distintos Middlewares cómo la captuta de errores globales, Autheticación y Authorización por Roles. Tambien la aplicación cuenta con una integración de RabbitMQ, un orquestador de mensajeria que permite enviar correos electronicos y guardar en una base de datos de MongoDb los posibles errores que se presenten, esto para permitir tener un alto grado de monitoreo y trazabilidad en las excepciones que se generan en el sistema.
Por otra parte, se implementa una estructura de respuesta estandar con el nombre de RequestResult, todas las peticiones están demominadas para responder a partir de esta clase y tener un estructura que facilite la integración con los cliente o terceros.
Adema, se implementa el HealthCheck, para validar la salud del sistema en terminos de disponibilidad de la base de datos, consutas a terceros, uso de memoria y generación de errores.


## Funcionalidades

La aplicación cuenta con diferentes APIs y EndPoints, por defecto hay una data que al iniciar la aplicación inserta información en las tablas. (Ver clase: 02. Employees -> Infraestructure -> DependencyInjection).

En el sistema hay 2 Roles.
El Rol 1, es el Admin, el 2 es el Usuario.

- Usuario Admin: Admin1234*
- Contraseña: Admin1234*

- Usuario normal: User1234*
- Contraseña: User1234*

El usuario sólo tiene permisos para consultar el API de GetAllEmployees, de resto, todos los demas EndPoints están configurados para ser consultados por un Admin.
El unico EndPoint que no requiere Authenticación es el de Login el cual se encuentra en la API de Gateway.
Todos los nuevos usuarios que se crean en el sistema quedan relacionados automaticamente con el Rol de Usuario.

En el API de Employee, hay operaciones para crear consultar, crear, modificar o eliminar Empleados.
Ademas, el servicio que se encarga de hacer el calculo anual de aumento de salario, es AnnualCalculatedIncreaseAllEmployee.
A su vez, hay otro EndPoint, que se encarga de relacionar poryectos a empleados: RegisterEmployeeProject. Esto con la finalidad de alimentar la consulta de los empleados relacionados a un departamento y al menos un proyecto proyecto activo.

Se creó una API en MongoDB, la cual no se pide en la prueba, pero me pareció interesante integrarla con el RabbitMQ para el reporte de errores y envio de correos, esto mas que todo a nivel de historico de posibles errores en la aplicación.

### Requisitos Previos
- .Net Core 8.0
-  RabbitMQ (Erlang)
-  SQL Server
-  MongoDb

### Pasos para la instalación:

1. **Clonar el repositorio**:

   ```bash
   git clone https://github.com/jozsefacostar/Samtel_Test.git](https://github.com/jozsefacostar/EMIGroup.API.git)

Ejecutar las APIs de forma general. Hay que subir las APIS de Web.Gateway.API, Web.Employee.API, Web.LogError.API.

La migración se encuentra en el proyecto de 02. Employees, por lo cual, al iniciar la aplicación esta se debe crear en la base de datos.
A tener en cuenta que las cadenas de conexión se encuentran en el app.settings de cada aplicación. Si es requerido modificarla se puede hacer el cambio directamente en el archivo.


   
