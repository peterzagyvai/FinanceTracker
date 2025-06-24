using System;

namespace FinanceTracker.Core.Interfaces;

public interface ITransactionParticipant
{
    /// <summary>
    /// Returns the name of the TransactionParticipant
    /// </summary>
    public string Name { get; }
}
