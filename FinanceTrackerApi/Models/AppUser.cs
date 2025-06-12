using System;
using FinanceTrackerApi.Interfaces;

namespace FinanceTrackerApi.Models;

public class AppUser : IUser
{
    private readonly string _username;
    public string Username => _username; 

    public AppUser(string username)
    {
        _username = username;
    }
}
