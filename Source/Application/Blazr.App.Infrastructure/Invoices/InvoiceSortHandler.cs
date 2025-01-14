/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Infrastructure;

public class InvoiceSortHandler : RecordSortHandler<DvoInvoice>, IRecordSortHandler<DvoInvoice>
{
    public InvoiceSortHandler()
    {
        DefaultSorter = (item) => item.Date;
        DefaultSortDescending = false;
    }
}
