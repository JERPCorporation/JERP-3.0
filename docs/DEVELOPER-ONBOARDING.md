# JERP 3.0 - Developer Onboarding Guide

## Table of Contents
- [Welcome](#welcome)
- [Setup Instructions](#setup-instructions)
- [Project Structure](#project-structure)
- [Development Workflow](#development-workflow)
- [Testing Strategy](#testing-strategy)
- [Common Tasks and Recipes](#common-tasks-and-recipes)
- [Troubleshooting](#troubleshooting)

---

## Welcome

Welcome to the JERP 3.0 development team! This guide will help you get your development environment set up and introduce you to our development practices.

### What is JERP 3.0?

JERP (Joint Enterprise Resource Planning) 3.0 is a comprehensive ERP system specifically designed for cannabis businesses. It provides:
- Financial management with FASB ASC compliance
- Payroll processing and management
- Inventory tracking with seed-to-sale traceability
- Cannabis-specific compliance (280E tax, state reporting)
- Multi-tenant SaaS architecture

### Tech Stack at a Glance

- **Backend**: .NET 8, ASP.NET Core Web API, Entity Framework Core, SQL Server
- **Frontend**: Next.js 14, React 18, TypeScript, Tailwind CSS
- **Database**: Microsoft SQL Server Express (local), Azure SQL (production)
- **Caching**: Redis
- **Authentication**: ASP.NET Core Identity with JWT
- **Containerization**: Docker, Docker Compose
- **CI/CD**: GitHub Actions

---

## Setup Instructions

### Prerequisites

Before you begin, ensure you have the following installed:

**Required:**
- **.NET 8 SDK or later**: [Download here](https://dotnet.microsoft.com/download)
- **Node.js 18+ and npm**: [Download here](https://nodejs.org/)
- **Git**: [Download here](https://git-scm.com/)
- **Docker Desktop**: [Download here](https://www.docker.com/products/docker-desktop)
- **SQL Server 2019 Express or later**: [Download here](https://www.microsoft.com/sql-server/sql-server-downloads)

**Recommended:**
- **Visual Studio 2022** (Windows): [Download here](https://visualstudio.microsoft.com/)
- **VS Code** (Cross-platform): [Download here](https://code.visualstudio.com/)
- **JetBrains Rider** (Cross-platform): [Download here](https://www.jetbrains.com/rider/)
- **SQL Server Management Studio (SSMS)**: [Download here](https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)
- **Azure Data Studio** (Cross-platform alternative to SSMS): [Download here](https://docs.microsoft.com/sql/azure-data-studio/download)

**Browser Extensions:**
- React Developer Tools
- Redux DevTools (if using Redux in future)

### Quick Start with Docker (Recommended)

This is the fastest way to get up and running:

```bash
# 1. Clone the repository
git clone https://github.com/ninoyerbas/JERP-3.0.git
cd JERP-3.0

# 2. Copy environment file and configure if needed
cp .env.example .env

# 3. Start all services with Docker Compose
docker-compose up -d

# 4. View logs to ensure everything started correctly
docker-compose logs -f

# 5. Access the applications:
# - Backend API: http://localhost:5000
# - Swagger UI: http://localhost:5000/swagger
# - Frontend: http://localhost:3000
# - SQL Server: localhost:1433 (sa/YourStrong@Passw0rd)
# - Redis: localhost:6379
```

**Verify the setup:**

```bash
# Check if all containers are running
docker-compose ps

# Test the API
curl http://localhost:5000/api/health

# Test the frontend
# Open http://localhost:3000 in your browser
```

### Manual Setup (Development)

If you prefer to run services individually or are having Docker issues:

#### 1. Database Setup

**Option A: SQL Server Express (Windows)**

```bash
# Install SQL Server Express from the link above
# Then create the database:

# Open SQL Server Management Studio or use sqlcmd:
sqlcmd -S localhost\SQLEXPRESS -E

# Run these SQL commands:
CREATE DATABASE JERP3_DB;
GO
USE JERP3_DB;
GO
```

**Option B: Docker SQL Server (Cross-platform)**

```bash
# Run SQL Server in Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 --name jerp-sqlserver \
  -d mcr.microsoft.com/mssql/server:2019-latest

# Create database
docker exec -it jerp-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P "YourStrong@Passw0rd" \
  -Q "CREATE DATABASE JERP3_DB"
```

#### 2. Configure Connection Strings

Create a `.env` file in the project root (copy from `.env.example`):

```env
# Database - Windows Authentication (local SQL Server Express)
DATABASE_CONNECTION_STRING=Server=localhost\SQLEXPRESS;Database=JERP3_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True

# OR Docker SQL Server
DATABASE_CONNECTION_STRING=Server=localhost,1433;Database=JERP3_DB;User Id=sa;Password=YourStrong@Passw0rd;MultipleActiveResultSets=true;TrustServerCertificate=True

# JWT Authentication
JWT_SECRET_KEY=your-super-secret-key-minimum-32-characters-for-security
JWT_ISSUER=https://jerp.local
JWT_AUDIENCE=https://jerp.local
JWT_EXPIRY_MINUTES=60

# Redis (optional for local dev)
REDIS_CONNECTION_STRING=localhost:6379

# Azure Blob Storage (optional, for document storage)
AZURE_STORAGE_CONNECTION_STRING=your-azure-storage-connection-string

# Environment
ASPNETCORE_ENVIRONMENT=Development
```

**Update appsettings.Development.json** (if not using .env):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=JERP3_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-minimum-32-characters",
    "Issuer": "https://jerp.local",
    "Audience": "https://jerp.local",
    "ExpiryMinutes": 60
  }
}
```

#### 3. Run Database Migrations

```bash
# Navigate to the infrastructure project
cd src/JERP.Infrastructure

# Create initial migration (if needed)
dotnet ef migrations add InitialCreate --startup-project ../JERP.Api

# Apply migrations to database
dotnet ef database update --startup-project ../JERP.Api

# Navigate back to root
cd ../..
```

#### 4. Start the Backend API

```bash
# Navigate to API project
cd src/JERP.Api

# Restore dependencies
dotnet restore

# Run the API
dotnet run

# OR use watch mode for hot reload
dotnet watch run

# The API will be available at:
# - HTTP: http://localhost:5000
# - HTTPS: https://localhost:5001
# - Swagger: http://localhost:5000/swagger
```

#### 5. Start Redis (Optional)

**Option A: Docker**

```bash
docker run -p 6379:6379 --name jerp-redis -d redis:7
```

**Option B: Windows**

Download and install from: https://github.com/microsoftarchive/redis/releases

**Option C: macOS with Homebrew**

```bash
brew install redis
redis-server
```

#### 6. Start the Frontend

```bash
# Navigate to frontend folder
cd landing-page

# Install dependencies (first time only)
npm install

# Start the development server
npm run dev

# The frontend will be available at:
# - http://localhost:3000
```

### Environment-Specific Configuration

#### Development
- Uses SQL Server Express or Docker SQL Server
- Detailed logging enabled
- Swagger UI enabled
- Hot reload for both backend and frontend
- CORS allows localhost origins

#### Staging
- Uses Azure SQL Database
- Moderate logging
- Swagger UI enabled (restricted)
- Deployed via GitHub Actions to Azure App Service

#### Production
- Uses Azure SQL Database with geo-replication
- Minimal logging (errors and warnings only)
- Swagger UI disabled
- HTTPS only
- Restricted CORS origins
- Application Insights enabled

---

## Project Structure

### High-Level Directory Structure

```
JERP-3.0/
├── src/                           # Backend .NET solution
│   ├── JERP.Api/                  # Web API project (controllers, middleware)
│   ├── JERP.Application/          # Application layer (services, DTOs, business logic)
│   ├── JERP.Core/                 # Domain layer (entities, interfaces, enums)
│   ├── JERP.Infrastructure/       # Infrastructure layer (data access, external services)
│   ├── JERP.Compliance/           # Compliance module (state integrations)
│   └── JERP.Desktop/              # Desktop app (future)
│
├── landing-page/                  # Frontend Next.js application
│   ├── app/                       # Next.js app directory (pages, layouts)
│   ├── components/                # React components
│   ├── lib/                       # Utility functions and helpers
│   ├── public/                    # Static assets
│   ├── styles/                    # Global styles
│   └── types/                     # TypeScript type definitions
│
├── docs/                          # Documentation
│   ├── ARCHITECTURE.md            # System architecture
│   ├── SCOPE-OF-WORK.md           # Project scope and roadmap
│   ├── DEVELOPER-ONBOARDING.md    # This file
│   └── [other documentation]
│
├── docker-compose.yml             # Docker Compose configuration
├── Dockerfile                     # Backend API Dockerfile
├── .env.example                   # Environment variables template
├── README.md                      # Project overview
└── JERP.slnx                      # Visual Studio solution file
```

### Backend Structure (src/)

#### JERP.Core - Domain Layer

The core domain layer containing business entities, interfaces, and enums.

```
JERP.Core/
├── Entities/
│   ├── Finance/
│   │   ├── Account.cs             # Chart of accounts entity
│   │   ├── JournalEntry.cs        # Journal entry (header)
│   │   ├── GeneralLedgerEntry.cs  # GL entry (line item)
│   │   ├── Vendor.cs              # Vendor/supplier entity
│   │   └── Customer.cs            # Customer entity
│   │
│   ├── Payroll/
│   │   ├── Employee.cs            # Employee entity
│   │   ├── Timesheet.cs           # Timesheet entry
│   │   ├── PayrollPeriod.cs       # Pay period
│   │   └── PayrollRecord.cs       # Payroll calculation result
│   │
│   ├── Inventory/                 # (Future: Product, Batch, etc.)
│   ├── Identity/                  # (User, Role entities)
│   └── BaseEntity.cs              # Base entity with common properties
│
├── Enums/
│   ├── AccountType.cs             # Asset, Liability, Equity, Revenue, Expense
│   ├── AccountSubType.cs          # Detailed account classifications
│   ├── JournalEntryStatus.cs      # Draft, Posted, Voided
│   ├── EntrySource.cs             # Manual, Payroll, Invoice, etc.
│   └── [other enums]
│
├── Interfaces/
│   ├── IRepository.cs             # Generic repository interface
│   ├── IUnitOfWork.cs             # Unit of work interface
│   └── [domain service interfaces]
│
└── Exceptions/
    ├── DomainException.cs         # Base domain exception
    └── [specific exceptions]
```

**Key Concepts:**
- **Entities**: Core business objects with identity and lifecycle
- **Enums**: Type-safe enumerations for domain values
- **Interfaces**: Contracts for repositories and services (dependency inversion)
- **No Dependencies**: Core layer has no external dependencies (pure C#)

#### JERP.Application - Application Layer

Business logic, DTOs, and application services.

```
JERP.Application/
├── Services/
│   ├── Finance/
│   │   ├── IAccountService.cs              # Account service interface
│   │   ├── AccountService.cs               # Account service implementation
│   │   ├── IJournalEntryService.cs         # Journal entry service interface
│   │   ├── JournalEntryService.cs          # Journal entry service
│   │   ├── IFinancialReportService.cs      # Financial reporting interface
│   │   ├── FinancialReportService.cs       # P&L, Balance Sheet generation
│   │   ├── IPayrollToFinanceService.cs     # Payroll integration interface
│   │   └── PayrollToFinanceService.cs      # Payroll-to-GL posting
│   │
│   ├── Payroll/
│   │   ├── IEmployeeService.cs
│   │   ├── EmployeeService.cs
│   │   ├── IPayrollService.cs
│   │   └── PayrollService.cs
│   │
│   └── [other service folders]
│
├── DTOs/
│   ├── Finance/
│   │   ├── AccountDto.cs                   # Account data transfer objects
│   │   │   ├── AccountResponse
│   │   │   ├── CreateAccountRequest
│   │   │   └── UpdateAccountRequest
│   │   │
│   │   ├── JournalEntryDto.cs              # Journal entry DTOs
│   │   ├── GeneralLedgerEntryDto.cs        # GL entry DTOs
│   │   └── FinancialReportDto.cs           # Report DTOs
│   │
│   └── [other DTO folders]
│
├── Mappings/
│   ├── AutoMapperProfile.cs                # AutoMapper configuration
│   └── [specific mapping profiles]
│
├── Validators/
│   ├── Finance/
│   │   ├── CreateAccountValidator.cs       # FluentValidation validators
│   │   └── [other validators]
│   │
│   └── [other validator folders]
│
└── Common/
    ├── Interfaces/
    └── Exceptions/
```

**Key Concepts:**
- **Services**: Orchestrate business logic and coordinate between entities
- **DTOs**: Data transfer objects for API communication (decoupled from entities)
- **Mapping**: AutoMapper profiles for entity-DTO transformation
- **Validation**: FluentValidation for complex business rules
- **Dependencies**: References Core layer only (clean architecture)

#### JERP.Infrastructure - Infrastructure Layer

Data access, external services, and infrastructure concerns.

```
JERP.Infrastructure/
├── Data/
│   ├── JerpDbContext.cs                    # Main EF Core DbContext
│   │
│   ├── Configurations/
│   │   ├── Finance/
│   │   │   ├── AccountConfiguration.cs      # EF Core entity configuration
│   │   │   ├── JournalEntryConfiguration.cs
│   │   │   └── GeneralLedgerEntryConfiguration.cs
│   │   │
│   │   └── [other configuration folders]
│   │
│   ├── Migrations/
│   │   ├── 20260204075145_AddFinanceModule.cs
│   │   ├── 20260204075145_AddFinanceModule.Designer.cs
│   │   ├── JerpDbContextModelSnapshot.cs
│   │   └── [other migrations]
│   │
│   └── Seeders/
│       ├── ChartOfAccountsSeeder.cs         # Seed default chart of accounts
│       ├── UserSeeder.cs                    # Seed default users
│       └── [other seeders]
│
├── Repositories/
│   ├── GenericRepository.cs                 # Base repository implementation
│   ├── Finance/
│   │   ├── AccountRepository.cs
│   │   ├── JournalEntryRepository.cs
│   │   └── [other repositories]
│   │
│   └── [other repository folders]
│
├── Services/
│   ├── EmailService.cs                      # Email sending service
│   ├── BlobStorageService.cs                # Azure Blob Storage
│   ├── CacheService.cs                      # Redis caching
│   └── [other external services]
│
└── Identity/
    ├── ApplicationUser.cs                   # Custom user entity
    ├── ApplicationRole.cs                   # Custom role entity
    └── IdentityConfiguration.cs             # ASP.NET Core Identity config
```

**Key Concepts:**
- **DbContext**: EF Core database context managing entity sets
- **Configurations**: Fluent API configuration for entities (preferred over attributes)
- **Migrations**: Code-First database schema changes
- **Repositories**: Data access implementations with query logic
- **External Services**: Integration with third-party services
- **Dependencies**: References Core and Application layers

#### JERP.Api - API Layer

Web API controllers, middleware, and API configuration.

```
JERP.Api/
├── Controllers/
│   ├── v1/
│   │   ├── Finance/
│   │   │   ├── AccountsController.cs        # Chart of accounts API
│   │   │   ├── JournalEntriesController.cs  # Journal entries API
│   │   │   ├── GeneralLedgerController.cs   # GL query API
│   │   │   └── FinancialReportsController.cs # Financial reports API
│   │   │
│   │   ├── Payroll/
│   │   │   ├── EmployeesController.cs
│   │   │   ├── TimesheetsController.cs
│   │   │   └── PayrollController.cs
│   │   │
│   │   ├── AuthController.cs                # Authentication endpoints
│   │   └── [other controllers]
│   │
│   └── BaseApiController.cs                 # Base controller with common logic
│
├── Middleware/
│   ├── ErrorHandlingMiddleware.cs           # Global exception handling
│   ├── RequestLoggingMiddleware.cs          # Request/response logging
│   └── TenantMiddleware.cs                  # Multi-tenancy support
│
├── Filters/
│   ├── ValidateModelAttribute.cs            # Model validation filter
│   └── AuthorizeRolesAttribute.cs           # Custom authorization filter
│
├── Extensions/
│   ├── ServiceCollectionExtensions.cs       # DI container extensions
│   └── ApplicationBuilderExtensions.cs      # Middleware pipeline extensions
│
├── Program.cs                                # Application entry point
├── appsettings.json                          # Base configuration
├── appsettings.Development.json              # Development config
├── appsettings.Production.json               # Production config
└── Dockerfile                                # Docker container definition
```

**Key Concepts:**
- **Controllers**: Handle HTTP requests, call services, return responses
- **API Versioning**: Version 1 under /api/v1/
- **Middleware**: Request pipeline components (error handling, logging, auth)
- **Filters**: Action filters for cross-cutting concerns
- **Program.cs**: Application bootstrapping and configuration
- **Dependencies**: References all other backend layers

### Frontend Structure (landing-page/)

#### Next.js App Directory Structure

```
landing-page/
├── app/
│   ├── (auth)/                              # Auth layout group
│   │   ├── login/
│   │   │   └── page.tsx                     # Login page
│   │   └── register/
│   │       └── page.tsx                     # Register page
│   │
│   ├── (dashboard)/                         # Dashboard layout group
│   │   ├── layout.tsx                       # Dashboard layout
│   │   │
│   │   ├── dashboard/
│   │   │   └── page.tsx                     # Dashboard home
│   │   │
│   │   ├── finance/
│   │   │   ├── accounts/
│   │   │   │   ├── page.tsx                 # Chart of accounts list
│   │   │   │   └── [id]/
│   │   │   │       └── page.tsx             # Account details
│   │   │   │
│   │   │   ├── journal-entries/
│   │   │   │   ├── page.tsx                 # Journal entries list
│   │   │   │   └── [id]/
│   │   │   │       └── page.tsx             # Entry details
│   │   │   │
│   │   │   └── reports/
│   │   │       ├── page.tsx                 # Reports menu
│   │   │       ├── profit-loss/
│   │   │       └── balance-sheet/
│   │   │
│   │   ├── payroll/
│   │   │   ├── employees/
│   │   │   ├── timesheets/
│   │   │   └── process/
│   │   │
│   │   └── [other modules]
│   │
│   ├── layout.tsx                           # Root layout
│   ├── page.tsx                             # Home/landing page
│   └── globals.css                          # Global styles
│
├── components/
│   ├── ui/                                  # Reusable UI components (shadcn/ui)
│   │   ├── button.tsx
│   │   ├── input.tsx
│   │   ├── table.tsx
│   │   ├── dialog.tsx
│   │   └── [other UI components]
│   │
│   ├── finance/
│   │   ├── account-form.tsx                 # Account create/edit form
│   │   ├── journal-entry-form.tsx           # Journal entry form
│   │   ├── account-list.tsx                 # Accounts table
│   │   └── [other finance components]
│   │
│   ├── payroll/
│   │   ├── employee-form.tsx
│   │   ├── timesheet-entry.tsx
│   │   └── [other payroll components]
│   │
│   ├── layout/
│   │   ├── header.tsx                       # App header
│   │   ├── sidebar.tsx                      # Navigation sidebar
│   │   └── footer.tsx                       # App footer
│   │
│   └── common/
│       ├── loading-spinner.tsx              # Loading indicator
│       ├── error-message.tsx                # Error display
│       └── [other common components]
│
├── lib/
│   ├── api/
│   │   ├── client.ts                        # API client configuration (fetch/axios)
│   │   ├── finance/
│   │   │   ├── accounts.ts                  # Account API calls
│   │   │   └── journal-entries.ts           # Journal entry API calls
│   │   │
│   │   └── [other API modules]
│   │
│   ├── hooks/
│   │   ├── use-accounts.ts                  # Account data hooks (TanStack Query)
│   │   ├── use-auth.ts                      # Authentication hooks
│   │   └── [other custom hooks]
│   │
│   └── utils/
│       ├── formatting.ts                    # Number, date formatting
│       ├── validation.ts                    # Client-side validation
│       └── [other utilities]
│
├── types/
│   ├── finance.ts                           # Finance-related TypeScript types
│   ├── payroll.ts                           # Payroll-related types
│   ├── api.ts                               # API response types
│   └── [other type definitions]
│
├── public/
│   ├── images/
│   ├── icons/
│   └── [static assets]
│
├── styles/
│   └── globals.css                          # Global CSS and Tailwind imports
│
├── middleware.ts                            # Next.js middleware (auth, redirects)
├── next.config.js                           # Next.js configuration
├── tailwind.config.ts                       # Tailwind CSS configuration
├── tsconfig.json                            # TypeScript configuration
└── package.json                             # Dependencies and scripts
```

**Key Concepts:**
- **App Directory**: Next.js 13+ app router (file-based routing)
- **Layout Groups**: Shared layouts for related pages (auth, dashboard)
- **Components**: Reusable React components (functional with hooks)
- **API Client**: Centralized API communication logic
- **Hooks**: Custom React hooks for data fetching and state
- **Types**: TypeScript type definitions for type safety
- **shadcn/ui**: Component library built on Radix UI and Tailwind CSS

---

## Development Workflow

### Git Branching Strategy

We follow a simplified Git Flow:

- **`main`**: Production-ready code
- **`develop`**: Integration branch for features
- **`feature/*`**: Feature branches (e.g., `feature/ap-payment-processing`)
- **`bugfix/*`**: Bug fix branches
- **`hotfix/*`**: Production hotfixes

#### Creating a New Feature

```bash
# 1. Ensure you're on develop and up to date
git checkout develop
git pull origin develop

# 2. Create a new feature branch
git checkout -b feature/your-feature-name

# Example: Adding invoice generation
git checkout -b feature/ar-invoice-generation

# 3. Make your changes (see below for the workflow)

# 4. Commit your changes (see commit message guidelines)
git add .
git commit -m "feat(finance): add invoice generation"

# 5. Push your branch
git push -u origin feature/your-feature-name

# 6. Create a Pull Request on GitHub
# - Target branch: develop
# - Add description and screenshots
# - Request reviews from team members
```

### Development Process for a New Feature

#### Step 1: Define the Requirements
- Understand the user story and acceptance criteria
- Review designs and mockups (if applicable)
- Ask questions and clarify ambiguities

#### Step 2: Design the Solution
- Identify entities, DTOs, and services needed
- Plan database schema changes
- Design API endpoints
- Sketch UI components

#### Step 3: Backend Implementation

**A. Create/Update Entities (JERP.Core)**

```csharp
// Example: src/JERP.Core/Entities/Finance/Invoice.cs
namespace JERP.Core.Entities.Finance
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public List<InvoiceLineItem> LineItems { get; set; }
    }
}
```

**B. Create EF Core Configuration (JERP.Infrastructure)**

```csharp
// src/JERP.Infrastructure/Data/Configurations/InvoiceConfiguration.cs
public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(i => i.TotalAmount)
            .HasColumnType("decimal(18,2)");
        
        builder.HasOne(i => i.Customer)
            .WithMany()
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(i => i.LineItems)
            .WithOne(li => li.Invoice)
            .HasForeignKey(li => li.InvoiceId);
    }
}
```

**C. Add DbSet to DbContext**

```csharp
// src/JERP.Infrastructure/Data/JerpDbContext.cs
public class JerpDbContext : DbContext
{
    // ... existing DbSets
    
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
}
```

**D. Create Migration**

```bash
cd src/JERP.Infrastructure
dotnet ef migrations add AddInvoiceEntity --startup-project ../JERP.Api

# Review the migration file to ensure it's correct

# Apply the migration
dotnet ef database update --startup-project ../JERP.Api
```

**E. Create DTOs (JERP.Application)**

```csharp
// src/JERP.Application/DTOs/Finance/InvoiceDto.cs
namespace JERP.Application.DTOs.Finance
{
    public class InvoiceResponse
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
    
    public class CreateInvoiceRequest
    {
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid CustomerId { get; set; }
        public List<InvoiceLineItemRequest> LineItems { get; set; }
    }
    
    public class InvoiceLineItemRequest
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
    }
}
```

**F. Create Service (JERP.Application)**

```csharp
// src/JERP.Application/Services/Finance/IInvoiceService.cs
public interface IInvoiceService
{
    Task<InvoiceResponse> GetByIdAsync(Guid id);
    Task<List<InvoiceResponse>> GetAllAsync();
    Task<InvoiceResponse> CreateAsync(CreateInvoiceRequest request);
    Task<InvoiceResponse> UpdateAsync(Guid id, UpdateInvoiceRequest request);
    Task DeleteAsync(Guid id);
}

// src/JERP.Application/Services/Finance/InvoiceService.cs
public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public InvoiceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<InvoiceResponse> CreateAsync(CreateInvoiceRequest request)
    {
        // Validate customer exists
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
        if (customer == null)
            throw new NotFoundException("Customer not found");
        
        // Create invoice
        var invoice = new Invoice
        {
            InvoiceNumber = await GenerateInvoiceNumberAsync(),
            InvoiceDate = request.InvoiceDate,
            DueDate = request.DueDate,
            CustomerId = request.CustomerId,
            Status = InvoiceStatus.Draft
        };
        
        // Add line items and calculate totals
        decimal subTotal = 0;
        decimal taxTotal = 0;
        
        foreach (var lineRequest in request.LineItems)
        {
            var lineAmount = lineRequest.Quantity * lineRequest.UnitPrice;
            var lineTax = lineAmount * lineRequest.TaxRate;
            
            invoice.LineItems.Add(new InvoiceLineItem
            {
                Description = lineRequest.Description,
                Quantity = lineRequest.Quantity,
                UnitPrice = lineRequest.UnitPrice,
                TaxRate = lineRequest.TaxRate,
                Amount = lineAmount,
                TaxAmount = lineTax
            });
            
            subTotal += lineAmount;
            taxTotal += lineTax;
        }
        
        invoice.SubTotal = subTotal;
        invoice.TaxAmount = taxTotal;
        invoice.TotalAmount = subTotal + taxTotal;
        
        // Save to database
        await _unitOfWork.Invoices.AddAsync(invoice);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<InvoiceResponse>(invoice);
    }
    
    // ... other methods
}
```

**G. Create Controller (JERP.Api)**

```csharp
// src/JERP.Api/Controllers/v1/Finance/InvoicesController.cs
[ApiController]
[Route("api/v1/finance/[controller]")]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    
    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<InvoiceResponse>>> GetAll()
    {
        var invoices = await _invoiceService.GetAllAsync();
        return Ok(invoices);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceResponse>> GetById(Guid id)
    {
        var invoice = await _invoiceService.GetByIdAsync(id);
        if (invoice == null)
            return NotFound();
        
        return Ok(invoice);
    }
    
    [HttpPost]
    public async Task<ActionResult<InvoiceResponse>> Create(CreateInvoiceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var invoice = await _invoiceService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
    }
    
    // ... other endpoints
}
```

**H. Register Services (JERP.Api/Program.cs)**

```csharp
// Add to Program.cs
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
```

#### Step 4: Frontend Implementation

**A. Define TypeScript Types**

```typescript
// landing-page/types/finance.ts
export interface Invoice {
  id: string;
  invoiceNumber: string;
  invoiceDate: string;
  dueDate: string;
  customerName: string;
  totalAmount: number;
  status: string;
}

export interface CreateInvoiceRequest {
  invoiceDate: string;
  dueDate: string;
  customerId: string;
  lineItems: InvoiceLineItem[];
}

export interface InvoiceLineItem {
  description: string;
  quantity: number;
  unitPrice: number;
  taxRate: number;
}
```

**B. Create API Client Functions**

```typescript
// landing-page/lib/api/finance/invoices.ts
import { apiClient } from '../client';
import { Invoice, CreateInvoiceRequest } from '@/types/finance';

export const invoiceApi = {
  getAll: async (): Promise<Invoice[]> => {
    const response = await apiClient.get('/api/v1/finance/invoices');
    return response.data;
  },
  
  getById: async (id: string): Promise<Invoice> => {
    const response = await apiClient.get(`/api/v1/finance/invoices/${id}`);
    return response.data;
  },
  
  create: async (data: CreateInvoiceRequest): Promise<Invoice> => {
    const response = await apiClient.post('/api/v1/finance/invoices', data);
    return response.data;
  },
  
  delete: async (id: string): Promise<void> => {
    await apiClient.delete(`/api/v1/finance/invoices/${id}`);
  },
};
```

**C. Create Custom Hook (Optional, using TanStack Query)**

```typescript
// landing-page/lib/hooks/use-invoices.ts
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { invoiceApi } from '@/lib/api/finance/invoices';
import { CreateInvoiceRequest } from '@/types/finance';

export function useInvoices() {
  return useQuery({
    queryKey: ['invoices'],
    queryFn: invoiceApi.getAll,
  });
}

export function useInvoice(id: string) {
  return useQuery({
    queryKey: ['invoices', id],
    queryFn: () => invoiceApi.getById(id),
    enabled: !!id,
  });
}

export function useCreateInvoice() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (data: CreateInvoiceRequest) => invoiceApi.create(data),
    onSuccess: () => {
      // Invalidate and refetch invoices list
      queryClient.invalidateQueries({ queryKey: ['invoices'] });
    },
  });
}
```

**D. Create React Component**

```typescript
// landing-page/components/finance/invoice-list.tsx
'use client';

import { useInvoices } from '@/lib/hooks/use-invoices';
import { Button } from '@/components/ui/button';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { formatCurrency, formatDate } from '@/lib/utils/formatting';

export function InvoiceList() {
  const { data: invoices, isLoading, error } = useInvoices();
  
  if (isLoading) {
    return <div>Loading invoices...</div>;
  }
  
  if (error) {
    return <div>Error loading invoices: {error.message}</div>;
  }
  
  return (
    <div>
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-2xl font-bold">Invoices</h2>
        <Button>Create Invoice</Button>
      </div>
      
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Invoice #</TableHead>
            <TableHead>Customer</TableHead>
            <TableHead>Date</TableHead>
            <TableHead>Due Date</TableHead>
            <TableHead>Amount</TableHead>
            <TableHead>Status</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {invoices?.map((invoice) => (
            <TableRow key={invoice.id}>
              <TableCell>{invoice.invoiceNumber}</TableCell>
              <TableCell>{invoice.customerName}</TableCell>
              <TableCell>{formatDate(invoice.invoiceDate)}</TableCell>
              <TableCell>{formatDate(invoice.dueDate)}</TableCell>
              <TableCell>{formatCurrency(invoice.totalAmount)}</TableCell>
              <TableCell>{invoice.status}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
```

**E. Create Page Component**

```typescript
// landing-page/app/(dashboard)/finance/invoices/page.tsx
import { InvoiceList } from '@/components/finance/invoice-list';

export default function InvoicesPage() {
  return (
    <div className="container mx-auto py-6">
      <InvoiceList />
    </div>
  );
}
```

#### Step 5: Testing

See the [Testing Strategy](#testing-strategy) section below for comprehensive testing approaches.

#### Step 6: Documentation

- Update API documentation if needed
- Add code comments for complex logic
- Update this guide if you added new patterns

#### Step 7: Code Review

- Create a pull request
- Address review comments
- Ensure CI/CD pipeline passes
- Get approval from at least one team member

#### Step 8: Merge and Deploy

- Merge to `develop` branch
- Verify deployment to staging environment
- After testing, merge `develop` to `main` for production

### Commit Message Guidelines

We follow [Conventional Commits](https://www.conventionalcommits.org/):

**Format:**
```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, no logic change)
- `refactor`: Code refactoring
- `perf`: Performance improvements
- `test`: Adding or updating tests
- `chore`: Maintenance tasks (dependencies, build scripts)
- `ci`: CI/CD changes

**Examples:**
```bash
# Feature
git commit -m "feat(finance): add invoice generation"

# Bug fix
git commit -m "fix(payroll): correct overtime calculation"

# Documentation
git commit -m "docs(onboarding): update setup instructions"

# Multi-line with body
git commit -m "feat(compliance): integrate Metrc API

- Add Metrc client service
- Implement package tracking
- Add transfer manifest generation

Closes #123"
```

### Code Review Process

**For Authors:**
1. Ensure your code builds and tests pass
2. Write clear PR description with context
3. Add screenshots for UI changes
4. Link related issues
5. Self-review your code before requesting review
6. Address all review comments
7. Keep PRs small and focused (< 500 lines if possible)

**For Reviewers:**
1. Review within 24 hours
2. Test the changes locally if significant
3. Check for:
   - Code quality and readability
   - Test coverage
   - Security vulnerabilities
   - Performance issues
   - Adherence to coding standards
4. Provide constructive feedback
5. Approve when satisfied

---

## Testing Strategy

### Backend Testing

#### Unit Tests

Unit tests test individual methods in isolation using mocking.

**Example: Testing a Service Method**

```csharp
// src/JERP.Tests/Unit/Finance/AccountServiceTests.cs
using Xunit;
using Moq;
using FluentAssertions;
using JERP.Application.Services.Finance;
using JERP.Core.Entities.Finance;
using JERP.Core.Interfaces;

public class AccountServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly AccountService _sut; // System Under Test
    
    public AccountServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAccountRepository = new Mock<IAccountRepository>();
        _mockUnitOfWork.Setup(u => u.Accounts).Returns(_mockAccountRepository.Object);
        
        _sut = new AccountService(_mockUnitOfWork.Object);
    }
    
    [Fact]
    public async Task CreateAccount_WithValidData_ReturnsCreatedAccount()
    {
        // Arrange
        var request = new CreateAccountRequest
        {
            AccountNumber = "1000",
            AccountName = "Cash",
            Type = AccountType.Asset,
            SubType = AccountSubType.CurrentAsset
        };
        
        _mockAccountRepository
            .Setup(r => r.AddAsync(It.IsAny<Account>()))
            .ReturnsAsync((Account a) => a);
        
        // Act
        var result = await _sut.CreateAsync(request);
        
        // Assert
        result.Should().NotBeNull();
        result.AccountNumber.Should().Be("1000");
        result.AccountName.Should().Be("Cash");
        
        _mockAccountRepository.Verify(
            r => r.AddAsync(It.Is<Account>(a => a.AccountNumber == "1000")),
            Times.Once
        );
    }
    
    [Fact]
    public async Task CreateAccount_WithDuplicateNumber_ThrowsException()
    {
        // Arrange
        var request = new CreateAccountRequest
        {
            AccountNumber = "1000",
            AccountName = "Cash",
            Type = AccountType.Asset
        };
        
        _mockAccountRepository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Account, bool>>>()))
            .ReturnsAsync(true);
        
        // Act & Assert
        await Assert.ThrowsAsync<BusinessException>(
            () => _sut.CreateAsync(request)
        );
    }
}
```

**Running Unit Tests:**

```bash
# Run all tests
dotnet test

# Run tests in specific project
dotnet test src/JERP.Tests/JERP.Tests.csproj

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run specific test
dotnet test --filter "FullyQualifiedName~AccountServiceTests.CreateAccount_WithValidData"
```

#### Integration Tests

Integration tests test API endpoints with a real database (usually in-memory).

**Example: Testing an API Endpoint**

```csharp
// src/JERP.Tests/Integration/Finance/AccountsControllerTests.cs
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using FluentAssertions;

public class AccountsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public AccountsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAccounts_ReturnsSuccessWithAccounts()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/finance/accounts");
        
        // Assert
        response.Should().BeSuccessful();
        
        var accounts = await response.Content.ReadFromJsonAsync<List<AccountResponse>>();
        accounts.Should().NotBeNull();
        accounts.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task CreateAccount_WithValidData_ReturnsCreated()
    {
        // Arrange
        var request = new CreateAccountRequest
        {
            AccountNumber = "9999",
            AccountName = "Test Account",
            Type = AccountType.Asset
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/finance/accounts", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var account = await response.Content.ReadFromJsonAsync<AccountResponse>();
        account.Should().NotBeNull();
        account.AccountNumber.Should().Be("9999");
    }
}
```

### Frontend Testing

#### Component Tests

Test React components in isolation.

**Example: Testing a Component**

```typescript
// landing-page/__tests__/components/finance/account-list.test.tsx
import { render, screen, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AccountList } from '@/components/finance/account-list';
import { invoiceApi } from '@/lib/api/finance/invoices';

// Mock the API
jest.mock('@/lib/api/finance/invoices');

describe('AccountList', () => {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: { retry: false },
    },
  });
  
  const wrapper = ({ children }: { children: React.ReactNode }) => (
    <QueryClientProvider client={queryClient}>
      {children}
    </QueryClientProvider>
  );
  
  it('renders accounts list', async () => {
    // Arrange
    const mockAccounts = [
      {
        id: '1',
        accountNumber: '1000',
        accountName: 'Cash',
        type: 'Asset',
        balance: 10000,
      },
    ];
    
    (invoiceApi.getAll as jest.Mock).mockResolvedValue(mockAccounts);
    
    // Act
    render(<AccountList />, { wrapper });
    
    // Assert
    await waitFor(() => {
      expect(screen.getByText('Cash')).toBeInTheDocument();
      expect(screen.getByText('1000')).toBeInTheDocument();
    });
  });
  
  it('displays loading state', () => {
    // Arrange
    (invoiceApi.getAll as jest.Mock).mockImplementation(
      () => new Promise(() => {}) // Never resolves
    );
    
    // Act
    render(<AccountList />, { wrapper });
    
    // Assert
    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });
});
```

**Running Frontend Tests:**

```bash
cd landing-page

# Run all tests
npm test

# Run tests in watch mode
npm test -- --watch

# Run tests with coverage
npm test -- --coverage

# Run specific test file
npm test -- account-list.test.tsx
```

#### E2E Tests with Playwright

Test complete user workflows.

**Example: E2E Test**

```typescript
// landing-page/e2e/finance/accounts.spec.ts
import { test, expect } from '@playwright/test';

test.describe('Chart of Accounts', () => {
  test.beforeEach(async ({ page }) => {
    // Login
    await page.goto('http://localhost:3000/login');
    await page.fill('input[name="username"]', 'admin');
    await page.fill('input[name="password"]', 'Admin@123');
    await page.click('button[type="submit"]');
    
    // Navigate to accounts
    await page.goto('http://localhost:3000/finance/accounts');
  });
  
  test('can view chart of accounts', async ({ page }) => {
    // Wait for accounts to load
    await expect(page.locator('h1')).toContainText('Chart of Accounts');
    
    // Verify table is visible
    await expect(page.locator('table')).toBeVisible();
    
    // Verify some accounts are displayed
    await expect(page.locator('td:has-text("Cash")')).toBeVisible();
  });
  
  test('can create new account', async ({ page }) => {
    // Click create button
    await page.click('button:has-text("Create Account")');
    
    // Fill form
    await page.fill('input[name="accountNumber"]', '9999');
    await page.fill('input[name="accountName"]', 'Test Account');
    await page.selectOption('select[name="type"]', 'Asset');
    
    // Submit
    await page.click('button[type="submit"]');
    
    // Verify success
    await expect(page.locator('text=Account created successfully')).toBeVisible();
    
    // Verify account in list
    await expect(page.locator('td:has-text("9999")')).toBeVisible();
  });
});
```

**Running E2E Tests:**

```bash
cd landing-page

# Install Playwright browsers (first time only)
npx playwright install

# Run E2E tests
npm run test:e2e

# Run E2E tests in UI mode (interactive)
npm run test:e2e:ui

# Run specific test
npx playwright test accounts.spec.ts
```

### Test Coverage Goals

- **Backend**: 80%+ code coverage
- **Frontend**: 70%+ code coverage
- **Critical Paths**: 100% coverage (authentication, financial calculations, compliance)

---

## Common Tasks and Recipes

### Task 1: Adding a New Entity

See the [Development Workflow](#development-workflow) section above for a complete example.

### Task 2: Creating a New API Endpoint

1. Create or update the controller
2. Define request/response DTOs
3. Implement service method
4. Add integration test
5. Update API documentation

### Task 3: Adding a React Component with API Integration

See the [Frontend Implementation](#step-4-frontend-implementation) section for a complete example.

### Task 4: Database Seeding

```csharp
// src/JERP.Infrastructure/Data/Seeders/YourDataSeeder.cs
public class YourDataSeeder
{
    public static async Task SeedAsync(JerpDbContext context)
    {
        // Check if data already exists
        if (await context.YourEntities.AnyAsync())
        {
            return; // Data already seeded
        }
        
        var items = new List<YourEntity>
        {
            new YourEntity { Name = "Item 1", Description = "..." },
            new YourEntity { Name = "Item 2", Description = "..." },
        };
        
        await context.YourEntities.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }
}

// Call in Program.cs (JERP.Api)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<JerpDbContext>();
    await context.Database.MigrateAsync(); // Apply migrations
    await YourDataSeeder.SeedAsync(context);
}
```

### Task 5: Debugging Tips

**Backend Debugging:**
- Use Visual Studio or Rider debugger with breakpoints
- Use `dotnet watch run` for hot reload
- Check Swagger UI for API testing: `http://localhost:5000/swagger`
- Use logging: `_logger.LogInformation("Message: {value}", someValue);`
- Inspect database with SSMS or Azure Data Studio

**Frontend Debugging:**
- Use React DevTools browser extension
- Use browser DevTools console and network tab
- Add `console.log()` statements (remove before committing)
- Use debugger in VS Code with launch configuration
- Check API responses in Network tab

**Common Issues:**
- **Database connection error**: Check connection string, ensure SQL Server is running
- **Migration error**: Delete migration, fix entity, recreate migration
- **CORS error**: Ensure API CORS policy includes frontend origin
- **Authentication error**: Check JWT token in request headers, verify token not expired
- **404 Not Found**: Check API route, ensure controller is registered

### Task 6: Working with Migrations

**Create a new migration:**

```bash
cd src/JERP.Infrastructure
dotnet ef migrations add YourMigrationName --startup-project ../JERP.Api
```

**Apply migrations to database:**

```bash
dotnet ef database update --startup-project ../JERP.Api
```

**Rollback to a previous migration:**

```bash
dotnet ef database update PreviousMigrationName --startup-project ../JERP.Api
```

**Remove the last migration (if not applied):**

```bash
dotnet ef migrations remove --startup-project ../JERP.Api
```

**Generate SQL script from migrations:**

```bash
dotnet ef migrations script --startup-project ../JERP.Api -o migration.sql
```

### Task 7: Running the Application

**Backend:**

```bash
# Development with hot reload
cd src/JERP.Api
dotnet watch run

# Production build
dotnet build -c Release
dotnet run -c Release

# Run in Docker
docker-compose up api
```

**Frontend:**

```bash
# Development server
cd landing-page
npm run dev

# Production build
npm run build
npm start

# Run in Docker
docker-compose up frontend
```

### Task 8: Dependency Management

**Backend (.NET):**

```bash
# Add a NuGet package
dotnet add package PackageName

# Update a package
dotnet add package PackageName --version x.x.x

# Remove a package
dotnet remove package PackageName

# Restore packages
dotnet restore
```

**Frontend (npm):**

```bash
# Add a package
npm install package-name

# Add a dev dependency
npm install --save-dev package-name

# Update a package
npm update package-name

# Remove a package
npm uninstall package-name

# Install all dependencies
npm install
```

---

## Troubleshooting

### Common Issues and Solutions

#### Issue: "Could not execute because the application was not found"

**Solution:** Ensure .NET 8 SDK is installed and in PATH.

```bash
dotnet --version
# Should output 8.0.x or later
```

#### Issue: "Login failed for user" or database connection errors

**Solution:**
1. Verify SQL Server is running
2. Check connection string in appsettings.Development.json or .env
3. Ensure database exists: `CREATE DATABASE JERP3_DB;`
4. Verify SQL Server is accepting connections (TCP/IP enabled)

#### Issue: "Unable to resolve service" or dependency injection errors

**Solution:**
1. Ensure service is registered in Program.cs: `builder.Services.AddScoped<IYourService, YourService>();`
2. Check interface and implementation are in correct projects
3. Verify project references are correct

#### Issue: CORS errors in frontend

**Solution:**
1. Check CORS policy in Program.cs includes frontend origin
2. Verify frontend is using correct API URL
3. Ensure credentials are included if needed: `credentials: 'include'`

#### Issue: Migration fails or database schema out of sync

**Solution:**
1. Drop database and recreate: `DROP DATABASE JERP3_DB; CREATE DATABASE JERP3_DB;`
2. Delete all migration files
3. Recreate initial migration: `dotnet ef migrations add InitialCreate`
4. Apply migration: `dotnet ef database update`

#### Issue: "npm ERR! network" or frontend dependency install fails

**Solution:**
1. Clear npm cache: `npm cache clean --force`
2. Delete node_modules and package-lock.json: `rm -rf node_modules package-lock.json`
3. Reinstall: `npm install`
4. If behind corporate proxy, configure npm proxy settings

#### Issue: Docker containers won't start

**Solution:**
1. Check Docker Desktop is running
2. Verify ports are not in use: `netstat -ano | findstr :5000` (Windows) or `lsof -i :5000` (Mac/Linux)
3. Check Docker logs: `docker-compose logs`
4. Rebuild containers: `docker-compose build --no-cache`
5. Remove old containers and volumes: `docker-compose down -v`

---

## Getting Help

### Resources

- **Project Documentation**: `/docs` directory
- **API Documentation**: Swagger UI at `http://localhost:5000/swagger`
- **Code Examples**: Existing implementations in the codebase
- **Team Chat**: Slack #jerp-dev channel
- **Issue Tracker**: GitHub Issues

### Team Contacts

- **Technical Lead**: [Name] - [email]
- **Backend Lead**: [Name] - [email]
- **Frontend Lead**: [Name] - [email]
- **DevOps**: [Name] - [email]
- **Product Owner**: [Name] - [email]

### Code Review

- Tag reviewers in your PR
- Expected response time: within 24 hours
- For urgent reviews, ping in Slack

### Onboarding Buddy

New developers will be assigned an onboarding buddy who can help with:
- Environment setup
- Codebase walkthrough
- Code review process
- Team practices and culture

---

## Next Steps

Now that you've completed the setup:

1. ✅ **Explore the Codebase**: Browse through existing code to understand patterns
2. ✅ **Run the Application**: Start both backend and frontend, test basic functionality
3. ✅ **Review Architecture**: Read [ARCHITECTURE.md](./ARCHITECTURE.md) for system overview
4. ✅ **Understand Scope**: Review [SCOPE-OF-WORK.md](./SCOPE-OF-WORK.md) for feature roadmap
5. ✅ **Pick a Starter Task**: Ask your team lead for a good first issue
6. ✅ **Join Team Meetings**: Attend daily standup and sprint planning
7. ✅ **Ask Questions**: Don't hesitate to ask for help!

Welcome to the team! 🎉

---

## Related Documentation

- [ARCHITECTURE.md](./ARCHITECTURE.md) - System architecture and technology stack
- [SCOPE-OF-WORK.md](./SCOPE-OF-WORK.md) - Project scope and roadmap
- [API-DOCUMENTATION.md](../API-DOCUMENTATION.md) - API endpoint documentation
- [TESTING-GUIDE.md](../TESTING-GUIDE.md) - Comprehensive testing guide
- [DOCKER-DEPLOYMENT.md](../DOCKER-DEPLOYMENT.md) - Docker deployment instructions
- [FINANCE-MODULE-IMPLEMENTATION.md](../FINANCE-MODULE-IMPLEMENTATION.md) - Finance module details
- [INVENTORY-MODULE-IMPLEMENTATION.md](../INVENTORY-MODULE-IMPLEMENTATION.md) - Inventory module planning

---

**Document Version:** 1.0  
**Last Updated:** February 5, 2026  
**Maintained By:** JERP Development Team  
**Feedback:** Please suggest improvements via GitHub Issues or Pull Requests
