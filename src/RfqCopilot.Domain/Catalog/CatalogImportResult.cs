using System.Collections.Generic;
using RfqCopilot.Domain.Matching;

namespace RfqCopilot.Domain.Catalog;

/// <summary>
/// Result of a catalog import. The import is atomic: on any error, Success is false
/// and Products is empty (nothing is published).
/// </summary>
/// <param name="Success">True only when every row validated.</param>
/// <param name="Products">Parsed products, empty when Success is false.</param>
/// <param name="Errors">Validation errors; empty when Success is true.</param>
public sealed record CatalogImportResult(
    bool Success,
    IReadOnlyList<CatalogProduct> Products,
    IReadOnlyList<string> Errors);
