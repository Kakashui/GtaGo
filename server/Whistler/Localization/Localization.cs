using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.SDK;

namespace Whistler 
{   
    static class Localization
    {       
        public static string Translate(this string key, params object[] args)
        {
            foreach (var arg in args)
            {
                key += $"@{arg}";
            }
            return key;
        }
    }
}
