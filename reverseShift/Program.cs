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

      //本番用
      //string fileName = System.Environment.CurrentDirectory + "\\test\\test_shift.xlsx";
      //var excel = new ExcelQueryFactory(fileName);
      //for Debug
      var excel = new ExcelQueryFactory(@"D:\\Documents\\Projects\\4_private\\rikoten\\reverseShift\\reverseShift\\test\\test_shift.xlsx");

      excel.ReadOnly = true;
      var worksheet = excel.Worksheet("Sheet1");

      //member追加
      worksheet.ForEach(row =>
      {
        var tasks = row.ConvertAll(cell => cell.Value.ToString());
        members.Add(new People(tasks[0], tasks.Skip(1).ToList()));
      });
      members.ForEach(member =>
      {
        Console.Write("Name:{0} Pos:",member.Name);
        member.getPosList().ForEach(pos => Console.Write(" {0} ", pos));
        Console.WriteLine("");
      });
      
      Console.ReadLine();
    }
  }
}
