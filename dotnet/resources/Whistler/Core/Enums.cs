using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Core
{
    public enum ChangeNameResult
    {
        BadCurrentName,
        NewNameIsExist,
        IncorrectNewName,
        Success,
    }
}