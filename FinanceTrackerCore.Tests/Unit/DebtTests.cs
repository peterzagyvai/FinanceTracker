using System;
using FinanceTracker.Core.Models;
using FinanceTrackerCore.Helpers;
using FinanceTrackerCore.Models;
using FinanceTrackerCore.Tests.Dummies;

namespace FinanceTrackerCore.Tests.Unit;

[TestClass]
public class DebtTests
{
    private Debt _debt;
    private DateTime _creationOfDebt = new DateTime(2024, 01, 01);

    [TestInitialize]
    public void Setup()
    {
        _debt = new(
            new Money() { Amount = 1000, CurrencyISO = "EUR" },
            new VirtualTransactionParticipant("Creditor"),
            new VirtualTransactionParticipant("Debtor"),
            _creationOfDebt,
            null,
            new CurrencyHelper(new DummyExchangeRepository()));
    }

    [TestMethod]
    public void InitDebtTest()
    {
        Debt debtDeafult = new(
            new Money() { Amount = 1000, CurrencyISO = "EUR" },
            new VirtualTransactionParticipant("Creditor"),
            new VirtualTransactionParticipant("Debtor"),
            _creationOfDebt);

        Debt debtWithDeadLine = new(
            new Money() { Amount = 1000, CurrencyISO = "EUR" },
            new VirtualTransactionParticipant("Creditor"),
            new VirtualTransactionParticipant("Debtor"),
            _creationOfDebt,
            new DateTime(2024, 01, 02));

        Assert.AreEqual(new DateTime(2024, 01, 01), debtDeafult.CreationOfDebt);
        Assert.IsNull(debtDeafult.Deadline);
        Assert.AreEqual(0, debtDeafult.Payments.Count);
        Assert.AreEqual(1000, debtDeafult.Loan.Amount);
        Assert.AreEqual("EUR", debtDeafult.Loan.CurrencyISO);
        Assert.AreEqual("Creditor", debtDeafult.Creditor.Name);
        Assert.AreEqual("Debtor", debtDeafult.Debtor.Name);
        Assert.AreEqual(1000, debtDeafult.RemainingLoan.Amount);
        Assert.AreEqual("EUR", debtDeafult.RemainingLoan.CurrencyISO);
        Assert.IsFalse(debtDeafult.IsCompleted);
        Assert.IsFalse(debtDeafult.HasDeadline);

        Assert.AreEqual(new DateTime(2024, 01, 01), debtWithDeadLine.CreationOfDebt);
        Assert.IsNotNull(debtWithDeadLine.Deadline);
        Assert.AreEqual(new DateTime(2024, 01, 02), debtWithDeadLine.Deadline);
        Assert.AreEqual(0, debtWithDeadLine.Payments.Count);
        Assert.AreEqual(1000, debtWithDeadLine.Loan.Amount);
        Assert.AreEqual("EUR", debtWithDeadLine.Loan.CurrencyISO);
        Assert.AreEqual("Creditor", debtWithDeadLine.Creditor.Name);
        Assert.AreEqual("Debtor", debtWithDeadLine.Debtor.Name);
        Assert.AreEqual(1000, debtWithDeadLine.RemainingLoan.Amount);
        Assert.AreEqual("EUR", debtWithDeadLine.RemainingLoan.CurrencyISO);
        Assert.IsFalse(debtWithDeadLine.IsCompleted);
        Assert.IsTrue(debtWithDeadLine.HasDeadline);
    }

    [TestMethod]
    public void CompletePaymentTest()
    {
        Money moneyPayed = _debt.CompletePayment(new DateTime(2024, 01, 01));

        Assert.AreEqual(new Money() { Amount = 1000, CurrencyISO = "EUR" }, moneyPayed);

        Assert.AreEqual(1, _debt.Payments.Count);
        Assert.AreEqual(new DateTime(2024, 01, 01), _debt.Payments[0].Date);
        Assert.AreEqual(moneyPayed, _debt.Payments[0].PaymentAmount);
        Assert.AreSame(_debt.Payments[0], _debt.Payments[0].Transaction.TransactionSource);

        Assert.AreEqual(new Money() { Amount = 0, CurrencyISO = "EUR" }, _debt.RemainingLoan);
        Assert.IsTrue(_debt.IsCompleted);
    }

    public void AddPaymentLessThanRemainingTest()
    {
        Money moneyToPay = new Money() { Amount = 600, CurrencyISO = "EUR" };
        Money? moneyPayed = _debt.AddPayment(moneyToPay, new DateTime(2024, 01, 01));

        Assert.AreEqual(new Money() { Amount = 600, CurrencyISO = "EUR" }, moneyPayed);

        Assert.AreEqual(1, _debt.Payments.Count);
        Assert.AreEqual(new Money() { Amount = 400, CurrencyISO = "EUR" }, _debt.RemainingLoan);
        Assert.IsFalse(_debt.IsCompleted);
    }

    public void AddPaymentMoreThanRemainingTest()
    {
        Money moneyToPay = new Money() { Amount = 1500, CurrencyISO = "EUR" };
        Money? moneyPayed = _debt.AddPayment(moneyToPay, new DateTime(2024, 01, 01));

        Assert.AreEqual(new Money() { Amount = 1000, CurrencyISO = "EUR" }, moneyPayed);

        Assert.AreEqual(1, _debt.Payments.Count);
        Assert.AreEqual(new Money() { Amount = 0, CurrencyISO = "EUR" }, _debt.RemainingLoan);
        Assert.IsTrue(_debt.IsCompleted);
    }

    [TestMethod]
    public void AddPaymentMoreThanOnceTest()
    {
        List<Money?> moneyPayed = new();

        moneyPayed.Add(_debt.AddPayment(new Money() { Amount = 500, CurrencyISO = "EUR" }, new DateTime(2024, 01, 01)));
        moneyPayed.Add(_debt.AddPayment(new Money() { Amount = 400, CurrencyISO = "EUR" }, new DateTime(2024, 01, 01)));
        moneyPayed.Add(_debt.AddPayment(new Money() { Amount = 1000, CurrencyISO = "EUR" }, new DateTime(2024, 01, 01)));
        moneyPayed.Add(_debt.AddPayment(new Money() { Amount = 1, CurrencyISO = "EUR" }, new DateTime(2024, 01, 01)));

        Assert.AreEqual(new Money() { Amount = 500, CurrencyISO = "EUR" }, moneyPayed[0]);
        Assert.AreEqual(new Money() { Amount = 400, CurrencyISO = "EUR" }, moneyPayed[1]);
        Assert.AreEqual(new Money() { Amount = 100, CurrencyISO = "EUR" }, moneyPayed[2]);
        Assert.IsNull(moneyPayed[3]);

        Assert.AreEqual(3, _debt.Payments.Count);
        Assert.IsTrue(_debt.IsCompleted);
    }

    [TestMethod]
    public void AddPaymentZeroMoney()
    {
        Money? moneyPayed = _debt.AddPayment(new Money() { Amount = 0, CurrencyISO = "EUR" }, new DateTime(2024, 01, 01));


        Assert.IsNull(moneyPayed);
        Assert.AreEqual(0, _debt.Payments.Count);
    }
}
