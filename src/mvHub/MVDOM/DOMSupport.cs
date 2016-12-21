namespace mvHub
{
     public partial class DomSupport
     {
          public enum DomLevel
          {
               Record = 0,
               Attribute = 1,
               Value = 2,
               SubValue = 3,
               Text = 4
          }

          public enum StringFormat
          {
               Json,
               NamedJson,
               Xml
          }
     }


}
