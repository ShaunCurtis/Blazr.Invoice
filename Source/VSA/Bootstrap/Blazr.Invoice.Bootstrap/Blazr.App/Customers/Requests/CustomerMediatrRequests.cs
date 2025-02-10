/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public readonly record struct CustomerCommandRequest(
        DmoCustomer Item,
        CommandState State)
    : IRequest<Result<CustomerId>>;

public record CustomerListRequest
    : BaseListRequest, IRequest<Result<ListResult<DmoCustomer>>>
{ }

public readonly record struct CustomerRecordRequest(CustomerId Id) : IRequest<Result<DmoCustomer>>;

public readonly record struct CustomerLookupRequest() : IRequest<Result<ListResult<CustomerLookUpItem>>>;
