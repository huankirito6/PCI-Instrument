using System.Collections.Generic;

namespace RfqCopilot.Domain.Requirements;

/// <summary>Operator used to evaluate a <see cref="FieldCondition"/>.</summary>
public enum ConditionOperator
{
    /// <summary>The field exists with a non-null value and a non-missing status.</summary>
    IsPresent,

    /// <summary>The field's value (as string, case-insensitive) equals a target value.</summary>
    Equals,
}

/// <summary>A condition evaluated against the current requirement set.</summary>
/// <param name="FieldKey">Field to inspect.</param>
/// <param name="Operator">How to evaluate.</param>
/// <param name="Value">Comparison value for <see cref="ConditionOperator.Equals"/>; null for IsPresent.</param>
public sealed record FieldCondition(string FieldKey, ConditionOperator Operator, string? Value);

/// <summary>An always-required field.</summary>
/// <param name="Key">Schema field key.</param>
/// <param name="Reason">Why the field is needed (shown to the user).</param>
public sealed record RequiredFieldRule(string Key, string Reason);

/// <summary>A rule that requires extra fields only when a condition holds.</summary>
/// <param name="Id">Stable rule id.</param>
/// <param name="When">Condition that activates the rule.</param>
/// <param name="Requires">Fields required when the condition holds.</param>
/// <param name="Reason">Why those fields are needed.</param>
public sealed record ConditionalRule(
    string Id,
    FieldCondition When,
    IReadOnlyList<string> Requires,
    string Reason);

/// <summary>A versioned set of required + conditional rules for one product family.</summary>
public sealed record RuleSet(
    IReadOnlyList<RequiredFieldRule> RequiredFields,
    IReadOnlyList<ConditionalRule> ConditionalRules);
