/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public sealed partial class Invoice
{
    public ValueTask<Result> DispatchAsync(UpdateInvoiceAction action, CancellationToken? cancellationToken = null)
    {
        this.UpdateInvoice(action.Item);
        return ValueTask.FromResult(Result.Success());
    }

    public ValueTask<Result> DispatchAsync(DeleteInvoiceAction action, CancellationToken cancellationToken)
    {
        this.State = CommandState.Delete;
        return ValueTask.FromResult(Result.Success());
    }

    public ValueTask<Result> DispatchAsync(DeleteInvoiceItemAction action, CancellationToken cancellationToken)
    {
        var invoiceItem = this.Items.FirstOrDefault(item => item.InvoiceItemRecord == action.Item);
        if (invoiceItem is null)
            return ValueTask.FromResult(Result.Fail(new ActionException($"No Invoice Item with Id: {action.Item.Id} exists in the Invoice")));

        invoiceItem.State = CommandState.Delete;
        this.ItemsBin.Add(invoiceItem);
        this.Items.Remove(invoiceItem);
        this.Process();

        return ValueTask.FromResult(Result.Success());
    }

    public ValueTask<Result> DispatchAsync(UpdateInvoiceItemAction action, CancellationToken cancellationToken)
    {
        var invoiceItem = this.Items.FirstOrDefault(item => item.InvoiceItemRecord == action.Item);

        if (invoiceItem is null)
            return ValueTask.FromResult(Result.Fail(new ActionException($"No Invoice Item with Id: {action.Item.Id} exists in the Invoice.")));

        invoiceItem.State = invoiceItem.State.AsDirty;
        invoiceItem.UpdateInvoiceItem(action.Item);
        this.Process();

        return ValueTask.FromResult(Result.Success());
    }


    public ValueTask<Result> DispatchAsync(AddInvoiceItemAction action, CancellationToken cancellationToken)
    {
        if (this.Items.Any(item => item.InvoiceItemRecord == action.Item))
            return ValueTask.FromResult(Result.Fail(new ActionException($"The Invoice Item with Id: {action.Item.Id} already exists in the Invoice.")));

        var invoiceItem = new InvoiceItem(action.Item, this.ItemUpdated, action.IsNew);
        this.Items.Add(invoiceItem);
        this.Process();

        return ValueTask.FromResult(Result.Success());
    }
}

