using System.Collections.Generic;
using System.Linq;

namespace mvHub
{
    public partial class DomSupport
    {
        public partial class MvNode
        {

            #region "Replace"
            public virtual void Replace(string newValue)
            {


                var match = false;

                if (_parent != null)
                {
                    match = newValue.IndexOf(_parent.Delimiter) != -1;
                }

                if (match)
                {
                    throw new RecordException("Insert Assignment not Allowed");
                }

                if (_children != null)
                {
                    foreach (var n in _children)
                    {
                        n.Dispose();
                    }
                    _children = null;
                }
                Value = newValue;
            }

            protected virtual void Replace(object newValue, params int[] positions)
            {

                var current = RootNode.GetNode(0);

                if (current == null)
                {
                    throw new RecordException("Root Node Not Defined");
                }

                foreach (var t in positions)
                {
                    var p = t;

                    if (p < 0)
                    {
                        if (current._children == null) { InitChildren(current); }
                        if (current._children != null) p = current._children.Count() + 1;
                    }
                    if (p == 0) { break; }
                    if (current._children == null) { InitChildren(current); }

                    if (current._children != null && p > current._children.Count)
                    {
                        do
                        {
                            var n = new MvNode(current);
                            current._children.Add(n);
                        } while (p > current._children.Count);
                    }
                    if (current._children != null) current = current._children[p - 1];
                }


                current.Replace(newValue.ToString());
            }

            #endregion
            private void InitChildren(MvNode current)
            {
                current._children = new List<MvNode>();
                var n = new MvNode(current) { Value = current.Value };
                current._children.Add(n);
                current.Value = string.Empty;
            }
        }

    }
}
