using System.ComponentModel.DataAnnotations;
using FinanceTrackerApi.Core.Models;

namespace FinanceTrackerApi.DataAccess.Entities;

public class ItemEntity
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
    
    [Required]
    public string? Category { get; set; }
    public UnitOfMeasure UnitOfMeasure { get; set; }
}