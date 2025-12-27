# Event Management System

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

## Getting Started
### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or Docker
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Git](https://git-scm.com/)

