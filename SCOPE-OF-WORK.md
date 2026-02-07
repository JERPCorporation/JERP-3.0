# JERP 3.0 - Scope of Work

## Table of Contents
1. [Executive Summary](#executive-summary)
2. [Project Overview](#project-overview)
3. [Feature Breakdown by Module](#feature-breakdown-by-module)
4. [Development Phases and Milestones](#development-phases-and-milestones)
5. [Resource Requirements](#resource-requirements)
6. [Timeline Estimates](#timeline-estimates)
7. [Success Criteria](#success-criteria)

---

## Executive Summary

**JERP 3.0** (Just Enterprise Resource Planning) is a comprehensive Enterprise Resource Planning (ERP) system designed specifically for cannabis businesses and general enterprises. The system provides integrated modules for Finance & Accounting (including Payroll, Accounts Payable/Receivable, Chart of Accounts, Journal Entries), Inventory Management, Sales Order Management, Purchasing, and Cannabis Compliance tracking.

Built with modern web technologies including **Next.js 16**, **React 18**, **ASP.NET Core 8.0**, **Entity Framework Core**, and **SQL Server 2022**, JERP 3.0 offers scalability, regulatory compliance support, and multi-tenant capabilities suitable for businesses of all sizes.

### Key Differentiators

1. **Cannabis Industry Focus**: Built-in support for 280E tax compliance, Metrc/BioTrack integration, batch tracking
2. **Modern Technology Stack**: Latest .NET and React frameworks with TypeScript
3. **Flexible Deployment**: Docker containers, cloud-ready, or on-premise
4. **Multi-Tenant Architecture**: Single codebase serves multiple companies
5. **Comprehensive Financial Management**: Full double-entry accounting with FASB ASC topic support
6. **Desktop Application**: Cross-platform desktop app for offline capabilities

### Target Market

- **Primary**: Cannabis dispensaries, cultivators, and processors
- **Secondary**: General small-to-medium businesses requiring ERP functionality
- **Tertiary**: Enterprise customers needing customization and white-label solutions

---

## Project Overview

### Vision Statement

To provide cannabis businesses with a comprehensive, compliant, and user-friendly ERP system that simplifies operations, ensures regulatory compliance, and provides real-time financial insights.

### Business Objectives

1. **Streamline Operations**: Reduce manual data entry and automate workflows
2. **Ensure Compliance**: Maintain cannabis regulatory compliance (280E, Metrc)
3. **Financial Visibility**: Real-time financial reporting and analytics
4. **Scalability**: Support business growth from startup to enterprise
5. **Integration**: Connect with existing tools (payment processors, compliance systems)

### Core Capabilities

- **Financial Management**: Full general ledger, AP/AR, payroll, financial reporting
- **Inventory Control**: Product catalog, stock levels, transfers, batch tracking
- **Sales Management**: Customer management, sales orders, invoicing
- **Compliance Tracking**: Cannabis regulations, tax requirements, audit trails
- **User Management**: Role-based access control, multi-user support
- **Reporting & Analytics**: Dashboards, custom reports, data export

---

## Feature Breakdown by Module

### 1. Finance & Accounting Module

#### 1.1 Chart of Accounts
**Status**: ✅ Completed

**Features:**
- Hierarchical account structure (5 main types: Asset, Liability, Equity, Revenue, Expense)
- Account sub-types for detailed categorization (Current Asset, Fixed Asset, etc.)
- FASB ASC Topic and Subtopic assignment for audit compliance
- Account number assignment (flexible format)
- Account status management (Active/Inactive)
- Parent-child account relationships
- Account balance tracking (debit/credit)
- Normal balance type enforcement

**API Endpoints:**
- `GET /api/accounts` - List all accounts
- `GET /api/accounts/{id}` - Get account details
- `POST /api/accounts` - Create new account
- `PUT /api/accounts/{id}` - Update account
- `DELETE /api/accounts/{id}` - Delete account (if no transactions)

**Business Rules:**
- Cannot delete accounts with transaction history
- Account numbers must be unique within company
- Balance type must match account type (Asset = Debit, Liability = Credit)

#### 1.2 Journal Entries & General Ledger
**Status**: ✅ Completed

**Features:**
- Double-entry bookkeeping enforcement
- Journal entry creation with multiple line items
- Debit/credit balance validation (must balance to zero)
- Entry status workflow: Draft → Posted → (Voided)
- Entry source tracking (Manual, Payroll, Invoice, etc.)
- Reference number assignment
- Memo/description fields for audit trail
- Post date and transaction date tracking
- General Ledger auto-population on posting
- Void functionality with reversal entries

**API Endpoints:**
- `GET /api/journal-entries` - List journal entries
- `GET /api/journal-entries/{id}` - Get entry details
- `POST /api/journal-entries` - Create entry (Draft)
- `PUT /api/journal-entries/{id}` - Update entry (Draft only)
- `POST /api/journal-entries/{id}/post` - Post entry to GL
- `POST /api/journal-entries/{id}/void` - Void posted entry
- `GET /api/general-ledger` - Query GL entries

**Business Rules:**
- Debits must equal credits
- Only Draft entries can be modified
- Posted entries create GL entries
- Voiding creates reversing entry
- Cannot void voided entries

#### 1.3 Accounts Payable (AP)
**Status**: ✅ Completed

**Features:**
- Vendor management (contact info, payment terms, tax ID)
- Vendor bill entry with line items
- Bill approval workflow
- Payment processing and tracking
- Aging reports (30/60/90 days)
- Vendor statements
- 1099 tracking and reporting
- Payment batching
- Check printing support (planned)

**Vendor Entity:**
- Company name, contact person, email, phone
- Address (billing and shipping)
- Payment terms (Net 30, Net 60, etc.)
- Tax ID/EIN for 1099 reporting
- Vendor type classification
- Credit limit
- Active/Inactive status

**Bill Processing:**
- Bill number, date, due date
- Multiple line items (description, quantity, unit price, total)
- Tax calculation
- Bill status: Draft → Pending Approval → Approved → Paid
- Partial payments supported
- Late payment tracking

**API Endpoints:**
- `GET /api/vendors` - List vendors
- `POST /api/vendors` - Create vendor
- `GET /api/bills` - List bills
- `POST /api/bills` - Create bill
- `POST /api/bills/{id}/pay` - Record payment
- `GET /api/reports/ap-aging` - AP aging report

#### 1.4 Accounts Receivable (AR)
**Status**: ✅ Completed

**Features:**
- Customer management (contact info, credit terms, credit limit)
- Invoice generation with line items
- Invoice delivery (email, print)
- Payment receipt recording
- Aging reports (30/60/90 days)
- Customer statements
- Credit memo processing
- Payment reminders (planned)
- Online payment portal (planned)

**Customer Entity:**
- Company/individual name
- Contact information (email, phone)
- Billing and shipping addresses
- Payment terms
- Credit limit
- Customer type (Wholesale, Retail, etc.)
- Tax-exempt status
- Account balance

**Invoice Processing:**
- Invoice number, date, due date
- Line items (product/service, quantity, price, total)
- Tax calculation (sales tax)
- Invoice status: Draft → Sent → Partial Paid → Paid → Overdue
- Payment application
- Discounts and adjustments

**API Endpoints:**
- `GET /api/customers` - List customers
- `POST /api/customers` - Create customer
- `GET /api/invoices` - List invoices
- `POST /api/invoices` - Create invoice
- `POST /api/invoices/{id}/payments` - Record payment
- `GET /api/reports/ar-aging` - AR aging report

#### 1.5 Financial Reporting
**Status**: ✅ Completed

**Features:**
- Profit & Loss Statement (Income Statement)
  - Revenue by account
  - Expenses by account
  - Net income calculation
  - Period comparison (month-over-month, year-over-year)
  - Customizable date ranges
- Balance Sheet
  - Assets (current and fixed)
  - Liabilities (current and long-term)
  - Equity
  - Balance verification (Assets = Liabilities + Equity)
- Cash Flow Statement (planned)
- Trial Balance
- Account Activity Report
- Budget vs. Actual (planned)

**Report Features:**
- Date range selection
- Company filtering (multi-tenant)
- Export to PDF, Excel
- Drill-down to transaction detail
- Customizable columns

**API Endpoints:**
- `GET /api/reports/profit-loss` - P&L statement
- `GET /api/reports/balance-sheet` - Balance sheet
- `GET /api/reports/trial-balance` - Trial balance
- `GET /api/reports/account-activity/{accountId}` - Account detail

#### 1.6 Payroll Module
**Status**: ✅ Completed

**Features:**
- Employee management
  - Personal information (name, SSN, address)
  - Employment details (hire date, status, department)
  - Compensation (salary, hourly rate)
  - Tax withholding (federal, state, local)
  - Deductions (health insurance, 401k, etc.)
- Timesheet management
  - Clock in/out
  - Regular and overtime hours
  - Approval workflow
  - Integration with journal entries
- Payroll processing
  - Pay period setup (weekly, bi-weekly, semi-monthly, monthly)
  - Gross pay calculation
  - Tax withholding (FICA, federal, state)
  - Deduction calculation
  - Net pay computation
  - Check/direct deposit generation
- Payroll reports
  - Payroll summary
  - Employee earnings
  - Tax liability
  - Quarterly reports (941, 940)
  - W-2 generation
- Payroll to GL integration
  - Automatic journal entry creation
  - Expense allocation by department
  - Liability accounts for taxes and deductions

**API Endpoints:**
- `GET /api/employees` - List employees
- `POST /api/employees` - Create employee
- `GET /api/timesheets` - List timesheets
- `POST /api/timesheets` - Submit timesheet
- `POST /api/payroll/process` - Process payroll
- `GET /api/reports/payroll-summary` - Payroll report

#### 1.7 Cannabis Compliance - 280E Tax Tracking
**Status**: 🔄 In Progress

**Features:**
- COGS (Cost of Goods Sold) tracking
  - Direct costs only (deductible under 280E)
  - Material costs (cannabis product costs)
  - Direct labor (cultivation, production wages)
  - Manufacturing overhead (production facility costs)
- Non-deductible expense tracking
  - Administrative expenses
  - Marketing and advertising
  - Sales expenses
  - General operating costs
- FASB ASC Topic mapping
  - Account classification by ASC topic
  - Automated expense categorization
  - Audit-ready reporting
- 280E Reports
  - COGS vs. Non-deductible expenses summary
  - Account detail by 280E category
  - Year-end tax preparation support

**Business Rules:**
- Only direct costs deductible for cannabis touching operations
- Clear separation of COGS and operating expenses
- Documentation required for all expense classifications
- Multi-state compliance support (different rules by state)

---

### 2. Inventory Management Module

**Status**: ✅ Foundation Complete, 🔄 Advanced Features In Progress

#### 2.1 Product Catalog
**Features:**
- Product master data
  - SKU, name, description
  - Product category/type
  - Unit of measure (each, gram, ounce, pound)
  - Base cost and retail price
  - Product images
  - Cannabis-specific attributes:
    - Strain name
    - Strain type (Indica, Sativa, Hybrid)
    - THC percentage
    - CBD percentage
    - Terpene profile
- Product variants
  - Size variants (1g, 3.5g, 7g, 28g)
  - Packaging variants
- Product categories
  - Hierarchical category structure
  - Category-based reporting

**API Endpoints:**
- `GET /api/products` - List products
- `GET /api/products/{id}` - Get product details
- `POST /api/products` - Create product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product (if no inventory)

#### 2.2 Inventory Tracking
**Features:**
- Multi-location inventory
  - Warehouse/location management
  - Stock levels by location
  - Location transfers
- Inventory transactions
  - Receipt (from purchase orders)
  - Sale (from sales orders)
  - Adjustment (inventory count corrections)
  - Transfer (between locations)
  - Waste/disposal
- Real-time inventory balances
- Inventory valuation methods
  - FIFO (First In, First Out)
  - LIFO (Last In, First Out)
  - Weighted Average
  - Specific Identification
- Low stock alerts
  - Reorder point settings
  - Automatic notifications
- Batch/Lot tracking
  - Batch number assignment
  - Expiration date tracking
  - Batch genealogy (parent/child batches)
  - Recall capability

**Warehouse Entity:**
- Location name and code
- Address
- Type (retail, cultivation, processing, storage)
- Active status
- Manager assignment

**API Endpoints:**
- `GET /api/inventory/levels` - Current stock levels
- `GET /api/inventory/levels/{productId}` - Product stock by location
- `POST /api/inventory/adjustments` - Create adjustment
- `POST /api/inventory/transfers` - Create transfer
- `GET /api/inventory/transactions` - Transaction history

#### 2.3 Purchasing
**Features:**
- Purchase order creation
  - Vendor selection
  - Line items (product, quantity, price)
  - PO approval workflow
  - PO status tracking (Draft → Sent → Received → Closed)
- Goods receipt
  - Receive against PO
  - Partial receipts supported
  - Quality inspection notes
  - Automatic inventory update
  - Bill creation from receipt
- Vendor performance tracking
  - On-time delivery rate
  - Quality metrics
  - Pricing history

**API Endpoints:**
- `GET /api/purchase-orders` - List POs
- `POST /api/purchase-orders` - Create PO
- `POST /api/purchase-orders/{id}/receive` - Receive goods
- `GET /api/purchase-orders/{id}/receipt-history` - Receipt history

#### 2.4 Physical Inventory Counts
**Features:**
- Count creation and scheduling
- Count sheet generation (print or mobile)
- Count entry (expected vs. actual)
- Variance reporting
- Adjustment creation from count
- Count approval workflow
- Count history and audit trail

**API Endpoints:**
- `GET /api/physical-counts` - List counts
- `POST /api/physical-counts` - Create count
- `POST /api/physical-counts/{id}/submit` - Submit count
- `GET /api/physical-counts/{id}/variance` - Variance report

#### 2.5 Cannabis Compliance Features
**Features:**
- Metrc/BioTrack integration (planned)
  - Package ID tracking
  - Movement tracking (transfers, sales)
  - Destruction tracking
  - Real-time sync with state systems
- Batch/Lot tracking
  - Harvest batch tracking
  - Processing batch tracking
  - Testing results association
  - Traceability from seed to sale
- Compliance reporting
  - Inventory reconciliation
  - Destruction reports
  - Transfer manifests
  - State-required reports

---

### 3. Sales & Order Management Module

**Status**: ⏳ Planned (Foundation in Progress)

#### 3.1 Sales Orders
**Features:**
- Sales order creation
  - Customer selection
  - Product selection with pricing
  - Quantity availability check
  - Discount application
  - Tax calculation
  - Shipping information
- Order workflow
  - Draft → Approved → Picked → Packed → Shipped → Invoiced
  - Backorder handling
  - Order cancellation
- Order fulfillment
  - Pick list generation
  - Packing slip
  - Shipping label integration
  - Tracking number assignment
- Invoicing from sales orders
  - Automatic invoice generation
  - Invoice delivery
  - Payment tracking

**API Endpoints:**
- `GET /api/sales-orders` - List orders
- `POST /api/sales-orders` - Create order
- `PUT /api/sales-orders/{id}` - Update order
- `POST /api/sales-orders/{id}/fulfill` - Fulfill order
- `POST /api/sales-orders/{id}/invoice` - Generate invoice

#### 3.2 Point of Sale (POS)
**Status**: ⏳ Planned

**Features:**
- POS interface
  - Product search and selection
  - Cart management
  - Price overrides (with permission)
  - Discount application
  - Payment processing
  - Receipt printing
  - Email receipt
- Payment methods
  - Cash
  - Credit/debit card
  - Check
  - Store credit
  - Split payments
- Cannabis POS features
  - Age verification
  - Purchase limits enforcement
  - Medical vs. recreational tracking
  - Compliance reporting
- End-of-day reconciliation
  - Cash drawer counting
  - Payment method totals
  - Variance reporting
  - Bank deposit preparation

#### 3.3 Sales Analytics
**Features:**
- Sales dashboards
  - Daily/weekly/monthly sales
  - Sales by product
  - Sales by customer
  - Sales by location
- Product performance
  - Top-selling products
  - Slow-moving inventory
  - Margin analysis
- Customer analytics
  - Customer lifetime value
  - Purchase frequency
  - Customer segmentation
- Sales forecasting (planned)
  - Trend analysis
  - Seasonal patterns
  - Demand prediction

---

### 4. Admin Portal

**Status**: ✅ Core Features Complete

#### 4.1 User Management
**Features:**
- User account creation
  - Username, email, password
  - Employee association
  - Multi-company access
- Role assignment
  - Pre-defined roles (Admin, Finance Manager, etc.)
  - Custom role creation
  - Permission granularity
- User status management
  - Active/Inactive
  - Password reset
  - Account lockout
- User activity logging
  - Login history
  - Action audit trail
  - Failed login attempts

**API Endpoints:**
- `GET /api/users` - List users
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `POST /api/users/{id}/reset-password` - Reset password
- `GET /api/users/{id}/activity` - User activity log

#### 4.2 Role & Permission Management
**Features:**
- Role definition
  - Role name and description
  - Permission assignment
- Permission categories
  - Finance (view reports, create entries, post entries, void entries)
  - Inventory (view, adjust, transfer, receive)
  - Sales (create orders, fulfill orders, process refunds)
  - Admin (manage users, manage settings, view audit logs)
- Permission inheritance
  - Role-based permissions
  - User-level permission overrides
- Permission testing
  - Check user permissions
  - Simulate role permissions

**API Endpoints:**
- `GET /api/roles` - List roles
- `POST /api/roles` - Create role
- `PUT /api/roles/{id}` - Update role
- `GET /api/permissions` - List all permissions
- `POST /api/roles/{id}/permissions` - Assign permissions

#### 4.3 Company/Tenant Configuration
**Features:**
- Company profile
  - Company name, logo
  - Business address
  - Tax ID/EIN
  - Business type
  - License numbers (cannabis)
- Fiscal year settings
  - Start month
  - Period setup (monthly, quarterly)
- Tax configuration
  - Sales tax rates by location
  - Tax jurisdictions
  - Tax-exempt handling
- Regional settings
  - Currency
  - Date format
  - Number format
  - Language (future)

**API Endpoints:**
- `GET /api/companies/{id}` - Get company settings
- `PUT /api/companies/{id}` - Update company settings
- `POST /api/companies/{id}/logo` - Upload logo

#### 4.4 System Settings
**Features:**
- Application configuration
  - Email server settings (SMTP)
  - Notification preferences
  - Backup schedule
  - Session timeout
- Security settings
  - Password policy
  - Two-factor authentication (planned)
  - IP whitelist (planned)
- Integration settings
  - API keys for external services
  - Webhook configuration
  - OAuth connections

---

### 5. Partner Portal

**Status**: ✅ Core Features Complete

#### 5.1 Vendor Access
**Features:**
- Vendor login
  - Separate authentication from main system
  - Email invitation system
- Vendor dashboard
  - Open purchase orders
  - Recent invoices
  - Payment history
  - Account balance
- Document access
  - PO documents
  - Invoice PDFs
  - Payment receipts
  - Tax forms (W-9, 1099)

#### 5.2 Vendor Self-Service
**Features:**
- Profile management
  - Update contact information
  - Update banking information
  - Update W-9 information
- Order management
  - View PO details
  - Confirm order receipt
  - Upload packing slips
  - Update tracking numbers
- Invoice submission
  - Upload invoices
  - Link to POs
  - Track approval status
- Communication
  - Message center
  - Notification preferences
  - Support tickets

**API Endpoints:**
- `GET /api/partner/dashboard` - Vendor dashboard
- `GET /api/partner/orders` - Vendor's POs
- `GET /api/partner/invoices` - Vendor's invoices
- `POST /api/partner/invoices` - Submit invoice
- `GET /api/partner/payments` - Payment history

---

## Development Phases and Milestones

### Phase 1: Foundation (✅ Completed - Q4 2025)
**Duration**: 3 months  
**Status**: 100% Complete

**Deliverables:**
- ✅ Project architecture and technology stack selection
- ✅ Development environment setup
- ✅ Database schema design
- ✅ Authentication and authorization system
- ✅ User management
- ✅ Role-based access control
- ✅ Basic API infrastructure
- ✅ Frontend shell with navigation
- ✅ Docker deployment configuration
- ✅ CI/CD pipeline setup (basic)

**Key Technologies Implemented:**
- ASP.NET Core 8.0 Web API
- Entity Framework Core
- SQL Server 2022
- JWT authentication
- Next.js frontend
- Docker containerization

**Success Metrics:**
- All core infrastructure components functional
- Authentication working end-to-end
- Basic user can log in and access system
- Docker deployment successful

---

### Phase 2: Finance & Payroll (🔄 Near Complete - Q1 2026)
**Duration**: 3 months  
**Status**: 85% Complete  
**Expected Completion**: February 2026

**Completed:**
- ✅ Chart of Accounts (CRUD operations)
- ✅ Journal Entries (Draft, Post, Void)
- ✅ General Ledger
- ✅ Accounts Payable (Vendors, Bills)
- ✅ Accounts Receivable (Customers, Invoices)
- ✅ Payroll (Employees, Timesheets, Processing)
- ✅ Payroll to Finance integration
- ✅ Basic financial reports (P&L, Balance Sheet)
- ✅ FASB ASC Topic/Subtopic entities (PR #27)
- ✅ Finance UI with 9 tabs (Dashboard, Accounts, Journal, GL, AP, AR, Reports, etc.)
- ✅ Partner Portal implementation
- ✅ Admin Portal implementation

**In Progress:**
- 🔄 FASB ASC topic integration with Chart of Accounts
- 🔄 Advanced financial reporting (Cash Flow, Trial Balance)
- 🔄 Budget vs. Actual reporting
- 🔄 Complete Finance UI polish

**Remaining:**
- ⏳ Multi-currency support
- ⏳ Bank reconciliation
- ⏳ Recurring journal entries
- ⏳ Financial statement consolidation

**Success Metrics:**
- Complete financial cycle (invoice → payment → GL → reports)
- P&L and Balance Sheet accurately reflect transactions
- 280E compliance features functional
- Finance module ready for production use

---

### Phase 2.5: Security Hardening (⏳ Planned - February 2026)
**Duration**: 3-4 weeks  
**Status**: 0% Complete  
**Expected Completion**: Early March 2026  
**Priority**: Critical - Must complete before Phase 3

**Security Features to Implement:**

**Week 1: Security Headers & Response Protection**
- ⏳ Security headers middleware implementation
  - HSTS (HTTP Strict Transport Security)
  - X-Content-Type-Options: nosniff
  - X-Frame-Options: DENY
  - X-XSS-Protection: 1; mode=block
  - Content-Security-Policy (CSP)
  - Referrer-Policy
- ⏳ Secure cookie configuration
- ⏳ HTTPS enforcement

**Week 2: Rate Limiting & API Protection**
- ⏳ Rate limiting middleware (per endpoint, per user, per IP)
- ⏳ API throttling policies
- ⏳ Brute force protection (login attempts)
- ⏳ DDoS mitigation strategies
- ⏳ Request size limits

**Week 3: Enhanced Logging & Monitoring**
- ⏳ Serilog structured logging integration
- ⏳ Security event logging (failed logins, authorization failures)
- ⏳ Audit trail enhancements
- ⏳ Log aggregation and analysis setup
- ⏳ Alerting for suspicious activities

**Week 4: Backup Automation & JWT Hardening**
- ⏳ Automated database backup system
- ⏳ Backup verification and testing
- ⏳ JWT token security review
- ⏳ Token refresh mechanism improvements
- ⏳ Session management enhancements

**Currently Implemented Security Features:**
- ✅ JWT authentication
- ✅ Role-Based Access Control (RBAC)
- ✅ BCrypt password hashing (cost factor 11)
- ✅ CORS configuration
- ✅ FluentValidation for input validation
- ✅ EF Core parameterized queries (SQL injection prevention)
- ✅ Basic audit logging

**Security Testing:**
- ⏳ Penetration testing (external service)
- ⏳ Vulnerability scanning (OWASP ZAP, Dependabot)
- ⏳ Security code review
- ⏳ OWASP Top 10 compliance verification

**Success Metrics:**
- All OWASP Top 10 vulnerabilities addressed
- Security headers passing Mozilla Observatory A+ grade
- Rate limiting preventing brute force attacks
- Automated daily backups with weekly verification
- Comprehensive security logging in place
- Zero critical vulnerabilities in dependency scan

---

### Phase 3: Inventory Management (⏳ Planned - Q2 2026)
**Duration**: 2-3 months  
**Status**: Foundation 60% Complete  
**Expected Start**: April 2026

**Foundation Complete:**
- ✅ Product entities (Product, ProductCategory, ProductBatch)
- ✅ Inventory entities (InventoryLevel, InventoryTransaction)
- ✅ Warehouse entities
- ✅ Purchase Order entities
- ✅ Stock Receipt entities
- ✅ Physical Count entities

**To Be Implemented:**
- ⏳ Product catalog UI (create, edit, search products)
- ⏳ Inventory dashboard (stock levels, low stock alerts)
- ⏳ Inventory adjustment UI
- ⏳ Stock transfer workflow
- ⏳ Purchase order workflow
- ⏳ Goods receipt processing
- ⏳ Physical count workflow
- ⏳ Batch/lot tracking UI
- ⏳ Cannabis-specific attributes (strain, THC/CBD %)
- ⏳ Inventory valuation reports
- ⏳ Inventory aging reports
- ⏳ ABC analysis

**Success Metrics:**
- Full inventory lifecycle (PO → Receipt → Stock → Sale → COGS)
- Accurate inventory balances across multiple locations
- Low stock alerts functional
- Batch traceability working
- Cannabis attributes tracked

---

### Phase 4: Sales & POS (⏳ Planned - Q3 2026)
**Duration**: 3-4 months  
**Status**: 0% Complete  
**Expected Start**: July 2026

**Planned Deliverables:**
- Sales order creation and management
- Order fulfillment workflow
- POS interface (web-based)
- Payment processing integration (Stripe)
- Receipt generation and printing
- Sales dashboards and reports
- Customer analytics
- Inventory reservation system
- Backorder handling
- Cannabis compliance in POS (age verification, limits)

**Integration Points:**
- Sales orders update inventory
- Sales create AR invoices
- Payments update GL
- POS transactions sync to main system

**Success Metrics:**
- Complete sales transaction (order → fulfillment → invoice → payment)
- POS processes sale in under 30 seconds
- Accurate inventory updates from sales
- Compliance rules enforced in POS

---

### Phase 5: Advanced Features (⏳ Planned - Q4 2026 - Q1 2027)
**Duration**: 4-6 months  
**Status**: 0% Complete  
**Expected Start**: October 2026

#### 5.1 Multi-Tenant Architecture Enhancement
**Features:**
- Tenant provisioning automation
- Tenant-specific customization
- Data isolation verification
- Tenant billing and subscription management

#### 5.2 Advanced Reporting & BI
**Features:**
- Custom report builder
- Scheduled reports (email delivery)
- Power BI integration
- Data export to Excel, CSV, PDF
- Dashboards customization
- Real-time KPI widgets

#### 5.3 Mobile Application
**Features:**
- iOS and Android apps (React Native or Flutter)
- Mobile POS functionality
- Inventory counting on mobile
- Timesheet entry on mobile
- Approvals on mobile
- Push notifications

#### 5.4 API Integrations
**Features:**
- Metrc API integration (cannabis compliance)
- BioTrack API integration (alternative compliance)
- QuickBooks API (accounting sync)
- Stripe/PayPal (payment processing)
- Shopify/WooCommerce (e-commerce integration)
- Mailchimp (email marketing)

#### 5.5 Advanced Compliance
**Features:**
- Multi-state compliance support
- Automated compliance reporting
- Audit trail enhancements
- Document management system
- Electronic signature support

**Success Metrics:**
- At least 3 external integrations live
- Mobile app published to app stores
- Custom reports in use by customers
- Multi-tenant system serving 10+ companies

---

## Resource Requirements

### Development Team

#### Core Team (Current)
- **1 Full-Stack Developer**: Architecture, backend, frontend, DevOps
  - Skills: C#, .NET Core, React, TypeScript, SQL, Docker
  - Responsibilities: Overall development, code reviews, deployment
  - Allocation: 100%

#### Recommended Team (Scaling)
- **1 Backend Developer**: .NET Core, C#, EF Core, SQL Server
  - Focus: API development, business logic, database optimization
  - Allocation: 100%
- **1 Frontend Developer**: React, TypeScript, Next.js, Tailwind CSS
  - Focus: UI/UX implementation, frontend architecture
  - Allocation: 100%
- **1 DevOps Engineer (Part-time)**: Docker, CI/CD, cloud infrastructure
  - Focus: Deployment automation, monitoring, scaling
  - Allocation: 50%
- **1 QA Engineer (Part-time)**: Testing, automation, quality assurance
  - Focus: Test case development, automated testing, bug tracking
  - Allocation: 50%
- **1 Product Manager (Part-time)**: Requirements, prioritization, stakeholder management
  - Focus: Feature definition, roadmap, user feedback
  - Allocation: 25-50%

#### Optional/Future Roles
- **1 UI/UX Designer**: Design system, mockups, user research
- **1 Technical Writer**: Documentation, user guides, help system
- **1 Cannabis Compliance Expert**: Regulatory guidance, compliance validation

### Infrastructure Requirements

#### Development Environment
- **Developer Workstations**: 
  - Windows or macOS machines
  - 16GB+ RAM, SSD storage
  - Visual Studio 2022 or VS Code
  - Docker Desktop

#### Staging Environment
- **Application Server**:
  - 4 vCPUs, 8GB RAM
  - Ubuntu Server or Windows Server
  - Docker or Kubernetes
- **Database Server**:
  - SQL Server Express (free) or Standard edition
  - 2 vCPUs, 8GB RAM, 100GB SSD
- **Web Server/Proxy**:
  - NGINX on Linux or IIS on Windows
  - SSL certificate (Let's Encrypt or commercial)

#### Production Environment
- **Application Servers (Load Balanced)**:
  - 2-3 instances
  - 4 vCPUs, 8GB RAM each
  - Container orchestration (Docker Swarm or Kubernetes)
- **Database Server (Primary)**:
  - SQL Server Standard or Enterprise
  - 8 vCPUs, 32GB RAM, 500GB SSD
  - High availability configuration (Always On)
- **Database Server (Read Replica)**:
  - For reporting queries
  - 4 vCPUs, 16GB RAM, 500GB SSD
- **Load Balancer**:
  - NGINX Plus or cloud load balancer (AWS ALB, Azure Load Balancer)
- **File Storage**:
  - 500GB+ for documents, backups
  - Cloud blob storage (AWS S3, Azure Blob) or NAS
- **Cache Layer (Planned)**:
  - Redis cluster
  - 2 instances, 4GB RAM each

### Cloud Hosting Options

#### Option 1: AWS
- **Compute**: EC2 instances (t3.medium or larger)
- **Database**: RDS for SQL Server
- **Storage**: S3 for file storage
- **CDN**: CloudFront
- **Estimated Cost**: $500-1,500/month (depending on scale)

#### Option 2: Azure
- **Compute**: Azure App Service or VMs
- **Database**: Azure SQL Database
- **Storage**: Azure Blob Storage
- **CDN**: Azure CDN
- **Estimated Cost**: $500-1,500/month

#### Option 3: Self-Hosted
- **Hosting**: On-premise servers or colocation
- **Initial Investment**: $10,000-20,000 (hardware)
- **Ongoing**: $200-500/month (bandwidth, power, cooling)
- **Best For**: Customers requiring on-premise deployment

### Third-Party Services

#### Required Services
- **SSL Certificates**: 
  - Let's Encrypt (free) or commercial ($50-200/year)
- **Email Service (SMTP)**:
  - SendGrid, AWS SES, or Mailgun
  - Cost: $10-50/month
- **Monitoring & Logging**:
  - Application Insights, Datadog, or ELK Stack
  - Cost: $50-200/month

#### Cannabis Compliance APIs
- **Metrc API**: 
  - State-specific fees
  - Cost: Varies by state ($500-2,000/year per license)
- **BioTrack API**:
  - Alternative to Metrc in some states
  - Cost: Similar to Metrc

#### Payment Processing
- **Stripe**:
  - 2.9% + $0.30 per transaction
  - No monthly fees
- **Cannabis-Specific Processors**:
  - Higher fees (3.5-5% + $0.50)
  - Compliance support included

### Software Licenses

#### Development Tools (Per Developer)
- **Visual Studio Professional**: $499/year (or VS Community free)
- **JetBrains Rider**: $149/year (optional)
- **GitHub**: Free (public repos) or $4/user/month (private)

#### Production Licenses
- **SQL Server Standard**: $3,717 (2-core license) or Azure SQL (consumption-based)
- **SQL Server Enterprise**: $13,748 (2-core license) - for large deployments
- **Windows Server**: $972 (if not using Linux)

---

## Timeline Estimates

### Overall Project Timeline

| Phase | Duration | Status | Dates |
|-------|----------|--------|-------|
| Phase 1: Foundation | 3 months | ✅ Complete | Oct 2025 - Dec 2025 |
| Phase 2: Finance & Payroll | 3 months | 🔄 85% Complete | Jan 2026 - Feb 2026 |
| Phase 2.5: Security Hardening | 3-4 weeks | ⏳ Planned | Feb 2026 - Early Mar 2026 |
| Phase 3: Inventory Management | 3 months | ⏳ Planned | Mar 2026 - Jun 2026 |
| Phase 4: Sales & POS | 4 months | ⏳ Planned | Jul 2026 - Oct 2026 |
| Phase 5: Advanced Features | 6 months | ⏳ Planned | Oct 2026 - Mar 2027 |

**Total Estimated Timeline**: 20 months (Oct 2025 - March 2027)

### Milestone Dates

| Milestone | Target Date | Status |
|-----------|-------------|--------|
| Alpha Release (Core Finance) | March 2026 | 🔄 In Progress |
| Beta Release (Finance + Inventory) | June 2026 | ⏳ Planned |
| V1.0 Release (Finance + Inventory + Sales) | October 2026 | ⏳ Planned |
| V1.5 Release (Mobile App + Integrations) | March 2027 | ⏳ Planned |
| V2.0 Release (Full Feature Set) | September 2027 | ⏳ Planned |

### Sprint Cadence
- **Sprint Length**: 2 weeks
- **Sprints per Phase**: 6-12 sprints
- **Release Cycle**: End of each phase
- **Hotfix Releases**: As needed for critical bugs

---

## Success Criteria

### Phase 2 Success Criteria (Near Complete)
- ✅ All finance entities created and tested
- ✅ Double-entry accounting enforced
- ✅ Financial reports (P&L, Balance Sheet) accurate
- ✅ Finance UI functional with all 9 tabs
- ✅ Partner Portal implemented
- ✅ Admin Portal implemented
- 🔄 FASB ASC topics integrated
- ⏳ User acceptance testing passed
- ⏳ Performance benchmarks met (<200ms API response time)

### Phase 2.5 Success Criteria (Security Hardening)
- ⏳ All security headers implemented and tested
- ⏳ Rate limiting active on all API endpoints
- ⏳ Serilog structured logging configured
- ⏳ Automated daily database backups running
- ⏳ JWT token security hardened
- ⏳ Zero critical or high-severity vulnerabilities
- ⏳ Security testing completed (OWASP Top 10)
- ⏳ Mozilla Observatory grade A or higher

### Phase 3 Success Criteria
- Full inventory CRUD operations
- Multi-location inventory tracking
- Purchase order to goods receipt workflow complete
- Batch/lot tracking functional
- Low stock alerts working
- Cannabis attributes tracked
- Integration with Finance (COGS posting)

### Phase 4 Success Criteria
- Sales order workflow complete
- POS functional (basic version)
- Payment processing integrated
- Sales reports available
- Integration with Inventory and Finance

### Overall Project Success Criteria
- **Functionality**: All planned features implemented and tested
- **Performance**: API response <500ms, page load <2s
- **Reliability**: 99.5% uptime in production
- **Security**: No critical vulnerabilities, passing security audit
- **Compliance**: Metrc integration working, 280E reports accurate
- **User Adoption**: 10+ companies using the system
- **User Satisfaction**: NPS score >50
- **Support**: <24 hour response time on support tickets
- **Documentation**: Complete user guides and API documentation

---

## Risks and Mitigation Strategies

### Technical Risks

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| Database performance issues with large datasets | High | Medium | Proper indexing, query optimization, read replicas |
| Integration complexity with Metrc/BioTrack | High | High | Start early, allocate time for testing, use staging environments |
| Multi-tenant data isolation bugs | Critical | Low | Thorough testing, code reviews, query filter validation |
| Security vulnerabilities | Critical | Medium | Regular security audits, penetration testing, code scanning |

### Business Risks

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| Regulatory changes in cannabis industry | High | High | Monitor regulations, flexible architecture, rapid updates |
| Competition from established ERP systems | Medium | High | Focus on cannabis niche, superior UX, competitive pricing |
| Low user adoption | High | Medium | User training, excellent support, gather feedback, iterate |
| Scope creep delaying launch | High | Medium | Strict prioritization, MVP approach, phased releases |

### Resource Risks

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| Key developer leaves | High | Medium | Documentation, code reviews, knowledge sharing |
| Budget overruns | Medium | Medium | Regular budget reviews, cost controls, prioritize features |
| Timeline delays | Medium | High | Buffer time in estimates, agile approach, frequent releases |

---

## Conclusion

JERP 3.0 represents a comprehensive ERP solution tailored for the cannabis industry while remaining flexible enough for general businesses. With a clear phased approach, strong technical foundation, and focus on compliance and usability, the project is well-positioned for success.

The current status (Phase 2, 85% complete) demonstrates significant progress, with the Finance and Payroll modules, Partner Portal, and Admin Portal successfully implemented. Before moving to Phase 3, a critical security hardening phase (Phase 2.5) will address remaining security gaps and ensure production readiness.

**Next Steps (February 2026):**
1. Complete Phase 2 (Finance FASB ASC integration, final UI polish)
2. Execute Phase 2.5: Security Hardening (Critical Priority)
   - Implement security headers and rate limiting
   - Deploy Serilog structured logging
   - Establish automated backup system
   - Complete security testing and vulnerability remediation
3. Begin Phase 3 (Inventory Management) in March 2026
4. Continue gathering user feedback and refining features
5. Maintain focus on code quality, testing, and documentation

**Security Priority:**
Security hardening (Phase 2.5) is a non-negotiable prerequisite before Phase 3. This ensures the system meets enterprise security standards and protects sensitive financial and operational data.

For detailed architecture information, see [ARCHITECTURE.md](ARCHITECTURE.md).  
For developer setup instructions, see [ONBOARDING.md](ONBOARDING.md).  
For security information, see [SECURITY.md](SECURITY.md).
