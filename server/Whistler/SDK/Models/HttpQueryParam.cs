using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.SDK.Models
{
    class HttpQueryParam
    {
        public string Key { get; set; }
        public dynamic Parameter { get; set; }
        public HttpQueryParam(string key, dynamic parameter)
        {
            Key = key;
            Parameter = parameter;
        }
        public override string ToString()
        {
            return $"{Key}={Parameter}";
        }
    }
}
