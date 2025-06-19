using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.DataAccess.Entities;

public class TransactionParticipantEntity
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
    public bool IsVirtual { get; set; }
}
