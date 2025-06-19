using System;

namespace FinanceTracker.Core.Models;

public class Item
{
    private string _name = "Unknown";
    public string Name
    {
        get { return _name; }
        set
        {
            if (value == _name)
            {
                return;
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            _name = value;
        }
    }

    private string _category = "Undefined";
    public string Category
    {
        get { return _category; }
        set
        {
            if (value == _category)
            {
                return;
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            _category = value;
        }
    }

    public UnitOfMeasure UnitOfMeasure { get; set; }

    public Item(string name, string category, UnitOfMeasure unitOfMeasure)
    {
        Name = name;
        Category = category;
        UnitOfMeasure = unitOfMeasure;
    }
}
