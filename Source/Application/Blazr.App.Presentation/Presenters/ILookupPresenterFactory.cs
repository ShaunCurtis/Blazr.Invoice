/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

public interface ILookupPresenterFactory<TRecord>
    where TRecord : class, IFkItem, new()
{
    public ValueTask<ILookUpPresenter<TRecord>> GetPresenterAsync();
}