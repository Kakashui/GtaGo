using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.ServerConfiguration
{
    public class MySQLConfig
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DataBase { get; set; }
        public string SSL { get; set; }
        public int Port { get; set; }
    }
}
