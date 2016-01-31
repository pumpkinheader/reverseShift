using System;
using System.Collections.Generic;
using System.Linq;

namespace reverseShift
{

  class Position
  {
    public string Name { get; private set; }
    public List<List<string>> _positions = new List<List<string>>();

    public Position(string name)
    {
      Name = name;
    }
    public Position(string name, List<string> times)
    {
      Name = name;
      foreach (var t in times.Select((time, index) => new { time, index }))
      {
        _positions[t.index].Add(t.time);
      }
    }
    public void addPeople(Tuple<string,int> positions)
    {
      var posName = positions.Item1;
      var posIndex = positions.Item2;
      _positions[posIndex].Add(posName);
    }
  }
}
