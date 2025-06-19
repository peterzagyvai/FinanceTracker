using System;
using FinanceTracker.Core.Interfaces;

namespace FinanceTracker.Core.Models;

public class Debt
{
    private static readonly decimal Epsilon = 0.001M;

    public DateTime CreationOfDebt { get; set; } = DateTime.Now;
    public DateTime? Deadline { get; set; }
    public List<Payment> Payments { get; } = new();
    public Money Loan { get; set; }
    public ITransactionParticipant Creditor { get; set; }
    public ITransactionParticipant Debtor { get; set; }

    public Debt(Money loan, ITransactionParticipant creditor, ITransactionParticipant debtor, DateTime? deadline = null)
    {
        Loan = loan;
        Creditor = creditor;
        Debtor = debtor;
        Deadline = deadline;
    }

    public void AddPayment(Money money, DateTime dateOfPayment)
    {
        Payments.Add(new Payment(
            money,
            dateOfPayment,
            Debtor,
            Creditor
        ));
    }

    public void CompletePayment(DateTime dateOdPayment)
    {
        Payments.Add(new Payment(
            RemainingLoan,
            dateOdPayment,
            Debtor,
            Creditor
        ));
    }

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

    public bool IsCompleted => RemainingLoan.Amount <= Epsilon;
}
