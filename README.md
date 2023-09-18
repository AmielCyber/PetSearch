# Pet Search
A website to find available pets for adaption within a zip code.
Website was made with React and ASP.NET. 

ASP.NET hosted at Azure and React hosted at Netlify.

## Live Demo

[React Application Live Demo](https://pet-search-react.netlify.app)

[ASP.NET Live Swagger UI](https://pet-search.azurewebsites.net/swagger/index.html)

**Note:** ASP.NET Server may take ~5 seconds on initial load. Long initial loading time is due to
a [cold start](https://azure.microsoft.com/en-us/blog/understanding-serverless-cold-start/)
where the server and the database are being instantiated after being inactive for more than 10 minutes.
I may upgrade in the future to have the server "always on" at Azure.

## Preview
![Desktop Preview](/Assets/DesktopPreview.gif)

![Mobile Preview](/Assets/MobilePreview.gif)


## Technology Stack
* C# and [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet)
* Typescript and [React](https://react.dev) with [Vite](https://vitejs.dev)
* Entity Framework
* MySQL
* Azure Web App deployment
* OpenApi/Swagger
* React Router
* Material UI

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
Note: You must set up a local environment with MySQL and change the following in file 
appsettings.Development.json and replace {} with your own MySQL settings:
```
Server=localhost;Port={};User Id={};Password={};Database=app
```
1. Go to the API application of the ASP.NET project: 
```bash
cd ../PetSearch.API
```
2. Register **your** keys:
```bash
dotnet user-secrets init
dotnet user-secrets set "PetFinder:ClientId" "your_client_id"
dotnet user-secrets set "PetFinder:ClientSecret" "your_client_secret"
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
6. Run the application:
   ```bash
   dotnet run
   ```
   1. In the terminal check what port the application is using: 
   ```
   http://localhost:{port}
   ```
   2. Open your browser and go to `http://localhost:{port}/swagger/index.html` to test the app endpoints.
   4. Close the application by entering <kbd>ctrl c</kbd> on your terminal where you ran `dotnet run`
