# JERP 3.0 - Developer Onboarding Guide

Welcome to the JERP 3.0 development team! This guide will help you get started with the project, understand the codebase, and become productive quickly.

---

## Table of Contents

1. [Setup Instructions](#1-setup-instructions)
2. [Project Structure](#2-project-structure-explanation)
3. [Development Workflow](#3-development-workflow)
4. [Testing Strategy](#4-testing-strategy)
5. [Common Tasks and Recipes](#5-common-tasks-and-recipes)

---

## 1. Setup Instructions

### 1.1 Prerequisites

Before you begin, ensure you have the following installed:

| Software | Version | Download Link |
|----------|---------|---------------|
| **Node.js** | 18+ | https://nodejs.org/ |
| **npm or yarn** | Latest | Included with Node.js |
| **.NET SDK** | 8.0 | https://dotnet.microsoft.com/download |
| **SQL Server** | 2019+ (Express/Developer) | https://www.microsoft.com/sql-server/sql-server-downloads |
| **Git** | Latest | https://git-scm.com/ |
| **Docker Desktop** | Latest (optional) | https://www.docker.com/products/docker-desktop |
| **Visual Studio Code** | Latest | https://code.visualstudio.com/ |
| **Visual Studio 2022** | Latest (optional) | https://visualstudio.microsoft.com/ |

**Recommended VS Code Extensions:**
- C# (Microsoft)
- C# Dev Kit (Microsoft)
- ESLint
- Prettier
- Docker
- GitLens
- REST Client
- SQL Server (mssql)

### 1.2 Getting the Code

```bash
# Clone the repository
git clone https://github.com/ninoyerbas/JERP-3.0.git
cd JERP-3.0

# Verify you're on the correct branch
git branch
# Should show: * main or * develop
```

### 1.3 Backend Setup

#### Step 1: Install .NET Dependencies

```bash
# Navigate to the API project
cd src/JERP.Api

# Restore NuGet packages
dotnet restore

# Verify the build works
dotnet build
```

#### Step 2: Configure Database Connection

Update the connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=JERP3_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**For SQL Server Authentication (instead of Windows Auth):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=JERP3_DB;User Id=your_username;Password=your_password;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

#### Step 3: Create Database

**Option 1: Using SQL Server Management Studio (SSMS)**
```sql
CREATE DATABASE JERP3_DB;
```

**Option 2: Using Command Line**
```bash
sqlcmd -S localhost\SQLEXPRESS -Q "CREATE DATABASE JERP3_DB"
```

#### Step 4: Run Database Migrations

```bash
# Make sure you're in the src/JERP.Api directory
cd src/JERP.Api

# Apply migrations to create database schema
dotnet ef database update --project ../JERP.Infrastructure
```

**If migrations don't exist yet, create initial migration:**
```bash
dotnet ef migrations add InitialCreate --project ../JERP.Infrastructure
dotnet ef database update --project ../JERP.Infrastructure
```

#### Step 5: Run the API

```bash
# Run the API (from src/JERP.Api directory)
dotnet run

# You should see output like:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:7001
#       Now listening on: http://localhost:5000
```

**Access Swagger UI:**
- Open browser to: https://localhost:7001/swagger
- You should see the API documentation

### 1.4 Frontend Setup

#### Step 1: Install Node Dependencies

```bash
# Navigate to the frontend directory
cd landing-page

# Install dependencies (choose npm or yarn)
npm install
# OR
yarn install
```

#### Step 2: Configure Environment Variables

Create a `.env.local` file in the `landing-page` directory:

```bash
# Copy the example file
cp .env.example .env.local
```

Update `.env.local` with your configuration:

```env
# API Configuration
NEXT_PUBLIC_API_URL=https://localhost:7001
NEXT_PUBLIC_API_VERSION=v1

# NextAuth Configuration
NEXTAUTH_URL=http://localhost:3000
NEXTAUTH_SECRET=your-secret-key-here-generate-with-openssl

# Database (Prisma)
DATABASE_URL="Server=localhost\\SQLEXPRESS;Database=JERP3_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

# Stripe (for payment processing)
NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY=pk_test_your_key_here
STRIPE_SECRET_KEY=sk_test_your_key_here

# Optional: Feature Flags
NEXT_PUBLIC_ENABLE_METRC=false
NEXT_PUBLIC_ENABLE_LOYALTY=true
```

**Generate NEXTAUTH_SECRET:**
```bash
openssl rand -base64 32
```

#### Step 3: Run Database Setup (Prisma)

```bash
# Generate Prisma client
npx prisma generate

# Run Prisma migrations (if using Prisma)
npx prisma db push
```

#### Step 4: Run the Frontend

```bash
# Start the development server (from landing-page directory)
npm run dev
# OR
yarn dev

# You should see output like:
# ready - started server on 0.0.0.0:3000, url: http://localhost:3000
```

**Access the Application:**
- Open browser to: http://localhost:3000

### 1.5 Docker Setup (Optional)

If you prefer to use Docker for local development:

```bash
# From the project root directory
docker-compose -f docker-compose.dev.yml up -d

# This will start:
# - SQL Server on port 1433
# - API on port 7001
# - Frontend on port 3000
```

**Stop Docker containers:**
```bash
docker-compose -f docker-compose.dev.yml down
```

### 1.6 Verify Installation

#### Backend Health Check
```bash
curl https://localhost:7001/health
# Should return: {"status":"Healthy"}
```

#### Test API Authentication
```bash
# Using the REST Client extension in VS Code, or curl:
curl -X POST https://localhost:7001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test123!"}'
```

#### Frontend Test
- Navigate to http://localhost:3000
- You should see the landing page
- Check browser console for any errors

### 1.7 Troubleshooting Common Setup Issues

**Issue: "SqlException: Cannot open database"**
- **Solution:** Ensure SQL Server is running and database exists
  ```bash
  # Check SQL Server status
  sqlcmd -S localhost\SQLEXPRESS -Q "SELECT @@VERSION"
  ```

**Issue: "Port already in use"**
- **Solution:** Change the port in `launchSettings.json` (backend) or `package.json` (frontend)
  ```json
  // For backend: src/JERP.Api/Properties/launchSettings.json
  "applicationUrl": "https://localhost:7002;http://localhost:5001"
  
  // For frontend: update dev script in package.json
  "dev": "next dev -p 3001"
  ```

**Issue: "CORS error when calling API from frontend"**
- **Solution:** Verify CORS settings in `Program.cs` include your frontend URL
  ```csharp
  builder.Services.AddCors(options =>
  {
      options.AddPolicy("AllowDevelopment", policy =>
      {
          policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
      });
  });
  ```

**Issue: "Entity Framework migrations not found"**
- **Solution:** Ensure you're running commands from the correct directory
  ```bash
  cd src/JERP.Api
  dotnet ef migrations list --project ../JERP.Infrastructure
  ```

**Issue: "npm/yarn install fails"**
- **Solution:** Clear cache and try again
  ```bash
  npm cache clean --force
  rm -rf node_modules package-lock.json
  npm install
  ```

---

## 2. Project Structure Explanation

### 2.1 Root Directory Structure

```
JERP-3.0/
├── .github/                        # GitHub Actions CI/CD workflows
│   └── workflows/
│       ├── build.yml              # Build and test workflow
│       └── deploy.yml             # Deployment workflow
├── docs/                          # Documentation
│   ├── architecture/              # Architecture documentation
│   ├── SOW.md                     # Scope of Work
│   └── ONBOARDING.md              # This file
├── src/                           # Source code
│   ├── JERP.Api/                  # Web API project
│   ├── JERP.Core/                 # Domain entities and interfaces
│   ├── JERP.Application/          # Business logic and DTOs
│   ├── JERP.Infrastructure/       # Data access and external services
│   ├── JERP.Compliance/           # Compliance-specific logic
│   └── JERP.Desktop/              # Desktop application (WPF)
├── landing-page/                  # Next.js frontend
│   ├── app/                       # Next.js 13+ app directory
│   ├── components/                # React components
│   ├── lib/                       # Utility libraries
│   ├── public/                    # Static assets
│   ├── prisma/                    # Prisma ORM configuration
│   └── styles/                    # Global styles
├── tests/                         # Test projects
│   ├── JERP.Api.Tests/
│   ├── JERP.Core.Tests/
│   └── JERP.Application.Tests/
├── docker/                        # Docker configurations
├── .editorconfig                  # Editor configuration
├── .gitignore                     # Git ignore rules
├── docker-compose.yml             # Production Docker Compose
├── docker-compose.dev.yml         # Development Docker Compose
├── JERP.slnx                      # Solution file
└── README.md                      # Project README
```

### 2.2 Backend Structure (src/)

#### JERP.Api/
**Purpose:** ASP.NET Core Web API project - entry point for HTTP requests

```
JERP.Api/
├── Controllers/                   # API endpoint controllers
│   ├── AuthController.cs          # Authentication endpoints
│   ├── FinanceController.cs       # Finance module endpoints
│   ├── InventoryController.cs     # Inventory module endpoints
│   ├── POSController.cs           # Point of Sale endpoints
│   └── ComplianceController.cs    # Compliance endpoints
├── Middleware/                    # Custom middleware
│   ├── ErrorHandlingMiddleware.cs # Global error handling
│   ├── LoggingMiddleware.cs       # Request/response logging
│   └── AuthenticationMiddleware.cs # Custom auth logic
├── Properties/
│   └── launchSettings.json        # Development launch settings
├── appsettings.json               # Application configuration
├── appsettings.Development.json   # Development-specific config
├── appsettings.Production.json    # Production-specific config
├── Program.cs                     # Application entry point
└── JERP.Api.csproj                # Project file
```

**Key Files:**

**Program.cs** - Application configuration and startup
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database
builder.Services.AddDbContext<JerpDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* JWT config */ });

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

#### JERP.Core/
**Purpose:** Domain layer - entities, interfaces, enums (no dependencies)

```
JERP.Core/
├── Entities/                      # Domain entities (business objects)
│   ├── BaseEntity.cs              # Base class for all entities
│   ├── Finance/
│   │   ├── Account.cs             # Chart of Accounts
│   │   ├── JournalEntry.cs        # Journal entries
│   │   ├── Invoice.cs             # Invoices
│   │   └── Payment.cs             # Payments
│   ├── Inventory/
│   │   ├── Product.cs             # Products
│   │   ├── Batch.cs               # Inventory batches
│   │   ├── Transfer.cs            # Transfer manifests
│   │   └── StockMovement.cs       # Inventory movements
│   ├── POS/
│   │   ├── Sale.cs                # Sales transactions
│   │   ├── Customer.cs            # Customers
│   │   └── Cart.cs                # Shopping carts
│   └── Compliance/
│       ├── MetrcPackage.cs        # METRC packages
│       └── AuditLog.cs            # Audit trail
├── Enums/                         # Enumerations
│   ├── AccountType.cs             # Asset, Liability, Equity, etc.
│   ├── TransactionStatus.cs       # Pending, Completed, Failed
│   └── ComplianceStatus.cs        # Compliant, Non-Compliant, etc.
├── Exceptions/                    # Custom exceptions
│   └── DomainException.cs         # Base domain exception
└── Interfaces/                    # Repository interfaces
    ├── IRepository.cs             # Generic repository interface
    ├── IAccountRepository.cs      # Account-specific operations
    └── IProductRepository.cs      # Product-specific operations
```

**Example Entity:**
```csharp
public class Product : BaseEntity
{
    public string SKU { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ProductCategory Category { get; set; }
    public decimal Price { get; set; }
    public decimal THCContent { get; set; }
    public decimal CBDContent { get; set; }
    public int QuantityOnHand { get; set; }
    public string MetrcId { get; set; }
    
    // Navigation properties
    public ICollection<StockMovement> StockMovements { get; set; }
    public ICollection<Batch> Batches { get; set; }
}
```

#### JERP.Application/
**Purpose:** Application layer - business logic, DTOs, validators

```
JERP.Application/
├── DTOs/                          # Data Transfer Objects
│   ├── Finance/
│   │   ├── CreateInvoiceDto.cs
│   │   └── InvoiceDto.cs
│   ├── Inventory/
│   │   ├── CreateProductDto.cs
│   │   └── ProductDto.cs
│   └── Common/
│       └── PagedResultDto.cs
├── Services/                      # Business logic services
│   ├── IFinanceService.cs
│   ├── FinanceService.cs
│   ├── IInventoryService.cs
│   ├── InventoryService.cs
│   └── IMetrcService.cs
├── Validators/                    # FluentValidation validators
│   ├── CreateInvoiceValidator.cs
│   └── CreateProductValidator.cs
├── Mappings/                      # AutoMapper profiles
│   └── MappingProfile.cs
└── JERP.Application.csproj
```

**Example Service:**
```csharp
public class FinanceService : IFinanceService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IJournalEntryRepository _journalEntryRepository;
    
    public FinanceService(
        IAccountRepository accountRepository,
        IJournalEntryRepository journalEntryRepository)
    {
        _accountRepository = accountRepository;
        _journalEntryRepository = journalEntryRepository;
    }
    
    public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto dto)
    {
        // Validate business rules
        // Create invoice entity
        // Create journal entries for accounting
        // Return DTO
    }
}
```

#### JERP.Infrastructure/
**Purpose:** Infrastructure layer - data access, external services

```
JERP.Infrastructure/
├── Data/                          # Database context and configurations
│   ├── JerpDbContext.cs           # Entity Framework DbContext
│   ├── Configurations/            # Entity configurations
│   │   ├── AccountConfiguration.cs
│   │   └── ProductConfiguration.cs
│   └── Seed/                      # Database seed data
│       └── DataSeeder.cs
├── Repositories/                  # Repository implementations
│   ├── Repository.cs              # Generic repository
│   ├── AccountRepository.cs
│   └── ProductRepository.cs
├── Services/                      # External service integrations
│   ├── MetrcApiService.cs         # METRC API integration
│   ├── StripePaymentService.cs    # Stripe payment integration
│   └── EmailService.cs            # Email service
├── Migrations/                    # Entity Framework migrations
│   └── 20260205_InitialCreate.cs
└── JERP.Infrastructure.csproj
```

**Example DbContext:**
```csharp
public class JerpDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Sale> Sales { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        
        // Seed data
        modelBuilder.Entity<Account>().HasData(/* seed data */);
    }
}
```

### 2.3 Frontend Structure (landing-page/)

```
landing-page/
├── app/                           # Next.js App Router
│   ├── (auth)/                    # Auth routes group
│   │   ├── login/
│   │   └── register/
│   ├── (dashboard)/               # Protected dashboard routes
│   │   ├── finance/
│   │   ├── inventory/
│   │   ├── pos/
│   │   └── compliance/
│   ├── api/                       # API routes (Next.js API)
│   │   └── auth/
│   ├── layout.tsx                 # Root layout
│   └── page.tsx                   # Home page
├── components/                    # React components
│   ├── ui/                        # Reusable UI components
│   │   ├── Button.tsx
│   │   ├── Input.tsx
│   │   ├── Modal.tsx
│   │   └── Table.tsx
│   ├── finance/                   # Finance module components
│   │   ├── InvoiceList.tsx
│   │   └── CreateInvoice.tsx
│   ├── inventory/                 # Inventory module components
│   ├── pos/                       # POS module components
│   └── layout/                    # Layout components
│       ├── Header.tsx
│       ├── Sidebar.tsx
│       └── Footer.tsx
├── lib/                           # Utility libraries
│   ├── api/                       # API client
│   │   ├── client.ts              # Axios/Fetch configuration
│   │   ├── finance.ts             # Finance API calls
│   │   └── inventory.ts           # Inventory API calls
│   ├── hooks/                     # Custom React hooks
│   │   ├── useAuth.ts
│   │   ├── useFinance.ts
│   │   └── useInventory.ts
│   ├── utils/                     # Utility functions
│   │   ├── format.ts              # Formatting utilities
│   │   └── validation.ts          # Validation utilities
│   └── context/                   # React Context providers
│       └── AuthContext.tsx
├── public/                        # Static assets
│   ├── images/
│   └── icons/
├── styles/                        # Global styles
│   └── globals.css
├── types/                         # TypeScript type definitions
│   ├── api.ts
│   └── models.ts
├── middleware.ts                  # Next.js middleware
├── next.config.js                 # Next.js configuration
├── tailwind.config.ts             # Tailwind CSS configuration
├── tsconfig.json                  # TypeScript configuration
└── package.json                   # Dependencies and scripts
```

**Example Component:**
```tsx
// components/finance/InvoiceList.tsx
import { useEffect, useState } from 'react';
import { getInvoices } from '@/lib/api/finance';
import { Invoice } from '@/types/models';

export function InvoiceList() {
  const [invoices, setInvoices] = useState<Invoice[]>([]);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    async function fetchInvoices() {
      try {
        const data = await getInvoices();
        setInvoices(data);
      } catch (error) {
        console.error('Failed to fetch invoices:', error);
      } finally {
        setLoading(false);
      }
    }
    
    fetchInvoices();
  }, []);
  
  if (loading) return <div>Loading...</div>;
  
  return (
    <div className="invoice-list">
      {invoices.map(invoice => (
        <div key={invoice.id}>{invoice.invoiceNumber}</div>
      ))}
    </div>
  );
}
```

---

## 3. Development Workflow

### 3.1 Git Workflow

#### Branch Strategy

We follow **Git Flow** branching model:

```
main (production)
  └── develop (integration)
       ├── feature/add-invoice-module
       ├── feature/metrc-integration
       ├── bugfix/fix-calculation-error
       └── hotfix/security-patch
```

**Branch Types:**
- `main` - Production-ready code only
- `develop` - Integration branch for features
- `feature/*` - New features (branch from develop)
- `bugfix/*` - Bug fixes (branch from develop)
- `hotfix/*` - Urgent production fixes (branch from main)
- `release/*` - Release preparation (branch from develop)

#### Creating a Feature Branch

```bash
# Make sure you're on develop and up to date
git checkout develop
git pull origin develop

# Create feature branch
git checkout -b feature/add-inventory-alerts

# Work on your feature, commit regularly
git add .
git commit -m "feat: add inventory low stock alerts"

# Push to remote
git push origin feature/add-inventory-alerts
```

### 3.2 Commit Convention

We use **Conventional Commits** specification:

**Format:**
```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation changes
- `style:` - Code style changes (formatting, no logic change)
- `refactor:` - Code refactoring
- `test:` - Adding or updating tests
- `chore:` - Maintenance tasks, dependencies
- `perf:` - Performance improvements
- `ci:` - CI/CD changes

**Examples:**
```bash
git commit -m "feat(finance): add 280E compliance report"
git commit -m "fix(inventory): correct stock calculation in batch tracking"
git commit -m "docs: update API documentation for invoice endpoint"
git commit -m "test(pos): add unit tests for cart validation"
git commit -m "chore: update Entity Framework to 8.0.1"
```

### 3.3 Pull Request Process

#### Step 1: Create Pull Request

1. Push your feature branch to GitHub
2. Navigate to the repository on GitHub
3. Click "Pull Request" → "New Pull Request"
4. Select base: `develop` and compare: `feature/your-feature`
5. Fill in the PR template:

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Related Issues
Closes #123

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] Manual testing completed

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] No console.log or debug code
- [ ] Tests pass locally
```

#### Step 2: Request Code Review

- Add at least one reviewer (preferably 2)
- Add appropriate labels (feature, bug, documentation)
- Link related issues

#### Step 3: Address Review Comments

```bash
# Make changes based on feedback
git add .
git commit -m "fix: address code review comments"
git push origin feature/your-feature
```

#### Step 4: Merge

Once approved:
- **Squash and merge** (for feature branches)
- **Rebase and merge** (for small bug fixes)
- Delete the feature branch after merging

### 3.4 Code Review Checklist

**As a Reviewer:**
- [ ] Code follows project style guidelines
- [ ] Logic is clear and well-commented when necessary
- [ ] No hardcoded values (use configuration)
- [ ] Error handling is appropriate
- [ ] Security best practices followed
- [ ] Performance considerations addressed
- [ ] Tests are included and passing
- [ ] Documentation is updated
- [ ] No console.log or debugging code
- [ ] Database migrations are backwards compatible

**As an Author:**
- [ ] Self-review completed before requesting review
- [ ] PR description is clear and complete
- [ ] All tests pass
- [ ] No merge conflicts
- [ ] Code is ready for production

### 3.5 Daily Development Workflow

**Morning:**
```bash
# 1. Update your local develop branch
git checkout develop
git pull origin develop

# 2. Merge develop into your feature branch
git checkout feature/your-feature
git merge develop

# 3. Run tests to ensure everything still works
dotnet test  # Backend
npm test     # Frontend
```

**During the Day:**
```bash
# Make changes, commit frequently
git add .
git commit -m "feat: implement feature X"

# Push to remote at end of day
git push origin feature/your-feature
```

**End of Day:**
```bash
# Ensure all changes are committed and pushed
git status
git push origin feature/your-feature
```

---

## 4. Testing Strategy

### 4.1 Testing Pyramid

Our testing strategy follows the testing pyramid:

```
        /\
       /E2E\        10% - End-to-End Tests
      /------\
     /Integration\  20% - Integration Tests
    /------------\
   /  Unit Tests  \ 70% - Unit Tests
  /----------------\
```

### 4.2 Backend Testing

#### Running Tests

```bash
# Run all tests
dotnet test

# Run tests in a specific project
cd tests/JERP.Api.Tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverageDirectory=./coverage

# Run specific test
dotnet test --filter "FullyQualifiedName~FinanceServiceTests.CreateInvoice_ValidData_ReturnsInvoice"

# Run tests by category
dotnet test --filter "Category=Integration"
```

#### Writing Unit Tests

**Test Structure:**
```csharp
// tests/JERP.Application.Tests/Services/FinanceServiceTests.cs
public class FinanceServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepoMock;
    private readonly FinanceService _service;
    
    public FinanceServiceTests()
    {
        _accountRepoMock = new Mock<IAccountRepository>();
        _service = new FinanceService(_accountRepoMock.Object);
    }
    
    [Fact]
    public async Task CreateInvoice_ValidData_ReturnsInvoice()
    {
        // Arrange
        var dto = new CreateInvoiceDto
        {
            CustomerName = "Test Customer",
            Amount = 100.00m
        };
        
        // Act
        var result = await _service.CreateInvoiceAsync(dto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Customer", result.CustomerName);
        _accountRepoMock.Verify(x => x.AddAsync(It.IsAny<Invoice>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateInvoice_InvalidAmount_ThrowsException()
    {
        // Arrange
        var dto = new CreateInvoiceDto { Amount = -10.00m };
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.CreateInvoiceAsync(dto)
        );
    }
}
```

#### Writing Integration Tests

```csharp
// tests/JERP.Api.Tests/Controllers/FinanceControllerIntegrationTests.cs
public class FinanceControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public FinanceControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAccounts_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/finance/accounts");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }
}
```

### 4.3 Frontend Testing

#### Running Tests

```bash
# Run all tests
npm test
# OR
yarn test

# Run tests in watch mode
npm test -- --watch

# Run tests with coverage
npm run test:coverage

# Run specific test file
npm test -- InvoiceList.test.tsx

# Run E2E tests
npm run test:e2e
```

#### Writing Component Tests

**Test Structure:**
```typescript
// components/__tests__/InvoiceList.test.tsx
import { render, screen, waitFor } from '@testing-library/react';
import { InvoiceList } from '../InvoiceList';
import * as api from '@/lib/api/finance';

jest.mock('@/lib/api/finance');

describe('InvoiceList', () => {
  it('renders loading state initially', () => {
    render(<InvoiceList />);
    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });
  
  it('renders invoices after loading', async () => {
    const mockInvoices = [
      { id: 1, invoiceNumber: 'INV-001', amount: 100 },
      { id: 2, invoiceNumber: 'INV-002', amount: 200 },
    ];
    
    (api.getInvoices as jest.Mock).mockResolvedValue(mockInvoices);
    
    render(<InvoiceList />);
    
    await waitFor(() => {
      expect(screen.getByText('INV-001')).toBeInTheDocument();
      expect(screen.getByText('INV-002')).toBeInTheDocument();
    });
  });
  
  it('handles errors gracefully', async () => {
    (api.getInvoices as jest.Mock).mockRejectedValue(new Error('API Error'));
    
    render(<InvoiceList />);
    
    await waitFor(() => {
      expect(screen.getByText(/error/i)).toBeInTheDocument();
    });
  });
});
```

### 4.4 Test Files Location

**Backend Tests:**
```
tests/
├── JERP.Api.Tests/
│   ├── Controllers/
│   └── Integration/
├── JERP.Application.Tests/
│   └── Services/
├── JERP.Core.Tests/
│   └── Entities/
└── JERP.Infrastructure.Tests/
    └── Repositories/
```

**Frontend Tests:**
```
landing-page/
├── components/
│   ├── __tests__/
│   │   ├── InvoiceList.test.tsx
│   │   └── Button.test.tsx
├── lib/
│   └── __tests__/
│       └── utils.test.ts
└── app/
    └── __tests__/
```

### 4.5 Test Coverage Goals

| Type | Target Coverage |
|------|-----------------|
| **Unit Tests** | 80%+ |
| **Integration Tests** | 60%+ |
| **E2E Tests** | Critical user flows |

**View Coverage Reports:**
```bash
# Backend
dotnet test /p:CollectCoverage=true
# Open: tests/coverage/index.html

# Frontend
npm run test:coverage
# Open: coverage/lcov-report/index.html
```

---

## 5. Common Tasks and Recipes

### 5.1 Add New Entity (Backend)

#### Step 1: Create Entity Class

```csharp
// src/JERP.Core/Entities/Inventory/Vendor.cs
public class Vendor : BaseEntity
{
    public string Name { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
    public VendorType Type { get; set; }
    
    // Navigation properties
    public ICollection<Product> Products { get; set; }
}
```

#### Step 2: Add DbSet to Context

```csharp
// src/JERP.Infrastructure/Data/JerpDbContext.cs
public DbSet<Vendor> Vendors { get; set; }
```

#### Step 3: Create Entity Configuration

```csharp
// src/JERP.Infrastructure/Data/Configurations/VendorConfiguration.cs
public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Name).IsRequired().HasMaxLength(200);
        builder.Property(v => v.ContactEmail).HasMaxLength(100);
        builder.HasIndex(v => v.Name);
    }
}

// Apply in DbContext
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfiguration(new VendorConfiguration());
}
```

#### Step 4: Create Migration

```bash
cd src/JERP.Api
dotnet ef migrations add AddVendorEntity --project ../JERP.Infrastructure
```

#### Step 5: Review Migration

```bash
# View the generated migration
cat ../JERP.Infrastructure/Migrations/XXXXXX_AddVendorEntity.cs

# Generate SQL script (optional)
dotnet ef migrations script --project ../JERP.Infrastructure
```

#### Step 6: Update Database

```bash
dotnet ef database update --project ../JERP.Infrastructure
```

#### Step 7: Create Repository Interface

```csharp
// src/JERP.Core/Interfaces/IVendorRepository.cs
public interface IVendorRepository : IRepository<Vendor>
{
    Task<IEnumerable<Vendor>> GetByTypeAsync(VendorType type);
    Task<Vendor> GetByNameAsync(string name);
}
```

#### Step 8: Implement Repository

```csharp
// src/JERP.Infrastructure/Repositories/VendorRepository.cs
public class VendorRepository : Repository<Vendor>, IVendorRepository
{
    public VendorRepository(JerpDbContext context) : base(context) { }
    
    public async Task<IEnumerable<Vendor>> GetByTypeAsync(VendorType type)
    {
        return await _context.Vendors
            .Where(v => v.Type == type)
            .ToListAsync();
    }
    
    public async Task<Vendor> GetByNameAsync(string name)
    {
        return await _context.Vendors
            .FirstOrDefaultAsync(v => v.Name == name);
    }
}
```

#### Step 9: Register Repository

```csharp
// src/JERP.Api/Program.cs
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
```

### 5.2 Add New API Endpoint

#### Step 1: Create DTO

```csharp
// src/JERP.Application/DTOs/Inventory/CreateVendorDto.cs
public class CreateVendorDto
{
    public string Name { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public VendorType Type { get; set; }
}

// src/JERP.Application/DTOs/Inventory/VendorDto.cs
public class VendorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContactEmail { get; set; }
    public VendorType Type { get; set; }
}
```

#### Step 2: Create Validator

```csharp
// src/JERP.Application/Validators/CreateVendorValidator.cs
public class CreateVendorValidator : AbstractValidator<CreateVendorDto>
{
    public CreateVendorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
        
        RuleFor(x => x.ContactEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.ContactEmail));
    }
}
```

#### Step 3: Create Service Method

```csharp
// src/JERP.Application/Services/IVendorService.cs
public interface IVendorService
{
    Task<VendorDto> CreateAsync(CreateVendorDto dto);
    Task<VendorDto> GetByIdAsync(int id);
    Task<IEnumerable<VendorDto>> GetAllAsync();
}

// src/JERP.Application/Services/VendorService.cs
public class VendorService : IVendorService
{
    private readonly IVendorRepository _repository;
    
    public VendorService(IVendorRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<VendorDto> CreateAsync(CreateVendorDto dto)
    {
        var vendor = new Vendor
        {
            Name = dto.Name,
            ContactEmail = dto.ContactEmail,
            Type = dto.Type
        };
        
        await _repository.AddAsync(vendor);
        
        return new VendorDto
        {
            Id = vendor.Id,
            Name = vendor.Name,
            ContactEmail = vendor.ContactEmail,
            Type = vendor.Type
        };
    }
}
```

#### Step 4: Create Controller

```csharp
// src/JERP.Api/Controllers/VendorController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VendorController : ControllerBase
{
    private readonly IVendorService _service;
    
    public VendorController(IVendorService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Get all vendors
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VendorDto>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var vendors = await _service.GetAllAsync();
        return Ok(vendors);
    }
    
    /// <summary>
    /// Create a new vendor
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(VendorDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateVendorDto dto)
    {
        var vendor = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = vendor.Id }, vendor);
    }
    
    /// <summary>
    /// Get vendor by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VendorDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var vendor = await _service.GetByIdAsync(id);
        if (vendor == null)
            return NotFound();
        
        return Ok(vendor);
    }
}
```

#### Step 5: Register Service

```csharp
// src/JERP.Api/Program.cs
builder.Services.AddScoped<IVendorService, VendorService>();
```

#### Step 6: Test with Swagger

1. Run the API: `dotnet run`
2. Navigate to: https://localhost:7001/swagger
3. Find the `/api/vendor` endpoints
4. Test the endpoints

### 5.3 Add New React Component

#### Step 1: Create Component File

```tsx
// landing-page/components/inventory/VendorList.tsx
'use client';

import { useEffect, useState } from 'react';
import { getVendors } from '@/lib/api/vendor';
import { Vendor } from '@/types/models';
import { Button } from '@/components/ui/Button';

interface VendorListProps {
  onVendorSelect?: (vendor: Vendor) => void;
}

export function VendorList({ onVendorSelect }: VendorListProps) {
  const [vendors, setVendors] = useState<Vendor[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  useEffect(() => {
    async function fetchVendors() {
      try {
        const data = await getVendors();
        setVendors(data);
      } catch (err) {
        setError('Failed to load vendors');
        console.error(err);
      } finally {
        setLoading(false);
      }
    }
    
    fetchVendors();
  }, []);
  
  if (loading) {
    return <div className="animate-pulse">Loading vendors...</div>;
  }
  
  if (error) {
    return <div className="text-red-500">{error}</div>;
  }
  
  return (
    <div className="vendor-list">
      <h2 className="text-2xl font-bold mb-4">Vendors</h2>
      <div className="grid gap-4">
        {vendors.map(vendor => (
          <div 
            key={vendor.id} 
            className="border p-4 rounded-lg hover:shadow-lg transition-shadow"
          >
            <h3 className="font-semibold">{vendor.name}</h3>
            <p className="text-gray-600">{vendor.contactEmail}</p>
            {onVendorSelect && (
              <Button onClick={() => onVendorSelect(vendor)}>
                Select
              </Button>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}
```

#### Step 2: Create API Client Function

```typescript
// landing-page/lib/api/vendor.ts
import { apiClient } from './client';
import { Vendor, CreateVendorDto } from '@/types/models';

export async function getVendors(): Promise<Vendor[]> {
  const response = await apiClient.get<Vendor[]>('/api/vendor');
  return response.data;
}

export async function getVendorById(id: number): Promise<Vendor> {
  const response = await apiClient.get<Vendor>(`/api/vendor/${id}`);
  return response.data;
}

export async function createVendor(data: CreateVendorDto): Promise<Vendor> {
  const response = await apiClient.post<Vendor>('/api/vendor', data);
  return response.data;
}
```

#### Step 3: Add Types

```typescript
// landing-page/types/models.ts
export interface Vendor {
  id: number;
  name: string;
  contactEmail: string;
  contactPhone: string;
  type: VendorType;
}

export interface CreateVendorDto {
  name: string;
  contactEmail: string;
  contactPhone?: string;
  type: VendorType;
}

export enum VendorType {
  Cultivator = 'Cultivator',
  Manufacturer = 'Manufacturer',
  Supplier = 'Supplier'
}
```

#### Step 4: Use Component

```tsx
// landing-page/app/(dashboard)/inventory/vendors/page.tsx
import { VendorList } from '@/components/inventory/VendorList';

export default function VendorsPage() {
  return (
    <div className="container mx-auto py-8">
      <VendorList />
    </div>
  );
}
```

#### Step 5: Add Tests

```typescript
// landing-page/components/inventory/__tests__/VendorList.test.tsx
import { render, screen, waitFor } from '@testing-library/react';
import { VendorList } from '../VendorList';
import * as api from '@/lib/api/vendor';

jest.mock('@/lib/api/vendor');

describe('VendorList', () => {
  it('renders vendors after loading', async () => {
    const mockVendors = [
      { id: 1, name: 'Vendor 1', contactEmail: 'vendor1@example.com' },
      { id: 2, name: 'Vendor 2', contactEmail: 'vendor2@example.com' },
    ];
    
    (api.getVendors as jest.Mock).mockResolvedValue(mockVendors);
    
    render(<VendorList />);
    
    await waitFor(() => {
      expect(screen.getByText('Vendor 1')).toBeInTheDocument();
      expect(screen.getByText('Vendor 2')).toBeInTheDocument();
    });
  });
});
```

### 5.4 Database Migrations

#### Create New Migration

```bash
cd src/JERP.Api
dotnet ef migrations add MigrationName --project ../JERP.Infrastructure
```

#### Apply Migration

```bash
# Update to latest
dotnet ef database update --project ../JERP.Infrastructure

# Update to specific migration
dotnet ef database update SpecificMigrationName --project ../JERP.Infrastructure
```

#### Rollback Migration

```bash
# Rollback to previous migration
dotnet ef database update PreviousMigrationName --project ../JERP.Infrastructure
```

#### Remove Last Migration (if not applied)

```bash
dotnet ef migrations remove --project ../JERP.Infrastructure
```

#### View Migration SQL

```bash
# Generate SQL for all migrations
dotnet ef migrations script --project ../JERP.Infrastructure

# Generate SQL for specific range
dotnet ef migrations script FromMigration ToMigration --project ../JERP.Infrastructure
```

### 5.5 Debugging Tips

#### Backend Debugging (Visual Studio Code)

Create `.vscode/launch.json`:
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (web)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/src/JERP.Api/bin/Debug/net8.0/JERP.Api.dll",
      "args": [],
      "cwd": "${workspaceFolder}/src/JERP.Api",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      },
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  ]
}
```

**Steps:**
1. Set breakpoints in code (click left of line number)
2. Press F5 to start debugging
3. Breakpoints will pause execution
4. Use Debug Console to inspect variables

#### Frontend Debugging (VS Code)

1. Install "Debugger for Chrome" extension
2. Set breakpoints in TypeScript files
3. Run `npm run dev`
4. Press F5 to attach debugger
5. Use browser DevTools for additional debugging

#### Common Debugging Commands

```bash
# Check API logs
cd src/JERP.Api
dotnet run  # Watch console output

