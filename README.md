# Pied Piper Event Management System

A comprehensive event management API built with ASP.NET Core 8.0, Entity Framework Core, and SQL Server. This system provides complete event lifecycle management, user authentication, registration handling, notifications, and analytics.

## Features

### Core Features
- **User Management**
  - JWT-based authentication
  - Department-based organization
  - Admin and regular user roles
  - Password management (change, reset, forgot)
  - Session management with token blacklisting

- **Event Management**
  - Create, update, delete events
  - Event types (In-person, Virtual, Hybrid)
  - Categories and tags
  - Capacity management (min/max)
  - Waitlist functionality
  - Visibility controls
  - Speaker and agenda management

- **Registration System**
  - User event registration/unregistration
  - Automatic waitlist management
  - Auto-promotion from waitlist
  - Registration status tracking (Confirmed, Waitlisted, Cancelled)
  - Registration deadlines

- **Notifications**
  - Real-time user notifications
  - Welcome messages
  - Registration confirmations
  - Event updates
  - Unregistration notifications
  - Mark as read/unread functionality

- **Analytics & Dashboard**
  - Key Performance Indicators (KPIs)
  - Event status distribution
  - Registration trends
  - Category distribution
  - Top events by demand
  - Department participation metrics

- **Security**
  - BCrypt password hashing
  - JWT token authentication
  - Token blacklist for logout
  - CORS configuration
  - Environment-based settings

## Tech Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server / SQL Server Express
- **ORM**: Entity Framework Core 8.0
- **Authentication**: JWT Bearer Tokens
- **Password Hashing**: BCrypt.Net
- **API Documentation**: Swagger/OpenAPI
- **Containerization**: Docker support

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or Docker
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/pied-piper-events.git
cd pied-piper-events
```

### 2. Configure Database Connection

Update `appsettings.json` with your SQL Server connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PiedPiperDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**Connection String Examples:**
- Local SQL Server: `Server=localhost;Database=PiedPiperDB;Trusted_Connection=True;TrustServerCertificate=True`
- SQL Express: `Server=localhost\\SQLEXPRESS;Database=PiedPiperDB;Trusted_Connection=True;TrustServerCertificate=True`
- With Credentials: `Server=localhost;Database=PiedPiperDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True`
- Docker: `Server=localhost,1433;Database=PiedPiperDB;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True`

### 3. Install Dependencies

```bash
dotnet restore
```

### 4. Run Database Migrations

```bash
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Create database and tables
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7232`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:7232/swagger`

## Docker Setup

### Using Docker Compose

```bash
# Start SQL Server and API
docker-compose up -d

# Run migrations
docker exec -it pied-piper-api dotnet ef database update

# View logs
docker-compose logs -f
```

### SQL Server Only (Docker)

```bash
# Run SQL Server container
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Password123" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest

# Update appsettings.json connection string
"DefaultConnection": "Server=localhost,1433;Database=PiedPiperDB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True"
```

## API Documentation

### Base URL
```
Development: https://localhost:7232/api
Production: https://your-domain.com/api
```

### Authentication

All protected endpoints require a JWT token in the Authorization header:
```
Authorization: Bearer <your-token-here>
```

### Endpoints Overview

#### Authentication (`/api/auth`)
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/auth/register` | ❌ | Register new user |
| POST | `/auth/login` | ❌ | Login and get JWT token |
| POST | `/auth/logout` | ✅ | Logout current session |
| POST | `/auth/logout-all` | ✅ | Logout all devices |
| GET | `/auth/me` | ✅ | Get current user info |
| POST | `/auth/refresh` | ✅ | Refresh JWT token |
| POST | `/auth/check-otp` | ❌ | Validate OTP code |
| PUT | `/auth/change-password` | ✅ | Change password |
| POST | `/auth/forgot-password` | ❌ | Request password reset |
| POST | `/auth/reset-password` | ❌ | Reset password with token |
| GET | `/auth/check-session` | ❌ | Check if token is valid |

#### Events - Public (`/api/events`)
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/events` | ❌ | Get all visible events |
| GET | `/events/{id}` | ❌ | Get event details |
| GET | `/events/sorted-by-popularity?n=10` | ❌ | Top N popular events |
| GET | `/events/sorted-by-upcoming-date?n=10` | ❌ | Top N upcoming events |
| GET | `/events/top-events?limit=10` | ❌ | Top events for dashboard |
| GET | `/events/types` | ❌ | Get all event types |
| GET | `/events/categories` | ❌ | Get all categories |
| GET | `/events/categories/search?title={title}` | ❌ | Search category by title |
| GET | `/events/types/search?name={name}` | ❌ | Search event type by name |

