﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reverseShift
{
  public class People
  {
    public string Name { get; private set; }
    private List<string> posList = new List<string>();

    public People(string name)
    {
      Name = name;
    }

    public void addPos(string name) {
      posList.Add(name);
    }
    public List<string> getPosList(string name)
    {
      return posList;
    }

  }
}