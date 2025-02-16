/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

public interface ILookupPresenterFactory
{
    public ValueTask<ILookUpPresenter<TLookupRecord>> GetPresenterAsync<TLookupRecord, TPresenter>()
            where TLookupRecord : class, IFkItem, new()
            where TPresenter : class, ILookUpPresenter<TLookupRecord>
;
}