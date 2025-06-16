using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.DataAccess.Entities;

public class TransactionEntity
{
    public int Id { get; set; }

    [Required]
    public TransactionParticipantEntity? Participant { get; set; }
    public MoneyEntity? TransactionAmount { get; set; }
}
