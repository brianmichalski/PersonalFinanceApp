namespace PersonalFinance.Model;

public class Category
{
	public string Name { get; set; }
	public double Limit { get; set; }

	public Category(string name, double limit)
	{
		Name = name;
		Limit = limit;
	}
}
