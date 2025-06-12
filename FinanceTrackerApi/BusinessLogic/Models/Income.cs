using System;
using System.ComponentModel.DataAnnotations;
using FinanceTrackerApi.BusinessLogic.Interfaces;
using FinanceTrackerApi.BusinessLogic.Utilities;

namespace FinanceTrackerApi.BusinessLogic.Models;

public class Income : TransactionSource
{
    private DateTime _dateOfIncome;
    public override DateTime DateOfTransaction
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

    public string SourceOfIncome { get; set; } = string.Empty;

    private Money _amountOfIncome;
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

    public Income(DateTime dateOfIncome, string sourceOfIncome, Money amountOfIncome, IUser from, IUser to)
        : base(from, to)
    {
        DateOfIncome = dateOfIncome;
        SourceOfIncome = sourceOfIncome;
        AmountOfIncome = amountOfIncome;

        From.TransactionAmount = AmountOfIncome.Negate();
        To.TransactionAmount = AmountOfIncome;
    }
}
