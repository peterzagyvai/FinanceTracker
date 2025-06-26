using System;
using FinanceTracker.Core.Interfaces;

namespace FinanceTracker.Core.Models;

public abstract class TransactionSource
{
    public abstract DateTime DateOfTransaction { get; set; }
    public Transaction Transaction { get; protected set; }
    protected TransactionSource(ITransactionParticipant from, ITransactionParticipant to)
    {
        Transaction = new Transaction(
            new Money(),
            this,
            from,
            to);
    }
}
