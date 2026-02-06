# JERP 3.0 - Project Scope of Work (SOW)

## Document Information

| Field | Value |
|-------|-------|
| **Project Name** | JERP 3.0 (Just Enough Resource Planning) |
| **Document Version** | 1.0 |
| **Last Updated** | February 5, 2026 |
| **Document Owner** | Julio Cesar Mendez Tobar |
| **Status** | Active Development |

---

## 1. Executive Summary

### 1.1 Project Overview

**JERP 3.0 (Just Enough Resource Planning)** is a comprehensive, industry-specific Enterprise Resource Planning (ERP) system designed exclusively for the cannabis industry. The platform provides end-to-end business management capabilities including finance, inventory, compliance, point-of-sale, and human resources functionality.

### 1.2 Purpose and Objectives

The primary purpose of JERP 3.0 is to provide cannabis businesses with a unified platform that addresses the unique challenges of operating in a highly regulated industry. Key objectives include:

- **Compliance-First Design:** Built-in METRC integration and seed-to-sale tracking
- **Financial Accuracy:** 280E tax compliance for cannabis businesses
- **Operational Efficiency:** Streamlined workflows for dispensaries, cultivators, and manufacturers
- **Scalability:** Support for multi-location operations and enterprise-scale businesses
- **Real-Time Visibility:** Comprehensive reporting and analytics across all business functions

### 1.3 Target Users

**Primary Users:**
- Cannabis dispensaries (retail operations)
- Cannabis cultivators (grow operations)
- Cannabis manufacturers (processors and extractors)
- Vertically integrated cannabis businesses
- Cannabis distribution companies

**User Roles:**
- Budtenders (POS operations)
- Inventory managers
- Compliance officers
- Finance/accounting staff
- Operations managers
- Business owners and executives

### 1.4 Key Differentiators

| Feature | JERP 3.0 | Generic ERP Systems |
|---------|----------|---------------------|
| **280E Compliance** | Native support for non-deductible expense tracking | Manual tracking required |
| **METRC Integration** | Built-in seed-to-sale tracking | No integration |
| **Cannabis-Specific Features** | Batch tracking, waste management, compliance reporting | Not available |
| **Industry Knowledge** | Workflows designed for cannabis operations | Generic workflows |
| **Regulatory Compliance** | Automated state reporting | Manual compliance processes |
| **Point of Sale** | Cannabis-specific (age verification, purchase limits) | Generic retail POS |

### 1.5 Business Value

- **Reduced Compliance Risk:** Automated tracking and reporting reduces regulatory violations
- **Cost Savings:** Single platform eliminates need for multiple disconnected systems
- **Operational Efficiency:** Streamlined workflows save 10-15 hours per week
- **Financial Accuracy:** 280E compliance tracking can save tens of thousands in tax penalties
- **Business Insights:** Real-time analytics enable data-driven decision making
- **Scalability:** Supports business growth from single location to multi-state operations

---

## 2. Feature Breakdown by Module

### 2.1 Finance Module

The Finance Module provides comprehensive accounting and financial management capabilities with specific support for cannabis industry requirements, including IRS Section 280E compliance.

#### 2.1.1 Chart of Accounts (COA) with FASB ASC Tracking

**Features:**
- FASB ASC (Accounting Standards Codification) compliant account structure
- Pre-configured cannabis industry COA templates
- Multi-level account hierarchy (Assets, Liabilities, Equity, Revenue, Expenses)
- Cost of Goods Sold (COGS) vs. non-deductible expense classification
- Custom account creation and modification
- Account status management (active/inactive)
- Account notes and descriptions

**Implementation Details:**
- Account number format: XXX-XXX-XXXX (Category-Subcategory-Account)
- 280E compliance tags for expense accounts
- FASB ASC reference codes for each account
- Support for multiple entities/companies

#### 2.1.2 General Ledger and Journal Entries

**Features:**
- Double-entry accounting system
- Manual journal entries
- Automated journal entries from transactions
- Period closing procedures
- Year-end closing
- Audit trail for all entries
- Reversing entries
- Recurring journal entries

**Capabilities:**
- Real-time GL balance updates
- Trial balance reports
- Account reconciliation
- Period comparison
- Inter-company transactions (planned)

#### 2.1.3 Accounts Payable (AP)

**Vendor Management:**
- Vendor master data (name, contact, payment terms)
- Vendor categories (cultivators, suppliers, service providers)
- W-9 document tracking
- Vendor performance metrics
- 1099 reporting support

**Bill Management:**
- Bill entry and approval workflow
- Three-way matching (PO, receipt, invoice)
- Bill scheduling and payment terms
- Early payment discounts
- Late fee calculations
- Bill attachments (invoices, receipts)

