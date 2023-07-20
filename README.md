# Inventory And Sales Management System
# Technology :
  1. ASP .NET Core Web API
  2. C# Programming Language
  3. MSSQL database
# Architecture & Design Pattern : 
  The project has been developed by implementing Repository Pattern. 
  The data access logic has been kept separate from business logic using Repositories & Services.
  The database and tables can be accessed only from repositories. Controllers get access through services.
  The business logic is implemented in Services and data access logic is implemented through Repositories.
# Installation :
  1.	Requirements:
    a.	MS SQL Server
    b.	Visual Studio 2022/2019
    c.	.NET 6 SDK
  2.	Extract the project file from the zip.
  3.	Open the .sln file with Visual Studio.
  4.	In the package manager console command “update-database”.
  5.	From the SQLScript folder, execute the scripts.
  6.	Run the project. Swagger UI will appear.

