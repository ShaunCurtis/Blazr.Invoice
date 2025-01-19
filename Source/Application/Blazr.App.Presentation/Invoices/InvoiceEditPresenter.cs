/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace Blazr.App.Presentation;

public class InvoiceEditPresenter
{
    private readonly IToastService _toastService;
    private readonly Invoice _invoice;

    public IDataResult LastResult { get; private set; } = DataResult.Success();
    public EditContext EditContext { get; private set; }
    public DmoInvoiceEditContext RecordEditContext { get; private set; }
    public bool IsNew => _invoice.State == CommandState.Add;

    public InvoiceEditPresenter(Invoice invoice, IToastService toastService)
    {
        _invoice = invoice;
        _toastService = toastService;
        this.RecordEditContext = new(_invoice.InvoiceRecord);
        this.EditContext = new(this.RecordEditContext);
    }

    public Task<IDataResult> SaveItemAsync()
    {

        if (!this.RecordEditContext.IsDirty)
        {
            this.LastResult = DataResult.Failure("The record has not changed and therefore has not been updated.");
            _toastService.ShowWarning("The record has not changed and therefore has not been updated.");
            return Task.FromResult(this.LastResult);
        }

        var result = _invoice.Dispatch(new InvoiceActions.UpdateInvoiceAction(this.RecordEditContext.AsRecord));

        if (result.IsSuccess)
            _toastService.ShowSuccess("The invoice data was updated.");
        else
            _toastService.ShowError(this.LastResult.Message ?? "The invoice data could not be updated.");

        return Task.FromResult(this.LastResult);
    }
}