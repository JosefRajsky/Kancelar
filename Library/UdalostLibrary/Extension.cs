using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UdalostLibrary
{
    public static class EmumExtension
    {
        public static string GetDescription(Enum value)
        {
            return value
             .GetType()
             .GetMember(value.ToString())
             .FirstOrDefault()
             ?.GetCustomAttribute<DescriptionAttribute>()
             ?.Description
         ?? value.ToString();
        }
    }
}


