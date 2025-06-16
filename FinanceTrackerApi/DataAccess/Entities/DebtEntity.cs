using System;
using System.ComponentModel.DataAnnotations;
using FinanceTrackerApi.Core.Interfaces;
using FinanceTrackerApi.Core.Models;

namespace FinanceTrackerApi.DataAccess.Entities;

public class DebtEntity
{
    public int Id { get; set; }
    public DateTime? DeadLine { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;
    
    [Required]
    public TransactionParticipantEntity? Creditor { get; set; }

    [Required]
    public TransactionParticipantEntity? Debtor { get; set; }

    [Required]
    public MoneyEntity? Loan { get; set; }

    public ICollection<PaymentEntity>? Payments { get; set; }
}
