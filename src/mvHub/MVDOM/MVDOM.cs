namespace mvHub
{
     public partial class Mvdom : DomSupport.MvNode
     {
          private DelimiterSet _delimiterSet;

          public Mvdom(): base(null)               
          {
               Init(string.Empty);

          }

          public Mvdom(string text)
               : base(null)
          {
               Init(text);  
          }

          private void Init(string text)
          {
               _delimiterSet = new DelimiterSet();
               
               Replace(text);
               

          }
          public override DelimiterSet DelimiterSet => _delimiterSet;

 }
}
