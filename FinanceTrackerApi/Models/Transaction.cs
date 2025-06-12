using System;
using FinanceTrackerApi.Interfaces;
using FinanceTrackerApi.Utilities;

namespace FinanceTrackerApi.Models;

public class Transaction
{
    public Money TransactionAmount { get; set; }
    public TransactionSource TransactionSource { get; }
    public IUser User { get; }
    public Transaction(Money transactionAmount, TransactionSource transactionSource, IUser user)
    {
        TransactionAmount = transactionAmount;
        TransactionSource = transactionSource;
        User = user;
    }
}
