using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CountryController
{
    public static CountryObject selectedFort;

    public static void AddMoney(Side side)
    {
        List<CountryObject> sideForts = side == Side.Blue ? GameController.blueForts : GameController.redForts;

        foreach (var fort in sideForts)
        {
            if (fort.side == side && fort.revenueTurnsLeft > 0)
            {
                fort.treasury += fort.fort.revenue;
                fort.revenueTurnsLeft--;
            }
        }
    }

    public static void RemoveMoney(int money)
    {
        //take selected country and remove money
        if (selectedFort != null)
        {
            selectedFort.treasury -= money;
        }
    }


    public static void Disclose(CountryObject fort, Side side)
    {
        fort.side = side;
        //TODO:color differenctly
    }

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