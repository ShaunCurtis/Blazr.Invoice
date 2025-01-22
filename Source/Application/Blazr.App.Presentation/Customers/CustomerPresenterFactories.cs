/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

public class EditPresenterFactory<TEditContext, TKey> : IEditPresenterFactory<TEditContext, TKey>
        where TKey : notnull, IEntityId
{
    private IEditPresenter<TEditContext, TKey> _presenter;

    public EditPresenterFactory(IEditPresenter<TEditContext, TKey> presenter)
    {
        _presenter = presenter;
    }

    public async ValueTask<IEditPresenter<TEditContext, TKey>> GetPresenterAsync(TKey id)
    {
        await _presenter.LoadAsync(id);

        return _presenter;
    }
}

public class CustomerEditPresenterFactory : IEditPresenterFactory<CustomerEditContext, CustomerId>
{
    private IServiceProvider _serviceProvider;
    public CustomerEditPresenterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<IEditPresenter<CustomerEditContext, CustomerId>> GetPresenterAsync(CustomerId id)
    {
        var presenter = ActivatorUtilities.CreateInstance<CustomerEditPresenter>(_serviceProvider);
        ArgumentNullException.ThrowIfNull(presenter, nameof(presenter));
        await presenter.LoadAsync(id);

        return presenter;
    }
}

public class CustomerReadPresenterFactory : IReadPresenterFactory<DmoCustomer, CustomerId>
{
    private IServiceProvider _serviceProvider;
    public CustomerReadPresenterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<IReadPresenter<DmoCustomer, CustomerId>> GetPresenterAsync(CustomerId id)
    {
        var presenter = ActivatorUtilities.CreateInstance<CustomerReadPresenter>(_serviceProvider);
        ArgumentNullException.ThrowIfNull(presenter, nameof(presenter));
        await presenter.LoadAsync(id);

        return presenter;
    }
}
