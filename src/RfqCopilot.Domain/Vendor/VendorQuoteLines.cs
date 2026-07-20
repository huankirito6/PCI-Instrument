namespace RfqCopilot.Domain.Vendor;

/// <summary>
/// The model/range/quantity the tool proposed for a line (a human-reviewed candidate).
/// </summary>
/// <param name="Model">Proposed model/order code.</param>
/// <param name="Range">Proposed measuring range, as text.</param>
/// <param name="Quantity">Proposed quantity.</param>
public sealed record ProposedLine(string Model, string Range, int Quantity);

/// <summary>
/// The model/range/quantity the vendor confirmed on their quotation. Stored
/// independently from the proposed line so mismatches can be flagged.
/// </summary>
/// <param name="Model">Vendor-confirmed model/order code.</param>
/// <param name="Range">Vendor-confirmed measuring range, as text.</param>
/// <param name="Quantity">Vendor-confirmed quantity.</param>
public sealed record VendorConfirmedLine(string Model, string Range, int Quantity);
