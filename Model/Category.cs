namespace PersonalFinanceApp.Model;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public double Limit { get; set; }
    public Category? Parent { get; set; }
    public Category(Category? parent, string name, double limit)
    {
        Parent = parent;
        Name = name;
        Limit = limit;
    }
    public Category(string name, double limit) : this(null, name, limit)
    {
    }
}
