namespace mvHub
{
     public partial class DomSupport
     {
          public partial class MvNode
          {

               public string this[int position]
               {
                    get
                    {
                        if (position > _children.Count || position < 1)
                         {
                              return string.Empty;
                         }
                        return _children[position - 1].ToString();
                    }
                   set
                    {
                         Replace(value, position );
                    }
               }
               public string this[int attribute, int valuePostion]
               {
                    get
                    {
                         return this[attribute, valuePostion, 0, 0];
                    }
                    set
                    {
                         Replace(value, attribute, valuePostion);
                    }
               }
               public string this[int attribute, int valuePostion, int subValuePostion]
               {
                    get
                    {
                         return this[attribute, valuePostion, subValuePostion, 0];
                    }
                    set
                    {
                         Replace(value, attribute, valuePostion, subValuePostion);
                    }
                    
               }
               public string this[int attribute, int valuePostion, int subValuePostion, int textMark]
               {
                    get
                    {
                        return Extract(attribute, valuePostion, subValuePostion,textMark);
                    }
                    set
                    {
                         Replace(value, attribute, valuePostion, subValuePostion, textMark);
                    }
               }
          }
     }
}
