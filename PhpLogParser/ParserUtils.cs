using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpLogParser
{
    public static class ParserUtils
    {
        public static int SScanf(string input, string format, params object[] args)
        {
            int count = 0;
            int index = 0;
            int startIndex = 0;
            int endIndex = 0;
            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == '%')
                {
                    if (format[i + 1] == 'd')
                    {
                        startIndex = input.IndexOf(" ", index);
                        endIndex = input.IndexOf(" ", startIndex + 1);
                        if (int.TryParse(input.Substring(startIndex, endIndex - startIndex), out int intValue))
                        {
                            args[count] = intValue;
                            count++;
                        }
                        index = endIndex;
                    }
                    else if (format[i + 1] == 'f')
                    {
                        startIndex = input.IndexOf(" ", index);
                        endIndex = input.IndexOf(" ", startIndex + 1);
                        if (float.TryParse(input.Substring(startIndex, endIndex - startIndex), out float floatValue))
                        {
                            args[count] = floatValue;
                            count++;
                        }
                        index = endIndex;
                    }
                    else if (format[i + 1] == 's')
                    {
                        startIndex = input.IndexOf(" ", index);
                        endIndex = input.IndexOf(" ", startIndex + 1);
                        args[count] = input.Substring(startIndex, endIndex - startIndex);
                        count++;
                        index = endIndex;
                    }
                    i++;
                }
            }
            return count;
        }
    }
}
