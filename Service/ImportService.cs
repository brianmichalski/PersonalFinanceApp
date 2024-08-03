
namespace PersonalFinanceApp.Service
{
    class ImportService
    {
        public ImportService() { }

        public void ImportTransactions(IEnumerable<Model.Transaction> transactions)
        {
            if (transactions == null)
            {
                return;
            }

            CategoryService categoryService = new CategoryService();
            TransactionService transactionService = new TransactionService();

            for (int i = 0; i < transactions.Count(); i++)
            {
                {
                    var transaction = transactions.ElementAt(i);
                    Model.Category category = categoryService.FindByName(transaction.Category.Name);
                    if (category != null)
                    {
                        transaction.Category = category;
                    }
                    else
                    {
                        transaction.Category = categoryService.Save(transaction.Category, true);
                    }
                    transactionService.Save(transaction, i+1 == transactions.Count());
                }
            }
        }

        public void DeleteDatabase()
        {
            Database.Instance.DeleteDatabase<Model.Transaction>();
            Database.Instance.DeleteDatabase<Model.Category>();
        }
    }
}
