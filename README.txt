==============================================================================
JERP 2.0 - Enterprise Resource Planning System
Installation and Quick Start Guide
==============================================================================

WHAT'S INCLUDED
---------------
✓ Payroll Module with CA & Federal tax calculations
✓ Compliance Engine with 7+ labor law rules
✓ Employee Management with full CRUD operations
✓ Timesheet Management with automatic overtime
✓ Professional pay stub PDF generation
✓ Windows Desktop Application with modern UI
✓ RESTful API with Swagger documentation
✓ PostgreSQL database with Entity Framework Core

SYSTEM REQUIREMENTS
-------------------
- Windows 10 or Windows 11 (64-bit)
- .NET 8.0 Runtime or SDK
- PostgreSQL 14+ database server
- 4 GB RAM minimum
- 500 MB disk space

INSTALLATION OPTIONS
--------------------
1. Complete Installation (Recommended)
   - Desktop application
   - API server
   - Database configuration

2. Desktop Only
   - Connects to existing API server

3. API Server Only
   - For server deployments

FIRST RUN SETUP
---------------
1. Launch JERP Desktop application
2. Login with default credentials:
   Username: admin
   Password: Admin@123

3. IMPORTANT: Change the admin password immediately!

4. Configure your company information:
   - Navigate to Settings
   - Enter company details
   - Set up departments

PAYROLL QUICK START
-------------------
1. Add Employees:
   - Go to Employees → Add New
   - Enter employee details
   - Set hourly rate or salary
   - Configure tax withholdings

2. Record Time:
   - Go to Timesheets → Add Entry
   - Enter clock in/out times
   - System calculates overtime automatically
   - Submit for approval

3. Process Payroll:
   - Go to Payroll → Create Pay Period
   - Select dates and frequency
   - Click "Process Payroll"
   - System calculates taxes and deductions
   - Review and approve
   - Download pay stubs (PDF)

COMPLIANCE FEATURES
-------------------
✓ California overtime rules (1.5x after 8h, 2x after 12h)
✓ Federal FLSA overtime (1.5x after 40h/week)
✓ Meal break requirements (30 min for 5+ hours)
✓ Rest break requirements (10 min per 4 hours)
✓ Minimum wage validation (CA $16/hr, Federal $7.25/hr)
✓ Child labor restrictions
✓ Real-time violation detection

DATABASE BACKUP
---------------
Regular backups are essential! Use PostgreSQL's pg_dump:

  pg_dump -U postgres -d jerpdb > backup_2024-01-01.sql

To restore:

  psql -U postgres -d jerpdb < backup_2024-01-01.sql

CONFIGURATION FILES
-------------------
Desktop: %APPDATA%\JERP\appsettings.json
API: C:\Program Files\JERP\API\appsettings.json

Registry Settings: HKEY_CURRENT_USER\Software\JERP

TROUBLESHOOTING
---------------
Problem: Cannot connect to API
Solution: Check API URL in Settings, ensure API service is running

Problem: Login fails
Solution: Verify database connection, check credentials

Problem: Payroll calculations incorrect
Solution: Verify employee tax withholdings and deduction configurations

Problem: PDF generation fails
Solution: Ensure QuestPDF license is set to Community in API configuration

SUPPORT
-------
Email: support@jerp.com
Website: https://www.jerp.com
Documentation: https://docs.jerp.com

For bug reports and feature requests, please visit:
https://github.com/ninoyerbas/JERP-3.0/issues

==============================================================================
Copyright (c) 2024 JERP Corporation. All rights reserved.
Version 2.0.0
==============================================================================
