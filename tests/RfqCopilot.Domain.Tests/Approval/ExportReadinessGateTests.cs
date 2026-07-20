using System.Collections.Generic;
using System.Linq;
using RfqCopilot.Domain.Approval;
using Xunit;

namespace RfqCopilot.Domain.Tests.Approval;

public class ExportReadinessGateTests
{
    // The critical items that must be approved before a quotation can be exported.
    private static IReadOnlyList<string> CriticalItems() =>
    [
        "customer_or_end_user",
        "model_or_order_code",
        "measurement_range",
        "quantity",
        "cost_price",
        "selling_price",
        "gross_profit",
        "vat",
        "delivery_time",
        "payment_terms",
        "warranty",
        "documents",
    ];

    private static IReadOnlyList<ApprovalItem> AllApproved() =>
        CriticalItems().Select(k => new ApprovalItem(k, ApprovalState.Approved)).ToList();

    [Fact]
    public void Export_is_allowed_when_all_critical_items_are_approved()
    {
        var result = ExportReadinessGate.Evaluate(AllApproved(), CriticalItems());

        Assert.True(result.CanExport);
        Assert.Empty(result.BlockingItems);
    }

    [Fact]
    public void Export_is_blocked_when_one_critical_item_is_pending()
    {
        var items = AllApproved().ToList();
        items[4] = new ApprovalItem("cost_price", ApprovalState.Pending);

        var result = ExportReadinessGate.Evaluate(items, CriticalItems());

        Assert.False(result.CanExport);
        Assert.Contains("cost_price", result.BlockingItems);
    }

    [Fact]
    public void Export_is_blocked_when_a_critical_item_is_rejected()
    {
        var items = AllApproved().ToList();
        items[1] = new ApprovalItem("model_or_order_code", ApprovalState.Rejected);

        var result = ExportReadinessGate.Evaluate(items, CriticalItems());

        Assert.False(result.CanExport);
        Assert.Contains("model_or_order_code", result.BlockingItems);
    }

    [Fact]
    public void A_critical_item_entirely_absent_from_the_checklist_blocks_export()
    {
        // Only submit 11 of 12 items; the missing one must block.
        var items = AllApproved().Where(i => i.Key != "warranty").ToList();

        var result = ExportReadinessGate.Evaluate(items, CriticalItems());

        Assert.False(result.CanExport);
        Assert.Contains("warranty", result.BlockingItems);
    }

    [Fact]
    public void All_unapproved_items_are_listed_not_just_the_first()
    {
        var items = AllApproved().ToList();
        items[0] = new ApprovalItem("customer_or_end_user", ApprovalState.Pending);
        items[7] = new ApprovalItem("vat", ApprovalState.Pending);

        var result = ExportReadinessGate.Evaluate(items, CriticalItems());

        Assert.False(result.CanExport);
        Assert.Contains("customer_or_end_user", result.BlockingItems);
        Assert.Contains("vat", result.BlockingItems);
        Assert.Equal(2, result.BlockingItems.Count);
    }

    [Fact]
    public void Empty_checklist_blocks_export_and_lists_every_critical_item()
    {
        var result = ExportReadinessGate.Evaluate(new List<ApprovalItem>(), CriticalItems());

        Assert.False(result.CanExport);
        Assert.Equal(CriticalItems().Count, result.BlockingItems.Count);
    }

    [Fact]
    public void Non_critical_items_do_not_affect_export_readiness()
    {
        var items = AllApproved().ToList();
        items.Add(new ApprovalItem("internal_note", ApprovalState.Pending)); // not in critical list

        var result = ExportReadinessGate.Evaluate(items, CriticalItems());

        Assert.True(result.CanExport);
    }

    [Fact]
    public void Duplicate_item_keys_do_not_bypass_the_gate()
    {
        // A duplicate approved entry must not override a pending one for the same key.
        var items = new List<ApprovalItem>
        {
            new("cost_price", ApprovalState.Pending),
            new("cost_price", ApprovalState.Approved),
        };

        var result = ExportReadinessGate.Evaluate(items, ["cost_price"]);

        Assert.False(result.CanExport);
        Assert.Contains("cost_price", result.BlockingItems);
    }

    [Fact]
    public void Blocking_items_preserve_critical_list_order()
    {
        var items = AllApproved().ToList();
        items[7] = new ApprovalItem("vat", ApprovalState.Pending);
        items[0] = new ApprovalItem("customer_or_end_user", ApprovalState.Pending);

        var result = ExportReadinessGate.Evaluate(items, CriticalItems());

        // customer_or_end_user (index 0) must come before vat (index 7).
        Assert.Equal("customer_or_end_user", result.BlockingItems[0]);
        Assert.Equal("vat", result.BlockingItems[1]);
    }
}
