using System.Collections.Generic;
using System.Linq;

namespace mvHub
{
    public class DelimiterSet
    {
        readonly char[] _dl;
        public DelimiterSet()
        {
            _dl = new char[5] { (char)255, (char)254, (char)253, (char)252, (char)251 };
        }
        public DelimiterSet(char[] delimiterArray)
        {
             if (delimiterArray.Length != 4)
             {
                  var ex = new DelimitSetException("Invalid Init Array of DelimiterSet");
                  ex.SuggestedHttpCode = 500;
                  throw ex;
             }
             _dl = delimiterArray;
        }

        public virtual char RecordMark => _dl[0];

        public virtual char Attribute => _dl[1];

        public virtual char ValueMark => _dl[2];

        public virtual char SubValueMark => _dl[3];

        public virtual char TextMark => _dl[4];

        public virtual char[] ToArray => (char[])_dl.Clone();

        public virtual char[] Array => _dl;

        public virtual List<char> ToList => ((char[])_dl.Clone()).ToList<char>();

        public virtual char Delimiter(DomSupport.DomLevel Level)
        {
             var level = (int)Level;

             if (level < 0 || level > 4)
             {
                  var ex = new DelimitSetException("Invalid Delimiter Level");
                  ex.SuggestedHttpCode = 500;
                  throw ex;
             }
             return _dl[level];
        }
    }
}
