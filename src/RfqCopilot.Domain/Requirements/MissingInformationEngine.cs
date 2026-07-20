using System;
using System.Collections.Generic;
using System.Linq;

namespace RfqCopilot.Domain.Requirements;

/// <summary>
/// Deterministic detection of missing/unverified required information.
///
/// Rules:
///   - An always-required field that is absent, Missing or Unknown -> RequiredMissing.
///   - A conditional rule's required field, when the condition holds and the field is
///     absent/Missing/Unknown -> ConditionalMissing.
///   - A field that is present but Unverified -> Unverified (a warning, not satisfied).
///   - Only Provided/Extracted/Approved with a non-null value counts as satisfied.
///   - Never fabricates values; it only reports what a human must supply/confirm.
/// </summary>
public static class MissingInformationEngine
{
    public static IReadOnlyList<MissingItem> Detect(
        IReadOnlyList<RequirementValue> requirements,
        RuleSet rules)
    {
        ArgumentNullException.ThrowIfNull(requirements);
        ArgumentNullException.ThrowIfNull(rules);

        var report = new List<MissingItem>();
        var reported = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var rule in rules.RequiredFields)
        {
            var value = Find(requirements, rule.Key);
            if (IsSatisfied(value))
            {
                continue;
            }

            AddOnce(report, reported, BuildItem(rule.Key, value, MissingKind.RequiredMissing, rule.Reason));
        }

        foreach (var conditional in rules.ConditionalRules)
        {
            if (!ConditionHolds(conditional.When, requirements))
            {
                continue;
            }

            foreach (var requiredKey in conditional.Requires)
            {
                if (reported.Contains(requiredKey))
                {
                    continue;
                }

                var value = Find(requirements, requiredKey);
                if (IsSatisfied(value))
                {
                    continue;
                }

                AddOnce(report, reported, BuildItem(requiredKey, value, MissingKind.ConditionalMissing, conditional.Reason));
            }
        }

        return report;
    }

    private static MissingItem BuildItem(string key, RequirementValue? value, MissingKind missingKind, string reason)
    {
        if (value is not null && value.Status == RequirementStatus.Unverified)
        {
            return new MissingItem(
                key,
                MissingKind.Unverified,
                $"'{key}' has an unverified value; please confirm rather than assume. {reason}");
        }

        return new MissingItem(key, missingKind, reason);
    }

    private static void AddOnce(List<MissingItem> report, HashSet<string> reported, MissingItem item)
    {
        if (reported.Add(item.FieldKey))
        {
            report.Add(item);
        }
    }

    private static RequirementValue? Find(IReadOnlyList<RequirementValue> requirements, string key)
        => requirements.FirstOrDefault(r => string.Equals(r.FieldKey, key, StringComparison.OrdinalIgnoreCase));

    private static bool IsSatisfied(RequirementValue? value)
    {
        if (value is null || value.Value is null)
        {
            return false;
        }

        return value.Status is RequirementStatus.Provided
            or RequirementStatus.Extracted
            or RequirementStatus.Approved;
    }

    private static bool ConditionHolds(FieldCondition condition, IReadOnlyList<RequirementValue> requirements)
    {
        var value = Find(requirements, condition.FieldKey);

        return condition.Operator switch
        {
            ConditionOperator.IsPresent =>
                value is not null
                && value.Value is not null
                && value.Status is not RequirementStatus.Missing
                && value.Status is not RequirementStatus.Unknown,

            ConditionOperator.Equals =>
                value is not null
                && value.Value is not null
                && string.Equals(value.Value.ToString(), condition.Value, StringComparison.OrdinalIgnoreCase),

            _ => false,
        };
    }
}