# Check frontend logs
cd landing-page
npm run dev  # Watch console output

# Test API with curl
curl -X GET https://localhost:7001/api/vendor -k

# Test API with Postman
# Import the Swagger JSON: https://localhost:7001/swagger/v1/swagger.json
```

### 5.6 Performance Tips

#### Backend Performance

**Use AsNoTracking for Read-Only Queries:**
```csharp
var products = await _context.Products
    .AsNoTracking()  // Improves performance for read-only
    .ToListAsync();
```

**Optimize Includes:**
```csharp
// Good: Only include what you need
var orders = await _context.Orders
    .Include(o => o.Customer)
    .Include(o => o.Items)
    .ToListAsync();

// Bad: Over-fetching
var orders = await _context.Orders
    .Include(o => o.Customer)
        .ThenInclude(c => c.Address)
        .ThenInclude(a => a.City)
    .ToListAsync();
```

**Use Pagination:**
```csharp
public async Task<PagedResult<Product>> GetProductsAsync(int page, int pageSize)
{
    var query = _context.Products.AsQueryable();
    var total = await query.CountAsync();
    
    var items = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    
    return new PagedResult<Product>
    {
        Items = items,
        TotalCount = total,
        Page = page,
        PageSize = pageSize
    };
}
```

**Use Caching:**
```csharp
// In-memory caching
private readonly IMemoryCache _cache;

