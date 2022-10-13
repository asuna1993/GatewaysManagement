# Gateway Management API
REST service (JSON/HTTP) for storing information about these gateways and their associated devices. 

## Build and Test
To build the project, an appsetings.json file, that defines a connection, needs to adjust the values as per your needs.

# Database
As per requisite you must have installed .Net Core 3 and EF Core Command Line tools (https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet).

We are using the .net migrations system to update the database.

In order to create the database and update it to the latest migration execute the following command in the Data project's root CLI:
```
Update-Database
```
