using System.Text;

namespace mvHub
{
    public static class MvCsv 
    {
        public static Mvdom ToMV(string csv)
        {
            return new Mvdom(MvCsv.ToMvString(csv));
        }

        public static string ToMvString(string csv)
        {
            var mvCsv = new StringBuilder();
            var dl = new DelimiterSet();
            var qMode = false;
            for (var i = 0; i < csv.Length - 1; i++)
            {
                var c = csv[i];
                if (qMode)
                {
                    switch (c)
                    {
                        case '"':
                            if (csv[i + 1] == '"')
                            {
                                mvCsv.Append(c);
                                i++;
                            }
                            else
                            {
                                qMode = false;
                            }
                            break;
                        default:
                            mvCsv.Append(c);
                            break;
                    }
                }
                else
                {
                    switch (c)
                    {
                        case ',':
                            mvCsv.Append(dl.ValueMark);
                            break;
                        case (char)13:
                            break;
                        case (char)10:
                            mvCsv.Append(dl.Attribute);
                            break;
                        case '"':
                            qMode = true;
                            break;
                        default:
                            mvCsv.Append(c);
                            break;
                    }
                }
            }

            return mvCsv.ToString();
        }
    }
}
