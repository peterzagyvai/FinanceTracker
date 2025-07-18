using FinanceTracker.Core.Interfaces;

namespace FinanceTracker.Core.Models;

public class Income : TransactionSource
{
    private DateTime _dateOfIncome;
    public override DateTime DateOfTransaction
    {
        get { return _dateOfIncome; }
        set
        {
            DateOfIncome = value;
        }
    }
    public DateTime DateOfIncome
    {
        get { return _dateOfIncome; }
        set
        {
            if (value.Equals(_dateOfIncome))
            {
                return;
            }

            _dateOfIncome = value;
        }
    }

    public string SourceOfIncome { get; set; }

    private Money _amountOfIncome = new Money();
    public Money AmountOfIncome
    {
        get { return _amountOfIncome; }
        set
        {
            if (value.Amount <= 0)
            {
                throw new ArgumentException($"Amount of income can't be less or equal to 0. Current amount: {value}");
            }

            _amountOfIncome = value;
        }
    }

    public Income(DateTime dateOfIncome, string sourceOfIncome, Money amountOfIncome, ITransactionParticipant from, ITransactionParticipant to)
        : base(from, to)
    {
        DateOfIncome = dateOfIncome;
        SourceOfIncome = sourceOfIncome;
        AmountOfIncome = amountOfIncome;

        Transaction.TransactionAmount = AmountOfIncome;
    }
}
