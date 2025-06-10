# Blazor WebAssembly Application

This project is a Blazor WebAssembly application. It includes authentication, dashboard, and profile features.

## Project Structure

- **Pages**: Contains the main pages of the application (Index, Home, Profile)
- **Pages/Auth**: Contains authentication pages (Login, Register)
- **Services**: Contains the authentication service and state provider
- **Layout**: Contains shared components (MainLayout, NavMenu, TopNav)

## Features

- User authentication (login/register)
- Protected routes with authorization
- Dashboard page
- User profile page
- Responsive layout

## Technologies Used

- ASP.NET Core Blazor WebAssembly
- MudBlazor Component Library
- Local Storage for authentication persistence

## Getting Started

1. Install the .NET SDK (version 8.0 or later)
2. Clone the repository
3. Navigate to the project directory
4. Run `dotnet restore` to restore dependencies
5. Run `dotnet run` to start the application

## Development

- Use `dotnet watch run` for hot reload during development
- Use `dotnet build` to build the project
- Use `dotnet publish -c Release` to publish the project

## Notes

This application uses mock authentication. In a production environment, you would replace the mock authentication with a real backend API.
