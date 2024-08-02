using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Repository;

public class CategoryRepository
{
    private Dictionary<int, Category> _categories;

    public CategoryRepository() 
    {
        this._categories = new Dictionary<int, Category>(); 
    }

    public IEnumerable<Category> FindAll()
    {
        return this._categories.Values;
    }

    private int GetNextId()
    {
        if (this._categories.Count() == 0)
        {
            return 1;
        }
        return 1 + this._categories
            .Keys
            .Last();
    }

    public Category Save(Category category)
    {
        if (category.CategoryId > 0)
        {
            this._categories[category.CategoryId] = category;
        } else { 
            int lastId = this.GetNextId();
            category.CategoryId = lastId;
            this._categories.Add(lastId, category);
        }
        return category;
    }
}