# JERP 3.0

Enterprise Resource Planning with Payroll Module

## 📜 License & Copyright

**JERP 3.0 is proprietary software.** All rights reserved.

```
Copyright © 2026 ninoyerbas
JERP™ is a trademark of ninoyerbas
```

### Usage Rights

- ✅ **SaaS Customers:** Full access via subscription at https://jerp.io/pricing
- ✅ **Evaluation:** Contact sales@jerp.io for demo access
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

- **Licensing inquiries:** licensing@jerp.io
- **Enterprise/white-label:** sales@jerp.io
- **Security issues:** security@jerp.io
- **General support:** support@jerp.io

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
