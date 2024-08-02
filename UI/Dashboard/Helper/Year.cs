using System.Collections.ObjectModel;

namespace PersonalFinanceApp.UI.Dashboard.Helper
{
	public class Year : ObservableCollection<string>
	{
		public Year()
		{
			Add("2024");
			Add("2023");
		}
	}
}