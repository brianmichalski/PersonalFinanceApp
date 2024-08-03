using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Repository;

public class CategoryRepository
{
    private static CategoryRepository instance = new CategoryRepository();
    private Dictionary<int, Category> categories;
    public static CategoryRepository Instance { get => instance; }

    private CategoryRepository() 
    {
        this.categories = new Dictionary<int, Category>();
        this.Save(new Category("Groceries", 1000));
        this.Save(new Category("Transportation", 300));
        this.Save(new Category("Leisure", 200));
    }

    public IEnumerable<Category> FindAll()
    {
        return this.categories.Values;
    }

    private int GetNextId()
    {
        if (this.categories.Count() == 0)
        {
            return 1;
        }
        return 1 + this.categories
            .Keys
            .Last();
    }

    public Category Save(Category category)
    {
        if (category.CategoryId > 0)
        {
            this.categories[category.CategoryId] = category;
        } else { 
            int lastId = this.GetNextId();
            category.CategoryId = lastId;
            this.categories.Add(lastId, category);
        }
        return category;
    }
    public void Delete(Category category)
    {
        if (!this.categories.ContainsKey(category.CategoryId))
        {
            throw new InvalidOperationException("The category provided is not in the database");
        }
        this.categories?.Remove(category.CategoryId);
    }
}