#### Events - Admin (`/api/admin`)
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/admin/events/all-including-hidden` | ✅ Admin | Get all events (including hidden) |
| POST | `/admin/events` | ✅ Admin | Create new event |
| PUT | `/admin/events/{id}` | ✅ Admin | Update event |
| DELETE | `/admin/events/{id}` | ✅ Admin | Delete event |
| DELETE | `/admin/events/bulk` | ✅ Admin | Bulk delete events |
| PATCH | `/admin/events/{id}/visibility` | ✅ Admin | Toggle event visibility |
| GET | `/admin/events/{id}/waitlist` | ✅ Admin | Get event waitlist & registrations |

#### Registrations (`/api/registration`)
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/registration/register` | ✅ | Register for an event |
| DELETE | `/registration/unregister/{eventId}` | ✅ | Unregister from event |
| GET | `/registration/my-registrations` | ✅ | Get user's registrations |
| GET | `/registration/check/{eventId}` | ✅ | Check registration status |
| GET | `/registration/event/{eventId}` | ✅ | Get event registrations |

#### Notifications (`/api/notification`)
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/notification/my-notifications` | ✅ | Get user notifications (marks as read) |
| GET | `/notification/unread-count` | ✅ | Get unread notification count |
| POST | `/notification/{id}/mark-as-read` | ✅ | Mark notification as read |
| DELETE | `/notification/{id}` | ✅ | Delete notification |

#### Users (`/api/user`)
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/user` | ✅ | Get all users |
| GET | `/user/{id}` | ✅ | Get user by ID |
| GET | `/user/email/{email}` | ✅ | Get user by email |

#### Analytics (`/api/analytics`)
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/analytics/dashboard` | ✅ Admin | Get dashboard analytics |

### Request/Response Examples

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "john.doe@company.com",
  "password": "Password123!",
  "firstName": "John",
  "lastName": "Doe"
}
```
**Response:** `204 No Content`

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john.doe@company.com",
  "password": "Password123!"
}
```
**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 3,
  "email": "john.doe@company.com",
  "fullName": "John Doe",
  "department": "Engineering",
  "isAdmin": false,
  "expiresAt": "2025-12-26T15:30:00Z"
}
```

#### Register for Event
```http
POST /api/registration/register
Authorization: Bearer <token>
Content-Type: application/json

{
  "eventId": 1
}
```
**Response:**
```json
{
  "message": "Successfully registered for the event",
  "registrationId": 12,
  "status": "Confirmed",
  "registration": {
    "id": 12,
    "eventId": 1,
    "eventTitle": "Python Workshop",
    "userId": 3,
    "userFullName": "John Doe",
    "statusName": "Confirmed",
    "registeredAt": "2025-12-24T12:30:00Z"
  }
}
```

#### Get Notifications
```http
GET /api/notification/my-notifications
Authorization: Bearer <token>
```
**Response:**
```json
{
  "new": [
    {
      "id": 5,
      "title": "Registration Confirmed",
      "message": "You have successfully registered for 'Python Workshop'. Your spot is confirmed!",
      "type": "Registration",
      "eventId": 3,
      "eventTitle": "Python Workshop",
      "isSeen": false,
      "createdAt": "2025-12-24T14:30:00Z"
    }
  ],
  "earlier": [],
  "totalUnseenCount": 1
}
```

## Security

### Password Security
- Passwords are hashed using **BCrypt** with automatic salting
- One-way hashing - passwords cannot be decrypted
- Slow hashing algorithm prevents brute-force attacks

### JWT Authentication
- Tokens expire after 24 hours (configurable)
- Token blacklist prevents use of revoked tokens
- Refresh token endpoint for extended sessions

### CORS Configuration
- Environment-based CORS policies
- Configurable allowed origins
- Supports credentials for authenticated requests

## 🗄️ Database Schema

### Main Entities

**Users**
- Id, Email, FullName, Password (hashed)
- DepartmentId, IsAdmin, IsActive
- PasswordResetToken, PasswordResetTokenExpires

**Departments**
- Id, Name, Description, IsActive

**Events**
- Event details, capacity, location
- Event type, category
- Waitlist configuration
- Visibility settings

**Registrations**
- User-Event relationship
- Status (Confirmed, Waitlisted, Cancelled)
- Registration timestamp

**Notifications**
- User notifications
- Type (Welcome, Registration, EventUpdate)
- Read/unread status

**TokenBlacklist**
- Revoked JWT tokens
- Expiration tracking

## Seed Data

The system includes comprehensive seed data:
- 7 Departments (Engineering, Marketing, Sales, HR, Finance, Operations, General)
- 5 Sample users (2 admins, 3 regular users)
- 14 Future events with realistic data
- 3 Event types (In-person, Virtual, Hybrid)
- 7 Categories (Conference, Workshop, Seminar, Networking, Social, Training, Webinar)
- 4 Registration statuses (Confirmed, Waitlisted, Cancelled, Pending)

### Test Credentials

**Admin User:**
```
Email: admin@company.com
Password: Admin123!
```

**Regular User:**
```
Email: john.doe@company.com
Password: Employee123!
```

## Environment Configuration

