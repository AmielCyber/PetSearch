# Pet Search

## Set Up

**Note:** *This set up assumes you have obtained keys from the PetFinder API*.

### Required Dependencies

* [Node.js](https://nodejs.org/en)
* [.NET SDK](https://dotnet.microsoft.com/en-us/download)

After cloning this repository, go to the repository directory: `cd PetSearch`

### React Setup

1. Go to the frontend application or the React project: `cd pet-search-client`
2. Install npm dependencies: `npm install`
3. Build the application: `npm run build`

### ASP.NET Setup

1. Go to the backend application or the ASP.NET project: `cd ../PetSearchAPI`
2. Register **your** keys:
    1. `dotnet user-secrets init`
    2. `dotnet user-secrets set "PetFinder:ClientId" "your_client_id"`
    3. `dotnet user-secrets set "PetFinder:ClientSecret" "your_client_secret"`
3. Download and install NuGet dependencies: `dotnet restore`
4. Build the .NET application: `dotnet build`
5. Test the application: `dotnet run`
    1. In the terminal check what port the application is using: `http://localhost:{port}`
    2. Open your browser and go to `http://localhost:{port}/swagger/index.html` to test the app endpoints.
    3. Go to `http://localhost:{port}` to interact with the React application
6. Close the application by entering <kbd>ctrl c</kbd> on your terminal where you ran `dotnet run`
