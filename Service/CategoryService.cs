using PersonalFinanceApp.Model;
using PersonalFinanceApp.Repository;

namespace PersonalFinanceApp.Service;

public class CategoryService
{
	private CategoryRepository repository;
	public CategoryService()
	{
		this.repository = CategoryRepository.Instance;
    }
    public IEnumerable<Category> FindAll()
    {
		return this.repository.FindAll();
    }
    public Category Save(Category category)
	{
		if (string.IsNullOrEmpty(category.Name))
		{
			throw new ArgumentNullException("Name can not be empty");
        }
        if (category.Limit < 0)
        {
            throw new ArgumentException("Limit can not be less than zero");
        }
        category = this.repository.Save(category);
		return category;
	}

	public void Delete(Category category)
	{
		this.repository.Delete(category);
	}
}