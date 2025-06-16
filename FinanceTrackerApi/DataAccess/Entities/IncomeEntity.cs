using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.DataAccess.Entities;

public class IncomeEntity
{
    public int Id { get; set; }
    public TransactionSourceEntity? TransactionSource { get; set; }
    public DateTime DateOfIncome { get; set; } = DateTime.Now;

    [Required]
    public string? SourceOfIncome { get; set; }
    
    [Required]
    public MoneyEntity? AmountOfIncome { get; set; }
}
