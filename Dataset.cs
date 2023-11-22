using Aspose.Cells;

public class Dataset
{
    static Workbook wb = new Workbook("Датасет.xlsx");
    static Worksheet ws = wb.Worksheets[0];
    public int rows = ws.Cells.MaxDataRow;
    public List<List<byte>> coefficients = new List<List<byte>>();
    public List<short> desiredValues = new List<short>();

    public Dataset()
    {
        for (int i = 1; i < rows + 1; i++)
        {
            coefficients.Add(new List<byte>());
            coefficients[i-1].Add((byte)(int)ws.Cells[i, 0].Value);
            coefficients[i-1].Add((byte)(int)ws.Cells[i, 1].Value);
            coefficients[i-1].Add((byte)(int)ws.Cells[i, 2].Value);
            desiredValues.Add((short)(int)ws.Cells[i, 3].Value);
        }
    }
}