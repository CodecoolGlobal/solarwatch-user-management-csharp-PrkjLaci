<p align="center">
    <h1 align="center">SOLARWATCH-USER-MANAGEMENT-CSHARP-PRKJLACI</h1>
</p>
<p align="center">
	<img src="https://img.shields.io/github/last-commit/CodecoolGlobal/solarwatch-user-management-csharp-PrkjLaci?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
	<img src="https://img.shields.io/github/languages/top/CodecoolGlobal/solarwatch-user-management-csharp-PrkjLaci?style=flat&color=0080ff" alt="repo-top-language">
	<img src="https://img.shields.io/github/languages/count/CodecoolGlobal/solarwatch-user-management-csharp-PrkjLaci?style=flat&color=0080ff" alt="repo-language-count">
</p>
<p align="center">
		<em>Built with the tools and technologies:</em>
</p>
<p align="center">
	<img src="https://img.shields.io/badge/JavaScript-F7DF1E.svg?style=flat&logo=JavaScript&logoColor=black" alt="JavaScript">
	<img src="https://img.shields.io/badge/HTML5-E34F26.svg?style=flat&logo=HTML5&logoColor=white" alt="HTML5">
	<img src="https://img.shields.io/badge/Vite-646CFF.svg?style=flat&logo=Vite&logoColor=white" alt="Vite">
	<br>
	<img src="https://img.shields.io/badge/React-61DAFB.svg?style=flat&logo=React&logoColor=black" alt="React">
	<img src="https://img.shields.io/badge/Docker-2496ED.svg?style=flat&logo=Docker&logoColor=white" alt="Docker">
</p>

<br>

