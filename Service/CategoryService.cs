using PersonalFinanceApp.Model;
using PersonalFinanceApp.Repository;

namespace PersonalFinanceApp.Service;

public class CategoryService
{
	private CategoryRepository repository;
	public CategoryService()
	{
		this.repository = new CategoryRepository();
    }
    public IEnumerable<Category> FindAll()
    {
		return this.repository.FindAll();
    }
    public Category Add(Category parent, string name, double limit)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("Name can not be empty");
        }
        if (limit < 0)
        {
            throw new ArgumentException("Limit can not be less than zero");
        }
        Category Category = new Category(parent, name, limit);
		Category = this.repository.Save(Category);
		return Category;
	}

	public void Edit(Category Category)
	{
        this.repository.Save(Category);
    }

	public void Delete(Category Category)
	{
	}
}