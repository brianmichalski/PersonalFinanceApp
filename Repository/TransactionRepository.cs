using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Repository;

public class TransactionRepository
{
    private Dictionary<int, Transaction> _transactions;

    public TransactionRepository() 
    {
        this._transactions = new Dictionary<int, Transaction>(); 
    }

    public IEnumerable<Transaction> FindAll()
    {
        return this._transactions.Values;
    }

    private int GetNextId()
    {
        if (this._transactions.Count() == 0)
        {
            return 1;
        }
        return 1 + this._transactions
            .Keys
            .Last();
    }

    public Transaction Save(Transaction transaction)
    {
        if (transaction.TransactionId > 0)
        {
            this._transactions[transaction.TransactionId] = transaction;
        } else { 
            int lastId = this.GetNextId();
            transaction.TransactionId = lastId;
            this._transactions.Add(lastId, transaction);
        }
        return transaction;
    }
}