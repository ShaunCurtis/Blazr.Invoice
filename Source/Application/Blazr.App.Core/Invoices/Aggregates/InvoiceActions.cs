/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public readonly record struct UpdateInvoiceAction(DmoInvoice Item);

public readonly record struct DeleteInvoiceAction();

public readonly record struct DeleteInvoiceItemAction(DmoInvoiceItem Item);

public readonly record struct UpdateInvoiceItemAction(DmoInvoiceItem Item);

public readonly record struct AddInvoiceItemAction(DmoInvoiceItem Item, bool IsNew = true);

public readonly record struct SetAsPersistedAction();