#####  Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Repository Structure](#repository-structure)
- [Modules](#modules)
- [Getting Started](#getting-started)
    - [Installation](#installation)
    - [Tests](#tests)
---

## Overview

The SolarWatch is an ASP.NET Web API project with React frontend that provides the sunrise and sunset times for a given city on a given date, in local or UTC times.

---

##  Features

### Authentication
- **User Login**: Secure login functionality allowing users to access their accounts.

### Sunset & Sunrise Data
- **City Data Retrieval**: Users can obtain sunrise and sunset times based on the city name.

### Admin Functionalities
- **Data Management**: Admins have the ability to edit sunset and sunrise times as well as city data.

### Notifications
- **User Alerts**: Notifications powered by React Toastify for real-time updates and alerts.


---

##  Repository Structure

```sh
└── solarwatch-user-management-csharp-PrkjLaci/
    ├── .github
    │   └── workflows
    ├── README.md
    ├── SolarWatch
    │   ├── .gitignore
    │   ├── Contracts
    │   ├── Controllers
    │   ├── Data
    │   ├── Dockerfile
    │   ├── Models
    │   ├── Program.cs
    │   ├── Properties
    │   ├── Repository
    │   ├── Service
    │   ├── SolarWatch.csproj
    │   ├── appsettings.Development.json
    │   └── appsettings.json
    ├── SolarWatch.IntegrationTests
    │   ├── Authentication
    │   ├── ControllerTests
    │   ├── SolarWatch.IntegrationTests.csproj
    │   ├── SolarWatchWebApplicationFactory.cs
    │   └── Usings.cs
    ├── SolarWatchTest
    │   ├── CityDataControllerTest.cs
    │   ├── SolarWatchTest.csproj
    │   ├── SunsetSunriseControllerTest.cs
    │   └── Usings.cs
    ├── SolarWatchUi
    │   ├── .eslintrc.cjs
    │   ├── .gitignore
    │   ├── README.md
    │   ├── index.html
    │   ├── package-lock.json
    │   ├── package.json
    │   ├── public
    │   ├── src
    │   └── vite.config.js
    ├── docker-compose.yml
    ├── package-lock.json
    ├── package.json
    └── solarwatch-user-management-csharp-PrkjLaci.sln
```

---

## Modules

### Authentication Module
- **Purpose**: Manages user authentication and session handling.
- **Key Components**:
  - **Login Component**: Handles user login and validation.
  - **Registration Component**: Manages new user registrations.
  - **Session Management**: Manages user sessions and tokens.

### Data Retrieval Module
- **Purpose**: Fetches and processes sunset and sunrise data based on city names.
- **Key Components**:
  - **API Client**: Interfaces with external APIs to fetch data.
  - **Data Processor**: Processes and formats the data for use within the application.
  - **Error Handling**: Manages errors and exceptions related to data retrieval.

### Admin Module
- **Purpose**: Provides functionality for administrative tasks.
- **Key Components**:
  - **City Data Management**: Allows admins to add, edit, and delete city information.
  - **Sunset/Sunrise Data Management**: Enables admins to update sunrise and sunset times.
  - **User Management**: Admin features for managing user accounts and permissions.

### Notifications Module
- **Purpose**: Manages and displays notifications to users.
- **Key Components**:
  - **Toast Notifications**: Uses React Toastify to show alerts and updates.
  - **Notification Settings**: Allows customization of notification preferences.
  - **Notification History**: Keeps track of past notifications for user reference.

### UI/UX Module
- **Purpose**: Handles the user interface and user experience aspects of the application.
- **Key Components**:
  - **Responsive Design**: Ensures the application works on various devices and screen sizes.
  - **Theming**: Allows customization of the application’s look and feel.
  - **Accessibility Features**: Implements features to make the application accessible to all users.
---

## Getting Started

### Installation

Build the project from source:

1. Clone the `solarwatch-user-management-csharp-PrkjLaci` repository:

    ```sh
    git clone https://github.com/CodecoolGlobal/solarwatch-user-management-csharp-PrkjLaci
    ```

2. Navigate to the project directory:

    ```sh
    cd solarwatch-user-management-csharp-PrkjLaci
    ```
    
### Set Up the Backend

1. Navigate to the backend directory:

    ```sh
    cd SolarWatch
    ```

2. Initialize user secrets if you haven't already:

    ```sh
    dotnet user-secrets init
    ```

3. Add your secrets:

    ```sh
    dotnet user-secrets set "OPENWEATHER_API_KEY" "your_openweather_api_key_here"
    dotnet user-secrets set "JwtOptions:IssuerSigningKey" "your_jwt_signing_key_here"
    dotnet user-secrets set "DB_CONNECTION_STRING" "your_connection_string here"
    ```

4. Run SQL Server in Docker (if you haven't already):

    ```sh
    docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Codecool12__' -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
    ```

5. Restore dependencies and apply database migrations:

    ```sh
    dotnet restore
    dotnet ef database update
    ```

6. Start the backend server:

    ```sh
    dotnet run
    ```

### Set Up the Frontend

1. Open a new terminal window and navigate to the frontend directory:

    ```sh
    cd ../SolarWatchUi
    ```

2. Install the required dependencies:

    ```sh
    npm install
    ```

3. Start the frontend application:

    ```sh
    npm run dev
    ```

   The React development server will start and should be accessible at `http://localhost:5173`.

###  Tests

Execute the test suite using the following command:

```sh
❯ dotnet test
```

### Verify the Application

1. Open your browser and go to `http://localhost:5173` to view the frontend. You should be able to interact with the application and see data from the backend.

### Troubleshooting

- **Backend Issues**: Ensure that SQL Server is running in Docker and is accessible. Verify the connection string and ensure Docker is correctly configured.
- **Frontend Issues**: If the frontend isn't loading, ensure the backend is running and reachable. Check the browser's developer tools for network request errors.

### FAQ

- **How do I change the API key?**
  Update the `OPENWEATHER_API_KEY` secret using the `dotnet user-secrets set` command.

- **What if I encounter database connection issues?**
  Verify the `DB_CONNECTION_STRING` and ensure that the SQL Server container is running and configured correctly. You can check the status of your Docker container with `docker ps`.

```
