using System;
using System.Collections.Generic;

namespace mvHub
{
     public partial class DomSupport
     {
          public partial class MvNode : IDisposable
          {
               private MvNode _parent;
               private List<MvNode> _children;

              public MvNode(MvNode parent)
               {
                    _children = null;
                    if (parent == null)
                    {
                         DomLevel = 0;
                         _children = new List<MvNode>();
                    }
                    else
                    {
                         _parent = parent;
                         if ((int)_parent.DomLevel + 1 > 4)
                         {
                              throw new DelimitSetException("Nesting DOM Error");
                         }
                         Value = string.Empty;
                         DomLevel = _parent.DomLevel + 1;
                    }
               }

               public virtual string Name { get; set; }
               public virtual string Conversion { get; set; }

              public string Value
               {
                    get;
                    set;
               }
               public virtual MvNode Parent => _parent;

              public virtual DelimiterSet DelimiterSet => _parent.DelimiterSet;

              public virtual char Delimiter => DelimiterSet.Delimiter(DomLevel);

              public virtual char ChildDelimiter
               {
                    get
                    {
                         if (DomLevel == DomLevel.Text)
                         {
                              return DelimiterSet.Delimiter(DomLevel);
                         }
                         return DelimiterSet.Delimiter(DomLevel + 1);
                    }
               }

               public virtual DomLevel DomLevel { get; }

              public MvNode RootNode
               {
                    get
                    {
                        if (_parent != null) { return _parent.RootNode; }
                        return this;
                    }
               }


               public virtual void Dispose()
               {
                    if (_children != null)
                    {
                         foreach (var n in _children)
                         {
                              if (n != null) n.Dispose();
                         }
                    }
                    _children = null;
                    _parent = null;
               }

          }
     }
}
