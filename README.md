# BlogPost Project

## Table of Contents

- [Introduction](#introduction)
- [Architecture](#architecture)
  - [Api](#api)
  - [Core](#core)
  - [Infrastructure](#infrastructure)
- [Design Decisions](#design-decisions)
- [Docker](#Docker)
- [How to Run the Project](#how-to-run-the-project)
- [Additional Features](#additional-features)

## Introduction

The BlogPost project is a web application designed to manage blog posts. It is built with a clean architecture in mind, separating concerns into different layers: Api, Core, and Infrastructure. This separation makes the application easier to maintain, test, and extend.

## Architecture

The project is structured into three main layers:

### Api

The `Api` layer handles HTTP requests and responses. It is responsible for exposing the endpoints and returning the appropriate data.

- **Endpoints**: Contains `BlogPostsEndpoints.cs`, which defines the API endpoints for managing blog posts.
- **Models**: Contains data transfer objects (DTOs) used by the endpoints.
  - `CreatePostDto.cs`
  - `PostDto.cs`
  - `UpdatePostDto.cs`
- **Program**: Contains the `Program.cs` file, which is the entry point of the application and configures the web host and middleware.

### Core

The `Core` layer contains the business logic and domain entities.

- **Entities**: Contains domain entities like `Post.cs`.
- **Exceptions**: Contains custom exceptions used throughout the application.
  - `BlogPostServiceException.cs`
  - `NotFoundException.cs`
  - `RepositoryException.cs`
- **Interfaces**: Contains interfaces for services and repositories.
  - `IBlogPostService.cs`
  - `IRepository.cs`
- **Mappers**: Contains mapping logic for converting between entities and DTOs.
  - `PostMapper.cs`
- **Services**: Contains business logic implementation.
  - `BlogPostService.cs`

### Infrastructure

The `Infrastructure` layer handles data access and external dependencies.

- **Data**: Contains the database context and migration scripts.
  - `Migrations` folder: Contains migration scripts for database schema changes.
  - `BlogPostContext.cs`: Defines the database context.
  - `DataExtensions.cs`: Contains extension methods for initializing and seeding the database.
- **Repositories**: Contains implementations of repository interfaces.
  - `Repository.cs`

## Design Decisions

- **Clean Architecture**: The project follows the principles of clean architecture to separate concerns and ensure that the business logic is not dependent on external frameworks or technologies.
- **DTOs for Data Transfer**: Using DTOs to transfer data between the client and server ensures that the internal representation of the data is decoupled from the API.
- **Custom Exceptions**: Custom exceptions provide more granular error handling and make the application more robust and easier to debug.
- **Minimal APIs**: The project uses minimal APIs to keep the endpoint definitions simple and the codebase clean.
- **Logging**: Serilog is used for logging to make it easy to track and debug the application's behavior.
- **SQLite**: SQLite is used as the database for this project since it is lightweight, easy to set up, and suitable for handling CRUD operations in a development or small-scale production environment.
- **Unit Testing**: Comprehensive unit tests have been implemented to ensure that all functionalities work as expected.

## Docker

### Containerization
The entire application has been containerized using Docker, ensuring that it can run consistently across different environments. This includes the API, the database, and all necessary dependencies.

## How to Run the Project

To get the project up and running, follow these steps:

1. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   cd <repository-name>
    ```
2. **Start the Application**:
    simply run:
    ```bash
    docker-compose up
    ```
This command will build and start all the services defined in the docker-compose.yaml file. Once the process is complete, your project will be up and running.

## Additional Features

**Swagger UI:**
Swagger UI is included for easy API exploration and testing. Once the project is running, you can access the Swagger UI at `http://localhost:8080/`. This interactive documentation makes it simple to understand and test the available endpoints.

