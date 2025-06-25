using System;
using System.Runtime;
using FinanceTracker.Core.Interfaces;
using FinanceTrackerCore.Helpers;

namespace FinanceTracker.Core.Models;

public class AppUser : ITransactionParticipant
{
    private readonly List<Debt> _debts = new();
    private readonly List<Income> _incomes = new();
    private readonly List<Purchase> _purchases = new();
    private readonly string _username;
    public string Name => _username;
    
    /// <summary>
    /// Returns the current amount of money on the users account
    /// </summary>
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


    /// <summary>
    /// Registers a new debt for the user
    /// </summary>
    /// <param name="debt">The debt that will be added to the user's current debts</param>
    public void AddDebt(Debt debt)
    {
        _debts.Add(debt);
    }

    /// <summary>
    /// Registers a new income for the user
    /// </summary>
    /// <param name="income">The new income that will be added to the user's current incomes</param>
    public void AddIncome(Income income)
    {
        _incomes.Add(income);
    }

    /// <summary>
    /// Registers a new purchase for the user
    /// </summary>
    /// <param name="purchase">The new purchase that will be added to the user's current purchases</param>
    public void AddPurchase(Purchase purchase)
    {
        _purchases.Add(purchase);
    }

}