public async Task<Product> GetProductAsync(int id)
{
    var cacheKey = $"product-{id}";
    
    if (!_cache.TryGetValue(cacheKey, out Product product))
    {
        product = await _context.Products.FindAsync(id);
        _cache.Set(cacheKey, product, TimeSpan.FromMinutes(5));
    }
    
    return product;
}
```

#### Frontend Performance

**Use React.memo for Expensive Components:**
```tsx
export const ExpensiveComponent = React.memo(({ data }) => {
  // Expensive rendering logic
  return <div>{/* ... */}</div>;
});
```

**Lazy Load Components:**
```tsx
import dynamic from 'next/dynamic';

const HeavyComponent = dynamic(() => import('./HeavyComponent'), {
  loading: () => <p>Loading...</p>,
});
```

**Optimize Images:**
```tsx
import Image from 'next/image';

<Image 
  src="/product.jpg" 
  alt="Product" 
  width={500} 
  height={300}
  priority  // For above-the-fold images
/>
```

### 5.7 Common Issues and Solutions

#### Issue: "Cannot connect to SQL Server"

**Solution:**
```bash
# 1. Check if SQL Server is running
sqlcmd -S localhost\SQLEXPRESS -Q "SELECT @@VERSION"

# 2. Verify connection string in appsettings.Development.json
# 3. Check firewall settings
# 4. Enable TCP/IP in SQL Server Configuration Manager
```

#### Issue: "Migration already applied"

**Solution:**
```bash
# View migration history
dotnet ef migrations list --project ../JERP.Infrastructure

