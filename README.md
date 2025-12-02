🗂️ RecordKeeper – Simple C# WinForms App with SQLite

RecordKeeper is a lightweight Windows desktop application built with C# (.NET WinForms) and SQLite.
It allows users to manage personal records by storing and retrieving data such as National ID, Full Name, and Case Number.

✨ Features

📝 Add new records (National ID, Full Name, Case Number)

🔍 Search by Name or National ID

🧾 View all records in a DataGridView

🔄 Update existing records

❌ Delete unwanted records

💾 Local SQLite database (no server required)

🧠 Tech Stack

Language: C# (.NET 6 / .NET Framework)

UI: Windows Forms

Database: SQLite (using Microsoft.Data.Sqlite)

ORM/Access: Raw SQL commands

⚙️ How It Works

The app connects to a local SQLite database (data.db) using Microsoft.Data.Sqlite.
All CRUD (Create, Read, Update, Delete) operations are performed via a simple user interface that displays data in a grid view.

🧩 Requirements:

Visual Studio 2022 or later

.NET 6 or .NET Framework 4.8

NuGet Packages:

Install-Package Microsoft.Data.Sqlite
Install-Package SQLitePCLRaw.bundle_e_sqlite3
