using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.DataAccess.Entities;

public class PaymentEntity
{
    public int Id { get; set; }
    public TransactionSourceEntity? TransactionSource { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    public MoneyEntity? Amount { get; set; }
}
