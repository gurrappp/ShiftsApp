using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftsLoggerUI.Validation
{
    public class Validate
    {
        public int? ValidateId(string? input)
        {
            if (!int.TryParse(input, out var result))
            {
                Console.WriteLine("Wrong input!");
                return null;
            }
                
            return result;
        }

        public int ValidateMenuOption(string? option)
        {
            if (!int.TryParse(option, out int result))
            {
                Console.WriteLine("Wrong input!");
                return -1;
            }

            return result;
        }

        public DateTime? ValidateTime(string? input)
        {
            
            if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out var timeResult))
                return timeResult;
            
           return null;
        }
    }
}
