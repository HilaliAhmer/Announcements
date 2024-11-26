# Announcements - ASP.NET Core MVC Project

## 📚 Project Overview
**Announcements** is a web-based announcement management platform designed to streamline the process of creating, scheduling, and distributing announcements within an organization. It features a user-friendly interface and role-based access control to ensure security and functionality.

---

## 🛠 Architecture and Technology Stack

### Architecture
The project is built using a layered architecture:
- **Business Layer**: Contains the core business logic.
- **Core Layer**: Provides shared utilities and configurations.
- **Data Access Layer**: Handles database operations using Entity Framework Core.
- **Entities Layer**: Defines the data models.
- **Web UI Layer**: The presentation layer for users.

### Technology Stack
- **Backend**: ASP.NET Core MVC 8.0
- **Frontend**:
  - Bootstrap 5.3.0
  - jQuery 3.6.4
  - FontAwesome 6.0.0
  - bootstrap-table 1.23.5
  - bootstrap-icons 1.11.3
- **Database**: SQL Server
- **ORM**: Entity Framework Core 7.0.0

---

## 📦 Installation and Setup

### Prerequisites
Ensure you have the following installed:
- **.NET SDK 8.0 or later**
- **Visual Studio 2022** (or another IDE supporting ASP.NET Core MVC)
- **SQL Server**

### Installation Steps
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/HilaliAhmer/Announcements.git
   cd Announcements
2. Update the `ConnectionStrings` section with your SQL Server details:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=AnnouncementsDB;Trusted_Connection=True;"
   }
