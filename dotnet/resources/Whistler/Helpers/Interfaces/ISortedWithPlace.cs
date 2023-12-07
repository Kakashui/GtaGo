using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Helpers.Interfaces
{
    public interface ISortedWithPlace
    {
        public int SortedValue { get; }
        public int Place { set; }
    }
}
