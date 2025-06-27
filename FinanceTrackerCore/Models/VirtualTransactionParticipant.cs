using System;
using FinanceTracker.Core.Interfaces;

namespace FinanceTrackerCore.Models;

public class VirtualTransactionParticipant : ITransactionParticipant
{
    private readonly string username;
    public string Name => username;

    public VirtualTransactionParticipant(string username)
    {
        this.username = username;
    }
}
