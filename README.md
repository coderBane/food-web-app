# Food Ordering Service ASP.NET Core .NET MAUI

This repository contains an **ASP.NET Core WebAPI application** that implements **JWT Authentication**, a **.NET MAUI application** for administrative frontend that consumes the protected WebAPI.

### Project structure
```
repository
|__src
|    |__Foody -> web api project
|    |__client -> maui project
|    |__.gitignore
|__.gitignore
|__ReadME.md
```

## Getting Started
Some requirement and workloads to install:
- Visual Studio or Visual Studio Code [link](https://visualstudio.microsoft.com/downloads/)
- .Net 6 dotnet [dotnet](https://dotnet.microsoft.com/en-us/download)
- Docker [docker](https://www.docker.com/)
- Git [git](https://git-scm.com/downloads)

## Setup

### Clone the repository
Open a command prompt(windows) or terminal(mac) and enter the command:
```
git clone https://github.com/coderBane/food-web-app.git
```

### Setup Postgres Database

Pull postgresql image from hub
```
docker pull postgres
```
Create a container instance
```
docker run --name postgresql -e POSTGRES_PASSWORD=<yourpassword> -p 55000:5432 -d postgres
```
Run psql
```
docker exec -it postgresql psql -U postgres
```

#### psql commands

First we'll need to create an new role. The role would have login and database creation priviledges.
```
CREATE ROLE devuser WITH LOGIN CREATEDB PASSWORD '<yourpassword>';
```
Switch to the new user
```
\c postgres devuser
```
Create the database
```
CREATE DATABASE foody;
```
`NOTE: You can run the command \l to view all existing databases`

### Configure the WebAPI

Before we can run the project we need to configure a few things for the web api.
Open the Foody.WebApi.csproj file and delete the UserSecretsId property.

#### User-Secrets
Run the command:
```
dotnet user-secrets init
```
Generate a random string using this website [random](https://www.random.org/strings/)
Create a new file 's.json' and populate it with the following
```
{
    "JwtConfig": {
        "Secret": "<generatedstring>",
        "ExpiryTimeFrame": "00:01:00"
    },
    "WatchDog": {
        "Username": "<yourusername>",
        "Password": "<yourpassword>"
    },
    "Postgres": "Server=localhost:55000;Database=foody;User Id=devuser;Password=<devuserpassword>;Integrated Security=true;Pooling=true;",
    "UserPW": "<yourpassword>"
}
```
Set the user secrets 
```
cat ./s.json | dotnet user-secrets set
```
`NOTE: You can delete the s.json file`

#### Apply Database Migrations
We need to create the tables in the database.
```
dotnet ef --startup-project ../Foody.WebApi database update
```

## Run the Projects

We can now run the projects

### ASP.NET Core WebAPI
launch without swagger
```
dotnet run 
```
launch with swagger UI
```
dotnet watch
```

### .NET MAUI

