using System;
using FinanceTrackerApi.Utilities;

namespace FinanceTrackerApi.Models;

public abstract class TransactionSource
{
    public abstract DateTime DateOfTransaction { get; set; }
    public Transaction Transaction { get; protected set; }
    protected TransactionSource()
    {
        Transaction = new Transaction(
            new Money(),
            this
        );
    }
}
