using System;
using System.Collections.Generic;
using System.Linq;

namespace reverseShift
{

  class Position
  {
    public string Name { get; private set; }
    public List<List<string>> Shifts = new List<List<string>>();

    public Position(string name)
    {
      Name = name;
    }
    public Position(string name, List<string> times)
    {
      Name = name;
      foreach (var t in times.Select((time, index) => new { time, index }))
      {
        Shifts[t.index].Add(t.time);
      }
    }
    public void AddShifts(Tuple<string,int> positions)
    {
      var posName = positions.Item1;
      var posIndex = positions.Item2;
      Shifts[posIndex].Add(posName);
    }
  }
}
