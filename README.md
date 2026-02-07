# JERP 3.0

**Just Enough Resource Planning** - Construction ERP & Payroll System

---

## 🎯 Built by a Contractor, for Contractors

JERP 3.0 is an affordable, full-featured ERP system designed specifically for construction contractors. Unlike expensive enterprise solutions that cost $20,000-$50,000/year, JERP 3.0 offers professional tools at prices small contractors can afford.

**Developer:** Julio Cesar Mendez Tobar Jr.  
🎓 Business Administration (Universidad de El Salvador) | Construction PM (UC Davis)  
👷 10+ years construction experience | Self-taught developer

---

## 📚 Documentation

| Document | Description |
|----------|-------------|
| **[Pricing Guide](docs/PRICING_GUIDE.md)** | All subscription plans, features, and pricing |
| **[Business Model](docs/BUSINESS_MODEL.md)** | Revenue strategy, cost structure, and projections |
| **[License Setup](docs/LICENSE_SETUP.md)** | Licensing system setup and configuration |
| **[Stripe Integration](docs/STRIPE_INTEGRATION.md)** | Payment processing and subscription management |
| **[Architecture Documentation](docs/architecture/README.md)** | Complete system architecture, technology stack, security |
| **[Developer Onboarding](docs/ONBOARDING.md)** | Setup instructions, development workflow |
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
- **14-Day Free Trial:** Start at https://jerp3.com/trial
- **Pricing:** See [Pricing Guide](docs/PRICING_GUIDE.md)
- **Plans:** From $79/month (Starter) to $999/month (Ultimate)
- **Support:** support@jerp3.com
- **Developer Contact:** ichbincesartobar@yahoo.com

---

## 💰 Subscription Plans

JERP 3.0 uses **employee-based pricing** (not per-user), making it affordable and predictable:

| Plan | Price | Employees | Best For |
|------|-------|-----------|----------|
| **Starter** | $79/mo | Up to 3 | Solo contractors, small crews |
| **Small Business** | $149/mo | Up to 10 | Growing contractors |
| **Professional** ⭐ | $299/mo | Up to 50 | Established contractors (MOST POPULAR) |
| **Enterprise** | $599/mo | Up to 150 | Large contractors, multi-company |
| **Ultimate** | $999/mo | Unlimited | Major firms, custom development |

**Annual Plans:** Save 10% (about 2 months free!)

See the complete [Pricing Guide](docs/PRICING_GUIDE.md) for features and comparison.

---

## 💡 Key Features

### Core Accounting & Finance
- **Chart of Accounts** - FASB-compliant, construction-specific
- **AP/AR** - Accounts Payable & Receivable management
- **General Ledger** - Complete GL with audit trails
- **Bank Reconciliation** - Automated bank statement matching

### Payroll (Professional+)
- **Basic Payroll** - Time tracking, tax calculations (Small Business+)
- **Certified Payroll** - Davis-Bacon compliance (Professional+)
- **Prevailing Wage** - Multi-rate tracking (Professional+)
- **Union/Non-Union** - Classification management (Professional+)

### Job Costing
- **Basic Job Costing** - Track costs per project (Small Business+)
- **Advanced Job Costing** - Multi-phase, cost codes (Professional+)
- **Change Orders** - Change order tracking (Professional+)
- **AIA Billing** - G702/G703 progress billing (Professional+)

### Project Management (Professional+)
- **Equipment Tracking** - Asset and equipment management
- **Lien Waivers** - Automated lien waiver tracking
- **Subcontractor Management** - Sub contracts and payments
- **GPS Time Tracking** - Mobile time clock with geofencing

### Advanced Features
- **Multi-Company** - Manage multiple companies (Enterprise+)
- **White-Label** - Custom branding (Enterprise+)
- **Custom Development** - Tailored features (Ultimate only)
- **API Access** - Integration capabilities (Professional+)

