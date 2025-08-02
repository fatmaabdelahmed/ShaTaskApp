# ShaTaskApp - Invoice Management System

A simple and clean Invoice Management System built with **ASP.NET Core MVC** and **SQL Server**, designed to manage invoices, cashiers, branches, and cities with ease. The system supports dynamic invoice creation, product selection, and real-time total calculation.

---

## 🛠 Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- HTML5, CSS3, JavaScript
- Bootstrap (for UI)
- LINQ

---

## 📚 Features

✅ **Invoice Management**  
- Create new invoices dynamically  
- Add multiple product rows with quantities and price auto-fill  
- Auto-calculate grand total  
- Select cashier based on branch  
- Validations and error messages  

✅ **Cashier Management**  
- Add, edit, delete cashiers  
- Filter by city or branch  

✅ **Branch & City Management**  
- Add and manage cities  
- Create branches under each city  

✅ **Dynamic Forms**  
- Invoice form supports adding/removing rows  
- Total updates instantly with JavaScript  
- Responsive layout  

---

## 🖥️ Screenshots

| Invoice Form | Cashier List |
|--------------|---------------|
| ![Invoice Form](screenshots/invoice-form.png) | ![Cashier List](screenshots/cashier-list.png) |

---

## 📁 Project Structure

ShaTaskApp/
│
├── Controllers/
│ └── InvoiceController.cs
│ └── CashierController.cs
│
├── Models/
│ └── InvoiceHeader.cs
│ └── InvoiceDetails.cs
│ └── Cashier.cs
│
├── ViewModels/
│ └── InvoiceCreateEditViewModel.cs
│
├── Views/
│ └── Invoice/
│ └── Create.cshtml
│ └── _InvoiceForm.cshtml
│
├── wwwroot/
│ └── css/
│ └── invoice-form.css
│ └── js/
│ └── invoice-form.js
│
└── Data/
└── AppDbContext.cs



---

## 🚀 How to Run

1. Clone the repo:
   ```bash
   git clone https://github.com/fatmaabdelahmed/ShaTaskApp.git
Open the solution in Visual Studio

Make sure you have SQL Server running and update the connection string in appsettings.json

Run the following in Package Manager Console:


Update-Database
Press F5 or run the project

## 🧪 Future Enhancements
Authentication & Roles (Admin / User)

Invoice PDF export

Product inventory tracking

Dark mode UI 🌙

## 👩‍💻 Developed By
Fatma Abdelhameed Helmy
Full-Stack .NET Developer | ITI Graduate



