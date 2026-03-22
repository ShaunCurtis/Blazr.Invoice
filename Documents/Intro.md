# Introduction

> **This set of documentation is in-the-making.  Some are rough-and-ready complete, others are still being written when I get time.**

Blazr.Invoice is a demonstration project showcasing a set of patterns and methodologies for building data centric applications.

1. *Functional Core/Imperative I/O*
1. *Vertically Sliced Minimalist Clean Design*
1. *CQS*
1. *Mediator*
1. *Message Bus* 
1. *Immutable and Sealed*


It's built on two fundimental coding principles:

## Functional Core/Imperative I/O

The core domain is coding in the functionl style.  An example to illustrate this:

```csharp
public Result<DmoInvoiceItem> GetInvoiceItem(InvoiceItemId id)
    => ResultT.Read(
        value: @this.InvoiceItems.SingleOrDefault(_item => _item.Id == id),
        exceptionMessage: $"The record with id {id} does not exist in the Invoice Items");
```

The method encapsulates getting an invoice item from the invoice entity. `Result<T>` is Monad with two states: success and failure.

## Vertically Sliced Minimalist Clean Design

Three application domains: Core, Infratructure and UI.  

Vertically sliced: separation of concerns is by feature, not by technical layer.  Each domain has *Customer* and *Invoice* primary folders.  There's no dependancies between the two.  The customer object used in the *Invoice* feature is defined in the *Shared* feature folder.  

Minimalist, meaning the minimum domain projects.

## CQS

The Command/Query separation pattern is applied to the data pipeline.

## Mediator

The Mediator pattern is used to decouple CQS pipeline from the Core and UI.

## Simple Message Bus

A simple message bus implementation [Blazr.Gallium] provides event notification.

## Immutable and Sealed

Everything is immutable and sealed by default.

