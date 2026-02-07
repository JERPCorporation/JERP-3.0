# JERP 3.0

**Just Enough Resource Planning** - Enterprise-grade ERP system designed for the cannabis industry.

## 📚 Documentation

| Document | Description |
|----------|-------------|
| **[Architecture Documentation](docs/architecture/README.md)** | Complete system architecture, technology stack, security, and deployment |
| **[Scope of Work (SOW)](docs/SOW.md)** | Project scope, features, timeline, and resource requirements |
| **[Developer Onboarding](docs/ONBOARDING.md)** | Setup instructions, development workflow, and common tasks |
| **[API Documentation](API-DOCUMENTATION.md)** | API endpoints and usage |
| **[Finance Module](FINANCE-MODULE-IMPLEMENTATION.md)** | Finance module implementation details |
| **[Inventory Module](INVENTORY-MODULE-IMPLEMENTATION.md)** | Inventory module implementation details |
| **[Testing Guide](TESTING-GUIDE.md)** | Testing strategy and guidelines |

## 🚀 Quick Start

### For Developers
See the [Developer Onboarding Guide](docs/ONBOARDING.md) for complete setup instructions.

**Quick Setup:**
```bash
# 1. Clone repository
git clone https://github.com/ninoyerbas/JERP-3.0.git
cd JERP-3.0

# 2. Setup backend
cd src/JERP.Api
dotnet restore
dotnet ef database update --project ../JERP.Infrastructure
dotnet run

# 3. Setup frontend (in new terminal)
cd landing-page
npm install
npm run dev
```

### For Business Users
- **Demo:** Contact ichbincesartobar@yahoo.com
- **Pricing:** https://jerp.io/pricing
- **Support:** ichbincesartobar@yahoo.com

## 💡 Key Features

- **Finance Management** - Chart of Accounts, AP/AR, 280E compliance
- **Inventory Management** - Real-time tracking, batch/lot tracking, METRC integration
- **Point of Sale** - Cannabis-specific POS with age verification and purchase limits
- **Compliance** - METRC API integration, audit trails, state reporting
- **Human Resources** - Employee management, time tracking, role-based access
- **Analytics** - Custom reports, dashboards, KPI tracking

## 🏗️ Technology Stack

**Frontend:** Next.js 16, React 18, TypeScript, TailwindCSS  
**Backend:** ASP.NET Core 8.0, C#, Entity Framework Core  
**Database:** Microsoft SQL Server 2019+  
**Authentication:** JWT, NextAuth.js  
**Cloud:** Azure / AWS (Docker ready)

Enterprise Resource Planning with Payroll Module

## 📜 License & Copyright

**JERP 3.0 is proprietary software.** All rights reserved.

```
Copyright © 2026 Julio Cesar Mendez Tobar
JERP™ is a trademark of Julio Cesar Mendez Tobar
```

### Usage Rights

- ✅ **SaaS Customers:** Full access via subscription at https://jerp.io/pricing
- ✅ **Evaluation:** Contact ichbincesartobar@yahoo.com for demo access
- ❌ **Copying/Distribution:** Prohibited without written permission
- ❌ **Reverse Engineering:** Prohibited
- ❌ **Commercial Use:** Requires valid subscription

### Source Code Access

This repository is **closed source**. Access is restricted to:
- Authorized developers under NDA
- Enterprise customers for security audits (on-site only, under NDA)
- Contractors working on the project (with signed agreements)

**Unauthorized copying, modification, or distribution is prohibited and will be prosecuted.**

See [LICENSE.md](LICENSE.md) for complete terms.

### Contact

- **Licensing inquiries:** ichbincesartobar@yahoo.com
- **Enterprise/white-label:** ichbincesartobar@yahoo.com
- **Security issues:** ichbincesartobar@yahoo.com
- **General support:** ichbincesartobar@yahoo.com
- **Alternative contact:** ichbincesartobar@gmail.com

---

## 🗄️ Database Configuration

JERP 3.0 uses **Microsoft SQL Server** as its database.

### Requirements
- SQL Server Express 2019 or later (free)
- OR SQL Server Developer/Standard/Enterprise Edition

### Connection String
Default configuration uses SQL Server Express with Windows Authentication:
```
Server=localhost\SQLEXPRESS;Database=JERP3_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

### Setup
1. **Install SQL Server Express** from: https://www.microsoft.com/sql-server/sql-server-downloads
2. **Create database:** `CREATE DATABASE JERP3_DB`
3. **Run migrations:** 
   ```bash
   cd src/JERP.Api
   dotnet ef database update --project ../JERP.Infrastructure
   ```

### Creating Fresh Migrations (if needed)
```bash
cd src/JERP.Api
dotnet ef migrations add InitialCreate --project ../JERP.Infrastructure
dotnet ef database update --project ../JERP.Infrastructure
```

---

## 📜 License & Copyright

**JERP 3.0 - Enterprise Resource Planning System**

**Copyright © 2026 Julio Cesar Mendez Tobar. All Rights Reserved.**

This software is proprietary and confidential. Unauthorized copying, distribution, modification, or use is strictly prohibited without the express written permission of the copyright holder.

### Author

**Julio Cesar Mendez Tobar**  
Software Architect & Developer  
Email: ichbincesartobar@yahoo.com  
Alternative: ichbincesartobar@gmail.com

For licensing inquiries, please contact: ichbincesartobar@yahoo.com

---

Built with passion and dedication in 2026.

