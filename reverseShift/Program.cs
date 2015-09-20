using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LinqToExcel;
using System.Text;
using System.Threading.Tasks;
using Extentions;

namespace reverseShift
{
  class Program
  {
    static void Main(string[] args)
    {
      var members = new List<People>();
      var positions = new List<Position>();

      //ファイル読み込み
      string fileName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/" + args[0];
      var excel = new ExcelQueryFactory(fileName);
      excel.ReadOnly = true;
      string worksheetName = excel.GetWorksheetNames().ToList()[0];
      var worksheet = excel.Worksheet(worksheetName);

      //header設定
      var columnNames = excel.GetColumnNames(worksheetName).ToList();
      columnNames.RemoveAt(0);

      //member追加
      worksheet.ForEach(row =>
      {
        var tasks = row.ConvertAll(cell => cell.Value.ToString());
        members.Add(new People(tasks[0], tasks.Skip(1).ToList()));
      });

      //job生成
      members
        .SelectMany(p => p.getPosList())
        .Distinct()
        .ForEach(name =>
        {
          if (name != "")
            positions.Add(new Position(name));
        });
      positions.ForEach(p =>
      {
        p._positions.Add(columnNames);
        members.ForEach(m =>
        {
          var list = new List<string>();
          var listFromJob =
            m.getPosList()
            .Indexed()
            .Where(l => l.Element == p.Name)
            .Select(s => new IndexedItem<string>(m.Name, s.Index));
          if (!listFromJob.IsEmpty())
            p._positions.Add(list.Renew(listFromJob).ToList());
        });
      });
      //arrayに変換
      var positionsArray = positions.Select(p => new { Name = p.Name, Datasets = listOfListToArray(p._positions) });
      //Excel書き込み
      using (var excelwriter = new ExcelOperator())
      {
        positionsArray.Indexed().ForEach(d =>
        {
          excelwriter.WriteFromArray(d.Element.Datasets,d.Index+1,d.Element.Name);
        });
        excelwriter.SaveBook();
      }
    }

    public static Object[,] listOfListToArray<T>(List<List<T>> source)
    {
      int rowNum = source.Count();
      int columNum = source[0].Count();
      var datasets = new Object[rowNum, 25];
      source.Indexed().ForEach(s =>
      {
        s.Element.Indexed().ForEach(p =>
        {
          datasets[s.Index, p.Index] = p.Element;
        });
      });
      return datasets;
    }
  }
}
