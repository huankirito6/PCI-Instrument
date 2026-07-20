using System;
using System.Collections.Generic;
using System.Linq;
using RfqCopilot.Domain.Matching;

namespace RfqCopilot.Domain.Catalog;

/// <summary>
/// Deterministic CSV catalog importer.
///
/// Contract (see catalog/import-templates/README.md):
///   Each row describes ONE attribute of ONE part number. Required columns:
///   Manufacturer, ProductFamily, ModelSeries, PartNumber, Description,
///   AttributeKey, AttributeValue, SourceDocument. (Unit, SourcePage optional.)
///
/// Rules:
///   - Missing a required column header fails the whole import.
///   - Any row with a blank required cell fails the whole import (atomic: no partial publish).
///   - Every attribute must carry a SourceDocument (for later citation).
///   - Duplicate AttributeKey for the same part number is an error.
///   - Blank lines are ignored. Header column order is flexible.
///
/// Note: this is a minimal comma-split parser for the controlled import template.
/// It does not implement full RFC-4180 quoting; that can be added when a real
/// catalog needs embedded commas/quotes.
/// </summary>
public static class CatalogImportParser
{
    private static readonly string[] RequiredColumns =
    [
        "Manufacturer", "ProductFamily", "ModelSeries", "PartNumber",
        "Description", "AttributeKey", "AttributeValue", "SourceDocument",
    ];

    public static CatalogImportResult Parse(string csv)
    {
        var errors = new List<string>();

        var lines = (csv ?? string.Empty)
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Split('\n')
            .ToList();

        // Find the first non-blank line as the header.
        var headerIndex = lines.FindIndex(l => !string.IsNullOrWhiteSpace(l));
        if (headerIndex < 0)
        {
            return Fail(["Empty CSV: no header row found."]);
        }

        var headers = SplitRow(lines[headerIndex]);
        var columnIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < headers.Count; i++)
        {
            columnIndex[headers[i].Trim()] = i;
        }

        foreach (var required in RequiredColumns)
        {
            if (!columnIndex.ContainsKey(required))
            {
                errors.Add($"Missing required column: {required}.");
            }
        }

        if (errors.Count > 0)
        {
            return Fail(errors);
        }

        // Accumulate attributes per part number.
        var products = new Dictionary<string, ProductAccumulator>(StringComparer.OrdinalIgnoreCase);
        var order = new List<string>();

        for (var lineNo = headerIndex + 1; lineNo < lines.Count; lineNo++)
        {
            var raw = lines[lineNo];
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            // Human-facing row number: 1-based counting from the header as row 1.
            var rowNumber = lineNo - headerIndex + 1;
            var cells = SplitRow(raw);

            string Cell(string column)
            {
                var idx = columnIndex[column];
                return idx < cells.Count ? cells[idx].Trim() : string.Empty;
            }

            var partNumber = Cell("PartNumber");
            var attributeKey = Cell("AttributeKey");
            var source = Cell("SourceDocument");

            if (string.IsNullOrWhiteSpace(partNumber))
            {
                errors.Add($"row {rowNumber}: PartNumber is required.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(attributeKey))
            {
                errors.Add($"row {rowNumber}: AttributeKey is required.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                errors.Add($"row {rowNumber}: SourceDocument is required for citation.");
                continue;
            }

            foreach (var col in new[] { "Manufacturer", "ProductFamily", "ModelSeries", "Description" })
            {
                if (string.IsNullOrWhiteSpace(Cell(col)))
                {
                    errors.Add($"row {rowNumber}: {col} is required.");
                }
            }

            if (!products.TryGetValue(partNumber, out var acc))
            {
                acc = new ProductAccumulator(
                    Cell("Manufacturer"), Cell("ModelSeries"), source);
                products[partNumber] = acc;
                order.Add(partNumber);
            }

            if (acc.Attributes.ContainsKey(attributeKey))
            {
                errors.Add($"row {rowNumber}: duplicate AttributeKey '{attributeKey}' for part '{partNumber}'.");
                continue;
            }

            acc.Attributes[attributeKey] = Cell("AttributeValue");
        }

        if (errors.Count > 0)
        {
            return Fail(errors);
        }

        var built = order
            .Select(pn =>
            {
                var acc = products[pn];
                return new CatalogProduct(
                    PartNumber: pn,
                    Manufacturer: acc.Manufacturer,
                    ModelSeries: acc.ModelSeries,
                    Attributes: acc.Attributes,
                    Source: acc.Source);
            })
            .ToList();

        return new CatalogImportResult(true, built, []);
    }

    private static CatalogImportResult Fail(IReadOnlyList<string> errors)
        => new(false, [], errors);

    private static List<string> SplitRow(string line)
        => line.Split(',').ToList();

    private sealed class ProductAccumulator(string manufacturer, string modelSeries, string source)
    {
        public string Manufacturer { get; } = manufacturer;
        public string ModelSeries { get; } = modelSeries;
        public string Source { get; } = source;
        public Dictionary<string, string> Attributes { get; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
