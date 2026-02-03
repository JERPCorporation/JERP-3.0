# JERP.Compliance

Complete compliance engine for labor law and financial compliance validation.

## Overview

The JERP.Compliance project provides a comprehensive compliance framework that evaluates business rules, labor laws, and financial regulations. It automatically detects violations and tracks compliance metrics.

## Features

### Labor Law Compliance
- **California Daily Overtime**: Validates 1.5x overtime for >8 hours/day, 2.0x for >12 hours/day
- **California 7th Day Overtime**: Validates premium pay for 7th consecutive work day
- **Federal Weekly Overtime (FLSA)**: Validates 1.5x overtime for >40 hours/week
- **Meal Break Rules**: Ensures 30-min breaks for shifts >5 hours, second break for >10 hours
- **Rest Break Rules**: Validates 10-min paid breaks per 4 hours worked
- **Minimum Wage Compliance**: Validates CA ($16.00/hr) and Federal ($7.25/hr) minimum wage
- **Child Labor Laws**: Restricts work hours and times for employees under 18

### Financial Compliance
- **GAAP Balance Sheet**: Validates Assets = Liabilities + Equity
- **GAAP Revenue Recognition**: Validates ASC 606 5-step model compliance
- **IFRS Inventory Valuation**: Ensures LIFO not used, validates lower of cost or NRV

## Architecture

### Core Components

#### IComplianceRule
Interface that all compliance rules must implement:
- `RuleName`: Unique identifier for the rule
- `ViolationType`: Type of violation (LaborLaw, FinancialCompliance, DataIntegrity)
- `DefaultSeverity`: Default severity level (Critical, High, Medium, Low)
- `EvaluateAsync()`: Evaluates the rule and returns results

#### ComplianceRuleBase
Abstract base class providing common functionality:
- Logging helpers
- Violation creation helpers
- Error handling

#### ComplianceResult
Result object containing:
- `IsCompliant`: Boolean indicating compliance status
- `Violations`: List of detected violations
- `Summary`: Human-readable summary
- `Metadata`: Additional context data

#### IComplianceEngine
Main engine interface providing:
- `RegisterRuleAsync<T>()`: Register compliance rules
- `EvaluateEmployeeAsync()`: Evaluate employee compliance
- `EvaluateTimesheetAsync()`: Evaluate timesheet compliance
- `EvaluatePayrollAsync()`: Evaluate payroll compliance
- `EvaluateAllAsync()`: Run all compliance checks
- `CalculateComplianceScoreAsync()`: Calculate overall compliance score
- `GetActiveViolationsAsync()`: Retrieve open violations

## Usage

### 1. Register Services

```csharp
services.AddComplianceServices();
```

### 2. Inject and Use Compliance Engine

```csharp
public class PayrollService
{
    private readonly IComplianceEngine _complianceEngine;
    
    public PayrollService(IComplianceEngine complianceEngine)
    {
        _complianceEngine = complianceEngine;
    }
    
    public async Task ProcessPayroll(Guid payrollRecordId)
    {
        // Process payroll...
        
        // Check compliance
        var violations = await _complianceEngine.EvaluatePayrollAsync(payrollRecordId);
        
        if (violations.Any())
        {
            // Handle violations
            foreach (var violation in violations)
            {
                Console.WriteLine($"{violation.Severity}: {violation.Description}");
            }
        }
    }
}
```

### 3. Run Compliance Checks

```csharp
// Evaluate specific timesheet
var violations = await complianceEngine.EvaluateTimesheetAsync(timesheetId);

// Evaluate specific employee
var violations = await complianceEngine.EvaluateEmployeeAsync(employeeId);

// Run all compliance checks
var allViolations = await complianceEngine.EvaluateAllAsync();

// Get compliance score
var score = await complianceEngine.CalculateComplianceScoreAsync();
Console.WriteLine($"Compliance Score: {score:F2}%");

// Get active violations
var activeViolations = await complianceEngine.GetActiveViolationsAsync();
```

