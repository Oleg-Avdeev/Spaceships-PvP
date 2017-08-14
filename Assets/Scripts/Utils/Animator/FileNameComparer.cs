using System.Collections;
using UnityEngine;
using System;

public class FileNameComparer : IComparer
{
   int IComparer.Compare(object a, object b)
   {
      Texture2D c1=a as Texture2D;
      Texture2D c2=b as Texture2D;
      if (c1.name.Length < c2.name.Length) return -1;
      if (c1.name.Length > c2.name.Length) return 1;
      return c1.name.CompareTo(c2.name);
   }
}