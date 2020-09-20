using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapCreator
{
    public static List<FortObject> blueForts = new List<FortObject>();
    public static List<FortObject> redForts = new List<FortObject>();
    public static string[] sideToName = new string[]
    {
        "RedFort","BlueFort","NeutralFort"
    };
    public static void CreateGameMap(MapData map)
    {
        CreateForts(map);
    }
    
    static void CreateForts(MapData map)
    {
        List<Country> availableCountries = new List<Country>();
        availableCountries.AddRange(map.countries);
        
        int count = map.countries.Count / 2;
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, availableCountries.Count);
            blueForts.Add(CreateFort(availableCountries[index], Side.Blue));
            availableCountries.RemoveAt(index);
        }
        
        for (int i = 0; i < availableCountries.Count; i++)
        {
            redForts.Add(CreateFort(availableCountries[i], Side.Red));
        }
    }

    static FortObject CreateFort(Country country,Side side)
    {
        GameObject newFort = new GameObject(sideToName[(int) side]);
        FortObject fortObject = newFort.AddComponent<FortObject>();
        fortObject.InitializeFort(country, side);
            
        Vector2Int coordinates = Util.FindCoordinates(country.fort);
        newFort.transform.localPosition = Util.GetPosition(coordinates);
            
        fortObject.fortHex = Util.HexExist(coordinates.x, coordinates.y);
        fortObject.fortHex.fort = fortObject;
        ColorFort(country, fortObject, side);

        return fortObject;
    }

    static void ColorFort(Country country, FortObject fort, Side side)
    {
        for(int i=0;i<country.hexes.Count;i++)
        {
            Vector2Int coordinates = Util.FindCoordinates(country.hexes[i]);
            if (country.fort == country.hexes[i])
            {
                MapController.sprites[coordinates.x, coordinates.y].color = side == Side.Blue ? Color.blue : Color.red;
            }
            else
            {
                MapController.sprites[coordinates.x, coordinates.y].color = side == Side.Blue ? Color.cyan : Color.magenta;
            }

            MapController.hexes[coordinates.x, coordinates.y].fort = fort;
            fort.hexes[i] = MapController.hexes[coordinates.x, coordinates.y];
        }
    }
}
