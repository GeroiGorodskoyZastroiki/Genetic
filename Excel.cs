using System.Collections.Generic;

public class Excel
{
    Workbook wb = new Workbook("Датасет.xlsx");
    WorksheetCollection ws = wb.Worksheets[0];
    public byte rows = worksheet.Cells.MaxDataRow;
    public List<List<byte>> coefficients = new List<List<byte>>();
    public List<short> desiredValue = new List<short>();

    public Excel()
    {
        for (int i = 1; i < rows + 1; i++)
        {
            values.Add(new List<byte>());
            values[i].Add(worksheet.Cells[i, 0].Value);
            values[i].Add(worksheet.Cells[i, 1].Value);
            values[i].Add(worksheet.Cells[i, 2].Value);
            desired.Add(worksheet.Cells[i, 3].Value);
        }
    }
}