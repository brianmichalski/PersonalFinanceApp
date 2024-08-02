using System.Collections.ObjectModel;
using System.Globalization;

namespace PersonalFinanceApp.UI.Dashboard.Helper
{
	public class Month : ObservableCollection<string>
    {
        public Month()
        {
            for (int month = 1; month <= 12; month++)
            {
                Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(month).ToUpper());
            }
        }
	}
}