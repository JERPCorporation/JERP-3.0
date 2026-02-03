using JERP.Core.Enums;

namespace JERP.Compliance.Core;

/// <summary>
/// Interface for compliance rules that can be evaluated
/// </summary>
public interface IComplianceRule
{
    /// <summary>
    /// Unique name of the compliance rule
    /// </summary>
    string RuleName { get; }

    /// <summary>
    /// Type of violation this rule checks for
    /// </summary>
    ViolationType ViolationType { get; }

    /// <summary>
    /// Default severity level for violations of this rule
    /// </summary>
    ViolationSeverity DefaultSeverity { get; }

    /// <summary>
    /// Evaluates the rule against the provided context
    /// </summary>
    /// <param name="context">Context object containing data to evaluate</param>
    /// <returns>Compliance result with any violations detected</returns>
    Task<ComplianceResult> EvaluateAsync(object context);
}
