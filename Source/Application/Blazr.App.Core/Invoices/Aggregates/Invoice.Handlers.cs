

/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed partial class Invoice
{
    public Result Dispatch(InvoiceActions.UpdateInvoiceAction action)
    {
        this.UpdateInvoice(action.Item);
        return Result.Success();
    }

    /// <summary>
    /// Marks the invoice for deletion
    /// You still need to persist the change to the data store
    /// </summary>
    /// <param name="action"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Result Dispatch(InvoiceActions.DeleteInvoiceAction action)
    {
        this.State = CommandState.Delete;
        return Result.Success();
    }

    public Result Dispatch(InvoiceActions.DeleteInvoiceItemAction action)
    {
        var invoiceItem = this.Items.FirstOrDefault(item => item.InvoiceItemRecord == action.Item);
        if (invoiceItem is null)
            return Result.Fail(new ActionException($"No Invoice Item with Id: {action.Item.Id} exists in the Invoice"));

        // we don#t set the Command State to delete because the handler needs to know
        // if the deleted item is New and therefore not in the data store
        // The fact that the item is in the Bin is enough to delete it.
        this.ItemsBin.Add(invoiceItem);
        this.Items.Remove(invoiceItem);
        this.Process();

        return Result.Success();
    }

    public Result DispatchAsync(InvoiceActions.UpdateInvoiceItemAction action)
    {
        var invoiceItem = this.Items.FirstOrDefault(item => item.InvoiceItemRecord == action.Item);

        if (invoiceItem is null)
            return Result.Fail(new ActionException($"No Invoice Item with Id: {action.Item.Id} exists in the Invoice."));

        invoiceItem.State = invoiceItem.State.AsDirty;
        invoiceItem.UpdateInvoiceItem(action.Item);
        this.Process();

        return Result.Success();
    }

    public Result Dispatch(InvoiceActions.AddInvoiceItemAction action)
    {
        if (this.Items.Any(item => item.InvoiceItemRecord == action.Item))
            return Result.Fail(new ActionException($"The Invoice Item with Id: {action.Item.Id} already exists in the Invoice."));

        var invoiceItem = new InvoiceItem(action.Item, this.ItemUpdated, action.IsNew);
        this.Items.Add(invoiceItem);
        this.Process();

        return Result.Success();
    }

    /// <summary>
    /// Sets the aggregate as saved.
    /// i.e. it sets the CommandState on the invoice and invoice items as none. 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Result Dispatch(InvoiceActions.SetAsPersistedAction action)
    {
        this.State = CommandState.None;

        foreach (var item in this.Items)
            item.State = CommandState.None;

        return Result.Success();
    }
}