**Payment Processing:**
- Multiple payment methods (check, ACH, wire, cash)
- Batch payment processing
- Payment approval workflow
- Payment scheduling
- Vendor payment history
- Check printing
- Payment reconciliation

**Reports:**
- Aged AP report (30/60/90 days)
- Vendor balances
- Cash requirements forecast
- Payment history by vendor
- 1099 report

#### 2.1.4 Accounts Receivable (AR)

**Customer Management:**
- Customer master data
- Credit limits and terms
- Customer categories (retail, wholesale, medical, recreational)
- Customer payment history
- Credit hold management

**Invoice Management:**
- Invoice creation and customization
- Recurring invoices
- Invoice templates
- Invoice approval workflow
- Invoice delivery (email, print)
- Payment tracking
- Partial payment handling
- Credit memos and refunds

**Collections Management:**
- Automated payment reminders
- Collection workflow
- Payment plans
- Write-off management
- Bad debt tracking

**Reports:**
- Aged AR report (30/60/90 days)
- Customer balances
- Collection effectiveness
- Days Sales Outstanding (DSO)
- Revenue by customer

#### 2.1.5 Financial Reports

**Standard Reports:**
- Income Statement (Profit & Loss)
  - Standard P&L
  - 280E compliant P&L (separate COGS and non-deductible expenses)
  - Multi-period comparison
  - Budget vs. actual
  - By location/department
- Balance Sheet
  - Standard balance sheet
  - Comparative balance sheets
  - Common-size balance sheet
- Cash Flow Statement
  - Operating activities
  - Investing activities
  - Financing activities
  - Direct vs. indirect method
- Trial Balance
- General Ledger Detail
- Account Activity Report

**Custom Reports:**
- Report builder with drag-and-drop interface
- Custom filters and grouping
- Scheduled report generation
- Export to Excel, PDF, CSV
- Email distribution lists

#### 2.1.6 280E Compliance Tracking

**Features:**
- Automatic expense categorization (COGS vs. non-deductible)
- 280E compliant financial statements
- Non-deductible expense tracking by category
- Tax adjustment calculations
- Documentation for IRS audits
- Historical compliance data

**Expense Categories:**
- **Deductible (COGS):** Direct materials, direct labor, inventory costs, cultivation costs
- **Non-Deductible:** Marketing, administrative salaries, rent (non-grow), general office expenses

#### 2.1.7 Multi-Entity Support

**Features:**
- Multiple legal entities within one system
- Consolidated financial reporting
- Inter-company transactions
- Entity-level permissions
- Separate books for each entity
- Roll-up reporting

---

### 2.2 Inventory Module

The Inventory Module provides comprehensive inventory tracking with cannabis-specific features including batch/lot tracking, METRC integration, and compliance reporting.

#### 2.2.1 Product Catalog Management

**Features:**
- Product master data (SKU, name, description)
- Product categories and subcategories
  - Flower (Indica, Sativa, Hybrid)
  - Concentrates (Shatter, Wax, Live Resin)
  - Edibles (Gummies, Chocolates, Beverages)
  - Pre-rolls
  - Topicals
  - Accessories
- Product variants (size, strain, potency)
- Pricing tiers (retail, wholesale, medical, bulk)
- Product images and documentation
- Cannabinoid profiles (THC, CBD, CBN content)
- Terpene profiles
- Lab testing results
- Product packaging requirements
- Compliance labeling

#### 2.2.2 Inventory Tracking (Real-Time)

**Features:**
- Real-time inventory levels across all locations
- Perpetual inventory system
- Stock movements (receipts, sales, transfers, adjustments)
- Inventory valuation (FIFO, LIFO, Weighted Average)
- Minimum stock levels and reorder points
- Inventory alerts (low stock, expiring products)
- Stock counting and cycle counts
- Inventory adjustments with reason codes
- Inventory aging reports

**Locations:**
- Warehouse locations
- Retail dispensary
- Grow facilities
- Manufacturing/processing areas
- Quarantine areas

#### 2.2.3 Batch/Lot Tracking

**Features:**
- Unique batch/lot numbers for traceability
- Batch creation from cultivation or manufacturing
- Batch splitting and merging
- Parent-child batch relationships
- Batch genealogy (seed-to-sale tracking)
- Harvest batch tracking
- Processing batch tracking
- Batch testing and COA (Certificate of Analysis)
- Batch expiration dates
- Batch recall management

**Batch Data:**
- Plant source information
- Harvest date
- Processing date
- Testing results
- METRC tracking numbers
- Compliance status

#### 2.2.4 METRC Integration (Seed-to-Sale)

