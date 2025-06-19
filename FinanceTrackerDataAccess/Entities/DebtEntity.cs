using System;
using System.ComponentModel.DataAnnotations;
using FinanceTracker.Core.Interfaces;
using FinanceTracker.Core.Models;

namespace FinanceTrackerApi.DataAccess.Entities;

public class DebtEntity
{
    public int Id { get; set; }
    public DateTime? DeadLine { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;
    

    public int CreditorId { get; set; }
    [Required]
    public TransactionParticipantEntity? Creditor { get; set; }


    public int DebtorId { get; set; }
    [Required]
    public TransactionParticipantEntity? Debtor { get; set; }

    [Required]
    public MoneyEntity? Loan { get; set; }

    public ICollection<PaymentEntity>? Payments { get; set; }
}
