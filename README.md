# Pet Search
A website to find available pets for adaption within a zip code.
Website was made with React and ASP.NET. Website deployed at Azure with ASP.NET. 

ASP.NET also serves the React static .js, .html, and .css files.

## Live Demo
**Note:** Website may take ~5 seconds on initial load. Long initial loading time is due to
a [cold start](https://azure.microsoft.com/en-us/blog/understanding-serverless-cold-start/)
where the server and the database are being instantiated after being inactive for more than 10 minutes.
I may upgrade in the future to have the server "always on" at Azure.

[Live Demo](https://pet-search.azurewebsites.net)

[Swagger UI](https://pet-search.azurewebsites.net/swagger/index.html)

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

## Local Development Set Up

*Note: This set up assumes you have obtained keys from PetFinder API.*

### Required Dependencies

* [Node.js](https://nodejs.org/en)
* [.NET SDK](https://dotnet.microsoft.com/en-us/download)


### Running the Production Build
1. Clone this repository: 
```bash
git clone https://github.com/AmielCyber/PetSearch
```
2. After cloning this repository, go to the repository directory:
```
cd PetSearch
```

#### ASP.NET Setup
Note: You must set up a local environment with MySQL and change the following in file 
appsettings.Development.json and replace {} with your own MySQL settings:
```
Server=localhost;Port={};User Id={};Password={};Database=app
```
1. Go to the backend application or the ASP.NET project: 
```
cd ../PetSearchAPI
```
2. Register **your** keys:
```bash
dotnet user-secrets init
dotnet user-secrets set "PetFinder:ClientId" "your_client_id"
dotnet user-secrets set "PetFinder:ClientSecret" "your_client_secret"
```
3. Download and install NuGet dependencies:
    ```
    dotnet restore
    ```
4. Build the .NET application:
   ```
   dotnet build
   ```
5. Create MySQL database:
   ```
   dotnet ef database update --project ../PetSearch.Data
   ```
6. Test the application:
   ```
   dotnet run
   ```
   1. In the terminal check what port the application is using: 
   ```
   http://localhost:{port}
   ```
   2. Open your browser and go to `http://localhost:{port}/swagger/index.html` to test the app endpoints.
   3. Go to `http://localhost:{port}` to interact with the React application
   4. Close the application by entering <kbd>ctrl c</kbd> on your terminal where you ran `dotnet run`

### Running in Development

#### Setup React

1. Go to the frontend application or the React project: 
```
cd pet-search-client
```
2. Install npm dependencies: 
```
npm install
```
3. Build the application: 
```
npm run build
```

#### Set Up the ASP.NET Application
Use the same steps as above for [ASP.NET Setup](#aspnet-setup)

*Note: Take note what port the ASP.NET application is running at.*

#### Configure React
1. Go to the React application directory: 
```
cd /pet-search-client
```
2. Change the port inside the file: `.env.development` to the port that **your** ASP.NET application is using.

#### Run the React Application
`npm run dev`
