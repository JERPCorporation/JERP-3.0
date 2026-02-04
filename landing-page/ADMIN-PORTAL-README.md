# Admin Portal Implementation Guide

## Overview
This document describes the implementation of the comprehensive Admin Portal UI/UX for JERP 3.0 payroll system.

## Location
- **Main Component**: `/landing-page/app/admin/page.tsx`
- **Route**: `/admin`
- **Authentication**: Protected by NextAuth middleware

## Features

### 1. Overview Dashboard
The main dashboard provides system-wide metrics and monitoring:

- **System Metrics Cards**:
  - Active Users count with trend indicator
  - API Calls (24h) with percentage change
  - Average Response Time with performance trend
  - Error count (24h) with comparison

- **System Status Panel**:
  - API Server status
  - Database status
  - Authentication service status
  - Email Service status
  - Color-coded indicators (green = operational, orange = degraded)

- **Interactive Charts**:
  - API Usage trend chart (last 7 days) - Area chart
  - Error trend chart (last 7 days) - Line chart
  - Built with Recharts library

### 2. User Management
Complete user administration interface:

- **User Table Features**:
  - Avatar generation based on initials
  - Role-based color coding
  - Status indicators (Active/Suspended)
  - Last login tracking
  - Email display

- **User Roles**:
  - `super_admin` - Purple badge
  - `admin` - Blue badge
  - `payroll_manager` - Teal badge
  - `hr_manager` - Green badge
  - `accountant` - Orange badge

- **CRUD Operations** (UI placeholders):
  - Create User button
  - Edit button per user
  - Delete button per user

### 3. Audit Log Panel
Hash-chained immutable audit trail:

- **Audit Entry Information**:
  - Timestamp (precise to seconds)
  - User who performed action
  - Action type with color-coded badge
  - Resource affected
  - IP address
  - Cryptographic hash (first 12 characters)

- **Action Types**:
  - `payroll_processed` - Green
  - `employee_updated` - Blue
  - `compliance_check` - Orange
  - `user_created` - Teal
  - `deduction_modified` - Purple
  - `system_backup` - Gray

- **Export Functionality**:
  - "Export para Compliance" button for audit reporting

### 4. Tax Rate Configuration
Federal and state tax management:

- **Tax Types Displayed**:
  - Social Security (OASDI) - 6.2% on $168,600 wage base
  - Medicare (HI) - 1.45% no wage base limit
  - Additional Medicare - 0.9% on $200,000 threshold
  - FUTA - 0.6% on $7,000 wage base
  - CA SDI - 0.9% no wage base limit
  - CA State Income Tax - 9.3% (varies)

- **Information Columns**:
  - Tax name
  - Type (Federal/State) with color badge
  - Rate percentage
  - Wage base limit
  - Applicability (Employee/Employer/Both)
  - Edit action button

### 5. System Settings
Company and system configuration:

- **Configuration Fields**:
  - Company Name (text input)
  - Tax ID / EIN (text input)
  - Company Address (text input)
  - Payroll Frequency (dropdown: weekly, biweekly, monthly)
  - Fiscal Year Start (dropdown: January-December)
  - Time Zone (dropdown: PT, MT, CT, ET)
  - Currency (dropdown: USD, EUR, GBP)

- **Actions**:
  - Save Changes button (gradient green)
  - Cancel button

## Technology Stack

### Dependencies
- **recharts**: ^2.13.3 - For data visualization and charts
- **next**: ^16.1.6 - React framework
- **react**: ^18.2.0 - UI library
- **next-auth**: ^4.24.13 - Authentication

### Development Approach
- **Language**: TypeScript with strict mode
- **Styling**: Inline CSS-in-JS for component isolation
- **State Management**: React useState hooks
- **Rendering**: Client-side with "use client" directive
- **Data**: Mock data for initial development

## Security Implementation

### Authentication Protection
The admin portal is protected by NextAuth middleware:

```typescript
// In middleware.ts
export const config = {
  matcher: ['/partners/dashboard/:path*', '/admin/:path*'],
};
```

### Access Control
- Requires valid authentication session
- Redirects to `/partners/login` if not authenticated
- Future: Role-based access control (RBAC) to restrict features by user role

## Design System

### Color Palette
- **Primary Blue**: `#2563eb`
- **Success Green**: `#10b981`
- **Warning Orange**: `#f59e0b`
- **Danger Red**: `#ef4444`
- **Purple**: `#8b5cf6`
- **Teal**: `#14b8a6`

### Layout
- **Background**: Gradient from `#0f172a` to `#1e293b`
- **Cards**: Dark glass-morphism with gradient overlays
- **Borders**: Subtle with opacity `rgba(71, 85, 105, 0.3)`
- **Typography**: Sans-serif with multiple weight variants

### Responsive Design
- Mobile-first approach
- Grid layouts with `auto-fit` for responsive columns
- Flexible tables with horizontal scrolling on small screens

## Mock Data Structure

### Users
```typescript
{
  id: number;
  name: string;
  email: string;
  role: "super_admin" | "admin" | "payroll_manager" | "hr_manager" | "accountant";
  status: "active" | "suspended";
  lastLogin: string; // Format: "YYYY-MM-DD HH:mm"
  created: string; // Format: "YYYY-MM-DD"
}
```

