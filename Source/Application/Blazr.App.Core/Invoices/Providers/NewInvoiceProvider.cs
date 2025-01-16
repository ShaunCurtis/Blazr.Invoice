/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class NewInvoiceProvider : INewRecordProvider<DmoInvoice>
{
    public DmoInvoice NewRecord()
    {
        return new DmoInvoice() 
        { 
            Id = new(Guid.CreateVersion7()), 
            Date = DateOnly.FromDateTime(DateTime.Now) 
        };
    }

    public DmoInvoice DefaultRecord()
    {
        return new DmoInvoice
        { 
            Id = InvoiceId.Default 
        };
    }
}