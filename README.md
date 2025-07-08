![Cenovus.com](Cenovuslogo.jpg)


<span style="color: #244c5a;">LineList.Cenovus.Com</span>
===============

LineList.Cenovus.Com is Web API Project developed using .NET Core 7.0 and Single Page Application(SPA) using Angular  16

## Technologies used
- .NET Core 7.0
- Angular 16
- Entity Framework core 7 - Code First Approach
- SQL Server 2019
- Swagger
- Fluent API
- AutoMapper

### Unit Tests 
- xUnit
- Moq
- SQLite In-Memory database

### UI - Single Page Application(SPA)
- Angular 16
- Bootstrap
- Toastr
- HttpClient

### How to Run
- Install node js version 16.2
    - https://nodejs.org/en/download/releases
- Open command prompt and run below command to install Angular CLI
  - npm install -g @angular/cli
- Open Visual Studio 2022
- Run Web API project
  - Set LineList.Cenovus.Com.API as startup project
  - Build Solution using Menu -> Build -> Build Solution(Ctrl + Shift + B)
  - Run Project using Menu -> Debud -> Start Debugging(F5)

- Run Angular SPA Project
    - Open LineList.Cenovus.Com.UI folder in Visual Studio Code
    - Run using Menu -> Run -> Start Debugging (F5)
    
- Run Unit Testing project
    - Build Solution using Menu -> Build -> Build Solution(Ctrl + Shift + B)
    - Run Unit Testing using Menu -> Test -> Run All Test(Ctrl + R, A)

- Change Database
    - LineList.Cenovus.Com.API - appsettings.json - DefaultConnection
    - LineList.Cenovus.Com.Infrastructure - Context - LineListDbContext.cs

- Setup Database - Only for new setup
    - Open package manager console
    - Select LineList.Cenovus.Com.Infrastructure from default project
    - Run command -> Update-Database

- Migrate Database - Adding or Changing Databse Object
    - Open package manager console
    - Select LineList.Cenovus.Com.Infrastructure from default project
    - Run commands
        - Add-Migration [Name Of Migration]
        - Update-Database
        - Script-Migration
        - Commit Changes

If we face issues while updating or deleting records
    - Create Web.config file in root folder with velow Code
        <configuration>
            <system.webServer>
                <modules>
                    <remove name="WebDAVModule" />
                </modules>
                <handlers>
                    <remove name="WebDAV" />
                </handlers>
            </system.webServer>
        </configuration>
