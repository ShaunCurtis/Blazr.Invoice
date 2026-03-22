/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.Diode;

namespace Blazr.Cadmium.Core;

public abstract class RecordMutor<TRecord>
    where TRecord : class
{
    public TRecord BaseRecord { get; protected set; }
    public bool IsDirty => !this.Record.Equals(BaseRecord);
    public virtual bool IsNew { get; }
    public virtual TRecord Record { get; } = default!;
    public abstract void Reset();

    protected RecordMutor(TRecord record)
    {
        this.BaseRecord = record;
        this.SetFields();
    }

    public RecordState State => (this.IsNew, this.IsDirty) switch
    {
        (true, _) => RecordState.CreateAsNewState,
        (false, false) =>RecordState.CreateAsCleanState,
        (false, true) => RecordState.CreateAsDirtyState,
    };

    protected abstract void SetFields();
}
