# Pet Search [ARCHIEVED]

## NOTE:
This project has been moved to https://github.com/AmielCyber/DotnetPetSearch and will no longer be recieving updates in this repository.
##


A user-friendly API allows for the search of available pets for adoption within a specific zip code. Users can also input a zip code or a set of coordinates to obtain the city’s information, enabling them to search for available pets in that area.

The Web application was developed using ASP.NET Web API and .NET 8.

The Angular application is hosted on Vercel, the React application is hosted on Netlify, and the ASP.NET Web API is hosted on Microsoft Azure.

[Pet Search Angular Application GitHub Repository](https://github.com/AmielCyber/pet-search-angular)

[Pet Search React Application GitHub Repository](https://github.com/AmielCyber/pet-search-react)

## Live Demo

**Note:**
Server response may take around 10 seconds during a
[cold start](https://azure.microsoft.com/en-us/blog/understanding-serverless-cold-start/cold) when the server is
reactivated after 10 minutes of inactivity. I'm considering upgrading the server to 'always on' on Microsoft Azure in
the future.

[ASP.NET Live Swagger UI](https://pet-search.azurewebsites.net/swagger/index.html)

[Angular Application Live Demo](https://pet-search-angular.vercel.app)

[React Application Live Demo](https://pet-search-react.netlify.app)

## Preview

![Desktop Preview](/Assets/DesktopPreview.gif)

![Mobile Preview](/Assets/MobilePreview.gif)

## Technology Stack

<div style="display: flex; flex-wrap: wrap; gap: 5px">
    <img alt="C Sharp" width="30px" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg"/>
    <img alt="Dotnet Core" width="30px" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dotnetcore/dotnetcore-original.svg"/>
    <img alt="Azure" width="30px" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/azure/azure-original.svg"/>
    <img alt="MySQL" width="30px" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/mysql/mysql-original.svg"/>
</div>

* Developed with C# and [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet) Web API
* Tested with [Xunit](https://xunit.net/)
* Hosted at [Microsoft Azure Web App Deployment](https://azure.microsoft.com/en-us/products/app-service/web)
* Token storage with [MySQL](https://www.mysql.com/)
* API Documentation with [OpenApi/Swagger](https://www.openapis.org)
* Adoptable pet retrieval with [PetFinder API](https://www.petfinder.com/developers/v2/docs/)
* Location authentication with [MapBox Geolocation API](https://www.mapbox.com)

## Angular Application

[Angular GitHub Source Code](https://github.com/AmielCyber/pet-search-angular)

## React Application

I am no longer serving the React files from ASP.NET. I have decided to host the React application at Netlify instead for
a better user experience and separation of concerns. I have also stopped providing updates for the React application.
All client updates will be done through the Angular application.

[React GitHub Source Code](https://github.com/AmielCyber/pet-search-react)

## Local Development Set Up

*Note: This set up assumes you have obtained keys from PetFinder API and MapBox API.*

### Required Dependencies

* [.NET SDK](https://dotnet.microsoft.com/en-us/download)

### Running the Production Build

1. Clone this repository:

```bash
git clone https://github.com/AmielCyber/PetSearch
```

2. After cloning this repository, go to the repository directory:

```bash
cd PetSearch
```

#### ASP.NET Setup

Note: You must set a MySQL database and create a username and password with DBManager privileges. You must also create a
connection for MySQL.

In MySQL Workbench run this script and replace 'username' and 'password'
```SQL
DROP
USER if exists 'username'@'localhost';

CREATE
USER 'username'@'localhost' IDENTIFIED BY 'password';

GRANT ALL PRIVILEGES ON * . * TO
'username'@'localhost';

```
Then set up a connection on [MySQL Workbench](https://dev.mysql.com/doc/workbench/en/wb-mysql-connections-new.html) with the credentials you put above.

Note: You must also get access tokens from MapBox and PetFinder API

In the file: appsettings.development.json replace the brackets ({}) and the contents inside with your own MySQL settings:

```
"DefaultConnection": "server=localhost;port=3306;database={yourDatabaseName};user={yourUserName};password={yourPassword}"
```

1. Go to the API application of the ASP.NET project:

```bash
cd ./PetSearch.API
```

2. Register **your** PetFinder and MapBox keys:

```bash
dotnet user-secrets init
dotnet user-secrets set "PetFinder:ClientId" "your_client_id"
dotnet user-secrets set "PetFinder:ClientSecret" "your_client_secret"
dotnet user-secrets set "MapBox:AccessToken" "your_access_token"
```

3. Download and install NuGet dependencies:
    ```bash
    dotnet restore
    ```
4. Build the .NET application:
   ```bash
   dotnet build
   ```
5. Create MySQL database:
   ```bash
   dotnet ef database update -p ../PetSearch.Data
   ```
6. Run tests:
   ```bash
   dotnet test
   ```
7. Run the application:
   ```bash
   dotnet run
   ```
    1. In the terminal check what port the application is using:
   ```
   http://localhost:{port}
   ```
    2. Open your browser and go to `http://localhost:{port}/swagger/index.html` to test the app endpoints.
    4. Close the application by entering <kbd>ctrl c</kbd> on your terminal where you ran `dotnet run`
