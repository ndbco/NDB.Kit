using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;

namespace NDB.Kit.Excel;
public sealed class ExcelHelper
{
    public Cell CreateCell(object? value, CellValues type, ExcelStyle style)
    {
        if (value is null)
            return new Cell { StyleIndex = (uint)style };

        if (value is DateTime dt)
        {
            return new Cell
            {
                CellValue = new CellValue(
                    dt.ToOADate().ToString(CultureInfo.InvariantCulture)),
                StyleIndex = (uint)style
            };
        }

        return new Cell
        {
            CellValue = new CellValue(value.ToString() ?? string.Empty),
            DataType = type,
            StyleIndex = (uint)style
        };
    }
    public Stylesheet GenerateStylesheet()
    {
        var fonts = new Fonts(
            new Font( // 0 Default
                new FontSize { Val = 11 }
            ),
            new Font( // 1 Header
                new Bold(),
                new FontSize { Val = 11 }
            )
        );

        var fills = new Fills(
            new Fill(new PatternFill { PatternType = PatternValues.None }),      // 0
            new Fill(new PatternFill { PatternType = PatternValues.Gray125 }),   // 1
            new Fill(new PatternFill(                                             // 2 Header
                new ForegroundColor { Rgb = new HexBinaryValue("E5E5E5") })
            { PatternType = PatternValues.Solid })
        );

        var borders = new Borders(
            new Border(), // 0
            new Border(   // 1 Thin border
                new LeftBorder { Style = BorderStyleValues.Thin },
                new RightBorder { Style = BorderStyleValues.Thin },
                new TopBorder { Style = BorderStyleValues.Thin },
                new BottomBorder { Style = BorderStyleValues.Thin },
                new DiagonalBorder())
        );

        var cellFormats = new CellFormats(
            new CellFormat(), // 0 Default

            new CellFormat // 1 Body
            {
                FontId = 0,
                BorderId = 1,
                ApplyBorder = true
            },

            new CellFormat // 2 Header
            {
                FontId = 1,
                FillId = 2,
                BorderId = 1,
                ApplyFill = true,
                ApplyBorder = true
            },

            new CellFormat // 3 Numeric
            {
                FontId = 0,
                BorderId = 1,
                ApplyBorder = true,
                ApplyNumberFormat = true
            }
        );

        return new Stylesheet(fonts, fills, borders, cellFormats);
    }

}
