using System;

namespace FinanceTrackerApi.DataAccess.Entities;

public class TransactionSourceEntity
{
    public int Id { get; set; }
    public int FromId { get; set; }
    public TransactionEntity? From { get; set; }

    public int ToId { get; set; }
    public TransactionEntity? To { get; set; }
}
