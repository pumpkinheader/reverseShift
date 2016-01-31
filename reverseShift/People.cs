using System.Collections.Generic;

namespace reverseShift
{
  public class People
  {
    public string Name { get; private set; }
    private List<string> PosList = new List<string>();

    public People(string name)
    {
      Name = name;
    }
    public People(string name, List<string> posList)
    {
      Name = name;
      PosList = posList;
    }

    public void addPos(string name) {
      PosList.Add(name);
    }
    public List<string> getPosList()
    {
      return PosList;
    }

  }
}
