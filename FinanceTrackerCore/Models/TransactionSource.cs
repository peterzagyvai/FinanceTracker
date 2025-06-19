using System;
using FinanceTrackerApi.Core.Interfaces;

namespace FinanceTrackerApi.Core.Models;

public abstract class TransactionSource
{
    public abstract DateTime DateOfTransaction { get; set; }
    public Transaction From { get; protected set; }
    public Transaction To { get; protected set; }
    protected TransactionSource(ITransactionParticipant from, ITransactionParticipant to)
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
