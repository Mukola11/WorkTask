# WorkTask

WorkTask is a task management system with user authentication.

## Setup Instructions

### Prerequisites

1. **.NET SDK**: You need the .NET SDK to run the project. Download and install it from the [official site](https://dotnet.microsoft.com/download).

2. **PostgreSQL**: PostgreSQL database must be installed and configured. Download it from the [official site](https://www.postgresql.org/download/).

### Steps to Run the Project

1. **Clone the Repository**:
   ```bash
   git clone https://your-repository-url.git
   cd your-repository-name
   
2. **Install Dependencies**:
   ```bash
   dotnet restore
   
3. **Configure Settings**: Create an `appsettings.json` file in the root directory of the project with the following settings:
   ```json
   {
     "Jwt": {
       "Key": "your-secret-key",
       "Issuer": "your-issuer",
       "Audience": "your-audience"
     },
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=your-database;Username=your-username;Password=your-password;"
     }
   }

4. **Apply Migrations**: First, create and apply migrations to set up the database schema:

   - **Create Migrations**: Run the following command to create migration files based on your model changes:
     ```bash
     dotnet ef migrations add InitialCreate
     ```

   - **Apply Migrations**: Run the following command to apply the migrations to the database:
     ```bash
     dotnet ef database update
     ```

## API Documentation

### User Registration

**POST** `/api/auth/register`

#### Request Body

```json
{
  "username": "string",
  "email": "string",
  "password": "string"
}
```
#### Responses

- **200 OK**: `"User registered successfully."`
- **400 Bad Request**: `"User with this email or username already exists."`
- **400 Bad Request**: `"Password does not meet complexity requirements."`


### User Login
**POST** `/api/auth/login`

#### Request Body

```json
{
  "usernameOrEmail": "string",
  "password": "string"
}
```
#### Responses

- **200 OK**: `{ "token": "jwt-token" }`
- **401 Unauthorized**: `"Invalid credentials."`

### Create Task
**POST** `/api/task`

#### Request Body

```json
{
  "title": "string",
  "description": "string",
  "dueDate": "datetime",
  "status": "enum (Pending, InProgress, Completed)",
  "priority": "enum (Low, Medium, High)"
}
```
#### Responses

- **201 Created**: `{ "id": "guid", "title": "string", ... }`
- **401 Unauthorized**: `"Unauthorized"`

### Get Tasks
**GET** `/api/task`

#### Query Parameters

- **status** (optional): `Enum value representing the task status (e.g., Pending, InProgress, Completed).`
- **dueDate** (optional): `Filter tasks by due date (datetime).`
- **priority** (optional): `Enum value representing the task priority (e.g., Low, Medium, High).`
- **sortBy** (optional): `Enum value for sorting tasks (e.g., DueDate, Priority).`
- **sortOrder** (optional): `Enum value for sorting order (e.g., Ascending, Descending).`

#### Responses

- **200 OK**: 
  ```json
  [
    {
      "id": "guid",
      "title": "string",
      "description": "string",
      "dueDate": "datetime",
      "status": "int",
      "priority": "int",
      "createdAt": "datetime",
      "updatedAt": "datetime"
    },
  ]

- **401 Unauthorized**: `"Unauthorized"`

### Get Task By ID
**GET** `/api/tasks/{id}`

#### Request Parameters

- **id** (path parameter): `The unique identifier of the task.`

#### Responses

- **200 OK**: 
  ```json
  {
    "id": "guid",
    "title": "string",
    "description": "string",
    "dueDate": "datetime",
    "status": "int",
    "priority": "int",
    "createdAt": "datetime",
    "updatedAt": "datetime"
  }
- **401 Unauthorized**: `"User ID is null while fetching task with ID {TaskId}"`
- **404 Not Found**: `"Task with ID {TaskId} not found or not authorized for user {UserId}"`

### Update Task
**PUT** `/api/task/{id}`

#### Request Parameters

- **id** (path parameter): `The unique identifier of the task.`

#### Request Body

```json
{
  "title": "string",
  "description": "string",
  "dueDate": "datetime",
  "status": "enum (Pending, InProgress, Completed)",
  "priority": "enum (Low, Medium, High)"
}
```
#### Responses

- **200 OK:
```json
{
  "id": "guid",
  "title": "string",
  "description": "string",
  "dueDate": "datetime",
  "status": "int",
  "priority": "int",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```
- **400 Bad Request**: `"Invalid model state"`
- **401 Unauthorized**: `"Unauthorized"`
- **404 Not Found**: `"Task not found"`

### Delete Task
**DELETE** `/api/task/{id}`

#### Request Parameters

- **id** (path parameter): `The unique identifier of the task.`

#### Responses

- **204 No Content**: `"No Content"`
- **401 Unauthorized**: `"Unauthorized"`
- **404 Not Found**: `"Task not found"`

## Architecture and Design Explanation

1. **Model-View-Controller (MVC)**: 
   - Used to clearly separate concerns between models, views, and controllers, making it easier to maintain and extend the code.

2. **REST API**: 
   - Provides well-defined endpoints for client-server interactions, enabling easy integration with other systems.

3. **JWT Authentication**: 
   - Offers a secure mechanism for user authentication and session management without storing sessions on the server.

4. **BCrypt for Password Hashing**: 
   - Ensures the protection of user passwords through cryptographically secure hashing.

5. **Entity Framework Core**: 
   - Used for interacting with the database via Object-Relational Mapping (ORM), simplifying data handling and database migrations.
