
using NPOI.HSSF.UserModel;
using System.Data;
using System.IO;

namespace Framework.Core.IO
{
    public class ExcelUtils
    {
        #region DataTable

        public static MemoryStream RenderToExcel(DataTable SourceTable)
        {
            var workbook = new HSSFWorkbook();
            var ms = new MemoryStream();
            var sheet = workbook.CreateSheet();
            var headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in SourceTable.Rows)
            {
                var dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        public static DataTable RenderFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
        {
            var workbook = new HSSFWorkbook(ExcelFileStream);
            var sheet = workbook.GetSheet(SheetName);

            var table = new DataTable();

            var headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                var dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                    dataRow[j] = row.GetCell(j).ToString();
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        public static DataTable RenderFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            var workbook = new HSSFWorkbook(ExcelFileStream);
            var sheet = workbook.GetSheetAt(SheetIndex);

            var table = new DataTable();

            var headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                var dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        #endregion

        #region DataSet

        public static MemoryStream RenderToExcel(DataSet Source)
        {
            var workbook = new HSSFWorkbook();
            var ms = new MemoryStream();

            foreach (DataTable table in Source.Tables)
            {
                var sheet = workbook.CreateSheet(table.TableName);
                var headerRow = sheet.CreateRow(0);

                // handling header.
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    var dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }

                sheet = null;
                headerRow = null;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;

            return ms;
        }

        public static DataSet RenderFromExcel(Stream ExcelFileStream)
        {
            if (ExcelFileStream == null || ExcelFileStream.Length < 1)
            {
                return null;
            }

            var workbook = new HSSFWorkbook(ExcelFileStream);
            var HeaderRowIndex = 0;

            var set = new DataSet();

            for (int index = 0; index < workbook.NumberOfSheets; index++)
            {
                var sheet = workbook.GetSheetAt(index);
                var headerRow = sheet.GetRow(HeaderRowIndex);

                if (headerRow == null)
                {
                    break;
                }

                int cellCount = headerRow.LastCellNum;
                var table = new DataTable(sheet.SheetName);

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    var dataRow = table.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.Cells[j] != null)
                            dataRow[j] = row.Cells[j].ToString();
                    }

                    table.Rows.Add(dataRow);
                }

                sheet = null;
                set.Tables.Add(table);
            }

            ExcelFileStream.Close();
            workbook = null;
            return set;
        }

        #endregion
    }
}
