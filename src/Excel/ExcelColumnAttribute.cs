using DocumentFormat.OpenXml.Spreadsheet;

namespace NDB.Kit.Excel;
[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelColumnAttribute : Attribute
{
    public string Header { get; }
    public int Order { get; init; }
    public ExcelStyle Style { get; init; } = ExcelStyle.Body;
    public CellValues CellType { get; init; } = CellValues.String;

    public ExcelColumnAttribute(string header)
    {
        Header = header;
    }
}
