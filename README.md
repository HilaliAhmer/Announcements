# Announcements - ASP.NET Core MVC Project

## 📖 Project Overview

**Announcements** is an ASP.NET Core MVC project developed to manage internal announcements within an organization. This platform aims to facilitate communication between IT teams and other users by providing a system for announcements such as outages, planned maintenance, procedures, and user guides.

## 🛠️ Architecture and Technology Stack

### Architecture Design

The project was developed using the following layered architecture:

- **Business Layer:** Business logic layer.
- **Core Layer:** The layer with common components and structures.
- **DataAccess Layer:** The layer that manages data access operations.
- **Entities Layer:** The layer where entity classes are defined.
- **Web UI Layer:** The layer that provides the user interface.

### Technologies Used

- **Backend:** ASP.NET Core MVC 8.0
- **Frontend:**
  - Bootstrap 5.3.x
  - jQuery 3.6.x
  - FontAwesome 6.x
  - SCSS 1.62.x
  - CSS (Bootstrap ile entegre)
- **Database:** SQL Server
- **ORM:** Entity Framework Core 8.0.0

## 🖥️ Installation and Setup

### Pre-requisites

Before running the project, make sure that the following components are installed:

- **.NET SDK 8.0** or higher
- **Visual Studio 2022** (or other IDE that supports ASP.NET Core MVC)
- **SQL Server**

### Installation Stages

1. **Clone the Warehouse**  
Clone the repository to your local machine:
```bash
git clone https://github.com/HilaliAhmer/Announcements.git
```

2. **Open Project**  
Open the project using Visual Studio 2022 or Visual Studio Code.

3. **Install Required Dependencies** 
Install NuGet dependencies:
   - In Visual Studio, go to `Tools > NuGet Package Manager > Manage NuGet Packages for Solution`.
   - Make sure all dependencies are installed.

4. **Configure Database**  
   - Open the `appsettings.json` file and configure the database connection settings.
   - Run the migration process using the following command:
```bash
dotnet ef database update
```

5. **Run the Application**  
Use the following command to run the project:
```bash
dotnet run
```

6. **Get Access from Your Web Browser**  
You can run the application by visiting the following URL:
```bash
http://localhost:5000
```

## 🤝 Contributing

You can follow the steps below to contribute to the project:

1. Fork the repository.
2. Develop a new feature or fix a bug.
3. Share your changes by sending a pull request.

## 📄 Licance

This project is licensed under the [MIT Lisansı](LICENSE).
