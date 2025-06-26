using System;
using FinanceTracker.Core.Interfaces;

namespace FinanceTracker.Core.Models;

public class Transaction
{
    public Money TransactionAmount { get; set; }
    public TransactionSource TransactionSource { get; }
    public ITransactionParticipant From { get; }
    public ITransactionParticipant To { get; }
    public Transaction(Money transactionAmount, TransactionSource transactionSource, ITransactionParticipant from, ITransactionParticipant to)
    {
        TransactionAmount = transactionAmount;
        TransactionSource = transactionSource;
        From = from;
        To = to;
    }
}
