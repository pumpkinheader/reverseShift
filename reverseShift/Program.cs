using System;
using System.Collections.Generic;
using System.Linq;
using LinqToExcel;
using Extentions;

namespace reverseShift
{
  class Program
  {
    static void Main(string[] args)
    {
      var members = new List<Member>();
      var positions = new List<Position>();

      //ファイル読み込み
      if (args.IsEmpty())
      {
        args = new string[]{"test_shift.xlsx"};
      }
      string fileName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/" + args[0];
      var excel = new ExcelQueryFactory(fileName);
      excel.ReadOnly = true;
      string worksheetName = excel.GetWorksheetNames().ToList()[0];
      var worksheet = excel.Worksheet(worksheetName);

      //header設定
      var header = excel.GetColumnNames(worksheetName).ToList();
      //時刻情報のみ取り出す
      header.RemoveAt(0);

      //member生成
      worksheet.ForEach(row =>
      {
        var cells = row.ConvertAll(cell => cell.Value.ToString());
        members.Add(new Member(cells[0], cells.Skip(1).ToList()));
      });

      //position生成
      members
        .SelectMany(m => m.PositionNames)
        .Distinct()
        .ForEach(name =>
        {
          //空欄でなければ追加
          if (name != "")
            positions.Add(new Position(name));
        });
      
      //Shift生成
      positions.ForEach(p =>
      {
        p.Shifts.Add(header);
        members.ForEach(m =>
        {
          //(名前, シフトは何時か)を出力
          var listFromJob =
            m.PositionNames
            .Indexed()
            .Where(l => l.Element == p.Name)
            .Select(s => new IndexedItem<string>(m.Name, s.Index));

          //該当シフトがあればフォーマットして追加
          //フォーマット: (A, 1) のとき list[1] = "A"
          if (!listFromJob.IsEmpty())
            p.Shifts.Add(new List<string>().FillIndex(listFromJob).ToList());
        });
      });

      //Excelに書き込めるよう、arrayに変換
      var positionsArray = positions.Select(p => new { Name = p.Name, Datasets = ListOfListToArray(p.Shifts) });
      
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

    public static Object[,] ListOfListToArray<T>(List<List<T>> source)
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
