﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Thanks! http://blog.xin9le.net/entry/2013/03/03/123246

namespace Extentions
{
  //index付きItemを定義
  public class IndexedItem<T>
  {
    public T Element { get; private set;}
    public int Index { get; private set;}
    public IndexedItem(T element, int index)
    {
      this.Element = element;
      this.Index = index;
    }
  }

  static class LINQExtenstions
  {
    //ForEach
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> act)
    {
      foreach (var item in source)
      {
        act(item);
      }
    }
    public static IEnumerable<T> Convert<T>(this IEnumerable<T> source, Func<object, T> converter)
    {
      foreach (var item in source)
        yield return converter(item);
    }
    //Add Index to Collection
    public static IEnumerable<IndexedItem<T>> Indexed<T>(this IEnumerable<T> source)
    {
      return source.Select((x, i) => new IndexedItem<T>(x, i));
    }
  }
}
