using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace reverseShift
{
  class ExcelOperator: IDisposable
  {
    //Excelオブジェクトの初期化:非.NETリソースと思われる
    Excel.Application ExcelApp = null;
    Excel.Workbooks workbooks = null;
    Excel.Workbook workbook = null;
    Excel.Sheets sheets = null;
    Excel.Worksheet worksheet = null;
    Excel.Range range = null;

    //通常オブジェクト
    public string directoryPath { get; set; }
    public string fileName { get; set; }

    //for writing
    public ExcelOperator()
    {
      ExcelApp = new Excel.Application();
      workbooks = ExcelApp.Workbooks;
      workbook = workbooks.Add();
      sheets = workbook.Sheets;
      worksheet = sheets[1];
      worksheet.Select(Type.Missing);

      directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
      fileName = "shift.xlsx";
    }

    public string GetDirPath()
    {
      return directoryPath + "/" +fileName;
    }
    public void WriteFromArray(Object[,] datasets)
    {
      var firstDimension = datasets.GetLength(0);
      var secondDimension = datasets.GetLength(1);
      range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[firstDimension, secondDimension]];
      range.Value2 = datasets;
    }
    public void WriteFromArray(Object[,] datasets, int sheetIndex)
    {
      if (null == sheets[sheetIndex])
      {
        sheets.Add();
      }
      worksheet = sheets[sheetIndex];
      WriteFromArray(datasets);
    }
    public void SaveBook()
    {
      workbook.SaveAs(GetDirPath());
    }


    //リソースの破棄
    ~ExcelOperator()
    {
      Dispose(false);
    }
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
      workbook.Close(false);
      ExcelApp.Quit();
      Marshal.ReleaseComObject(worksheet);
      Marshal.ReleaseComObject(sheets);
      Marshal.ReleaseComObject(workbook);
      Marshal.ReleaseComObject(workbooks);
      Marshal.ReleaseComObject(ExcelApp);

      range = null;
      worksheet = null;
      sheets = null;
      workbook = null;
      workbooks = null;
      ExcelApp = null;
    }


  }
}
