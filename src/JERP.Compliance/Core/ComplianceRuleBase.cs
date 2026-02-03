using JERP.Core.Entities;
using JERP.Core.Enums;
using Microsoft.Extensions.Logging;

namespace JERP.Compliance.Core;

/// <summary>
/// Abstract base class for compliance rules
/// </summary>
public abstract class ComplianceRuleBase : IComplianceRule
{
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the compliance rule
    /// </summary>
    protected ComplianceRuleBase(ILogger logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public abstract string RuleName { get; }

    /// <inheritdoc/>
    public abstract ViolationType ViolationType { get; }

    /// <inheritdoc/>
    public abstract ViolationSeverity DefaultSeverity { get; }

    /// <inheritdoc/>
    public abstract Task<ComplianceResult> EvaluateAsync(object context);

    /// <summary>
    /// Logs the start of rule evaluation
    /// </summary>
    protected void LogEvaluationStart(string entityType, Guid entityId)
    {
        Logger.LogInformation("Starting evaluation of {RuleName} for {EntityType} {EntityId}", 
            RuleName, entityType, entityId);
    }

    /// <summary>
    /// Logs the completion of rule evaluation
    /// </summary>
    protected void LogEvaluationComplete(bool isCompliant, int violationCount)
    {
        if (isCompliant)
        {
            Logger.LogInformation("{RuleName} evaluation complete: Compliant", RuleName);
        }
        else
        {
            Logger.LogWarning("{RuleName} evaluation complete: {ViolationCount} violations found", 
                RuleName, violationCount);
        }
    }

    /// <summary>
    /// Creates a violation for this rule
    /// </summary>
    protected ComplianceViolation CreateViolation(
        string entityType,
        Guid entityId,
        string description,
        ViolationSeverity? severity = null,
        decimal? financialImpact = null,
        string? details = null)
    {
        return new ComplianceViolation
        {
            Id = Guid.NewGuid(),
            ViolationType = ViolationType,
            Severity = severity ?? DefaultSeverity,
            Status = ViolationStatus.Open,
            EntityType = entityType,
            EntityId = entityId,
            RuleName = RuleName,
            Description = description,
            Details = details,
            FinancialImpact = financialImpact,
            DetectedAt = DateTime.UtcNow
        };
    }
}
