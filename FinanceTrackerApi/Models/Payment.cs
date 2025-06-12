using System;
using FinanceTrackerApi.Interfaces;
using FinanceTrackerApi.Utilities;

namespace FinanceTrackerApi.Models;

public class Payment : TransactionSource
{
    private DateTime _date = DateTime.Now;
    public override DateTime DateOfTransaction
    {
        get { return _date; }
        set
        {
            if (value.Equals(_date))
            {
                return;
            }

            _date = value;
        }
    }

    public DateTime Date
    {
        get { return _date; }
        set
        {
            if (value.Equals(_date))
            {
                return;
            }

            _date = value;
        }
    }

    private Money _amount;
    public Money Amount
    {
        get { return _amount; }
        set
        {
            if (value.Amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Payment amount can't be less or equal to 0");
            }

            _amount = value;
        }
    }

    public Payment(Money amount, DateTime date, IUser from, IUser to)
        : base(from, to)
    {
        Amount = amount;
        Date = date;
        From.TransactionAmount = Amount.Negate();
        To.TransactionAmount = Amount;
    }
}
