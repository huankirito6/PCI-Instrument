using System;
using System.Collections.Generic;
using System.Linq;

namespace RfqCopilot.Domain.Approval;

/// <summary>Outcome of the export-readiness check.</summary>
/// <param name="CanExport">True only when every critical item is Approved.</param>
/// <param name="BlockingItems">Critical item keys that are missing, pending or rejected,
/// in critical-list order.</param>
public sealed record ExportReadiness(bool CanExport, IReadOnlyList<string> BlockingItems);

/// <summary>
/// Deterministic gate that decides whether a quotation may be exported.
///
/// Rule: export is allowed ONLY when every critical item has been explicitly Approved
/// by a human. A critical item that is missing from the checklist, still Pending, or
/// Rejected blocks the export. A duplicate entry never upgrades an item: if any entry
/// for a key is not Approved, that key blocks. Non-critical items are ignored.
/// </summary>
public static class ExportReadinessGate
{
    public static ExportReadiness Evaluate(
        IReadOnlyList<ApprovalItem> checklist,
        IReadOnlyList<string> criticalItems)
    {
        ArgumentNullException.ThrowIfNull(checklist);
        ArgumentNullException.ThrowIfNull(criticalItems);

        var blocking = new List<string>();

        foreach (var key in criticalItems)
        {
            var entries = checklist
                .Where(i => string.Equals(i.Key, key, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Missing entirely, or ANY entry for this key is not Approved -> blocks.
            var satisfied = entries.Count > 0 && entries.All(e => e.State == ApprovalState.Approved);

            if (!satisfied)
            {
                blocking.Add(key);
            }
        }

        return new ExportReadiness(blocking.Count == 0, blocking);
    }
}
