# Survey Basket API Project

# Project Mind Map
![Image](https://github.com/user-attachments/assets/a8225e4b-45b0-4111-8842-3e3f2b864e23)

## Glimpse of the working solution
![Image](https://github.com/user-attachments/assets/d170babe-44df-434c-a85f-45decb20b47a)

![Image](https://github.com/user-attachments/assets/d8718041-7f84-40b1-b858-be869169076b)

![Image](https://github.com/user-attachments/assets/6f6d9cfb-5b57-4666-a117-3251deb2583b)

![Image](https://github.com/user-attachments/assets/7f22791c-756a-45dc-b636-db02d0b3450f)

![Image](https://github.com/user-attachments/assets/a6354f4b-4c58-4f71-ae58-c06e410032c2)

![Image](https://github.com/user-attachments/assets/1a716638-50c3-4849-83c6-26e881e1ce02)

![Image](https://github.com/user-attachments/assets/d75d2bfd-5786-43c6-a5af-10e7d8bb256d)

![Image](https://github.com/user-attachments/assets/330ee994-fd92-4d0f-bb70-b91a6ff4b652)

![Image](https://github.com/user-attachments/assets/3add9385-a0ad-4846-b0ba-68ed24204998)

![Image](https://github.com/user-attachments/assets/9e21ca0a-93e0-4741-aa5d-e371b6242a5e)
## Project Overview

**Objective:** 
SurveyBasket is a web application or API designed for creating, managing, and responding to surveys.
It provides a platform for users to create custom surveys, collect responses, and analyze results.
Built with .NET and ASP.NET Core, this project is ideal for businesses, 
educational institutions, or anyone looking to gather feedback or conduct research.

## Tech Stack
-**Backend**: .NET 9 (Web API)

-**Database**: SQL Server 

-**Authentication**: Secure access to surveys and responses using JWT token and refresh token authentication 

-**ORM**: Entity Framework Core for database interactions

## Key Features
-**🔒 User and Role Management**: Leveraged JWT for secure authentication and authorization, allowing for seamless and secure access control.

-**📊 Polls and Surveys**: Users can easily create, manage, and participate in polls, facilitating effective data collection and engagement.

-**📝 Audit Logging**: Implemented audit logging to track changes on resources, ensuring transparency and accountability in user actions.

-**🚨 Exception Handling**: Integrated centralized exception handling to manage errors gracefully, significantly enhancing the user experience.

-**⚠️ Error Handling with Result Pattern**: Employed a result pattern for structured error handling, providing clear and actionable feedback to users.

-**🚦CORS (Cross-Origin Resource Sharing)**: a security feature implemented by web browsers to prevent web pages from making requests to a different domain than the one that served the web page. 

-**🔄 Automapper/Mapster**: Utilized for efficient object mapping between models, improving data handling and reducing boilerplate code.

-**✅ Fluent Validation**: Ensured data integrity by effectively validating inputs, leading to user-friendly error messages.

-**🔑 Account Management**: Implemented features for user account management, including change password and reset password functionalities.

-**🚦 Rate Limiting**: Controlled the number of requests to prevent abuse, ensuring fair usage across all users.

-**🛠️ Background Jobs**: Used Hangfire for managing background tasks like sending confirmation emails and processing password resets seamlessly.

-**🔍 Health Checks**: Incorporated health checks to monitor the system’s status and performance, ensuring reliability and uptime.

-**🗃️ Distributed Caching**: Optimized performance with caching for frequently accessed data, significantly improving response times.

-**📧 Email Confirmation**: Managed user email confirmations, password changes, and resets seamlessly to enhance security.

-**📊Pagination**:To manage and display large datasets by breaking them into smaller.

-**🚦Sorting**: the ability to organize and return data in a specific order based on one or more criteria .

-**🔍Searching**: the ability to filter and retrieve data based on specific criteria provided by the client.

-**📈 API Versioning**: Supported multiple versions of the API to ensure backward compatibility and smooth transitions as the project evolves.


## Development Focus

### 1. [Genaric Repository Pattern](#repository-pattern)
- **Description:** Implement the Repository Pattern to abstract data access logic, making the code more testable and maintainable. 
- **Functionality:**
  - **Genaric Repository Pattern:** Simplifies data access by providing a consistent API for CRUD operations.
  - **Unit of Work:** Manages transactions across multiple repositories, ensuring data integrity.


### 2. [Entity Framework Core](#entity-framework-core)
- **Description:** Handle database interactions using Entity Framework Core, allowing for seamless integration with the database. The use of code-first migrations ensures that the database schema is in sync with the application models.
- **Features:**
  - **Code-First Migrations:** Automatically generate database schema from your code.
  - **Entity Mapping:** Ensure proper mapping of domain entities to database tables.

### 3. [Auth Section](#auth-section)
- **Login:** Secure user authentication.
- **Reset Password:** Provide password recovery options.
- **Confirm Email** Sent Email virefication to user to avoid fake emails.
- **Edit Profile:** Enable users to update their personal information and settings.

### 4. [Pagination](#pagination)
- **Description:** Implement pagination to manage large sets of products across multiple pages, ensuring a user-friendly experience.
- **Functionality:** Pagination will be integrated with search and filter functionalities to allow users to easily navigate through products.

### 6. [Publishing to Monester](#publishing-to-monester)
- **Description:** Deploy the APIs on Monester, ensuring the deployment process is smooth and the application is optimized for the platform.
- **Deployment Focus:** Ensure the application is configured for performance, security, and scalability in a cloud environment.

### 7. [Publishing locally on IIS (Internet Information Services)](#Publishing-locally-on-IIS-(Internet-Information-Services))
- **Isolated Environment:** Running your website locally on IIS allows you to test and debug in an environment
    that is isolated from your production server. This helps in identifying and fixing issues without affecting live users.

### 8. [Data Seeding](#data-seeding)
- **Description:** Seed initial data for the admin role and users to ensure the system starts with essential data, improving ease of testing and initial use.
- **Seeded Data:**
- **Admin Role:** Pre-configured admin role with full access.
- **Sample Users:** Initial users with different roles for testing purposes.

## Links
- **[Project Repository](https://github.com/mohammedabdelaleem/SurveyBasket)**
