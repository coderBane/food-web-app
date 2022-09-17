# Food Ordering Website ASP.NET Core .NET MAUI

This repository contains an **ASP.NET Core WebAPI application** that implements **JWT Authentication**, a **.NET MAUI application** for administrative frontend that consumes the protected WebAPI.

### Project structure
repository
|__src
|    |__Foody -> web api project
|    |__client -> maui project
|    |__.gitignore
|__.gitignore
|__ReadME.md

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
`Extra: You can run the command \l to view all existing databases`

### 

