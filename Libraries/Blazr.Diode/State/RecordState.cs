/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Diode;

public abstract record RecordState
{
    public sealed record New() : RecordState;
    public sealed record Dirty() : RecordState;
    public sealed record Clean() : RecordState;
    public sealed record Deleted() : RecordState;

    public static RecordState CreateAsNewState => new RecordState.New();
    public static RecordState CreateAsDirtyState => new RecordState.Dirty();
    public static RecordState CreateAsCleanState => new RecordState.Clean();
    public static RecordState CreateAsDeletedState => new RecordState.Deleted();
}
