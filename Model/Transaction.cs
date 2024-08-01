namespace PersonalFinance.Model;

public class Transaction
{
    public int TransactionId { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public double Amount { get; set; }

    public Category Category { get; set; }

    public TransactionType TransactionType {
        get => this.Amount < 0 ? TransactionType.Output : TransactionType.Input;
    }

    public Transaction(Category category, string description, DateTime dateTime, double amount)
    {
        this.Category = category;
        this.Description = description;
        this.DateTime = dateTime;
        this.Amount = amount;
    }
}