### Mobile & Reporting
- **Mobile App** - iOS/Android field access (All plans)
- **Advanced Reporting** - Custom reports and dashboards (Professional+)
- **Analytics** - KPI tracking and insights (Enterprise+)

## 🏗️ Technology Stack

**Backend:** ASP.NET Core 8.0, C#, Entity Framework Core, SQL Server  
**Desktop:** WPF (.NET 8), ModernWPF UI, MVVM (CommunityToolkit.Mvvm)  
**Mobile:** .NET MAUI (Planned)  
**Authentication:** JWT with role-based permissions  
**Payment Processing:** Stripe for subscriptions  
**Database:** Microsoft SQL Server 2019+  
**Cloud:** Azure/AWS ready (Docker containers)

---

## 🔐 Security Status

**Current Security Posture: 65% Complete** (Updated: February 2026)

### ✅ Implemented Security Features

**Strong Foundation:**
- ✅ **JWT Authentication** - Industry-standard token-based authentication
- ✅ **Role-Based Access Control (RBAC)** - Granular permission management
- ✅ **BCrypt Password Hashing** - Work factor 12 with automatic salting
- ✅ **Multi-Tenant Isolation** - Query-level data segregation
- ✅ **CORS Protection** - Configured for secure cross-origin requests
- ✅ **Input Validation** - FluentValidation for all API inputs
- ✅ **SQL Injection Prevention** - EF Core parameterized queries
- ✅ **HTTPS/TLS** - Encrypted data in transit
- ✅ **Audit Logging** - Basic operation tracking

### ⏳ Upcoming Security Hardening (Phase 2.5 - February 2026)

**Critical Priority - Before Phase 3:**
- ⏳ **Security Headers** - HSTS, CSP, X-Frame-Options, X-Content-Type-Options
- ⏳ **Rate Limiting** - API throttling and brute force protection
- ⏳ **Enhanced Logging** - Serilog structured logging with security events
- ⏳ **Automated Backups** - Daily database backups with verification
- ⏳ **JWT Hardening** - Token refresh mechanism and improved session management
- ⏳ **Security Testing** - OWASP Top 10 compliance verification

### 🎯 Security Roadmap

```
February 2026:
├── Week 1: Security headers implementation
├── Week 2: Rate limiting and API protection
├── Week 3: Enhanced logging with Serilog
└── Week 4: Backup automation and JWT hardening
```

### 🛡️ Security Best Practices for Deployment

**For Production Deployments:**
1. Use HTTPS only (TLS 1.2+)
2. Configure strong JWT secret keys (256-bit minimum)
3. Enable automated database backups
4. Use environment variables for all secrets
5. Configure firewall rules to restrict database access
6. Enable SQL Server Transparent Data Encryption (TDE)
7. Monitor security logs for suspicious activity
8. Keep all dependencies updated

**For Development:**
1. Never commit secrets to version control
2. Use `.env` files for local configuration (not tracked in git)
3. Follow secure coding guidelines in [docs/ONBOARDING.md](docs/ONBOARDING.md)
4. Run security scans before committing code

### 📋 Vulnerability Reporting

**Found a security issue?** Please report responsibly:

- **Email:** ichbincesartobar@yahoo.com
- **Subject:** [SECURITY] Brief description
- **DO NOT** create public GitHub issues for security vulnerabilities

See [SECURITY.md](SECURITY.md) for our complete security policy and response process.

---

## 🔒 Licensing System

JERP 3.0 includes a complete licensing and subscription management system:

- **Trial System:** 14-day free trial with full Professional features
- **Employee-Based Pricing:** Fair pricing based on workforce size
- **Feature Gating:** Automatic feature access based on subscription
- **Stripe Integration:** Secure payment processing
- **Machine Binding:** License tied to hardware for security
- **Offline Grace Period:** 7 days offline operation allowed

See [License Setup Guide](docs/LICENSE_SETUP.md) for complete documentation.

---

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

