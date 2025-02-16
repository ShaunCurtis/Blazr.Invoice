/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation.FluentUI;

public interface IFluentGridPresenter<TGridItem>
{
    public ValueTask<GridItemsProviderResult<TGridItem>> GetItemsAsync(GridItemsProviderRequest<TGridItem> request);
}
