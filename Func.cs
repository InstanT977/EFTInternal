using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromGod
{
    class Func
    {
        public static int takeNDigits(int number, int N)
        {
            // this is for handling negative numbers, we are only insterested in postitve number
            number = Math.Abs(number);
            // special case for 0 as Log of 0 would be infinity
            if (number == 0)
                return number;
            // getting number of digits on this input number
            int numberOfDigits = (int)Math.Floor(Math.Log10(number) + 1);
            // check if input number has more digits than the required get first N digits
            if (numberOfDigits >= N)
                return (int)Math.Truncate((number / Math.Pow(10, numberOfDigits - N)));
            else
                return number;
        }
    }
}
