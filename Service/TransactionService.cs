using PersonalFinanceApp.Model;
using PersonalFinanceApp.Repository;

namespace PersonalFinanceApp.Service;

public class TransactionService
{
	private TransactionRepository repository;
	public TransactionService()
	{
		this.repository = TransactionRepository.Instance;
    }
    public IEnumerable<Transaction> FindAll()
    {
		return this.repository.FindAll();
    }
    public Transaction Save(Transaction transaction)
    {
        if (transaction.Category == null)
        {
            throw new ArgumentException("A Category must be provided");
        }
        if (string.IsNullOrEmpty(transaction.Description))
		{
			throw new ArgumentNullException("Description can not be empty");
        }
        if (transaction.Amount == 0)
        {
            throw new ArgumentException("Value can not be equal to zero");
        }
        transaction = this.repository.Save(transaction);
		return transaction;
	}

	public void Edit(Transaction transaction)
	{
        this.repository.Save(transaction);
    }

	public void Delete(Transaction transaction)
	{
        this.repository.Delete(transaction);
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