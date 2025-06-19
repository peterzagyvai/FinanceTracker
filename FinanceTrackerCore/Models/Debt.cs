using System.Collections.ObjectModel;
using FinanceTracker.Core.Interfaces;

namespace FinanceTracker.Core.Models;

public class Debt
{
    private static readonly decimal Epsilon = 0.001M;

    /// <summary>
    /// Debt's creation date
    /// </summary>
    public DateTime CreationOfDebt { get; set; } = DateTime.Now;

    /// <summary>
    /// Debt's deadline. If debt has no deadline value will be null.
    /// </summary>
    public DateTime? Deadline { get; set; }

    private readonly List<Payment> _payments;
    /// <summary>
    /// The amount of money payed back represented as a read only collection of payments 
    /// </summary>
    public ReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    /// <summary>
    /// Amount of money lent from one participant to the other
    /// </summary>
    public Money Loan { get; set; }

    /// <summary>
    /// The participant who lends the loan
    /// </summary>
    public ITransactionParticipant Creditor { get; set; }

    /// <summary>
    /// The participant who owes the loan
    /// </summary>
    public ITransactionParticipant Debtor { get; set; }

    /// <summary>
    /// The difference of the original loan and the payments
    /// </summary>
    public Money RemainingLoan
    {
        get
        {
            decimal payedLoan = 0;
            foreach (var payment in Payments)
            {
                payedLoan += payment.PaymentAmount.ConvertAmountToCurrency(Loan.CurrencyISO);
            }

            return new Money()
            {
                Amount = Loan.Amount - payedLoan,
                CurrencyISO = Loan.CurrencyISO
            };
        }
    }

    /// <summary>
    /// True if the debt has no remaining loan, else false
    /// </summary>
    public bool IsCompleted => RemainingLoan.ConvertAmountToCurrency("EUR") <= Epsilon;


    /// <summary>
    /// True if debt has a deadline, else false
    /// </summary>
    public bool HasDeadline => Deadline != null;

    public Debt(Money loan, ITransactionParticipant creditor, ITransactionParticipant debtor, DateTime? deadline = null)
    {
        _payments = new();
        Loan = loan;
        Creditor = creditor;
        Debtor = debtor;
        Deadline = deadline;
    }



    /// <summary>
    /// Creates and adds a new payment to the debt
    /// </summary>
    /// <param name="money">Money payed</param>
    /// <param name="dateOfPayment">Date of the payment</param>
    /// <returns>The amount of money that was added to the payments</returns>
    public Money AddPayment(Money money, DateTime dateOfPayment)
    {
        if (money > RemainingLoan)
        {
            return CompletePayment(dateOfPayment);
        }

        _payments.Add(new Payment(
            money,
            dateOfPayment,
            Debtor,
            Creditor
        ));

        return money;
    }

    /// <summary>
    /// Adds a new payment to the debt with just enough money to complete the debt
    /// </summary>
    /// <param name="dateOfPayment">Date of the payment</param>
    /// <returns>The amount of money that was added to the payments</returns>
    public Money CompletePayment(DateTime dateOfPayment)
    {
        Money moneyPayed = new()
        {
            Amount = RemainingLoan.Amount,
            CurrencyISO = RemainingLoan.CurrencyISO
        };

        _payments.Add(new Payment(
            moneyPayed,
            dateOfPayment,
            Debtor,
            Creditor
        ));

        return moneyPayed;
    }

 
}
