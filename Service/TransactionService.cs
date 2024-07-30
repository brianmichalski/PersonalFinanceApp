using PersonalFinance.Model;
using PersonalFinance.Repository;

namespace PersonalFinance.Service;

public class TransactionService
{
	private TransactionRepository repository;
	public TransactionService()
	{
		this.repository = new TransactionRepository();
    }
    public IEnumerable<Transaction> FindAll()
    {
		return this.repository.FindAll();
    }
    public Transaction Add(TransactionType type, string description, DateTime datetime, double amount)
	{
		if (string.IsNullOrEmpty(description))
		{
			throw new ArgumentNullException("Description can not be empty");
        }
        if (amount == 0)
        {
            throw new ArgumentException("Value can not be equal to zero");
        }
        Transaction transaction = new Transaction(description, datetime, amount);
		transaction = this.repository.Save(transaction);
		return transaction;
	}

	public void Edit(Transaction transaction)
	{
        this.repository.Save(transaction);
    }

	public void Delete(Transaction transaction)
	{
	}
	public double GetCurrentBalance()
    {
        return 0.0;
    }
    public double GetBalance(DateOnly date)
    {
        return 0.0;
    }
    public Dictionary<int, double> GetBalanceByMonth(int year)
    {
        return new Dictionary<int, double>();
    }
  }