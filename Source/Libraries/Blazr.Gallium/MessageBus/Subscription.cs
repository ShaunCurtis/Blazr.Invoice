﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Gallium;

public readonly record struct Subscription(Type SubscriptionType, Action<object> SubscriptionAction);
