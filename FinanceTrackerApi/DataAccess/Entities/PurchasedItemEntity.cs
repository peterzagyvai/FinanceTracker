using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.DataAccess.Entities;

public class PurchasedItemEntity
{
    public int Id { get; set; }
    public decimal Amount { get; set; }

    [Required]
    public ItemEntity? Item { get; set; }

    [Required]
    public MoneyEntity? Price { get; set; }
}