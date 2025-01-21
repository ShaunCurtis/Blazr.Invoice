/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

public interface ILookupPresenterFactory
{
    public ValueTask<ILookUpPresenter<TRecord>> GetPresenterAsync<TRecord, TPresenter>()
            where TRecord : class, IFkItem, new()
            where TPresenter : class, ILookUpPresenter<TRecord>
;
}