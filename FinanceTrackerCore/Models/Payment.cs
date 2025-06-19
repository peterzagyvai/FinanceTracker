using System;
using FinanceTracker.Core.Interfaces;

namespace FinanceTracker.Core.Models;

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
    public Money PaymentAmount
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

    public Payment(Money amount, DateTime date, ITransactionParticipant from, ITransactionParticipant to)
        : base(from, to)
    {
        PaymentAmount = amount;
        Date = date;
        From.TransactionAmount = PaymentAmount.Negate();
        To.TransactionAmount = PaymentAmount;
    }
}
