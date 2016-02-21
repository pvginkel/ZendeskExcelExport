using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ZendeskExcelExport
{
    internal static class RegistryKeyExtensions
    {
        public static bool? GetBoolean(this RegistryKey self, string name)
        {
            var result = self.GetValue(name);

            return result is int && (int)result != 0;
        }
    }
}
