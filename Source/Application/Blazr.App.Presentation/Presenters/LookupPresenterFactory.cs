/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

public class LookupPresenterFactory : ILookupPresenterFactory
{
    private IServiceProvider _serviceProvider;
    public LookupPresenterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<ILookUpPresenter<TRecord>> GetPresenterAsync<TRecord, TPresenter>()
    where TRecord : class, IFkItem, new()
    where TPresenter : class, ILookUpPresenter<TRecord>
    {
        var presenter = ActivatorUtilities.CreateInstance<TPresenter>(_serviceProvider);
        ArgumentNullException.ThrowIfNull(presenter, nameof(presenter));
        await presenter.LoadAsync();

        return presenter;
    }
}
