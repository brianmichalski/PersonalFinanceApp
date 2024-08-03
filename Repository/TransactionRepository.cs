using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Repository;

public class TransactionRepository
{
    private static TransactionRepository instance = new TransactionRepository();
    private Dictionary<int, Transaction> transactions;
    public static TransactionRepository Instance { get => instance; }

    private TransactionRepository()
    {
        this.LoadDatabase();
        if (this.transactions == null)
        {
            this.transactions = new Dictionary<int, Transaction>();
        }
    }
    private void LoadDatabase()
    {
        IEnumerable<Transaction> _transactions = Database.Instance.Restore<Transaction>();
        if (_transactions != null && _transactions.Count() > 0)
        {
            this.transactions = _transactions.ToDictionary(k => k.TransactionId, v => v);
        }
    }

    public IEnumerable<Transaction> FindAll()
    {
        this.LoadDatabase();
        return this.transactions.Values;
    }

    private int GetNextId()
    {
        if (this.transactions.Count() == 0)
        {
            return 1;
        }
        return 1 + this.transactions
            .Keys
            .Last();
    }

    public Transaction Save(Transaction transaction, bool persist = true)
    {
        if (transaction.TransactionId > 0)
        {
            this.transactions[transaction.TransactionId] = transaction;
        } else { 
            int lastId = this.GetNextId();
            transaction.TransactionId = lastId;
            this.transactions.Add(lastId, transaction);
        }
        if (persist)
        {
            Database.Instance.Save<Transaction>(this.transactions.Values);
        }

        return transaction;
    }
    public void Delete(Transaction transaction)
    {
        if (!this.transactions.ContainsKey(transaction.TransactionId))
        {
            throw new InvalidOperationException("The transaction provided is not in the database");
        }
        this.transactions?.Remove(transaction.TransactionId);
        Database.Instance.Save<Transaction>(this.transactions.Values);
    }
}