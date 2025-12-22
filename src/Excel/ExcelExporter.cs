using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;

namespace NDB.Kit.Excel;
public sealed class ExcelExporter
{
    private readonly ExcelHelper _helper;

    public ExcelExporter(ExcelHelper helper)
    {
        _helper = helper;
    }
    public byte[] Export<T>(IEnumerable<T> data, string sheetName)
    {
        var columns = GetColumns<T>();

        using var ms = new MemoryStream();

        using (var doc = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
        {
            var wbPart = doc.AddWorkbookPart();
            wbPart.Workbook = new Workbook();

            var wsPart = wbPart.AddNewPart<WorksheetPart>();
            wsPart.Worksheet = new Worksheet(new SheetData());

            var styles = wbPart.AddNewPart<WorkbookStylesPart>();
            styles.Stylesheet = _helper.GenerateStylesheet();
            styles.Stylesheet.Save();

            var sheets = wbPart.Workbook.AppendChild(new Sheets());
            sheets.Append(new Sheet
            {
                Id = wbPart.GetIdOfPart(wsPart),
                SheetId = 1,
                Name = sheetName
            });

            var sheetData = wsPart.Worksheet.GetFirstChild<SheetData>()
                ?? throw new InvalidOperationException("SheetData not initialized");

            BuildHeader(sheetData, columns);
            BuildBody(sheetData, data, columns);

            wsPart.Worksheet.Save();
            wbPart.Workbook.Save();
        } 

        return ms.ToArray(); 
    }


    private static IReadOnlyList<ColumnMeta> GetColumns<T>()
    {
        var cols = typeof(T)
            .GetProperties()
            .Select(p => new
            {
                Property = p,
                Attr = p.GetCustomAttribute<ExcelColumnAttribute>()
            })
            .Where(x => x.Attr != null)
            .Select(x => new ColumnMeta(
                x.Attr!.Order,
                x.Attr.Header,
                x.Property,
                x.Attr.CellType,
                x.Attr.Style))
            .OrderBy(x => x.Order)
            .ToList();

        if (!cols.Any())
            throw new InvalidOperationException(
                $"DTO {typeof(T).Name} tidak punya ExcelColumnAttribute");

        if (cols.Select(x => x.Order).Distinct().Count() != cols.Count)
            throw new InvalidOperationException(
                $"Order ExcelColumnAttribute duplikat di {typeof(T).Name}");

        return cols;
    }

    private void BuildHeader(SheetData sheetData, IReadOnlyList<ColumnMeta> cols)
    {
        var row = new Row();
        foreach (var col in cols)
            row.Append(_helper.CreateCell(col.Header, CellValues.String, ExcelStyle.Header));
        sheetData.Append(row);
    }

    private void BuildBody<T>(
        SheetData sheetData,
        IEnumerable<T> data,
        IReadOnlyList<ColumnMeta> cols)
    {
        foreach (var item in data)
        {
            var row = new Row();
            foreach (var col in cols)
            {
                var value = col.Property.GetValue(item);
                row.Append(_helper.CreateCell(value, col.CellType, col.Style));
            }
            sheetData.Append(row);
        }
    }

    private sealed record ColumnMeta(
        int Order,
        string Header,
        PropertyInfo Property,
        CellValues CellType,
        ExcelStyle Style);
}
