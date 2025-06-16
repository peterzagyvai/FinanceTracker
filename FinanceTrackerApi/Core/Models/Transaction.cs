using System;
using FinanceTrackerApi.Core.Interfaces;

namespace FinanceTrackerApi.Core.Models;

public class Transaction
{
    public Money TransactionAmount { get; set; }
    public TransactionSource TransactionSource { get; }
    public ITransactionParticipant User { get; }
    public Transaction(Money transactionAmount, TransactionSource transactionSource, ITransactionParticipant user)
    {
        TransactionAmount = transactionAmount;
        TransactionSource = transactionSource;
        User = user;
    }
}
