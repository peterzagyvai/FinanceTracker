using System;
using FinanceTrackerApi.Utilities;

namespace FinanceTrackerApi.Models;

public class Transaction
{
    public Money TransactionAmount { get; set; }
    public TransactionSource TransactionSource { get; }
    public Transaction(Money transactionAmount, TransactionSource transactionSource)
    {
        TransactionAmount = transactionAmount;
        TransactionSource = transactionSource;
    }
}
