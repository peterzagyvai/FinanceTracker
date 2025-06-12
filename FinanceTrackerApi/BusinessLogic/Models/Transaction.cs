using System;
using FinanceTrackerApi.BusinessLogic.Interfaces;
using FinanceTrackerApi.BusinessLogic.Utilities;

namespace FinanceTrackerApi.BusinessLogic.Models;

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
