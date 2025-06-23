# WebAPIDemo

WebAPIDemo is a simple RESTful API built with ASP.NET Core, demonstrating CRUD operations for a Shirt catalog along with JWT-based authentication and fine-grained claim-based authorization. It uses Entity Framework Core with in-memory/seeding data and custom validation and action filters.

---

## Features

- Manage shirts with full CRUD: Create, Read, Update, Delete  
- JWT token-based authentication with short-lived tokens  
- Claim-based authorization controlling access to API endpoints  
- Custom validation filters for request data integrity  
- Data seeding for initial shirt dataset  
- API versioning support  
- Clean separation of concerns via filters and repositories  

---

## Technologies Used

- ASP.NET Core Web API  
- Entity Framework Core  
- JWT (JSON Web Tokens) for Authentication  
- API Versioning  
- Custom Action, Authorization, and Exception Filters  
- Data Annotations for model validation  

---

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download) or later  
- Visual Studio 2022 / VS Code or any preferred IDE  

### Setup Instructions

1. Clone the repository:

   bash
   git clone https://github.com/yourusername/WebAPIDemo.git
   cd WebAPIDemo
Configure appsettings.json:

Add a "SecretKey" value for JWT token signing in your configuration file.

Run the application:

dotnet run
Access the API:

By default, the API runs on https://localhost:5001 or http://localhost:5000.

Authentication
Obtain JWT Token
Endpoint: POST /auth

Payload:

json
Copy
Edit
{
  ClientId = "53D3C1E6-4587-4AD5-8C6E-A8E4BD59940E",
  Secret = "0673FC70-0514-4011-B4A3-DF9BC03201BC",
}
Successful authentication returns a short-lived JWT token (expires in 1 minute).

Usage
Include the token in the Authorization header as Bearer <token> when calling protected API endpoints.

API Endpoints
Method	Endpoint	Description	Required Claim
GET	/api/shirts	Get all shirts	read: true
GET	/api/shirts/{id}	Get shirt by ID	read: true
POST	/api/shirts	Create a new shirt	write: true
PUT	/api/shirts/{id}	Update existing shirt	write: true
DELETE	/api/shirts/{id}	Delete shirt	delete: true

Data Model
Shirt
Property	Type	Description
ShirtId	int	Unique identifier (auto-increment)
Brand	string	Brand name (required)
Description	string?	Optional description
Color	string	Color of the shirt (required)
Size	int?	Shirt size with custom validation
Gender	string	Gender category (required)
Price	double?	Price of the shirt

Data Seeding
The application seeds initial shirts data with 4 sample shirts covering different brands, colors, sizes, and genders.

Custom Filters and Validation
JWT Token Authentication filter for API requests

Claim-based authorization checks per endpoint

Custom validation filters for verifying shirt IDs and payload integrity

Exception filters for handling update conflicts and errors

Contribution
Contributions are welcome! Please fork the repository and submit pull requests.

License
This project is licensed under the MIT License. See LICENSE for details.

Author
Abdul Kader (Fahad)
Toronto, Canada
Email: hello.fahadkader@gmail.com
GitHub: https://github.com/hello-fahad
LinkedIn: https://linkedin.com/in/hello-fahad

Acknowledgments
ASP.NET Core Documentation

JWT Authentication best practices

Entity Framework Core

OpenAPI / Swagger tools

Thank you for checking out WebAPIDemo!
