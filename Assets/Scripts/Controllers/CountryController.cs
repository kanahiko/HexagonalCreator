using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CountryController
{
    public static CountryObject selectedCountry;

    public static CountryObject lastDisclosedCountry;

    public static void AddMoney(Side side)
    {
        List<CountryObject> sideCountries = side == Side.Blue ? GameController.blueCountries : GameController.redCountries;

        foreach (var country in sideCountries)
        {
            if (country.side == side && country.revenueTurnsLeft > 0 && country.canGetMoney)
            {
                country.treasury += country.fort.revenue;
                country.revenueTurnsLeft--;
            }
        }
    }
    public static void AddDiclosedMoney(Side side)
    {
        List<CountryObject> sideCountries = side == Side.Blue ? GameController.blueCountries : GameController.redCountries;

        foreach (var country in sideCountries)
        {
            if (country.side == side && !country.hadDisclosedMoney)
            {
                country.treasury += country.fort.secretArmy;
                country.hadDisclosedMoney = true;
            }
        }
    }

    public static void RemoveMoney(int money)
    {
        //take selected country and remove money
        if (selectedCountry != null)
        {
            selectedCountry.treasury -= money;
        }
    }


    public static void Disclose(CountryObject countryObject, Side side)
    {
        countryObject.side = side;
        AddDiclosedMoney(side);
        lastDisclosedCountry = countryObject;
        MapController.GetNewCountryColors();
        //TODO:color differenctly
    }



    public static void SelectFort(Hex hex, Side side)
    {
        if (hex.fort != null && hex.fort.side == side)
        {
            if (selectedCountry == null)
            {
                selectedCountry = hex.fort;
            }
            else
            {
                selectedCountry = null;
            }
        }
        else
        {
            selectedCountry = null;
        }
    }

    public static bool SelectedDisclosableFort(Hex hex, Side side, HashSet<CountryObject> selectFromThoseCountries)
    {
        if (hex.fort != null && hex.fort.side == Side.None && hex.fort.initialSide == side && (selectFromThoseCountries == null || selectFromThoseCountries.Contains(hex.fort)))
        {
            return true;
        }

        return false;
    }

    public static void ResetController()
    {
        selectedCountry = null;
    }
    public static void SelectedEmptyDisclosedHexes(Side side, ref HashSet<Hex> highlight)
    {
        highlight.Clear();
        foreach (var hex in lastDisclosedCountry.hexes)
        {
            if (hex.unit == null && lastDisclosedCountry.fortHex != hex)
            {
                highlight.Add(hex);
            }
        }
    }

    public static void SelectedEmptyHexes(Side side, ref HashSet<Hex> highlight)
    {
        highlight.Clear();
        var sidedCountries = side == Side.Blue ? GameController.blueCountries : GameController.redCountries;
        
        foreach(var country in sidedCountries)
        {
            if (country.hadDisclosedMoney && country.treasury!= 0)
            {
                foreach(var hex in country.hexes)
                {
                    if (hex.unit == null && country.fortHex != hex)
                    {
                        highlight.Add(hex);
                    }
                }
            }
        }
    }

}