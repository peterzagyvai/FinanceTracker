using System;
using FinanceTrackerApi.Interfaces;
using FinanceTrackerApi.Utilities;

namespace FinanceTrackerApi.Models;

public abstract class TransactionSource
{
    public abstract DateTime DateOfTransaction { get; set; }
    public Transaction From { get; protected set; }
    public Transaction To { get; protected set; }
    protected TransactionSource(IUser from, IUser to)
    {
        From = new Transaction(
            new Money(),
            this,
            from
        );

        To = new Transaction(
            new Money(),
            this,
            to
        );
    }
}
