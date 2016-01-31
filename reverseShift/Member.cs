using System.Collections.Generic;

namespace reverseShift
{
  public class Member
  {
    public string Name { get; private set; }
    public List<string> PositionNames { get; private set; }

    public Member(string name)
    {
      Name = name;
      PositionNames = new List<string>();
    }
    public Member(string name, List<string> positionNames)
    {
      Name = name;
      PositionNames = positionNames;
    }

    public void AddPositionName(string name) {
      PositionNames.Add(name);
    }

  }
}
