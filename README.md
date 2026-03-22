# Shifting Management System 🚚

A comprehensive desktop application for managing goods shifting operations built with C# WinForms and SQL Server.

## 📋 Features

### 👤 Customer Module
- User registration and login
- Create new shift requests
- Track ongoing jobs in real-time
- View job history with completed/cancelled status
- Update profile information
- Cancel pending jobs

### 👑 Admin Module
- Admin dashboard with overview
- Manage all shift requests
- Update shift status (Pending → Ongoing → Completed)
- View and manage customer details
- Generate reports

### 🎨 UI Features
- Color-coded job status indicators
  - 🟡 Pending: Yellow background
  - 🔵 Ongoing: Blue background  
  - 🟢 Completed: Green background
  - 🔴 Cancelled: Red background
- Modern, responsive design
- Intuitive navigation with tabbed interface

## 🛠️ Technologies Used

- **Frontend:** C# WinForms
- **Database:** SQL Server
- **Data Access:** ADO.NET
- **IDE:** Visual Studio 2022
- **Framework:** .NET Framework 4.7.2

## 📊 Database Schema

### Customers Table
- CustomerID (PK)
- FirstName, LastName
- Email, Username, Password
- Phone, CreatedDate

### Shifts Table
- ShiftID (PK)
- CustomerID (FK)
- ShiftDate, Status
- ServiceType
- Pickup/Delivery details (Province, District, City, Address)
- Contact information
- Package description and weight

## 🚀 Setup Instructions

### Prerequisites
- Visual Studio 2022 or later
- SQL Server (Express or Developer edition)
- .NET Framework 4.7.2

### Database Setup
1. Open SQL Server Management Studio
2. Run the script in `EShiftSQL/SQLQuery1.sql`
3. This creates the database `E-ShiftDB` and all required tables

### Application Setup
1. Clone this repository
2. Open `eShiftManagementSystem.sln` in Visual Studio
3. Update connection string in `App.config` if needed:
   ```xml
   <connectionStrings>
       <add name="EShiftDB" 
            connectionString="Server=YOUR_SERVER_NAME;Database=E-ShiftDB;Trusted_Connection=True;" />
   </connectionStrings>