**Features:**
- Bidirectional METRC API integration
- Automated data synchronization
- Plant tracking from clone/seed to harvest
- Package tracking from creation to sale
- Tag management (METRC RFID tags)
- Location tracking
- Transfer manifests
- Testing integration
- Destruction/waste tracking
- Compliance reporting to state authorities

**METRC Data Synced:**
- Strains
- Inventory locations
- Plant batches
- Packages
- Transfers
- Sales
- Lab results
- Waste

#### 2.2.5 Transfer Manifests

**Features:**
- Create transfer manifests for product movement
- Multi-stop route support
- Driver and vehicle information
- Manifest approvals
- Digital signatures
- Real-time tracking (planned)
- Manifest templates
- METRC manifest synchronization
- Transfer history and audit trail

**Transfer Types:**
- Facility to facility (same owner)
- Wholesale transfers
- Return transfers
- Testing laboratory transfers

#### 2.2.6 Waste Tracking

**Features:**
- Waste entry and categorization
- Waste methods (compost, landfill, incineration)
- Waste weight tracking
- Waste approval workflow
- Photo documentation
- METRC waste synchronization
- Regulatory compliance
- Waste reports

**Waste Categories:**
- Plant waste (stalks, roots, stems)
- Product waste (expired, damaged, failed testing)
- Production waste (trim, shake)
- Returns and recalls

#### 2.2.7 Compliance Reporting

**Features:**
- Daily sales reports
- Inventory reconciliation reports
- Plant tracking reports
- Destruction reports
- Testing reports
- Transfer reports
- State-specific compliance reports
- Automated report submission (where applicable)

#### 2.2.8 Barcode/QR Code Generation

**Features:**
- Generate barcodes for products and packages
- QR codes for batch information
- Mobile scanning app support
- Label printing integration
- METRC tag printing
- Custom label templates
- Batch printing

---

### 2.3 Point of Sale (POS) Module

The POS Module provides a complete retail solution designed specifically for cannabis dispensaries, with age verification, purchase limits, and seamless integration with inventory and compliance systems.

#### 2.3.1 Customer Check-In

**Features:**
- Customer registration
- ID scanning and age verification (21+/18+ medical)
- Customer search and lookup
- Check-in queue management
- Medical card verification
- Appointment scheduling (planned)
- Customer notifications (text/email when ready)
- Visit history

#### 2.3.2 Product Browsing and Search

**Features:**
- Product catalog display with images
- Search by product name, SKU, strain, category
- Filter by THC/CBD content, price, effects
- Product details (cannabinoids, terpenes, effects)
- Product recommendations
- Featured products and promotions
- Low stock indicators
- Lab results display

#### 2.3.3 Cart Management

**Features:**
- Add products to cart
- Quantity adjustments
- Remove items from cart
- Cart notes and special instructions
- Save cart for later
- Cart validation (purchase limits, age restrictions)
- Price calculations with taxes
- Apply discounts and promotions
- Split payment support

#### 2.3.4 Payment Processing

**Supported Payment Methods:**
- Cash
- Debit card (PIN-based)
- Credit card (where permitted)
- Check (rare)
- Cashless ATM
- Store credit
- Loyalty points redemption

**Features:**
- Integrated payment terminal support
- Cash drawer management
- Change calculation
- Payment split (multiple payment methods)
- Refund processing
- Tip handling
- Gift card sales and redemption (planned)

#### 2.3.5 Receipt Printing

**Features:**
- Digital receipts (email/SMS)
- Printed receipts
- Customizable receipt templates
- Compliance information on receipts
- Return policy
- Barcode/QR code for receipt lookup
- Receipt reprinting
- Itemized pricing with taxes

#### 2.3.6 Age Verification

