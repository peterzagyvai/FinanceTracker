using System;
using System.Runtime;
using FinanceTracker.Core.Interfaces;

namespace FinanceTracker.Core.Models;

public class AppUser : ITransactionParticipant
{
    private readonly List<Debt> _debts = new();
    private readonly List<Income> _incomes = new();
    private readonly List<Purchase> _purchases = new();
    private readonly string _username;
    public string Username => _username;
    
    public Money Money
    {
        get
        {
            Money moneyOnAccount = new();

            foreach (var income in _incomes)
            {
                moneyOnAccount += income.AmountOfIncome;
            }

            foreach (var purchase in _purchases)
            {
                foreach (var purchasedItem in purchase.PurchasedItems)
                {
                    moneyOnAccount -= purchasedItem.Price;
                }
            }

            foreach (var debt in _debts)
            {
                var paymentAmounts = debt.Payments.Select(payment => payment.PaymentAmount);
                foreach (var amount in paymentAmounts)
                {
                    if (debt.Creditor == this)
                    {
                        moneyOnAccount += amount;
                    }
                    if (debt.Debtor == this)
                    {
                        moneyOnAccount -= amount;
                    }
                }
            }

            return moneyOnAccount;
        }
    }


    public AppUser(string username)
    {
        _username = username;
    }

    public void AddDebt(Debt debt)
    {
        _debts.Add(debt);
    }

    public void AddIncome(Income income)
    {
        _incomes.Add(income);
    }

    public void AddPurchase(Purchase purchase)
    {
        _purchases.Add(purchase);
    }

}
