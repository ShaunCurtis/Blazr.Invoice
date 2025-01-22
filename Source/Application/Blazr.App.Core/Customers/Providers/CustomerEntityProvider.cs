/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;
public interface IEntityProvider<TRecord, TKey>
{
    public Func<IMediator, TKey, Task<Result<TRecord>>> RecordRequest { get; }

    public Func<IMediator, TRecord, CommandState, Task<Result<TKey>>> RecordCommand { get; }

    public TKey GetKey(object key);

    public CustomerId GetKey(TRecord record);

    public TRecord NewRecord();

    public TRecord DefaultRecord();
}


public class CustomerEntityProvider : IEntityProvider<DmoCustomer, CustomerId>, IRecordIdProvider<DmoCustomer, CustomerId>, IRecordFactory<DmoCustomer>
{
    public Func<IMediator, CustomerId,  Task<Result<DmoCustomer>>> RecordRequest
        => (broker, id) => broker.Send(new CustomerRecordRequest(id));

    public Func<IMediator, DmoCustomer, CommandState,  Task<Result<CustomerId>>> RecordCommand
        => (broker, record, state) => broker.Send(new CustomerCommandRequest(record, state));

    public CustomerId GetKey(object key)
    {
        return key switch
        {
            CustomerId id => id,
            Guid guid => new CustomerId(guid),
            _ => CustomerId.Default
        };
    }

    public CustomerId GetKey(DmoCustomer record)
    {
        return record.Id;
    }
    //TODO - obselete - now uses tostring
    public object GetValueObject(CustomerId key)
    {
        return key.Value;
    }

    public DmoCustomer NewRecord()
    {
        return new DmoCustomer()
        {
            Id = CustomerId.Create,
        };
    }

    public DmoCustomer DefaultRecord()
    {
        return new DmoCustomer { Id = CustomerId.Default };
    }
}
