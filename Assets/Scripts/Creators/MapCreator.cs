using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapCreator
{
    //public static List<CountryObject> blueForts = new List<CountryObject>();
    //public static List<CountryObject> redForts = new List<CountryObject>();
    public static string[] sideToName = new string[]
    {
        "BlueFort","RedFort","NeutralFort"
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
            GameController.blueCountries.Add(CreateCountry(availableCountries[index], Side.Blue, map.countries.IndexOf(availableCountries[index])));
            availableCountries.RemoveAt(index);
        }
        
        for (int i = 0; i < availableCountries.Count; i++)
        {
            GameController.redCountries.Add(CreateCountry(availableCountries[i], Side.Red, map.countries.IndexOf(availableCountries[i])));
        }
    }

    static CountryObject CreateCountry(Country country,Side side, int id)
    {
        GameObject newFort = new GameObject(sideToName[(int) side]);
        CountryObject countryObject = newFort.AddComponent<CountryObject>();
        countryObject.InitializeFort(country, side, id);
            
        Vector2Int coordinates = Util.FindCoordinates(country.fort);
        newFort.transform.localPosition = Util.GetPosition(coordinates);
            
        countryObject.fortHex = Util.HexExist(coordinates.x, coordinates.y);
        countryObject.fortHex.fort = countryObject;
        ColorFort(country, countryObject, side);

        return countryObject;
    }

    static void ColorFort(Country country, CountryObject fort, Side side)
    {
        for(int i=0;i<country.hexes.Count;i++)
        {
            Vector2Int coordinates = Util.FindCoordinates(country.hexes[i]);
            /*if (country.fort == country.hexes[i])
            {
                MapController.hexes[coordinates.x, coordinates.y].hexHighlight.color = side == Side.Blue ? Color.blue : Color.red;
            }
            else
            {
                MapController.hexes[coordinates.x, coordinates.y].hexHighlight.color = side == Side.Blue ? Color.cyan : Color.magenta;
            }*/

            MapController.hexes[coordinates.x, coordinates.y].fort = fort;
            fort.hexes[i] = MapController.hexes[coordinates.x, coordinates.y];
        }
    }
}