**Features:**
- ID scanning (driver's license, state ID, passport)
- Automatic age calculation
- Medical card validation
- Out-of-state ID recognition
- Fake ID detection
- Age verification logs
- Compliance alerts

#### 2.3.7 Purchase Limits

**Features:**
- State-mandated purchase limits enforcement
- Daily limits (recreational vs. medical)
- Per-transaction limits
- Product category limits (flower, concentrate, edibles)
- Warning alerts when approaching limits
- Override capability for managers
- Purchase limit tracking across visits

**Example Limits:**
- Recreational: 1 oz flower, 5g concentrate, 500mg edibles per day
- Medical: 2.5 oz flower, 8g concentrate, 2000mg edibles per day

#### 2.3.8 Loyalty Programs

**Features:**
- Points-based rewards program
- Points accumulation on purchases
- Points redemption
- Tier-based programs (Bronze, Silver, Gold)
- Birthday rewards
- Referral bonuses
- Loyalty reports and analytics
- Integration with CRM

#### 2.3.9 Discounts and Promotions

**Features:**
- Percentage discounts
- Fixed amount discounts
- BOGO (Buy One Get One)
- Bundle deals
- Happy hour specials
- First-time customer discounts
- Medical patient discounts
- Veteran/senior discounts
- Coupon codes
- Automatic discount application
- Promotion scheduling

---

### 2.4 Compliance Module

The Compliance Module ensures regulatory adherence through METRC integration, state reporting, and comprehensive audit trails.

#### 2.4.1 METRC API Integration

**Features:**
- Real-time bidirectional synchronization
- Automated data push to METRC
- METRC data pull and reconciliation
- Error handling and retry logic
- API rate limit management
- Compliance status dashboard
- Historical data access

#### 2.4.2 State Reporting Requirements

**Reports:**
- Daily sales summary
- Monthly inventory reconciliation
- Plant tracking reports
- Transfer manifests
- Destruction/waste reports
- Testing results
- Employee registry
- Security incident reports

**State-Specific Features:**
- California (BCC, DCC, CDPH reporting)
- Colorado (MED reporting)
- Oregon (OLCC reporting)
- Washington (WSLCB reporting)
- Other states as applicable

#### 2.4.3 License Tracking

**Features:**
- Business license management
- License expiration alerts
- License renewal tracking
- Employee license/badge tracking
- Vendor license verification
- License document storage
- Multi-license support (multiple locations)

#### 2.4.4 Audit Trail

**Features:**
- Comprehensive activity logging
- User action tracking
- Data change history
- Timestamp and user identification
- Tamper-proof audit logs
- Audit log search and filtering
- Audit log export for inspections
- Compliance officer access

**Events Tracked:**
- User login/logout
- Data creation, modification, deletion
- Financial transactions
- Inventory movements
- Compliance submissions
- Failed access attempts
- System configuration changes

#### 2.4.5 Document Management

**Features:**
- Secure document storage
- Document categorization (licenses, COAs, SOPs, policies)
- Version control
- Access permissions
- Document expiration alerts
- Full-text search
- Document templates
- Digital signatures (planned)

#### 2.4.6 Regulatory Alerts

**Features:**
- Automated compliance alerts
- License expiration warnings
- Inventory discrepancy alerts
- Failed METRC sync notifications
- Regulatory update notifications
- Customizable alert rules
- Email/SMS notifications
- Alert dashboard

---

### 2.5 Human Resources Module

The HR Module provides employee management, time tracking, and compliance features specific to the cannabis industry.

#### 2.5.1 Employee Records

**Features:**
- Employee master data
- Personal information
- Contact information
- Emergency contacts
- Employment history
- Job title and department
- Pay rate and compensation
- Tax withholding (W-4)
- Direct deposit information
- Employee status (active, terminated, on leave)

#### 2.5.2 Time Tracking

**Features:**
- Clock in/clock out
- Time clock terminal
- Mobile time tracking
- Break tracking
- Overtime calculation
- Shift scheduling
- Timesheet approval
- Time off requests
- PTO accrual tracking
- Attendance reports

#### 2.5.3 Payroll Integration

**Features:**
- Export time data for payroll processing
- Integration with QuickBooks, ADP, Gusto
- Pay stub generation
- W-2 generation
- Payroll reports
- Tax withholding calculations
- Direct deposit file generation

#### 2.5.4 Role and Permission Management

**Features:**
- Role-based access control (RBAC)
- Pre-defined roles (Admin, Manager, Budtender, etc.)
- Custom role creation
- Granular permissions
- Module-level access control
- Data-level access control (view own vs. view all)
- Permission inheritance
- Role assignment

**Example Roles:**
- **System Administrator:** Full access
- **Owner/Manager:** All modules except system configuration
- **Accountant:** Finance module only
- **Inventory Manager:** Inventory and compliance modules
- **Budtender:** POS module, limited inventory viewing
- **Compliance Officer:** Compliance module, audit trail access

#### 2.5.5 Training Records

**Features:**
- Training course tracking
- Certification management
- Compliance training requirements
- Training completion status
- Training expiration alerts
- Training materials library
- Training assignments
- Training reports

**Required Training:**
- State-mandated responsible vendor training
- Internal SOPs
- Safety training
- Security training
- Compliance training

#### 2.5.6 Background Checks

**Features:**
- Background check request workflow
- Integration with background check providers
- Background check status tracking
- Document storage
- Recertification alerts
- Compliance reporting

---

### 2.6 Reporting & Analytics Module

The Reporting & Analytics Module provides comprehensive business intelligence and data visualization capabilities.

#### 2.6.1 Custom Report Builder

**Features:**
- Drag-and-drop report designer
- Multiple data sources
- Custom fields and calculations
- Grouping and sorting
- Filtering and parameters
- Conditional formatting
- Charts and visualizations
- Save and share reports
- Report templates

#### 2.6.2 Dashboard Widgets

**Available Widgets:**
- Sales summary (daily, weekly, monthly)
- Top selling products
- Revenue trends
- Inventory levels
- Low stock alerts
- Compliance status
- Customer metrics
- Financial KPIs
- Custom metrics

**Dashboard Features:**
- Customizable layout
- Drag-and-drop widgets
- Real-time data updates
- Time period selection
- Export to PDF/Excel
- Role-based dashboards

#### 2.6.3 Data Export

**Formats:**
- PDF (formatted reports)
- Excel (raw data and formatted)
- CSV (data import/export)
- JSON (API integrations)

**Features:**
- Scheduled exports
- Email delivery
- FTP upload
- Cloud storage integration

#### 2.6.4 Scheduled Reports

**Features:**
- Schedule report generation
- Recurring schedules (daily, weekly, monthly)
- Multiple recipients
- Conditional delivery (only if data matches criteria)
- Report history
- Failed delivery alerts

#### 2.6.5 KPI Tracking

**Key Performance Indicators:**
- **Financial KPIs:**
  - Gross profit margin
  - Net profit margin
  - Revenue growth rate
  - Operating expenses ratio
  - Days Sales Outstanding (DSO)
  - Days Payable Outstanding (DPO)
  
- **Inventory KPIs:**
  - Inventory turnover
  - Days Inventory Outstanding (DIO)
  - Stock-out rate
  - Inventory accuracy
  
- **Sales KPIs:**
  - Average transaction value
  - Items per transaction
  - Conversion rate
  - Sales per square foot
  - Customer retention rate
  
- **Compliance KPIs:**
  - METRC sync success rate
  - Inventory discrepancy rate
  - License compliance rate
  - Audit findings

---

## 3. Development Phases and Milestones

### Phase 1: Foundation (Weeks 1-4)

**Objectives:**
- Establish development infrastructure
- Design database schema
- Implement authentication and authorization
- Create base entity framework
- Set up API structure

**Deliverables:**
- [ ] Repository setup with CI/CD pipeline
- [ ] Database schema design (ER diagrams)
- [ ] SQL Server database creation
- [ ] Entity Framework Core models
- [ ] JWT authentication implementation
- [ ] Role-based authorization
- [ ] API project structure
- [ ] Swagger documentation
- [ ] Logging framework (Serilog)
- [ ] Error handling middleware
- [ ] Health check endpoints
- [ ] Unit test framework

**Success Criteria:**
- All developers can run the application locally
- Authentication and authorization work correctly
- API documentation is accessible via Swagger
- Database migrations run successfully

---

### Phase 2: Finance Module (Weeks 5-8)

**Objectives:**
- Implement core financial management functionality
- Build Chart of Accounts with 280E compliance
- Develop Accounts Payable and Receivable
- Create financial reporting

**Deliverables:**
- [ ] Chart of Accounts (COA) with FASB ASC structure
- [ ] General Ledger implementation
- [ ] Journal entry creation and posting
- [ ] Vendor management (AP)
- [ ] Bill entry and payment processing
- [ ] Customer management (AR)
- [ ] Invoice creation and payment tracking
- [ ] 280E expense categorization
- [ ] Financial reports (P&L, Balance Sheet, Cash Flow)
- [ ] Aged AP/AR reports
- [ ] Trial balance report
- [ ] Finance API endpoints
- [ ] Finance module unit tests

**Success Criteria:**
- Chart of Accounts follows FASB ASC standards
- Double-entry accounting maintains balanced books
- 280E compliant financial statements generated
- All financial reports are accurate

---

### Phase 3: Inventory Module (Weeks 9-12)

**Objectives:**
- Implement inventory management system
- Build product catalog
- Develop batch/lot tracking
- Integrate with METRC for compliance

**Deliverables:**
- [ ] Product catalog management
- [ ] Product categories and variants
- [ ] Real-time inventory tracking
- [ ] Stock movement tracking (receipts, sales, adjustments)
- [ ] Batch/lot tracking system
- [ ] Transfer manifest creation
- [ ] Waste tracking and reporting
- [ ] Barcode/QR code generation
- [ ] METRC API integration (basic)
  - [ ] Plant tracking
  - [ ] Package tracking
  - [ ] Sales reporting
- [ ] Inventory reports (on-hand, movements, aging)
- [ ] Low stock alerts
- [ ] Inventory API endpoints
- [ ] Inventory module unit tests

**Success Criteria:**
- Real-time inventory accuracy
- Batch traceability from seed to sale
- METRC integration passes test transactions
- Inventory reports match physical counts

---

### Phase 4: POS Module (Weeks 13-16)

**Objectives:**
- Build point-of-sale system for dispensaries
- Implement customer check-in and cart management
- Integrate payment processing
- Enforce purchase limits and age verification

**Deliverables:**
- [ ] Customer registration and check-in
- [ ] ID scanning and age verification
- [ ] Product browsing and search
- [ ] Shopping cart management
- [ ] Purchase limit enforcement
- [ ] Payment processing integration
  - [ ] Cash handling
  - [ ] Debit card support
  - [ ] Payment terminal integration
- [ ] Receipt generation (digital and print)
- [ ] Discount and promotion engine
- [ ] Loyalty program implementation
- [ ] POS dashboard for budtenders
- [ ] Shift management and cash drawer
- [ ] POS API endpoints
- [ ] POS module unit tests

**Success Criteria:**
- Complete a sale from check-in to receipt
- Age verification prevents underage sales
- Purchase limits enforced correctly
- Payment processing works with test accounts
- Receipts comply with state regulations

---

### Phase 5: Compliance & Advanced Features (Weeks 17-20)

**Objectives:**
- Complete METRC integration
- Build compliance reporting
- Implement document management
- Add human resources functionality

**Deliverables:**
- [ ] Full METRC bidirectional sync
- [ ] METRC error handling and reconciliation
- [ ] State-specific compliance reports
- [ ] License tracking and expiration alerts
- [ ] Comprehensive audit trail
- [ ] Document management system
- [ ] Regulatory alert system
- [ ] Employee management (HR)
- [ ] Time tracking and scheduling
- [ ] Training records
- [ ] Role and permission management
- [ ] Background check tracking
- [ ] Custom report builder
- [ ] Dashboard widgets
- [ ] Scheduled reports
- [ ] KPI tracking
- [ ] Compliance module unit tests

**Success Criteria:**
- METRC sync is reliable and bidirectional
- All required state reports can be generated
- Audit trail captures all required events
- Custom reports can be created without coding

---

### Phase 6: Testing & Refinement (Weeks 21-24)

**Objectives:**
- Comprehensive testing of all modules
- User acceptance testing (UAT)
- Performance optimization
- Bug fixes
- Documentation finalization

**Deliverables:**
- [ ] Integration test suite
- [ ] End-to-end test scenarios
- [ ] Performance testing and optimization
- [ ] Security testing and penetration testing
- [ ] Load testing
- [ ] User acceptance testing with stakeholders
- [ ] Bug fixes based on testing
- [ ] User documentation
- [ ] Administrator documentation
- [ ] API documentation refinement
- [ ] Deployment documentation
- [ ] Training materials
- [ ] Release notes
- [ ] Production deployment plan

**Success Criteria:**
- 90% unit test coverage
- All critical bugs resolved
- UAT sign-off from stakeholders
- API response times < 500ms for 95th percentile
- System handles 1000 concurrent users
- Documentation is complete and accurate
- Production deployment checklist completed

---

## 4. Resource Requirements

### 4.1 Development Team

| Role | Quantity | Time Commitment | Responsibilities |
|------|----------|-----------------|------------------|
| **Full-Stack Developer (Lead)** | 1 | Full-time | Architecture, code review, mentoring, critical features |
| **Frontend Developer** | 1 | Full-time | Next.js/React UI, POS interface, admin dashboards |
| **Backend Developer** | 1 | Full-time | C#/.NET API, business logic, EF Core |
| **Database Administrator** | 1 | Part-time (50%) | Schema design, optimization, migrations, backups |
| **QA Engineer** | 1 | Full-time | Test planning, manual testing, automated test creation |
| **DevOps Engineer** | 1 | Part-time (50%) | CI/CD, cloud infrastructure, monitoring |
| **UI/UX Designer** | 1 | Contract (200 hrs) | Wireframes, mockups, user flow design |
| **Project Manager** | 1 | Part-time (25%) | Planning, coordination, stakeholder communication |

**Total Team Size:** 6-8 people (5.25 FTE)

### 4.2 Infrastructure Requirements

#### Development Environment
- **Code Repository:** GitHub (private repository)
- **Development Servers:** Local development machines
- **Development Database:** SQL Server Developer Edition (free)
- **Development Tools:** Visual Studio 2022, VS Code, SQL Server Management Studio

#### Staging Environment
- **Hosting:** Azure App Service (Standard S2 tier)
- **Database:** Azure SQL Database (Standard S2, 50 DTU)
- **Storage:** Azure Blob Storage (Standard, 100 GB)
- **CDN:** Azure CDN (Standard Microsoft)
- **Estimated Monthly Cost:** $300-400

#### Production Environment
- **Hosting:** Azure App Service (Premium P2v2 tier) × 2 instances
- **Database:** Azure SQL Database (Standard S3, 100 DTU)
- **Storage:** Azure Blob Storage (Standard, 500 GB)
- **CDN:** Azure CDN (Standard Microsoft)
- **Redis Cache:** Azure Cache for Redis (Standard C1)
- **Application Insights:** Included with App Service
- **Estimated Monthly Cost:** $800-1000

#### Additional Services
- **Email:** SendGrid or Azure Communication Services
- **Payment Processing:** Stripe (transaction fees apply)
- **METRC API:** State-provided (license required)
- **SSL Certificates:** Let's Encrypt (free) or Azure SSL

### 4.3 Tools & Services

| Category | Tool/Service | Purpose | Cost |
|----------|--------------|---------|------|
| **Version Control** | GitHub | Code repository | $0 (free for public/private repos) |
| **Project Management** | Jira or Linear | Sprint planning, issue tracking | $10-20/user/month |
| **Design** | Figma | UI/UX design, prototyping | $12-45/user/month |
| **API Testing** | Postman | API development and testing | $0-12/user/month |
| **Error Tracking** | Sentry | Production error monitoring | $26-80/month |
| **Documentation** | Notion or Confluence | Team documentation | $8-10/user/month |
| **Communication** | Slack | Team communication | $7.25/user/month |
| **CI/CD** | GitHub Actions | Automated testing and deployment | Included with GitHub |

**Estimated Monthly Tool Cost:** $150-300

### 4.4 Software Licenses

| Software | License Type | Cost |
|----------|--------------|------|
| **Visual Studio 2022** | Professional or Community | $0-45/month per developer |
| **.NET 8.0 SDK** | Free | $0 |
| **SQL Server** | Express/Developer (dev), Standard (prod) | $0 (dev), $3,717 (prod, one-time) |
| **Windows Server** | N/A (using Azure PaaS) | Included in Azure costs |

---

## 5. Timeline Estimates

### 5.1 Overall Timeline

**Total Project Duration:** 24 weeks (6 months)

**Development:** 20 weeks
**Testing & Refinement:** 4 weeks

### 5.2 Milestone Schedule

| Week | Phase | Milestone | Deliverables |
|------|-------|-----------|--------------|
| **Week 4** | Foundation | Infrastructure Complete | Database, auth, API structure |
| **Week 8** | Finance | Finance Module MVP | COA, AP, AR, basic reports |
| **Week 12** | Inventory | Inventory Module MVP | Products, tracking, basic METRC |
| **Week 16** | POS | POS Module MVP | Full POS workflow functional |
| **Week 20** | Compliance | Compliance Module Complete | Full METRC, reports, HR |
| **Week 24** | Testing | Production-Ready Release | Complete, tested, documented system |

### 5.3 Critical Path

```
Foundation → Finance → Inventory → POS → Compliance → Testing → Production
```

**Dependencies:**
- Finance module requires Foundation
- Inventory requires Foundation
- POS requires Inventory
- Compliance requires Inventory
- Testing requires all modules

### 5.4 Risk Mitigation Timeline

**Buffer Time:** 2 weeks built into the 24-week schedule

**Risk Areas:**
- METRC API complexity (add 1-2 weeks if issues)
- Payment processing integration (add 1 week if issues)
- Performance optimization (add 1 week if needed)
- State-specific compliance variations (add 1 week per state beyond first)

### 5.5 Go-Live Strategy

**Soft Launch:** Week 24 (limited beta users)
**Public Launch:** Week 26 (after soft launch feedback)

**Launch Checklist:**
- [ ] All modules complete and tested
- [ ] UAT sign-off
- [ ] Security audit passed
- [ ] Performance benchmarks met
- [ ] Documentation complete
- [ ] Training materials ready
- [ ] Support processes in place
- [ ] Monitoring and alerting configured
- [ ] Backup and DR tested
- [ ] Compliance verification complete

---

## 6. Budget Estimate

### 6.1 Personnel Costs (24 weeks)

| Role | Rate | Hours/Week | Weeks | Total |
|------|------|------------|-------|-------|
| Full-Stack Lead | $100/hr | 40 | 24 | $96,000 |
| Frontend Dev | $85/hr | 40 | 24 | $81,600 |
| Backend Dev | $85/hr | 40 | 24 | $81,600 |
| DBA | $90/hr | 20 | 24 | $43,200 |
| QA Engineer | $70/hr | 40 | 24 | $67,200 |
| DevOps | $95/hr | 20 | 24 | $45,600 |
| UI/UX Designer | $80/hr | - | 200 hrs | $16,000 |
| Project Manager | $75/hr | 10 | 24 | $18,000 |
| **TOTAL PERSONNEL** | | | | **$449,200** |

### 6.2 Infrastructure Costs (6 months)

| Item | Monthly Cost | Months | Total |
|------|--------------|--------|-------|
| Development Environment | $100 | 6 | $600 |
| Staging Environment | $350 | 6 | $2,100 |
| Production Environment | $900 | 6 | $5,400 |
| Tools & Services | $250 | 6 | $1,500 |
| **TOTAL INFRASTRUCTURE** | | | **$9,600** |

### 6.3 Software & Licenses

| Item | Cost |
|------|------|
| Visual Studio licenses | $1,200 |
| Third-party libraries | $2,000 |
| SSL certificates | $0 (Let's Encrypt) |
| **TOTAL SOFTWARE** | **$3,200** |

### 6.4 Contingency

| Item | Cost |
|------|------|
| Contingency (10% of personnel) | $44,920 |

### 6.5 Total Project Budget

| Category | Amount |
|----------|--------|
| Personnel | $449,200 |
| Infrastructure | $9,600 |
| Software | $3,200 |
| Contingency | $44,920 |
| **TOTAL PROJECT COST** | **$506,920** |

---

## 7. Success Metrics

### 7.1 Functional Metrics

- [ ] All 6 modules implemented and functional
- [ ] 200+ API endpoints operational
- [ ] 90%+ unit test coverage
- [ ] 100% of critical user workflows working
- [ ] METRC integration passes compliance audit

### 7.2 Performance Metrics

- [ ] API response time < 500ms (95th percentile)
- [ ] Page load time < 2 seconds
- [ ] System uptime > 99.5%
- [ ] Database query time < 100ms (average)
- [ ] Support for 1000+ concurrent users

### 7.3 Quality Metrics

- [ ] < 1 critical bug per 1000 lines of code
- [ ] Code review completion rate 100%
- [ ] Zero security vulnerabilities (OWASP Top 10)
- [ ] Accessibility compliance (WCAG 2.1 Level AA)

### 7.4 Business Metrics

- [ ] Time to complete a sale: < 2 minutes
- [ ] Inventory accuracy: > 98%
- [ ] Compliance report generation: < 30 seconds
- [ ] User training time: < 4 hours
- [ ] Customer satisfaction: > 4.5/5

---

## 8. Risks and Mitigation

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| METRC API changes | High | Medium | Use versioned API, maintain fallback logic |
| State regulation changes | High | Medium | Modular compliance design, monitor regulatory updates |
| Payment processing issues | High | Low | Multiple payment provider options |
| Key developer leaves | High | Low | Code documentation, knowledge sharing, pair programming |
| Performance issues at scale | Medium | Medium | Performance testing throughout, scalable architecture |
| Security breach | High | Low | Security audits, penetration testing, best practices |
| Budget overrun | Medium | Medium | Bi-weekly budget reviews, scope management |
| Timeline delays | Medium | Medium | Agile methodology, regular check-ins, buffer time |

---

## 9. Post-Launch Support

### 9.1 Maintenance Plan

- **Bug fixes:** Ongoing as reported
- **Security patches:** Monthly or as critical issues arise
- **Feature enhancements:** Quarterly releases
- **Performance optimization:** Ongoing monitoring and tuning

### 9.2 Support Model

- **Tier 1:** Email support (response within 24 hours)
- **Tier 2:** Priority support for critical issues (response within 4 hours)
- **Tier 3:** Dedicated account manager for enterprise clients

### 9.3 Training and Documentation

- **User training:** 4-hour onboarding session
- **Administrator training:** 8-hour advanced session
- **Video tutorials:** 20+ videos covering all features
- **Knowledge base:** Searchable help articles
- **Release notes:** Published with each update

---

## 10. Conclusion

JERP 3.0 represents a comprehensive solution for cannabis businesses, addressing the unique challenges of operating in a highly regulated industry. The 24-week development timeline is aggressive but achievable with the proper team and resources. The modular architecture allows for phased delivery, with each module providing immediate value to users.

**Next Steps:**
1. Secure funding and resources
2. Assemble development team
3. Kick-off meeting and project planning
4. Begin Phase 1 development
5. Regular stakeholder updates and demos

For additional information, see:
- [Architecture Documentation](architecture/README.md)
- [Developer Onboarding Guide](ONBOARDING.md)
- [API Documentation](../API-DOCUMENTATION.md)

---

**Document Approval:**

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Project Owner | Julio Cesar Mendez Tobar | _________________ | _________ |
| Technical Lead | _________________ | _________________ | _________ |
| Stakeholder | _________________ | _________________ | _________ |