### Audit Logs
```typescript
{
  id: number;
  timestamp: string; // Format: "YYYY-MM-DD HH:mm:ss"
  user: string; // email
  action: "payroll_processed" | "employee_updated" | "compliance_check" | "user_created" | "deduction_modified" | "system_backup";
  resource: string;
  ip: string; // Format: "xxx.xxx.xxx.xxx"
  hash: string; // First 12 characters of cryptographic hash
}
```

### System Metrics
```typescript
{
  date: string; // Format: "DD MMM"
  apiCalls: number;
  users: number;
  errors: number;
  avgResponseTime: number; // in milliseconds
}
```

### Tax Rates
```typescript
{
  id: number;
  name: string;
  type: "Federal" | "California";
  rate: number; // percentage
  wageBase: number | null; // dollar amount or null
  applies: string; // "Employee only" | "Employer only" | "Employee & Employer"
}
```

## Future Enhancements

### Backend Integration
1. Replace mock data with real API calls
2. Connect to existing audit log service (`src/JERP.Application/Services/Security/AuditLogService.cs`)
3. Implement user CRUD operations
4. Connect tax rate management to database
5. Implement system settings persistence

### Real-time Features
1. WebSocket connections for live updates
2. Real-time system status monitoring
3. Live user activity tracking
4. Push notifications for critical events

### Advanced Features
1. **Filtering & Search**:
   - User search by name, email, role
   - Audit log filtering by date range, user, action type
   - Tax rate search and filtering

2. **Export Functionality**:
   - PDF export for audit logs
   - CSV export for user lists
   - Compliance report generation

3. **Role-Based Access Control**:
   - Restrict certain tabs/features by role
   - Granular permissions for CRUD operations
   - Audit trail of admin actions

4. **Enhanced Visualizations**:
   - More detailed analytics dashboard
   - Custom date range selection
   - Drill-down capabilities in charts

5. **Email Notifications**:
   - Failed login attempts
   - System status changes
   - Critical errors
   - Compliance reminders

## Testing

### Manual Testing Completed
- ✅ All 5 tabs render correctly
- ✅ Tab navigation works smoothly
- ✅ Charts display with proper data
- ✅ Tables are responsive
- ✅ Forms capture input correctly
- ✅ Authentication protection works
- ✅ No console errors or warnings

### Build & Compilation
- ✅ Next.js build passes
- ✅ TypeScript compilation successful
- ✅ No type errors
- ✅ Production build optimization

### Security Scanning
- ✅ CodeQL scan: 0 vulnerabilities
- ✅ No hardcoded credentials
- ✅ Input validation considerations documented
- ✅ CSRF protection considerations documented

## Usage Instructions

### Development
```bash
cd landing-page
npm install
npm run dev
```
Then navigate to: `http://localhost:3000/admin`

### Production Build
```bash
cd landing-page
npm run build
npm start
```

### Access Requirements
- Valid authentication session (NextAuth)
- User must be logged in via `/partners/login`
- Future: Specific admin role required

## Maintenance Notes

### Adding New Tax Rates
1. Add to `mockTaxRates` array in `page.tsx`
2. Future: Implement backend API call to persist

### Adding New User Roles
1. Add role to type union in user interface
2. Add color mapping in `getRoleBadgeColor()` function
3. Update role badges in UserManagementTable component

### Adding New Audit Actions
1. Add action to audit log interface type
2. Add color mapping in `getActionBadgeColor()` function
3. Ensure backend audit service includes new action type

### Customizing Colors
Update the `COLORS` constant at the top of the component:
```typescript
const COLORS = {
  primary: "#2563eb",
  success: "#10b981",
  warning: "#f59e0b",
  danger: "#ef4444",
  purple: "#8b5cf6",
  teal: "#14b8a6",
};
```

## Support & Documentation

### Related Files
- Backend Audit Log: `src/JERP.Core/Entities/AuditLog.cs`
- Backend User Entity: `src/JERP.Core/Entities/User.cs`
- Backend Audit Service: `src/JERP.Application/Services/Security/AuditLogService.cs`
- Implementation Docs: `ADMIN-PORTAL-IMPLEMENTATION.md`

### External Libraries
- [Recharts Documentation](https://recharts.org/)
- [Next.js App Router](https://nextjs.org/docs/app)
- [NextAuth.js](https://next-auth.js.org/)

## Troubleshooting

### Charts Not Rendering
- Ensure recharts is installed: `npm install recharts`
- Check browser console for errors
- Verify data structure matches expected format

### Authentication Issues
- Check NextAuth configuration in `lib/auth.ts`
- Verify middleware configuration in `middleware.ts`
- Check if NEXTAUTH_SECRET is set in environment

### Build Failures
- Clear `.next` folder: `rm -rf .next`
- Delete node_modules and reinstall: `rm -rf node_modules && npm install`
- Check for TypeScript errors: `npx tsc --noEmit`

## License
JERP 3.0 - Payroll & ERP System  
Copyright (c) 2026 ninoyerbas. All Rights Reserved.  
PROPRIETARY AND CONFIDENTIAL
