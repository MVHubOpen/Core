using System;
using System.Linq;
using System.Text;

namespace mvHub
{
     public partial class DomSupport
     {
          public partial class MvNode
          {
               #region "Extract"
               public string Extract(params int[] positions)
               {

                    var current = RootNode.GetNode(0);

                    if (current == null){ return null;}

                   foreach (var p in positions)
                   {
                       if (p == 0) { return current.ToString(); }
                       var ncurrent = current.GetNode(p) ?? new MvNode(current.Parent);
                       current = ncurrent;
                       if (current._children == null)
                       {
                           return current.ToString();
                       }
                   }
                    return current.ToString();

               }

               public MvNode GetNode(int index)
               {
                    if (_children == null)
                    {
                        return _parent == null ? this : null;
                    }

                   if (index > _children.Count)
                    {
                         return null;
                    }
                   index -= 1;
                   if (index < 0) { index = 0; }
                   return _children[index];
               }
               #endregion


               #region "ToArray"
               public string[] ToArray()
               {
                    return ToArray(0, 0, 0, DelimiterSet.Attribute);
               }
               public string[] ToArray(char delimter)
               {
                    return ToArray(0, 0, 0, delimter);
               }
               public string[] ToArray(int attribute, char delimter)
               {
                    return ToArray(attribute, 0, 0, delimter);
               }
               public string[] ToArray(int attribute, int valuePostion, char delimter)
               {
                    return ToArray(attribute, valuePostion, 0, delimter);
               }
               public string[] ToArray(int attribute, int valuePostion, int subValuePostion, char delimter)
               {

                    var val = Extract(attribute, valuePostion, subValuePostion);

                    return val.Split(delimter);

               }
               #endregion
               #region "DCount"
               public int DCount()
               {
                    return DCount(0, 0, 0, 0, ChildDelimiter);
               }
               public int DCount(char delimter) { return DCount(0, 0, 0, 0, delimter); }
               public int DCount(int attribute) { return DCount(attribute, 0, 0, 0, Delimiter); }
               public int DCount(int attribute, char delimter) { return DCount(attribute, 0, 0, 0, delimter); }
               public int DCount(int attribute, int valuePostion, char delimter) { return DCount(attribute, valuePostion, 0, 0, delimter); }
               public int DCount(int attribute, int valuePostion, int subValuePostion, char delimter) { return DCount(attribute, valuePostion, subValuePostion, 0, delimter); }
               public int DCount(int attribute, int valuePostion, int subValuePostion, int textMarkPosition, char delimter)
               {
                    var val = Extract(attribute, valuePostion, subValuePostion, textMarkPosition);
                    var dcnt = val.Count(f => f == delimter);
                    if (val.Length > 0) { dcnt++; }
                    return dcnt;
               }
               #endregion
               #region "Count"
               public int Count(char delimter) { return Count(0, 0, 0, delimter); }
               public int Count(int attribute, char delimter) { return Count(attribute, 0, 0, delimter); }
               public int Count(int attribute, int valuePostion, char delimter) { return Count(attribute, valuePostion, 0, delimter); }
               public int Count(int attribute, int valuePostion, int subValuePostion, char delimter)
               {
                    var val = Extract(attribute, valuePostion, subValuePostion);
                    return val.Count(f => f == delimter);
               }
               #endregion
               public DateTime ToDate(params int[] positions)
               {
                    var val = Extract(positions);

                    return MvConversion.ToDate(val);
               }
               public DateTime ToTime(params int[] positions)
               {
                    var val = Extract(positions);
                    return MvConversion.ToTime(val);
               }
               public int ToInt(params int[] positions)
               {
                    var val = Extract(positions);
                    return MvConversion.ToInt(val);

               }
               public double ToDouble(params int[] positions)
               {
                    var val = Extract(positions);
                    return MvConversion.ToDouble(val);
               }


               public override string ToString()
               {
                    if (_children == null)
                    {
                         return Value;
                    }
                    var s = new StringBuilder();
                    var addDelimiter = false;

                    foreach (var n in _children)
                    {
                         if (addDelimiter) { s.Append(Delimiter); } else { addDelimiter = true; }
                         s.Append(n);
                    }
                    return s.ToString();
               }
               public string ToString(StringFormat format)
               {
                    if (_children == null)
                    {
                         return "\"" + Value + "\"";
                    }
                    var s = new StringBuilder();
                    var addDelimiter = false;
                    const char delimiter = ',';
                   if (_children.Count <= 0) return s.ToString();
                   if (format == StringFormat.Json || format == StringFormat.NamedJson) s.Append('[');

                   foreach (var n in _children)
                   {
                       if (addDelimiter) { s.Append(delimiter); } else { addDelimiter = true; }
                       s.Append(n.ToString(format));
                   }
                   if (format == StringFormat.Json || format == StringFormat.NamedJson) s.Append(']');
                   return s.ToString();
               }
          }
     }
}