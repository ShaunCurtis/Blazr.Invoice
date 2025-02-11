namespace SO79428044.Components;

public class ComponentContext
{
    public event EventHandler? AnEvent;

    public void RaiseEvent(object? sender, EventArgs e)
    {
        this.AnEvent?.Invoke(sender, e);
    }
}
