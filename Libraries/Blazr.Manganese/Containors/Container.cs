/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Manganese;

public readonly record struct Container<T>
{
    public T Value { get; private init; }

    public Container(T value)
        => Value = value;

    public static Container<T> Read(T Value)
        => new Container<T>(Value);

    public T Write()
        => this.Value;

    public void Write(Action<T> write)
        => write.Invoke(Value);

    public Container<TOut> Bind<TOut>(Func<T, Container<TOut>> func)
        => func.Invoke(Value);

    public Container<TOut> Map<TOut>(Func<T, TOut> func)
        => Container<TOut>.Read(func.Invoke(Value));
}

public static class ContainerT
{
    public static Container<T> Read<T>(Func<T> input)
        => new Container<T>(input.Invoke());

    public static Container<T> Read<T>(T value)
        => new Container<T>(value);

    extension<T, TOut>(Container<T> @this)
    {
        public async Task<Container<TOut>> BindAsync(Func<T, Task<Container<TOut>>> function)
            => await function(@this.Value);

        public async Task<Container<TOut>> MapAsync(Func<T, Task<TOut>> function)
            => ContainerT.Read(await function(@this.Value));
    }

    extension<T, TOut>(Task<Container<T>> @this)
    {
        public async Task<Container<TOut>> BindAsync(Func<T, Task<Container<TOut>>> function)
            => await (await @this)
                .BindAsync(function);

        public async Task<Container<TOut>> MapAsync(Func<T, Task<TOut>> function)
            => await (await @this)
                .MapAsync(function);
    }
}

