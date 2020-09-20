using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CountryController
{
    public static FortObject selectedFort;
    public static void SelectFort(Hex hex, Side side)
    {
        if (hex.fort != null && hex.fort.side == side)
        {
            if (selectedFort == null)
            {
                selectedFort = hex.fort;
            }
            else
            {
                selectedFort = null;
            }
        }
        else
        {
            selectedFort = null;
        }
    }

    public static bool SelectDisclosableFort(Hex hex, Side side)
    {
        if (hex.fort != null && hex.fort.side == Side.None && hex.fort.initialSide == side)
        {
            return true;
        }

        return false;
    }

    public static void ResetController()
    {
        selectedFort = null;
    }
}