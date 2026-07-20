using System.Linq;
using RfqCopilot.Domain.Catalog;
using Xunit;

namespace RfqCopilot.Domain.Tests.Catalog;

public class CatalogImportParserTests
{
    private const string Header =
        "Manufacturer,ProductFamily,ModelSeries,PartNumber,Description,AttributeKey,AttributeValue,Unit,SourceDocument,SourcePage";

    [Fact]
    public void Valid_single_product_with_two_attributes_is_parsed()
    {
        var csv = string.Join("\n",
            Header,
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,measurement_type,gauge_pressure,,GS-01,3",
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,output_signal,4-20mA_HART,,GS-01,3");

        var result = CatalogImportParser.Parse(csv);

        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        var product = Assert.Single(result.Products);
        Assert.Equal("EJA430E-A", product.PartNumber);
        Assert.Equal("Yokogawa", product.Manufacturer);
        Assert.Equal("EJA430E", product.ModelSeries);
        Assert.Equal("gauge_pressure", product.Attributes["measurement_type"]);
        Assert.Equal("4-20mA_HART", product.Attributes["output_signal"]);
        Assert.Equal("GS-01", product.Source);
    }

    [Fact]
    public void Two_part_numbers_produce_two_products()
    {
        var csv = string.Join("\n",
            Header,
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,measurement_type,gauge_pressure,,GS-01,3",
            "Yokogawa,pressure_transmitter,EJA530E,EJA530E-A,Gauge PT,measurement_type,gauge_pressure,,GS-02,4");

        var result = CatalogImportParser.Parse(csv);

        Assert.True(result.Success);
        Assert.Equal(2, result.Products.Count);
    }

    [Fact]
    public void Missing_required_column_fails_the_whole_import()
    {
        // No PartNumber column.
        var badHeader = "Manufacturer,ProductFamily,ModelSeries,Description,AttributeKey,AttributeValue,Unit,SourceDocument,SourcePage";
        var csv = string.Join("\n",
            badHeader,
            "Yokogawa,pressure_transmitter,EJA430E,Gauge PT,measurement_type,gauge_pressure,,GS-01,3");

        var result = CatalogImportParser.Parse(csv);

        Assert.False(result.Success);
        Assert.Empty(result.Products);
        Assert.Contains(result.Errors, e => e.Contains("PartNumber"));
    }

    [Fact]
    public void A_row_with_blank_required_cell_rejects_whole_import_no_partial_publish()
    {
        var csv = string.Join("\n",
            Header,
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,measurement_type,gauge_pressure,,GS-01,3",
            // second row has blank PartNumber
            "Yokogawa,pressure_transmitter,EJA530E,,Gauge PT,measurement_type,gauge_pressure,,GS-02,4");

        var result = CatalogImportParser.Parse(csv);

        Assert.False(result.Success);
        Assert.Empty(result.Products); // atomic: nothing published
        Assert.Contains(result.Errors, e => e.Contains("row 3"));
    }

    [Fact]
    public void Blank_attribute_key_is_an_error()
    {
        var csv = string.Join("\n",
            Header,
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,,gauge_pressure,,GS-01,3");

        var result = CatalogImportParser.Parse(csv);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("AttributeKey"));
    }

    [Fact]
    public void Missing_source_document_is_an_error()
    {
        // Every attribute must carry a source for later citation.
        var csv = string.Join("\n",
            Header,
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,measurement_type,gauge_pressure,,,3");

        var result = CatalogImportParser.Parse(csv);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("SourceDocument"));
    }

    [Fact]
    public void Duplicate_attribute_key_for_same_part_is_an_error()
    {
        var csv = string.Join("\n",
            Header,
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,measurement_type,gauge_pressure,,GS-01,3",
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,measurement_type,absolute_pressure,,GS-01,3");

        var result = CatalogImportParser.Parse(csv);

        Assert.False(result.Success);
        Assert.Contains(result.Errors, e => e.Contains("measurement_type") && e.Contains("EJA430E-A"));
    }

    [Fact]
    public void Empty_content_yields_no_products_and_no_crash()
    {
        var result = CatalogImportParser.Parse(Header);

        Assert.True(result.Success);
        Assert.Empty(result.Products);
    }

    [Fact]
    public void Blank_lines_are_ignored()
    {
        var csv = string.Join("\n",
            Header,
            "",
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,measurement_type,gauge_pressure,,GS-01,3",
            "   ");

        var result = CatalogImportParser.Parse(csv);

        Assert.True(result.Success);
        Assert.Single(result.Products);
    }

    [Fact]
    public void Header_column_order_is_flexible()
    {
        var reordered =
            "PartNumber,AttributeKey,AttributeValue,Manufacturer,ProductFamily,ModelSeries,Description,Unit,SourceDocument,SourcePage";
        var csv = string.Join("\n",
            reordered,
            "EJA430E-A,measurement_type,gauge_pressure,Yokogawa,pressure_transmitter,EJA430E,Gauge PT,,GS-01,3");

        var result = CatalogImportParser.Parse(csv);

        Assert.True(result.Success);
        var product = Assert.Single(result.Products);
        Assert.Equal("gauge_pressure", product.Attributes["measurement_type"]);
    }

    [Fact]
    public void Parsed_products_feed_the_matching_engine()
    {
        // Integration-ish: the parser output is directly usable by matching.
        var csv = string.Join("\n",
            Header,
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,product_family,pressure_transmitter,,GS-01,3",
            "Yokogawa,pressure_transmitter,EJA430E,EJA430E-A,Gauge PT,measurement_type,gauge_pressure,,GS-01,3");

        var result = CatalogImportParser.Parse(csv);

        Assert.True(result.Success);
        var product = Assert.Single(result.Products);
        Assert.IsType<RfqCopilot.Domain.Matching.CatalogProduct>(product);
    }
}
