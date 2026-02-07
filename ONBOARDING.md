# JERP 3.0 - Developer Onboarding Guide

Welcome to the JERP 3.0 development team! This guide will help you get set up and productive quickly.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Setup Instructions](#setup-instructions)
3. [Project Structure](#project-structure)
4. [Development Workflow](#development-workflow)
5. [Testing Strategy](#testing-strategy)
6. [Common Tasks and Recipes](#common-tasks-and-recipes)
7. [Troubleshooting](#troubleshooting)
8. [Resources](#resources)

---

## Prerequisites

Before you begin, ensure you have the following installed on your development machine:

### Required Software

#### 1. Git
- **Version**: 2.30+
- **Download**: https://git-scm.com/downloads
- **Verify**: `git --version`

#### 2. Docker Desktop (Recommended for Quick Start)
- **Version**: 4.0+
- **Download**: 
  - Windows/Mac: https://www.docker.com/products/docker-desktop
  - Linux: Install Docker Engine + Docker Compose
- **Verify**: 
  ```bash
  docker --version
  docker-compose --version
  ```
- **Requirements**: 
  - 8GB+ RAM allocated to Docker
  - 50GB+ disk space

#### 3. .NET SDK (For Backend Development)
- **Version**: 8.0 LTS
- **Download**: https://dotnet.microsoft.com/download/dotnet/8.0
- **Verify**: `dotnet --version`
- **Note**: Should output 8.0.x

#### 4. Node.js and npm (For Frontend Development)
- **Version**: Node.js 18.x or 20.x LTS
- **Download**: https://nodejs.org/
- **Verify**: 
  ```bash
  node --version
  npm --version
  ```

#### 5. SQL Server (For Local Development)
- **Options**:
  - **SQL Server Express 2022** (Free): https://www.microsoft.com/en-us/sql-server/sql-server-downloads
  - **SQL Server Developer Edition** (Free): Same link as above
  - **Docker SQL Server** (Easiest): Included in docker-compose.yml
- **Management Tools**:
  - SQL Server Management Studio (SSMS): https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
  - Azure Data Studio: https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio
  - VS Code with SQL Server extension

### Recommended Software

#### IDEs and Editors
- **Visual Studio 2022** (Windows): https://visualstudio.microsoft.com/
  - Workloads: ASP.NET and web development, .NET desktop development
- **Visual Studio Code**: https://code.visualstudio.com/
  - Extensions: C#, C# Dev Kit, ESLint, Prettier, Docker, GitLens
- **JetBrains Rider** (Alternative): https://www.jetbrains.com/rider/

#### API Testing Tools
- **Postman**: https://www.postman.com/downloads/
- **Insomnia**: https://insomnia.rest/download

#### Database Tools
- **SQL Server Management Studio (SSMS)**: For Windows users
- **Azure Data Studio**: Cross-platform SQL tool
- **DBeaver**: Universal database tool

### System Requirements
- **OS**: Windows 10/11, macOS 11+, or Linux (Ubuntu 20.04+)
- **RAM**: 16GB minimum, 32GB recommended
- **Storage**: 20GB free space
- **CPU**: Multi-core processor (4+ cores recommended)

---

## Setup Instructions

### Option 1: Quick Start with Docker (Recommended)

This is the fastest way to get JERP 3.0 running with all dependencies.

#### Step 1: Clone the Repository
```bash
git clone https://github.com/ninoyerbas/JERP-3.0.git
cd JERP-3.0
```

#### Step 2: Configure Environment Variables
```bash
# Copy the example environment file
cp .env.example .env

# Edit .env file with your settings (use any text editor)
# On Windows: notepad .env
# On macOS/Linux: nano .env or vim .env
```

**Key Environment Variables:**
```bash
# Database
SQLSERVER_PASSWORD=YourStrong!Passw0rd

# JWT Authentication
JWT_SECRET_KEY=YourSuperSecretKeyForJWTTokenGenerationMinimum32Characters!
JWT_ISSUER=JERP-API
JWT_AUDIENCE=JERP-Client
JWT_EXPIRATION_HOURS=1

# CORS (Add localhost for development)
CORS_ORIGINS=http://localhost:3000,http://localhost:3001

# Logging
SERILOG_MINIMUM_LEVEL=Information
```

#### Step 3: Start All Services with Docker
```bash
# Build and start all containers (API + SQL Server)
docker-compose up --build

# Or run in detached mode (background)
docker-compose up -d --build
```

**Services Started:**
- **SQL Server**: http://localhost:1433
- **JERP API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger

#### Step 4: Run Database Migrations
```bash
# Wait for SQL Server to be ready (30-60 seconds on first run)
# Then run migrations
docker-compose exec api dotnet ef database update --project /app/JERP.Infrastructure --startup-project /app/JERP.Api

# Or if API container isn't running migrations automatically, you can run them locally:
cd src/JERP.Api
dotnet ef database update --project ../JERP.Infrastructure
```

#### Step 5: Seed Initial Data (Optional)
```bash
# Seed sample data (accounts, users, sample transactions)
docker-compose exec api dotnet run --project /app/JERP.Api -- seed

# Or manually via API once it's running (see API documentation)
```

#### Step 6: Start Frontend (Separate Terminal)
The frontend runs outside Docker for faster development with hot reload.

```bash
# In a new terminal, navigate to frontend directory
cd landing-page

# Install dependencies
npm install

# Start development server
npm run dev
```

**Frontend URL**: http://localhost:3000

#### Step 7: Verify Installation
1. Open browser to http://localhost:3000
2. You should see the JERP 3.0 login page
3. Default credentials:
   - **Email**: ichbincesartobar@yahoo.com
   - **Password**: Admin@123
4. After login, you should see the dashboard

**API Documentation**: http://localhost:5000/swagger

---

### Option 2: Manual Setup (Full Control)

For developers who prefer to run services individually or don't use Docker.

#### Backend Setup

##### 1. Install SQL Server Locally
- Download and install SQL Server Express or Developer Edition
- Note the connection string (usually `Server=localhost\SQLEXPRESS;...`)

##### 2. Configure Database Connection
Edit `src/JERP.Api/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=JERP3_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJWTTokenGenerationMinimum32Characters!",
    "Issuer": "JERP-API",
    "Audience": "JERP-Client",
    "ExpirationHours": 1
  }
}
```

##### 3. Restore Dependencies
```bash
cd /path/to/JERP-3.0
dotnet restore
```

##### 4. Create Database
```bash
# Option A: Create empty database via SQL
# Connect to SQL Server and run:
CREATE DATABASE JERP3_DB;

# Option B: Let migrations create it (skip to next step)
```

##### 5. Run Migrations
```bash
cd src/JERP.Api
dotnet ef database update --project ../JERP.Infrastructure
```

**Troubleshooting Migrations:**
```bash
# If you get "No executable found matching command dotnet-ef"
dotnet tool install --global dotnet-ef

# Then try again
dotnet ef database update --project ../JERP.Infrastructure
```

##### 6. Run the API
```bash
cd src/JERP.Api
dotnet run
```

**API will start on**: http://localhost:5000  
**Swagger UI**: http://localhost:5000/swagger

#### Frontend Setup

##### 1. Navigate to Frontend Directory
```bash
cd /path/to/JERP-3.0/landing-page
```

##### 2. Install Dependencies
```bash
npm install
# or
yarn install
```

##### 3. Configure Environment (if needed)
Create `landing-page/.env.local`:
```bash
NEXT_PUBLIC_API_URL=http://localhost:5000
NEXTAUTH_SECRET=your-nextauth-secret-here
NEXTAUTH_URL=http://localhost:3000
```

##### 4. Run Development Server
```bash
npm run dev
# or
yarn dev
```

**Frontend URL**: http://localhost:3000

---

### Option 3: Using Visual Studio (Windows)

#### 1. Open Solution
- Launch Visual Studio 2022
- Open `JERP.slnx` or `src/JERP.Api/JERP.Api.csproj`

#### 2. Configure Startup Projects
- Right-click solution → Properties
- Set startup projects:
  - `JERP.Api` (Always start)
  - Optional: Set multiple startup projects if you have additional services

#### 3. Configure Database
- Update connection string in `appsettings.Development.json`
- Open Package Manager Console (View → Other Windows → Package Manager Console)
- Run: `Update-Database -Project JERP.Infrastructure -StartupProject JERP.Api`

#### 4. Run
- Press F5 or click "Start Debugging"
- API will launch with Swagger UI

#### 5. Run Frontend Separately
- Open terminal in Visual Studio or external terminal
- Navigate to `landing-page`
- Run `npm install` and `npm run dev`

---

## Project Structure

### High-Level Overview

```
JERP-3.0/
├── src/                          # Source code
│   ├── JERP.Api/                 # ASP.NET Core Web API
│   ├── JERP.Core/                # Domain models and interfaces
│   ├── JERP.Application/         # Business logic layer
│   ├── JERP.Infrastructure/      # Data access & external services
│   ├── JERP.Compliance/          # Cannabis compliance features
│   └── JERP.Desktop/             # Desktop application (optional)
├── landing-page/                 # Next.js frontend
├── tests/                        # Test projects (planned)
├── docs/                         # Documentation
├── docker-compose.yml            # Docker orchestration
└── README.md                     # Project overview
```

### Backend Structure (ASP.NET Core)

#### JERP.Api - Web API Layer
```
JERP.Api/
├── Controllers/                  # API endpoints
│   ├── AccountsController.cs    # Chart of Accounts CRUD
│   ├── JournalEntriesController.cs
│   ├── GeneralLedgerController.cs
│   ├── EmployeesController.cs
│   ├── PayrollController.cs
│   ├── VendorsController.cs
│   ├── CustomersController.cs
│   ├── ProductsController.cs
│   ├── InventoryController.cs
│   └── FinancialReportsController.cs
├── Middleware/                   # Custom middleware
│   ├── ErrorHandlingMiddleware.cs
│   ├── AuthenticationMiddleware.cs
│   └── TenantResolutionMiddleware.cs
├── Program.cs                    # Application entry point
├── appsettings.json              # Configuration
├── appsettings.Development.json  # Dev-specific config
└── Dockerfile                    # Docker image definition
```

**Key Files:**
- **Program.cs**: Configures services (DI), middleware pipeline, authentication, CORS
- **Controllers**: REST API endpoints, route definitions, request/response handling

#### JERP.Core - Domain Layer
```
JERP.Core/
├── Entities/                     # Domain entities
│   ├── BaseEntity.cs             # Base class (Id, CreatedAt, UpdatedAt)
│   ├── Finance/                  # Finance entities
│   │   ├── Account.cs
│   │   ├── JournalEntry.cs
│   │   ├── GeneralLedgerEntry.cs
│   │   ├── Vendor.cs
│   │   ├── VendorBill.cs
│   │   ├── Customer.cs
│   │   ├── CustomerInvoice.cs
│   │   ├── FASBTopic.cs
│   │   └── FASBSubtopic.cs
│   ├── Inventory/                # Inventory entities
│   │   ├── Product.cs
│   │   ├── ProductBatch.cs
│   │   ├── Warehouse.cs
│   │   ├── InventoryLevel.cs
│   │   ├── InventoryTransaction.cs
│   │   ├── PurchaseOrder.cs
│   │   └── StockReceipt.cs
│   ├── SalesOrders/              # Sales entities
│   ├── Employee.cs
│   ├── Timesheet.cs
│   ├── PayrollRecord.cs
│   ├── Department.cs
│   ├── User.cs
│   ├── Role.cs
│   └── Company.cs
├── Enums/                        # Enumerations
│   ├── AccountType.cs
│   ├── AccountSubType.cs
│   ├── JournalEntryStatus.cs
│   ├── EmploymentStatus.cs
│   └── PaymentStatus.cs
└── Interfaces/                   # Repository interfaces
    ├── IRepository.cs
    ├── IUnitOfWork.cs
    └── IFinanceService.cs
```

**Entity Design Principles:**
- All entities inherit from `BaseEntity` (Id, CreatedAt, UpdatedAt, CompanyId)
- Navigation properties for relationships
- Validation attributes for data integrity
- Immutable properties where appropriate

#### JERP.Application - Business Logic Layer
```
JERP.Application/
├── DTOs/                         # Data Transfer Objects
│   ├── Finance/
│   │   ├── AccountDto.cs
│   │   ├── CreateAccountDto.cs
│   │   ├── UpdateAccountDto.cs
│   │   ├── JournalEntryDto.cs
│   │   ├── CreateJournalEntryDto.cs
│   │   └── FinancialReportDto.cs
│   ├── Inventory/
│   └── Payroll/
├── Services/                     # Business services
│   ├── Finance/
│   │   ├── IAccountService.cs
│   │   ├── AccountService.cs
│   │   ├── IJournalEntryService.cs
│   │   ├── JournalEntryService.cs
│   │   ├── IFinancialReportService.cs
│   │   ├── FinancialReportService.cs
│   │   ├── IPayrollToFinanceService.cs
│   │   └── PayrollToFinanceService.cs
│   ├── Inventory/
│   ├── Payroll/
│   └── Auth/
│       ├── IAuthService.cs
│       └── AuthService.cs
├── Validators/                   # FluentValidation validators
│   ├── CreateAccountValidator.cs
│   ├── CreateJournalEntryValidator.cs
│   └── CreateEmployeeValidator.cs
├── Mappings/                     # AutoMapper profiles
│   ├── FinanceMappingProfile.cs
│   ├── InventoryMappingProfile.cs
│   └── PayrollMappingProfile.cs
└── Exceptions/                   # Custom exceptions
    ├── NotFoundException.cs
    ├── ValidationException.cs
    └── BusinessRuleException.cs
```

**Service Layer Responsibilities:**
- Implement business logic and rules
- Validate input (FluentValidation)
- Orchestrate data access via repositories
- Map entities to DTOs (AutoMapper)
- Handle exceptions and return appropriate responses

#### JERP.Infrastructure - Data Access Layer
```
JERP.Infrastructure/
├── Data/
│   ├── JerpDbContext.cs          # EF Core DbContext
│   ├── Configurations/           # Entity configurations
│   │   ├── AccountConfiguration.cs
│   │   ├── JournalEntryConfiguration.cs
│   │   ├── EmployeeConfiguration.cs
│   │   └── ProductConfiguration.cs
│   ├── Seeders/                  # Data seeders
│   │   ├── ChartOfAccountsSeeder.cs
│   │   ├── UserSeeder.cs
│   │   └── SampleDataSeeder.cs
│   └── Migrations/               # EF Core migrations
│       ├── 20260204075145_AddFinanceModule.cs
│       └── 20260204075145_AddFinanceModule.Designer.cs
├── Repositories/                 # Repository implementations
│   ├── Repository.cs             # Generic repository
│   ├── AccountRepository.cs
│   └── EmployeeRepository.cs
└── Services/                     # Infrastructure services
    ├── EmailService.cs
    ├── FileStorageService.cs
    └── CacheService.cs
```

**Key Files:**
- **JerpDbContext.cs**: Database context, DbSets, query filters
- **Configurations/**: Fluent API entity configurations (relationships, indexes, constraints)
- **Migrations/**: Database schema versions

#### JERP.Compliance - Compliance Features (Optional)
```
JERP.Compliance/
├── Services/
│   ├── IMetrcService.cs
│   ├── MetrcService.cs
│   ├── IBioTrackService.cs
│   └── BioTrackService.cs
└── Models/
    ├── MetrcPackage.cs
    └── ComplianceReport.cs
```

---

### Frontend Structure (Next.js)

```
landing-page/
├── app/                          # Next.js 13+ App Router
│   ├── (auth)/                   # Auth pages
│   │   ├── login/
│   │   └── register/
│   ├── (dashboard)/              # Protected routes
│   │   ├── layout.tsx
│   │   ├── page.tsx              # Dashboard home
│   │   ├── finance/              # Finance module
│   │   │   ├── accounts/
│   │   │   ├── journal-entries/
│   │   │   ├── general-ledger/
│   │   │   ├── vendors/
│   │   │   ├── customers/
│   │   │   └── reports/
│   │   ├── inventory/            # Inventory module
│   │   ├── sales/                # Sales module
│   │   └── admin/                # Admin portal
│   ├── api/                      # API routes (if using Next.js API)
│   └── layout.tsx                # Root layout
├── components/                   # React components
│   ├── Finance/
│   │   ├── AccountForm.tsx
│   │   ├── AccountList.tsx
│   │   ├── JournalEntryForm.tsx
│   │   └── FinancialReports.tsx
│   ├── Inventory/
│   ├── Common/                   # Shared components
│   │   ├── Button.tsx
│   │   ├── Table.tsx
│   │   ├── Modal.tsx
│   │   ├── LoadingSpinner.tsx
│   │   └── Navbar.tsx
│   └── Charts/                   # Chart components (Recharts)
│       ├── BarChart.tsx
│       └── LineChart.tsx
├── lib/                          # Utility functions
│   ├── api.ts                    # API client
│   ├── auth.ts                   # Authentication helpers
│   └── utils.ts                  # General utilities
├── hooks/                        # Custom React hooks
│   ├── useAuth.ts
│   ├── useApi.ts
│   └── useLocalStorage.ts
├── types/                        # TypeScript type definitions
│   ├── finance.ts
│   ├── inventory.ts
│   └── user.ts
├── styles/                       # Global styles
│   └── globals.css
├── public/                       # Static assets
│   ├── images/
│   └── icons/
├── middleware.ts                 # Next.js middleware (auth)
├── next.config.js                # Next.js configuration
├── tailwind.config.ts            # Tailwind CSS config
├── tsconfig.json                 # TypeScript config
└── package.json                  # Dependencies
```

**Key Concepts:**
- **App Router**: Next.js 13+ uses file-system based routing in `app/` directory
- **Server Components**: Default in Next.js 13+, rendered on server
- **Client Components**: Use `"use client"` directive for interactive components
- **API Integration**: API calls to ASP.NET Core backend

---

## Development Workflow

### Branch Strategy (Git Flow)

```
main (production-ready)
  └── develop (integration branch)
       ├── feature/add-bank-reconciliation
       ├── feature/implement-pos
       ├── bugfix/fix-journal-entry-validation
       └── hotfix/critical-security-patch
```

**Branch Types:**
- **main**: Production-ready code, tagged releases
- **develop**: Integration branch, latest development
- **feature/***: New features (branch from develop)
- **bugfix/***: Bug fixes (branch from develop)
- **hotfix/***: Critical production fixes (branch from main)

### Development Process

#### 1. Start a New Feature
```bash
# Ensure you're on develop and up-to-date
git checkout develop
git pull origin develop

# Create feature branch
git checkout -b feature/add-bank-reconciliation

# Do your work...
```

#### 2. Make Changes with Commits
```bash
# Stage changes
git add .

# Commit with descriptive message
git commit -m "feat: Add bank reconciliation entity and repository"

# More commits as needed
git commit -m "feat: Add bank reconciliation service and API endpoints"
git commit -m "test: Add unit tests for bank reconciliation"
git commit -m "docs: Update API documentation for bank reconciliation"
```

**Commit Message Convention:**
- `feat:` New feature
- `fix:` Bug fix
- `refactor:` Code refactoring
- `test:` Adding or updating tests
- `docs:` Documentation changes
- `chore:` Maintenance tasks (dependencies, build scripts)
- `style:` Code style changes (formatting, no logic change)

#### 3. Push to Remote
```bash
git push origin feature/add-bank-reconciliation
```

#### 4. Create Pull Request
1. Go to GitHub repository
2. Click "New Pull Request"
3. Base: `develop`, Compare: `feature/add-bank-reconciliation`
4. Fill in PR description:
   - What does this PR do?
   - How to test?
   - Screenshots (if UI changes)
   - Related issue numbers
5. Request review from team members
6. Address review comments
7. Merge once approved

#### 5. Deployment
```bash
# After PR merged to develop, test in staging
# When ready for production:
git checkout main
git pull origin main
git merge develop
git push origin main

# Tag release
git tag -a v1.2.0 -m "Release version 1.2.0"
git push origin v1.2.0
```

### Code Standards

#### C# Code Standards
- **Naming Conventions**:
  - Classes: `PascalCase`
  - Methods: `PascalCase`
  - Properties: `PascalCase`
  - Private fields: `_camelCase` (with underscore)
  - Parameters: `camelCase`
  - Constants: `UPPER_SNAKE_CASE`
- **Use async/await** for I/O operations
- **Use LINQ** for collections instead of loops where appropriate
- **XML comments** for public APIs:
  ```csharp
  /// <summary>
  /// Creates a new journal entry in draft status.
  /// </summary>
  /// <param name="dto">Journal entry data</param>
  /// <returns>Created journal entry DTO</returns>
  public async Task<JournalEntryDto> CreateJournalEntryAsync(CreateJournalEntryDto dto)
  ```
- **Use nullable reference types** (enabled in project)
- **Avoid magic numbers**, use constants or enums

#### TypeScript/React Code Standards
- **Naming Conventions**:
  - Components: `PascalCase` (e.g., `AccountForm.tsx`)
  - Functions: `camelCase`
  - Constants: `UPPER_SNAKE_CASE` or `PascalCase` for components
  - Types/Interfaces: `PascalCase`
- **Functional components** with hooks (no class components)
- **Props interfaces** defined for all components:
  ```typescript
  interface AccountFormProps {
    accountId?: string;
    onSave: (account: Account) => void;
    onCancel: () => void;
  }
  ```
- **Use TypeScript**, no `any` types (use `unknown` if truly unknown)
- **ESLint and Prettier** for code formatting
- **Destructure props** in component parameters

#### Database Standards
- **Table names**: Plural, PascalCase (e.g., `Accounts`, `JournalEntries`)
- **Column names**: PascalCase
- **Foreign keys**: `{Entity}Id` (e.g., `AccountId`, `CompanyId`)
- **Indexes**: Add on foreign keys and commonly queried columns
- **Migrations**: Descriptive names (e.g., `AddFASBTopicToAccount`)

---

## Testing Strategy

### Backend Testing

#### Unit Tests
**Framework**: xUnit  
**Location**: `tests/JERP.Application.Tests/`

**Example Test:**
```csharp
using Xunit;
using Moq;
using FluentAssertions;

namespace JERP.Application.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IRepository<Account>> _mockRepo;
        private readonly AccountService _service;

        public AccountServiceTests()
        {
            _mockRepo = new Mock<IRepository<Account>>();
            _service = new AccountService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateAccount_ValidInput_ReturnsAccountDto()
        {
            // Arrange
            var createDto = new CreateAccountDto
            {
                AccountNumber = "1000",
                Name = "Cash",
                Type = AccountType.Asset
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Account>()))
                     .ReturnsAsync(new Account { Id = Guid.NewGuid(), Name = "Cash" });

            // Act
            var result = await _service.CreateAccountAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Cash");
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Account>()), Times.Once);
        }
    }
}
```

**Run Tests:**
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/JERP.Application.Tests

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

#### Integration Tests
**Framework**: xUnit + Testcontainers  
**Location**: `tests/JERP.Api.Tests/`

**Example Test:**
```csharp
public class AccountsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AccountsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAccounts_ReturnsOkResult()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/accounts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var accounts = await response.Content.ReadFromJsonAsync<List<AccountDto>>();
        accounts.Should().NotBeNull();
    }
}
```

### Frontend Testing

#### Component Tests
**Framework**: Jest + React Testing Library (to be set up)

**Example Test:**
```typescript
import { render, screen, fireEvent } from '@testing-library/react';
import AccountForm from '@/components/Finance/AccountForm';

describe('AccountForm', () => {
  it('renders form fields', () => {
    render(<AccountForm onSave={jest.fn()} onCancel={jest.fn()} />);
    
    expect(screen.getByLabelText(/Account Number/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Account Name/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Account Type/i)).toBeInTheDocument();
  });

  it('calls onSave when form is submitted', async () => {
    const mockSave = jest.fn();
    render(<AccountForm onSave={mockSave} onCancel={jest.fn()} />);
    
    fireEvent.change(screen.getByLabelText(/Account Number/i), {
      target: { value: '1000' }
    });
    fireEvent.change(screen.getByLabelText(/Account Name/i), {
      target: { value: 'Cash' }
    });
    
    fireEvent.click(screen.getByText(/Save/i));
    
    expect(mockSave).toHaveBeenCalled();
  });
});
```

**Run Tests:**
```bash
cd landing-page
npm run test
npm run test:coverage
```

### Testing Guidelines

1. **AAA Pattern**: Arrange, Act, Assert
2. **One assertion per test** (when possible)
3. **Descriptive test names**: `MethodName_Scenario_ExpectedResult`
4. **Test edge cases**: Null inputs, empty lists, boundary conditions
5. **Mock external dependencies**: Database, APIs, file system
6. **Test both success and failure paths**
7. **Keep tests fast**: Unit tests < 100ms each
8. **80%+ code coverage goal** for business logic

---

## Common Tasks and Recipes

### Backend Tasks

#### 1. Add a New Entity

**Step 1: Create Entity Class**
```bash
# Create file: src/JERP.Core/Entities/Finance/BankAccount.cs
```

```csharp
namespace JERP.Core.Entities.Finance
{
    public class BankAccount : BaseEntity
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public Guid AccountId { get; set; } // FK to Chart of Accounts
        public Account Account { get; set; } = null!;
    }
}
```

**Step 2: Add DbSet to DbContext**
```csharp
// src/JERP.Infrastructure/Data/JerpDbContext.cs
public DbSet<BankAccount> BankAccounts { get; set; }
```

**Step 3: Create Entity Configuration**
```csharp
// src/JERP.Infrastructure/Data/Configurations/BankAccountConfiguration.cs
public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("BankAccounts");
        
        builder.Property(b => b.AccountNumber)
               .IsRequired()
               .HasMaxLength(50);
               
        builder.Property(b => b.Balance)
               .HasPrecision(18, 2);
               
        builder.HasOne(b => b.Account)
               .WithMany()
               .HasForeignKey(b => b.AccountId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
```

**Step 4: Generate and Apply Migration**
```bash
cd src/JERP.Api
dotnet ef migrations add AddBankAccount --project ../JERP.Infrastructure
dotnet ef database update --project ../JERP.Infrastructure
```

#### 2. Create a New API Endpoint

**Step 1: Create DTOs**
```csharp
// src/JERP.Application/DTOs/Finance/BankAccountDto.cs
public record BankAccountDto(
    Guid Id,
    string AccountNumber,
    string BankName,
    decimal Balance
);

public record CreateBankAccountDto(
    string AccountNumber,
    string BankName,
    Guid AccountId
);
```

**Step 2: Create Service Interface and Implementation**
```csharp
// src/JERP.Application/Services/Finance/IBankAccountService.cs
public interface IBankAccountService
{
    Task<BankAccountDto> GetByIdAsync(Guid id);
    Task<IEnumerable<BankAccountDto>> GetAllAsync();
    Task<BankAccountDto> CreateAsync(CreateBankAccountDto dto);
}

// src/JERP.Application/Services/Finance/BankAccountService.cs
public class BankAccountService : IBankAccountService
{
    private readonly IRepository<BankAccount> _repository;
    private readonly IMapper _mapper;

    public BankAccountService(IRepository<BankAccount> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BankAccountDto> CreateAsync(CreateBankAccountDto dto)
    {
        var bankAccount = _mapper.Map<BankAccount>(dto);
        await _repository.AddAsync(bankAccount);
        return _mapper.Map<BankAccountDto>(bankAccount);
    }
    
    // ... other methods
}
```

**Step 3: Create Controller**
```csharp
// src/JERP.Api/Controllers/BankAccountsController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BankAccountsController : ControllerBase
{
    private readonly IBankAccountService _service;

    public BankAccountsController(IBankAccountService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankAccountDto>>> GetAll()
    {
        var accounts = await _service.GetAllAsync();
        return Ok(accounts);
    }

    [HttpPost]
    public async Task<ActionResult<BankAccountDto>> Create([FromBody] CreateBankAccountDto dto)
    {
        var account = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<BankAccountDto>> GetById(Guid id)
    {
        var account = await _service.GetByIdAsync(id);
        if (account == null) return NotFound();
        return Ok(account);
    }
}
```

**Step 4: Register Service in DI Container**
```csharp
// src/JERP.Api/Program.cs
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
```

**Step 5: Test with Swagger**
- Run API: `dotnet run`
- Open http://localhost:5000/swagger
- Test endpoints

#### 3. Database Migrations

**Create Migration:**
```bash
cd src/JERP.Api
dotnet ef migrations add MigrationName --project ../JERP.Infrastructure
```

**Apply Migration:**
```bash
dotnet ef database update --project ../JERP.Infrastructure
```

**Rollback Migration:**
```bash
# Rollback to specific migration
dotnet ef database update PreviousMigrationName --project ../JERP.Infrastructure

# Remove last migration (before applying it)
dotnet ef migrations remove --project ../JERP.Infrastructure
```

**Generate SQL Script:**
```bash
# Generate script from all migrations
dotnet ef migrations script --project ../JERP.Infrastructure --output migration.sql

# Generate script from specific migration to latest
dotnet ef migrations script FromMigration --project ../JERP.Infrastructure --output migration.sql
```

### Frontend Tasks

#### 1. Add a New Frontend Component

**Step 1: Create Component File**
```typescript
// landing-page/components/Finance/BankAccountForm.tsx
'use client';

import React, { useState } from 'react';

interface BankAccountFormProps {
  onSave: (data: BankAccountFormData) => void;
  onCancel: () => void;
}

interface BankAccountFormData {
  accountNumber: string;
  bankName: string;
  accountId: string;
}

export default function BankAccountForm({ onSave, onCancel }: BankAccountFormProps) {
  const [formData, setFormData] = useState<BankAccountFormData>({
    accountNumber: '',
    bankName: '',
    accountId: ''
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label className="block text-sm font-medium text-gray-700">
          Account Number
        </label>
        <input
          type="text"
          value={formData.accountNumber}
          onChange={(e) => setFormData({ ...formData, accountNumber: e.target.value })}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
          required
        />
      </div>
      
      <div>
        <label className="block text-sm font-medium text-gray-700">
          Bank Name
        </label>
        <input
          type="text"
          value={formData.bankName}
          onChange={(e) => setFormData({ ...formData, bankName: e.target.value })}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm"
          required
        />
      </div>

      <div className="flex justify-end space-x-2">
        <button
          type="button"
          onClick={onCancel}
          className="px-4 py-2 border border-gray-300 rounded-md"
        >
          Cancel
        </button>
        <button
          type="submit"
          className="px-4 py-2 bg-blue-600 text-white rounded-md"
        >
          Save
        </button>
      </div>
    </form>
  );
}
```

**Step 2: Use Component in Page**
```typescript
// landing-page/app/(dashboard)/finance/bank-accounts/page.tsx
'use client';

import BankAccountForm from '@/components/Finance/BankAccountForm';

export default function BankAccountsPage() {
  const handleSave = async (data: BankAccountFormData) => {
    // Call API
    const response = await fetch('http://localhost:5000/api/bankaccounts', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(data)
    });
    
    if (response.ok) {
      // Handle success
    }
  };

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Bank Accounts</h1>
      <BankAccountForm onSave={handleSave} onCancel={() => {}} />
    </div>
  );
}
```

#### 2. Call API from Frontend

**Create API Service:**
```typescript
// landing-page/lib/api.ts
const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function fetchBankAccounts(token: string) {
  const response = await fetch(`${API_BASE_URL}/api/bankaccounts`, {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    }
  });
  
  if (!response.ok) {
    throw new Error('Failed to fetch bank accounts');
  }
  
  return response.json();
}

export async function createBankAccount(data: BankAccountFormData, token: string) {
  const response = await fetch(`${API_BASE_URL}/api/bankaccounts`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
  });
  
  if (!response.ok) {
    throw new Error('Failed to create bank account');
  }
  
  return response.json();
}
```

**Use in Component:**
```typescript
import { fetchBankAccounts, createBankAccount } from '@/lib/api';
import { useSession } from 'next-auth/react';

export default function BankAccountsPage() {
  const { data: session } = useSession();
  const [accounts, setAccounts] = useState([]);

  useEffect(() => {
    if (session?.accessToken) {
      fetchBankAccounts(session.accessToken)
        .then(setAccounts)
        .catch(console.error);
    }
  }, [session]);

  // ... rest of component
}
```

---

## Troubleshooting

### Common Issues and Solutions

#### Backend Issues

**Problem: Build Errors**
```
Solution:
# Restore NuGet packages
cd /path/to/JERP-3.0
dotnet restore

# Clean and rebuild
dotnet clean
dotnet build
```

**Problem: Database Connection Errors**
```
Error: "A network-related or instance-specific error occurred..."

Solutions:
1. Verify SQL Server is running:
   - Windows: Open Services, check "SQL Server (MSSQLSERVER)" or "SQL Server (SQLEXPRESS)"
   - Docker: docker ps (check if container is running)

2. Check connection string:
   - Verify server name (localhost\SQLEXPRESS or localhost,1433)
   - Verify database name (JERP3_DB)
   - Check authentication (Trusted_Connection or User Id/Password)

3. Test connection with sqlcmd:
   sqlcmd -S localhost\SQLEXPRESS -E -Q "SELECT @@VERSION"
```

**Problem: Migration Errors**
```
Error: "No executable found matching command 'dotnet-ef'"

Solution:
dotnet tool install --global dotnet-ef
# Then restart terminal and try again
```

**Problem: Port Already in Use**
```
Error: "Failed to bind to address http://localhost:5000: address already in use"

Solution:
# Find process using port (Windows PowerShell):
netstat -ano | findstr :5000

# Kill process:
taskkill /PID <process-id> /F

# Or change port in launchSettings.json or appsettings.json
```

#### Frontend Issues

**Problem: `npm install` Fails**
```
Solution:
# Clear npm cache
npm cache clean --force

# Delete node_modules and package-lock.json
rm -rf node_modules package-lock.json

# Reinstall
npm install
```

**Problem: CORS Errors**
```
Error: "Access to fetch at 'http://localhost:5000/api/accounts' from origin 'http://localhost:3000' has been blocked by CORS policy"

Solution:
# Update CORS policy in Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

app.UseCors("AllowFrontend");
```

**Problem: Environment Variables Not Loading**
```
Solution:
# Ensure .env.local exists (for Next.js)
# Restart dev server after creating/updating .env.local
npm run dev
```

#### Docker Issues

**Problem: Container Won't Start**
```
Solution:
# View logs
docker-compose logs

# Specific service logs
docker-compose logs api
docker-compose logs sqlserver

# Restart containers
docker-compose down
docker-compose up --build
```

**Problem: Database Not Ready**
```
Error: "Cannot create database because file already exists"

Solution:
# Remove volumes and restart
docker-compose down -v
docker-compose up --build
```

---

## Resources

### Documentation
- **Project Documentation**: 
  - [README.md](README.md) - Project overview
  - [ARCHITECTURE.md](ARCHITECTURE.md) - Architecture details
  - [SCOPE-OF-WORK.md](SCOPE-OF-WORK.md) - Feature breakdown
  - [API-DOCUMENTATION.md](API-DOCUMENTATION.md) - API reference
  - [TESTING-GUIDE.md](TESTING-GUIDE.md) - Testing guide
- **Official Docs**:
  - [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
  - [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
  - [Next.js](https://nextjs.org/docs)
  - [React](https://react.dev/)
  - [Tailwind CSS](https://tailwindcss.com/docs)

### Learning Resources
- **C# and .NET**:
  - [Microsoft Learn - C#](https://learn.microsoft.com/en-us/dotnet/csharp/)
  - [Clean Architecture with .NET](https://github.com/jasontaylordev/CleanArchitecture)
- **React and TypeScript**:
  - [React TypeScript Cheatsheet](https://react-typescript-cheatsheet.netlify.app/)
  - [Next.js Tutorial](https://nextjs.org/learn)
- **Database**:
  - [SQL Server Tutorial](https://www.sqlservertutorial.net/)
  - [Entity Framework Core Tutorial](https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx)

### Tools and Extensions

#### VS Code Extensions
- **C# Dev Kit**: Microsoft's official C# extension
- **C#**: C# language support
- **REST Client**: Test APIs directly in VS Code
- **Docker**: Docker support
- **GitLens**: Enhanced Git capabilities
- **ESLint**: JavaScript/TypeScript linting
- **Prettier**: Code formatting
- **Tailwind CSS IntelliSense**: Tailwind CSS autocomplete

#### Chrome Extensions
- **React Developer Tools**: Inspect React components
- **Redux DevTools**: If using Redux

### Getting Help

**Internal Resources:**
- **Slack/Teams Channel**: #jerp-development (if applicable)
- **Wiki**: Internal documentation wiki
- **Code Reviews**: Ask questions in PR comments

**External Resources:**
- **Stack Overflow**: Search or ask questions
- **GitHub Issues**: For library-specific questions
- **Discord/Reddit**: .NET and React communities

### Useful Commands Cheat Sheet

**Git:**
```bash
git status                    # Check status
git log --oneline --graph     # View commit history
git diff                      # View changes
git stash                     # Temporarily save changes
git stash pop                 # Restore stashed changes
```

**.NET:**
```bash
dotnet --version              # Check .NET version
dotnet new                    # List project templates
dotnet watch run              # Run with hot reload
dotnet user-secrets set "Key" "Value"  # Set user secrets
```

**Docker:**
```bash
docker ps                     # List running containers
docker ps -a                  # List all containers
docker logs <container-id>    # View container logs
docker exec -it <container-id> bash  # Access container shell
docker-compose down           # Stop all services
docker-compose down -v        # Stop and remove volumes
```

**Database:**
```bash
# Connect to SQL Server (Docker)
docker exec -it jerp-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P <password>

# List databases
SELECT name FROM sys.databases;
GO

# Switch to JERP database
USE JERP3_DB;
GO

# List tables
SELECT * FROM INFORMATION_SCHEMA.TABLES;
GO
```

---

## Next Steps

Now that you're set up, here's what to do next:

1. ✅ **Verify Setup**: Ensure everything is running (API, frontend, database)
2. 📖 **Read Architecture Docs**: Understand the system design ([ARCHITECTURE.md](ARCHITECTURE.md))
3. 🔍 **Explore Codebase**: Browse through entities, services, and components
4. 🎯 **Pick a Task**: Choose a small task or bug from the backlog
5. 🌿 **Create Branch**: Follow the branch strategy
6. 💻 **Write Code**: Implement your changes with tests
7. 🔬 **Test Locally**: Verify your changes work
8. 📝 **Create PR**: Submit for review
9. 🔄 **Iterate**: Address feedback and improve
10. 🎉 **Merge**: Celebrate your contribution!

**Welcome to the team! Happy coding! 🚀**
