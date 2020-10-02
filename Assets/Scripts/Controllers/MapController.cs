using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapData currentMapTest;
    public MapData currentMap;
    public HexCreator creator;

    public static int width;
    public static int height;

    public static Hex[,] hexes;

    public List<Fort> fortPrefabs;
    public static List<Fort> fortTypes;

    public List<Unit> unitPrefabs;
    public static List<Unit> unitTypes;

    public Material countryMaskMaterial;
    public Material countryBorderMaterial;
    public static int countryCount;

    HashSet<Hex> highlightedHexes;

    public void CreateMap(MapData map)
    {
        highlightedHexes = new HashSet<Hex>();
        MaskCreator.InitializeCreator(countryMaskMaterial, countryBorderMaterial);
        currentMap = map;

        fortTypes = fortPrefabs;
        unitTypes = unitPrefabs;
        
        width = currentMap.width;
        height = currentMap.height;

        hexes = creator.CreateMap(currentMap);
        MapCreator.CreateGameMap(currentMap);
        countryCount = currentMap.countries.Count;
    }

    public void DesetupForDisclosure()
    {
        int mask = 0;
        MaskCreator.CreateZeroMask(countryCount);
    }

    public void SetupForDisclosure(Side side)
    {
        int mask = GameController.GetDisclosableCountry(side);
        MaskCreator.CreateMask(mask, countryCount);
    }

    public void SetupForBuying(Side side, bool isDisclosing)
    {
        if (highlightedHexes.Count != 0)
        {
            DesetupForBuying(false);
        }
        if (isDisclosing)
        {            
            CountryController.SelectedEmptyDisclosedHexes(side, ref highlightedHexes);
        }
        else
        {
            CountryController.SelectedEmptyHexes(side, ref highlightedHexes);
        }
        HighLightHexes(highlightedHexes, true);
    }
    public void DesetupForBuying(bool isDisclosing)
    {
        if (isDisclosing)
        {
            CountryController.lastDisclosedCountry = null;
        }
        HighLightHexes(highlightedHexes, false);
        highlightedHexes.Clear();
    }
    public void HighLightHexes(HashSet<Hex> hexes, bool hightlight = true)
    {
        foreach(var hex in hexes)
        {
            hex.HightLight(hightlight);
        }
    }

    public void ClearAllHexes()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                hexes[j, i].HightLight(false);
            }
        }
    }

    public static void GetNewCountryColors()
    {
        int hasColor = 0;
        int hasWar = 0;
        int color = 0;

        foreach(var country in GameController.blueCountries)
        {
            int flag = 1 << country.id;
            if (country.side != Side.None)
            {
                hasColor += flag;
                if (country.side == Side.Blue)
                {
                    color += flag;
                }
                if (!country.canGetMoney)
                {
                    hasWar += flag;
                }
            }
        }
        foreach (var country in GameController.redCountries)
        {
            int flag = 1 << country.id;
            if (country.side != Side.None)
            {
                hasColor += flag;
                if (country.side == Side.Blue)
                {
                    color += flag;
                }
                if (!country.canGetMoney)
                {
                    hasWar += flag;
                }
            }
        }

        MaskCreator.CreateBorderMask(hasColor, color, hasWar, GameController.blueCountries.Count + GameController.redCountries.Count);

    }
    internal void ResetController()
    {
        ClearAllHexes();
        DesetupForDisclosure();
    }
}
