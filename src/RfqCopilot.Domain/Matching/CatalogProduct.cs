using System.Collections.Generic;

namespace RfqCopilot.Domain.Matching;

/// <summary>
/// A catalog product entry used as a matching candidate. Attributes come from an
/// approved catalog/GS import; every product carries its source for citation.
/// </summary>
/// <param name="PartNumber">Order code / part number.</param>
/// <param name="Manufacturer">Manufacturer, e.g. Yokogawa.</param>
/// <param name="ModelSeries">Model series, e.g. EJA430E.</param>
/// <param name="Attributes">Attribute key/value pairs from the catalog.</param>
/// <param name="Source">Catalog/GS source identifier for citation.</param>
public sealed record CatalogProduct(
    string PartNumber,
    string Manufacturer,
    string ModelSeries,
    IReadOnlyDictionary<string, string> Attributes,
    string Source);
