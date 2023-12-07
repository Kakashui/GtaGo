using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace Whistler.EFCore
{
    public class Configuration
    {
        private static IConfiguration Config { get; }

        static Configuration()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json");

            Config = configBuilder.Build();
        }

        public static IConfigurationSection GetSection(string sectionKey)
            => Config.GetSection(sectionKey);
        

        public static string GetConnectionString(string name)
            => Config.GetConnectionString(name);
    }
}
