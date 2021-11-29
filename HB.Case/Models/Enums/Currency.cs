using System;

namespace HB.Case.Models.Enums
{
    [Flags]
    public enum Currency
    {
        None = 0,
        TL = 1,
        USD = 2,
        Euro = 4
    }
}
