# Pet Search
A website to search available pets for adaption within a given zip code. Users can enter a zipcode or share their
location with the browser to locate their zipcode.

Website was made with React and ASP.NET Web Api. React application hosted at Netlify and ASP.NET Web API hosted at
Microsoft Azure.

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

* C# and [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet) Web API
* Tested with Xunit
* [Microsoft Azure Web App Deployment](https://azure.microsoft.com/en-us/products/app-service/web)
* MySQL
* [OpenApi/Swagger](https://www.openapis.org)
* [PetFinder API](https://www.petfinder.com/developers/v2/docs/)
* [MapBox Geolocation API](https://www.mapbox.com)
* 
## Angular Application
[Angular GitHub Source Code](https://github.com/AmielCyber/pet-search-angular)

## React Application
I am no longer serving the React files from ASP.NET. I have decided to host the React application at Netlify instead for
a better user experience and separation of concerns.

[React GitHub Source Code](https://github.com/AmielCyber/pet-search-react)

## Local Development Set Up

*Note: This set up assumes you have obtained keys from PetFinder API.*

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
Note: You must set a MySQL database and create a username and password with DBManager privileges. You must also create a connection for MySQL.
```SQL
DROP USER if exists 'your-user-name'@'localhost';

CREATE USER 'your-user-name'@'localhost' IDENTIFIED BY 'your-password';

GRANT ALL PRIVILEGES ON * . * TO 'your-user-name'@'localhost';

```

Note: You must also get access tokens from MapBox and PetFinder API

In the file: appsettings.development.json replace the empty {} with your own MySQL settings:
```
"DefaultConnection": "server=localhost;port=3306;database={yourDatabaseName};user={yourUserName};password={yourPassword}"
```
1. Go to the API application of the ASP.NET project: 
```bash
cd ./PetSearch.API
```
2. Register **your** keys:
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