# Rollback if needed
dotnet ef database update PreviousMigration --project ../JERP.Infrastructure

# Remove migration
dotnet ef migrations remove --project ../JERP.Infrastructure
```

#### Issue: "CORS error from frontend"

**Solution:**
```csharp
// In Program.cs, ensure CORS is configured correctly
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

// Use CORS in middleware pipeline
app.UseCors("AllowFrontend");
```

#### Issue: "401 Unauthorized" on API calls

**Solution:**
```typescript
// Ensure JWT token is included in request headers
import axios from 'axios';

const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

---

## 6. Additional Resources

### 6.1 Documentation

- [Architecture Documentation](docs/architecture/README.md)
- [Project Scope of Work](docs/SOW.md)
- [API Documentation](API-DOCUMENTATION.md)
- [Finance Module Implementation](FINANCE-MODULE-IMPLEMENTATION.md)
- [Inventory Module Implementation](INVENTORY-MODULE-IMPLEMENTATION.md)

### 6.2 Technology Documentation

**Backend:**
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [C# Documentation](https://docs.microsoft.com/dotnet/csharp)

**Frontend:**
- [Next.js Documentation](https://nextjs.org/docs)
- [React Documentation](https://react.dev)
- [TypeScript Documentation](https://www.typescriptlang.org/docs)
- [Tailwind CSS](https://tailwindcss.com/docs)

**Database:**
- [SQL Server Documentation](https://docs.microsoft.com/sql/sql-server)
- [T-SQL Reference](https://docs.microsoft.com/sql/t-sql/language-reference)

### 6.3 Tools

- [Postman](https://www.postman.com) - API testing
- [SQL Server Management Studio](https://docs.microsoft.com/sql/ssms) - Database management
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio) - Cross-platform DB tool

### 6.4 Getting Help

**Internal Resources:**
- Slack channel: `#jerp-dev`
- Weekly team sync: Mondays 10am
- Code review sessions: Wednesdays 2pm

**External Resources:**
- Stack Overflow
- GitHub Issues
- Team documentation wiki

---

## Conclusion

This onboarding guide should help you get started with JERP 3.0 development. Remember:

1. **Ask Questions** - Don't hesitate to reach out to the team
2. **Read the Code** - The best documentation is the code itself
3. **Follow Standards** - Maintain consistency with existing patterns
4. **Test Your Changes** - Always write tests for new features
5. **Document as You Go** - Update documentation when you make changes

Welcome to the team! 🎉

---

**Document Version:** 1.0  
**Last Updated:** February 5, 2026  
**Maintained By:** JERP Development Team
