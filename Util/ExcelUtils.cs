using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Util;

public abstract class ExcelUtils
{
    public static List<Transaction> LoadSpreadSheet(string fileName, bool hasHeader)
    {
        List<Transaction> transactions = new List<Transaction>();

        using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(fileName, true))
        {
            IEnumerable<Sheet> sheets = spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            WorksheetPart worksheetPart = (WorksheetPart)spreadSheet.WorkbookPart.GetPartById(sheets.First().Id.Value);
            Worksheet workSheet = worksheetPart.Worksheet;
            SheetData sheetData = workSheet.GetFirstChild<SheetData>();
            IEnumerable<Row> rows = sheetData.Descendants<Row>();

            bool firstRowRead = false;
            foreach (Row row in rows)
            {
                if (!firstRowRead && hasHeader)
                {
                    firstRowRead = true;
                    continue;
                }
                string date = GetCellValue(spreadSheet, row.Descendants<Cell>().ElementAt(0));
                string description = GetCellValue(spreadSheet, row.Descendants<Cell>().ElementAt(1));
                string category = GetCellValue(spreadSheet, row.Descendants<Cell>().ElementAt(2));
                string income = GetCellValue(spreadSheet, row.Descendants<Cell>().ElementAt(3));
                string expense = GetCellValue(spreadSheet, row.Descendants<Cell>().ElementAt(4));
                if (!double.TryParse(income, out var incomeValue))
                {
                    throw new InvalidCastException(string.Format(
                        "Invalid value for Income (col 4) at row {0}", 
                        row.RowIndex+1));
                }
                if (!double.TryParse(expense, out var expenseValue))
                {
                    throw new InvalidCastException(string.Format(
                        "Invalid value for Expense (col 5) at row {0}",
                        row.RowIndex + 1));
                }
                Transaction transaction = new Transaction(
                    new Category(category, 0),
                    description,
                    DateTime.FromOADate(double.Parse(date)),
                    expenseValue > 0 ? (expenseValue * -1) : incomeValue
                );
                transactions.Add(transaction);
            }
            return transactions;
        }
    }

    // source: https://stackoverflow.com/questions/3321082/from-excel-to-datatable-in-c-sharp-with-open-xml
    public static string GetCellValue(SpreadsheetDocument document, Cell cell)
    {
        SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
        string value = cell.CellValue.InnerXml;

        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
        }
        else
        {
            return value;
        }
    }

}