## Compliance Rules Details

### California Daily Overtime Rule
- **Hours 0-8**: Regular pay
- **Hours 8-12**: 1.5x overtime
- **Hours 12+**: 2.0x double-time

### California 7th Day Overtime Rule
- Applies when employee works 7 consecutive days
- **First 8 hours on 7th day**: 1.5x
- **After 8 hours on 7th day**: 2.0x

### Federal Weekly Overtime Rule (FLSA)
- Applies to non-exempt employees only
- **Hours 0-40**: Regular pay
- **Hours 40+**: 1.5x overtime

### Meal Break Rule
- **Shifts > 5 hours**: One 30-minute meal break required
- **Shifts > 10 hours**: Two 30-minute meal breaks required

### Rest Break Rule
- **3.5-6 hours**: 1 break (10 min)
- **6-10 hours**: 2 breaks (20 min)
- **10-14 hours**: 3 breaks (30 min)
- Rest breaks are PAID and included in work hours

### Child Labor Rule (Age < 18)
- **Maximum daily hours**: 8 hours
- **School days**: Maximum 3 hours
- **School year hours**: 7:00 AM - 7:00 PM only
- **Weekly limit (school week)**: 18 hours
- **Weekly limit (non-school)**: 40 hours

### Minimum Wage Rule
- **California**: $16.00/hour (2024)
- **Federal**: $7.25/hour
- Applies higher of state or federal minimum
- Validates both hourly and salaried employees

## Violation Tracking

All violations are automatically:
1. Created with proper severity and metadata
2. Stored in the database
3. Tracked until resolved
4. Included in compliance score calculations

### Violation Severity Levels
- **Critical**: Immediate action required (e.g., minimum wage violation)
- **High**: Serious issue requiring prompt attention (e.g., overtime miscalculation)
- **Medium**: Important but not urgent (e.g., meal break violations)
- **Low**: Minor issues or warnings

### Violation Status
- **Open**: Newly detected, awaiting review
- **InProgress**: Being investigated or remediated
- **Resolved**: Issue fixed and verified
- **Dismissed**: Determined to be false positive or exception

## Compliance Score

The compliance score is calculated as:

```
score = ((total_checks - violations) / total_checks) * 100
```

Where:
- `total_checks` = employees + timesheets + payroll_records
- `violations` = count of open violations

## Extending the Engine

### Creating a Custom Rule

```csharp
public class CustomComplianceRule : ComplianceRuleBase
{
    public override string RuleName => "My Custom Rule";
    public override ViolationType ViolationType => ViolationType.LaborLaw;
    public override ViolationSeverity DefaultSeverity => ViolationSeverity.Medium;
    
    public CustomComplianceRule(ILogger<CustomComplianceRule> logger) 
        : base(logger)
    {
    }
    
    public override async Task<ComplianceResult> EvaluateAsync(object context)
    {
        LogEvaluationStart("Entity", entityId);
        
        var violations = new List<ComplianceViolation>();
        
        // Your evaluation logic here
        if (/* violation detected */)
        {
            violations.Add(CreateViolation(
                "EntityType",
                entityId,
                "Description of violation",
                ViolationSeverity.High,
                financialImpact: 100.00m
            ));
        }
        
        LogEvaluationComplete(violations.Count == 0, violations.Count);
        
        return violations.Count == 0
            ? ComplianceResult.Compliant("Compliant message")
            : ComplianceResult.NonCompliant("Non-compliant message", violations);
    }
}
```

### Register Custom Rule

```csharp
services.AddScoped<CustomComplianceRule>();
```

## Dependencies

- **JERP.Core**: Core entities and enums
- **JERP.Infrastructure**: Database context and infrastructure
- **Microsoft.Extensions.Logging.Abstractions**: Logging support
- **Microsoft.Extensions.DependencyInjection.Abstractions**: DI support

## License

Part of the JERP 3.0 Enterprise Resource Planning system.
