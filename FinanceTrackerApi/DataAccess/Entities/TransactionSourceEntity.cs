using System;

namespace FinanceTrackerApi.DataAccess.Entities;

public class TransactionSourceEntity
{
    public int Id { get; set; }
    public TransactionEntity? From { get; set; }
    public TransactionEntity? To { get; set; }
}
