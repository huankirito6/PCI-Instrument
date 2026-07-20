namespace RfqCopilot.Domain.Approval;

/// <summary>Approval state of a single checklist item before export.</summary>
public enum ApprovalState
{
    /// <summary>Not yet reviewed/confirmed by a human.</summary>
    Pending,

    /// <summary>Reviewed and confirmed by a human.</summary>
    Approved,

    /// <summary>Reviewed and explicitly rejected.</summary>
    Rejected,
}

/// <summary>One entry in the pre-export approval checklist.</summary>
/// <param name="Key">The critical item key, e.g. "selling_price".</param>
/// <param name="State">Its current approval state.</param>
public sealed record ApprovalItem(string Key, ApprovalState State);
