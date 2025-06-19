using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApi.DataAccess.Entities;

[Owned]
public class MoneyEntity
{
    public decimal Amount { get; set; }

    [Required]
    [Length(3,3)]
    public string? CurrencyISO { get; set; }
}
