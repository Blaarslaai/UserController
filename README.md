![Head Logo](documents/readmeHeader.png)

# Welcome to this project

## Hello from the other side...

My approach to this project was the following:

1. Debug and Fix Issues, basically get both projects running.
2. Deploy to IIS
3. Deploy to Docker

Extra things I identified:

- Create DB

I always start with debugging as it is a system I am unfamiliar with, therefore I first need to see how things work and whether or not I need to accomplish extra tasks.

## Step 1: Debug and Fix Existing Issues

### api

Within the file **ServicesController.cs** the following had to be fixed:

```C#
_response = await _ius.UserLogin(uiReq);
_response = await _ius.UpdateUser(uiReq);
_response = await _ius.DeleteAllUsers(uiReq);
```

Within the file **IUserDBServices.cs** the following had to be fixed:

```C#
Task<ServiceModel> UserLogin(string UserName);
Task<ServiceModel> GetAllUsers(SelectionFilterModel payload);
Task<ServiceModel> DeleteSingleUser(string UserId);
```

Within the file **UserDBServices.cs** the following had to be fixed:

In the method **RegisterUser** the following has to be added:

```C#
// Add parameters for the stored procedure
command.Parameters.AddWithValue("@UserId", usrm.UserId);
command.Parameters.AddWithValue("@UserName", usrm.UserName);
command.Parameters.AddWithValue("@UserToken", usrm.UserToken);
command.Parameters.AddWithValue("@Epoc", usrm.Epoc);
```

In the method **UserLogin** the following has to be added:

```C#
UserToken = reader.GetString(reader.GetOrdinal("UserToken")),
Epoc = reader.GetString(reader.GetOrdinal("Epoc")),
```

In the method **GetAllUsers** the following has to changed and added:

```C#
using (SqlCommand command = new SqlCommand("dbo.GetAllUsers", connection))
connection.Open();
UserName = reader.GetString(reader.GetOrdinal("UserName")),
```

In the method **DeleteAllUsers** the following has to be fixed:

```C#
using (SqlCommand command = new SqlCommand("dbo.DeleteAllUsers", connection))
```

### webapp

Within the file **appsettings.json** the following had to be added:

```json
"DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=db_UserController;Integrated Security=True"
```

### DB

```SQL
USE [db_UserController]
GO

IF OBJECT_ID('users', 'U') IS NOT NULL
	DROP TABLE [users];

GO

CREATE TABLE [users]
(
	[id] INT IDENTITY(1,1) PRIMARY KEY,
	[UserId] VARCHAR(50),
	[UserName] VARCHAR(50),
	[UserToken] VARCHAR(MAX),
	[Epoc] VARCHAR(50),
	[DateTime] DATETIME DEFAULT GETDATE()
)

```

## Step 2: Deploy to IIS

### Webconfig: API

```XML
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" arguments=".\UsersController.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
<!--ProjectGuid: 43dcf7a6-2e8c-4050-97f0-a4f92fc92d51-->
```

### Webconfig: WEBAPP (For Angular Routing Module of IIS)

```XML
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <staticContent>
      <mimeMap fileExtension=".json" mimeType="application/json" />
    </staticContent>
    <rewrite>
      <rules>
        <rule name="Angular Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/index.html" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>

```

## Step 3: Deploy to Docker

### Dockerfile: API

```Dockerfile
# Use .NET SDK to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet publish -c Release -o out

# Use ASP.NET runtime to run the published app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "UsersController.dll"]
```

### Dockerfile: WEBAPP

```Dockerfile
# Use Node.js to build Angular app
FROM node:20 AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build --configuration=production

# Use Nginx to serve the built Angular app
FROM nginx:alpine
COPY --from=build /app/dist/webapp /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]
```
