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

    /// <summary>
    /// Marks the invoice for deletion
    /// You still need to persist the change to the data store
    /// </summary>
    /// <param name="action"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

        // we don#t set the Command State to delete because the handler needs to know
        // if the deleted item is New and therefore not in the data store
        // The fact that the item is in the Bin is enough to delete it.
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

    /// <summary>
    /// Sets the aggregate as saved.
    /// i.e. it sets the CommandState on the invoice and invoice items as none. 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public ValueTask<Result> DispatchAsync(SetAsPersistedAction action, CancellationToken cancellationToken)
    {
        this.State = CommandState.None;

        foreach (var item in this.Items)
            item.State = CommandState.None;

        return ValueTask.FromResult(Result.Success());
    }
}

