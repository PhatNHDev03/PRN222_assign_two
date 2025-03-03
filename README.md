![image](https://github.com/user-attachments/assets/fe8d1f80-504c-46b2-bbdd-9affcc7e13c9)# News Articles Management System

## Overview

This project is a **News Articles Management System** built using **ASP.NET Core Razor Pages** and **Entity Framework Core**. It allows users to create, update, and manage news articles with various attributes such as title, headline, content, source, category, tags, and author information.

## Features

- **List all news articles** in a table format.
- **Create, edit, and delete articles** using Razor Pages.
- **Show categories, tags, and authors** dynamically.
- **Bootstrap 5 integration** for a responsive and modern UI.

## Technologies Used

- **ASP.NET Core Razor Pages**
- **Entity Framework Core**
- **Bootstrap 5** for styling
- **JavaScript (Fetch API)** for dynamic modal handling

## Architecture

This project follows the **Three-Layer Architecture** pattern:

1. **BusinessObjects Layer**: Contains entity classes and business models.
2. **DataAccess Layer**: Handles database operations using Entity Framework Core.
3. **Services Layer**: Implements business logic and interacts with the DataAccess layer.
4. **WebRazorPage Layer**: The presentation layer using Razor Pages.

## Installation

### Prerequisites

- .NET SDK (latest version)
- SQL Server
- Visual Studio / VS Code

### Steps

1. **Clone the repository**
   ```sh
   git clone <repository-url>
   cd <project-folder>
   ```
2. **Setup the database**
   - Update the `appsettings.json` with your SQL Server connection string.
   - Run migrations:
     ```sh
     dotnet ef database update
     ```
3. **Run the application**
   ```sh
   dotnet run
   ```
4. Open your browser and navigate to `http://localhost:5000/` (or the specified port).

## Usage

- Click **"Add New Article"** to create a new article.
- Click **"Edit"** to modify an existing article.
- Click **"Delete"** to remove an article.
- Articles are listed in a table with category, tags, and author details.
## Architecture
![image](https://github.com/user-attachments/assets/a3b62f0b-8f98-4c57-b7be-f0f3e29795bf)




## File Structure

```
📁 FUNewsManagementSystem
│── 📂 FUNewsManagementSystem.BusinessObjects  # Entity and domain models
│── 📂 FUNewsManagementSystem.DataAccess       # Database access layer
│── 📂 FUNewsManagementSystem.Business         # Business logic layer
│── 📂 FUNewsManagementSystem.WebRazorPage     # Presentation layer (Razor Pages)
│── appsettings.json
│── Program.cs
│── Startup.cs
│── README.md
```

## License

This project is open-source and available under the MIT License.