### appsettings.json (Development)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PiedPiperDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Secret": "PiedPiper-SuperSecret-JWT-Key-Must-Be-At-Least-32-Characters-Long-2024",
    "Issuer": "PiedPiperAPI",
    "Audience": "PiedPiperApp",
    "ExpirationMinutes": 1440
  },
  "AllowedOrigins": [
    "http://localhost:4200",
    "http://localhost:3000",
    "http://localhost:8100"
  ]
}
```

### appsettings.Production.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=production-server;Database=PiedPiperDB;User Id=sa;Password=ProductionPassword;TrustServerCertificate=True"
  },
  "AllowedOrigins": [
    "https://your-production-domain.com"
  ],
  "EnableSwaggerInProduction": false
}
```

## Testing

### Using Swagger UI
1. Navigate to `https://localhost:7232/swagger`
2. Click "Authorize" button
3. Login to get token: `POST /api/auth/login`
4. Copy the token from response
5. Paste in format: `Bearer <token>`
6. Click "Authorize"
7. Test any endpoint

### Using cURL

**Login:**
```bash
curl -X POST https://localhost:7232/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@company.com","password":"Admin123!"}'
```

**Get Events:**
```bash
curl https://localhost:7232/api/events
```

**Register for Event (with auth):**
```bash
curl -X POST https://localhost:7232/api/registration/register \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <your-token>" \
  -d '{"eventId":1}'
```

## Project Structure

```
Pied_Piper/
├── Controllers/
│   ├── AdminController.cs          # Admin event management
│   ├── AnalyticsController.cs      # Dashboard analytics
│   ├── AuthController.cs           # Authentication & password
│   ├── EventsController.cs         # Public event endpoints
│   ├── NotificationController.cs   # User notifications
│   ├── RegistrationController.cs   # Event registrations
│   └── UserController.cs           # User management
├── Data/
│   ├── ApplicationDbContext.cs     # EF Core DbContext
│   └── SeedData.cs                 # Database seeding
├── DTOs/
│   ├── Auth DTOs
│   ├── Event DTOs
│   ├── Registration DTOs
│   ├── Notification DTOs
│   └── Analytics DTOs
├── Middleware/
│   └── TokenBlacklistMiddleware.cs # Token validation
├── Models/
│   ├── User.cs
│   ├── Department.cs
│   ├── Event.cs
│   ├── EventType.cs
│   ├── Category.cs
│   ├── Registration.cs
│   ├── RegistrationStatus.cs
│   ├── Speaker.cs
│   ├── Agenda.cs
│   ├── Notification.cs
│   └── TokenBlacklist.cs
├── Repositories/
│   ├── IEventRepository.cs
│   ├── EventRepository.cs
│   ├── IRegistrationRepository.cs
│   ├── RegistrationRepository.cs
│   ├── IUserRepository.cs
│   └── UserRepository.cs
├── Services/
│   ├── IJwtService.cs
│   ├── JwtService.cs
│   ├── INotificationService.cs
│   ├── NotificationService.cs
│   ├── ITokenService.cs
│   └── TokenService.cs
├── Migrations/
├── appsettings.json
├── appsettings.Development.json
├── appsettings.Production.json
└── Program.cs
```

## Deployment

### Prerequisites
- SQL Server database
- .NET 8.0 Runtime
- Reverse proxy (Nginx/Apache) for HTTPS

### Steps

1. **Publish the application:**
```bash
dotnet publish -c Release -o ./publish
```

2. **Copy files to server:**
```bash
scp -r ./publish/* user@server:/var/www/pied-piper-api/
```

3. **Update production settings:**
```bash
# Edit appsettings.Production.json on server
nano /var/www/pied-piper-api/appsettings.Production.json
```

4. **Run migrations:**
```bash
cd /var/www/pied-piper-api
dotnet ef database update
```

5. **Start the application:**
```bash
# Using systemd service
sudo systemctl start pied-piper-api
sudo systemctl enable pied-piper-api

# Or using screen/tmux
screen -S api
dotnet Pied_Piper.dll
```

## Troubleshooting

### Database Connection Issues
```
Error: Cannot open database "AppDb"
Solution: Run migrations: dotnet ef database update
```

### CORS Errors
```
Error: CORS policy blocked
Solution: Add frontend URL to AllowedOrigins in appsettings.json
```

### Authentication Errors
```
Error: Unable to resolve INotificationService
Solution: Ensure service is registered in Program.cs:
         builder.Services.AddScoped<INotificationService, NotificationService>();
```

### Migration Errors
```
Error: Project file does not exist
Solution: Navigate to project directory with .csproj file
```

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/AmazingFeature`
3. Commit changes: `git commit -m 'Add AmazingFeature'`
4. Push to branch: `git push origin feature/AmazingFeature`
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Authors

- **Your Name** - *Initial work* - [YourGitHub](https://github.com/yourusername)

## Acknowledgments

- ASP.NET Core team for the amazing framework
- Entity Framework Core for ORM
- BCrypt.Net for password security
- All contributors and testers

## Support

For support, email support@piedpiper.com or open an issue on GitHub.

---

**Built with ❤️ using ASP.NET Core 8.0**