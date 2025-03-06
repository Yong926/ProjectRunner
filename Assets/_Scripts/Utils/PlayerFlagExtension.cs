using System;
using System.Collections.Generic;

public static class PlayerFlagExtension
{
    public static bool HasAny(this PlayerState state, params PlayerState[] compares)
    {
        foreach (var c in compares)
        {
            if ((state & c) != 0)
                return true;
        }
        return false;
    }
}