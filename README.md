# CoffeeShop Admin Panel

---

## 📖 Introduction

**CoffeeShop Admin Panel** is an ASP.NET MVC application that empowers administrators to manage coffee products, orders, and users. Leveraging the NiceAdmin Bootstrap theme for a sleek UI, it follows a traditional MVC architecture without Entity Framework Core, instead using ADO.NET for database interactions.

## 🌳 Repository Structure

```text
├── .idea/                             # IDE settings (JetBrains Rider)
├── .vs/                               # Visual Studio cache files
├── NiceAdminTheme/                    # ASP.NET MVC project
│   ├── Controllers/                   # MVC controllers
│   ├── Models/                        # Data models and view models
│   ├── Views/                         # Razor views
│   ├── wwwroot/                       # Static assets
│   │   ├── assets/                    # CSS, JS, images (NiceAdmin theme)
│   │   └── lib/                       # Third-party libraries (Bootstrap, jQuery)
│   ├── Scripts/                       # JavaScript files
│   ├── appsettings.json               # Configuration (connection strings)
│   ├── NiceAdminTheme.csproj          # Project file
│   ├── Program.cs                     # App startup
│   └── Startup.cs                     # Middleware and service configuration
├── NiceAdminTheme.sln                 # Solution file
├── README.md                          # Project overview (this file)
└── LICENSE                            # License file
```

## 🚀 Features

* **Dashboard**: Overview panels for total sales, orders, and user metrics.
* **Product Management**: CRUD operations on coffee products with image uploads.
* **Order Management**: View and update order statuses.
* **User Management**: Manage user roles and profiles.
* **Responsive UI**: Built on Bootstrap 5 using the NiceAdmin theme.
* **Authentication & Authorization**: Secure login using ASP.NET Identity.

## 🛠️ Tech Stack

| Layer           | Technology                      |
| --------------- | ------------------------------- |
| Framework       | ASP.NET MVC (.NET 8)            |
| Frontend        | Bootstrap 5, jQuery             |
| Data Access     | ADO.NET (System.Data.SqlClient) |
| Database        | SQL Server                      |
| Authentication  | ASP.NET Identity                |
| Development IDE | Visual Studio 2022              |

## ⚙️ Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* SQL Server
* Visual Studio 2022

## 💻 Installation & Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/krishiraj123/CoffeeShop_AdminPanel.git
   ```
2. **Open solution**

   * Open `NiceAdminTheme.sln` in Visual Studio or Rider.
3. **Configure database connection**

   * In `NiceAdminTheme/appsettings.json`, set `DefaultConnection` under `ConnectionStrings` to your database.
4. **Initialize database**

   * Create the database and tables by running the provided SQL script `Database/InitDatabase.sql` in your SQL Server or SQLite client.
5. **Run the application**

   ```bash
   cd NiceAdminTheme
   dotnet run
   ```

   * Visit `https://localhost:5001` in your browser.

## 🏃‍♂️ Development Workflow

* **Adding a new entity**:

  1. Define the model class under `Models/`.
  2. Create corresponding controller in `Controllers/`.
  3. Scaffold Razor views under `Views/`.
  4. Write SQL commands in `Database/InitDatabase.sql` and ADO.NET code in your repository to query/update.

* **UI Customization**:

  * Edit HTML/CSS in `Views/Shared/_Layout.cshtml` and `wwwroot/assets/css`.

* **Seeding Data**:

  * Use `Database/SeedData.sql` to populate initial records.

## 📦 Deployment

1. **Publish**

   ```bash
   cd NiceAdminTheme
   dotnet publish -c Release -o ./publish
   ```
2. **Deploy**

   * Copy the contents of `publish` to your hosting environment (IIS, Docker container, etc.).

## 🤝 Contributing

Contributions are welcome! Fork the repo, create a feature branch, and submit a pull request.

1. Fork this repository
2. Create a branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m "Add feature"`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

See [CONTRIBUTING.md](docs/CONTRIBUTING.md) for more details.

## 📞 Contact

**Maintainer**: Krishiraj Vansia
**Email**: [krishirajvansia123@gmail.com](mailto:krishirajvansia123@gmail.com)

---

> *CoffeeShop Admin Panel* © 2025 Krishiraj Vansia. All rights reserved.
