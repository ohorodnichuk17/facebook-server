# Qubix - Social Network (Backend)

This is the backend API for **Qubix**, a social network developed as part of a diploma project at IT Step Academy.

## Project Links

- **Swagger API Documentation:** [Qubix API](https://api-qubix.itstep.click/swagger/index.html)

## Technologies Used

- **Backend Framework:** ASP.NET Core 8.0
- **Architecture:** Clean Architecture, Domain-Driven Design (DDD), Command Query Responsibility Segregation (CQRS)
- **Database:** PostgreSQL
- **Authentication:** JWT (JSON Web Tokens)
- **API Documentation:** Swagger

## Features

1. **Authentication and Authorization:**
   - User registration and login.
   - Admin login.
   - Password recovery feature.

2. **User Management:**
   - Create, update, and delete user profiles.
   - Friend management (add/remove friends).
   - Profile privacy settings.

3. **Post and Story Management:**
   - Create, edit, and delete posts (text and images).
   - Publish stories that expire after 24 hours.
   - Add tags and location to posts.

4. **Interaction with Posts:**
   - Like, comment, and react to posts.
   - Different reactions (e.g., emojis).

5. **Messaging:**
   - One-on-one chats.

6. **Admin Features:**
   - Manage users (view, block, and delete accounts).
   - Moderate content (remove inappropriate posts).

## Getting Started

### Prerequisites:
- **.NET SDK** version >= 8.0
- **PostgreSQL**

### Installation:

1. Clone the repository:
    ```bash
    git clone https://github.com/ohorodnichuk17/facebook-server.git
    ```
2. Set up the database connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Database=qubixdb;Username=postgres;Password=yourpassword"
    }
    ```
3. Run database migrations:
    ```bash
    dotnet ef database update
    ```
4. Start the API server:
    ```bash
    dotnet run
    ```
5. Access the API documentation via Swagger.

## Deployment

The API can be deployed to **Azure**, **AWS**, or any cloud service supporting **.NET Core**.
