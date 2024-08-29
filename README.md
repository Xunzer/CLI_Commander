# CLI Commander REST API (ASP.NET Core MVC)
## [Please click here to access the site.](http://clicommanderapicontainer.d0fmcwgzeyb3fkea.canadacentral.azurecontainer.io/index.html)
#### An API that stores and retrieves CLI commands you often forget.

### The goal of this project is to explore concepts related to:
- Building a REST API
- ASP.NET Core
- Entity Framework
- MVC Pattern
- C#

### By applying:
- RESTful API principles
- Repository design pattern
- Dependency injection
- SQL Server Express and SSMS
- Entity Framework Core O/RM (DBContext, Migration)
- Data Transfer Object (DTO)
- AutoMapper
- HTTP methods (GET, POST, PUT, PATCH, DELETE, status codes)
- Postman and Swagger UI (for testing endpoints purpose)
- Docker (image, container and delopyed on Docker Hub)
- Azure (Deployed Docker Image and moved SQL Database from local)

### Application Architecture:
![Application_Architecture](https://github.com/user-attachments/assets/4042fca6-601e-452e-9678-949bf12eb564)

### API Endpoints (CRUD):
| URI                | Verb        | Operation   | Description                | Success          | Failure                             |
|:------------------ |:----------- |:----------- |:-------------------------- |:-------------    |:----------------------------------- |
|/api/commands       |GET          |READ         |Read all resources          |200 OK            |400 Bad Request, 404 Not Found       |
|/api/commands/{id}  |GET          |READ         |Read a single resource      |200 OK            |400 Bad Request, 404 Not Found       |
|/api/commands       |POST         |CREATE       |Create a new resource       |201 Created       |400 Bad Request, 405 Not Allowed     |
|/api/commands/{id}  |PUT          |UPDATE       |Update an entire resource   |204 No Content    |..                                   |
|/api/commands/{id}  |PATCH        |UPDATE       |Update partial resource     |204 No Content    |..                                   |
|/api/commands/{id}  |DELETE       |DELETE       |Delete a single resource    |204 No Content    |..                                   |

### Website UI:
![Website_UI](https://github.com/user-attachments/assets/1ecf4ac8-7974-4628-ae70-f22fd67cc0ae)

### Testing with Postman:
#### [HttpGet] Returns all commands and a "200 OK" status code.
![Postman_Test](https://github.com/user-attachments/assets/bb794e6b-57ab-4524-b81e-e8501a87ab92)
