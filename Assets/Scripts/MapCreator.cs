using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapCreator
{
    public static List<FortObject> blueForts = new List<FortObject>();
    public static List<FortObject> redForts = new List<FortObject>();
    
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
            
            GameObject newFort = new GameObject("BlueFort");
            FortObject fortObject = newFort.AddComponent<FortObject>();
            fortObject.InitializeFort(availableCountries[index], Side.Blue);
            
            Vector2Int coordinates = Util.FindCoordinates(availableCountries[index].fort);
            newFort.transform.localPosition = Util.GetPosition(coordinates);
            
            fortObject.hex = Util.HexExist(coordinates.x, coordinates.y);
            fortObject.hex.fort = fortObject;
            
            blueForts.Add(fortObject);
            availableCountries.RemoveAt(index);
        }
        
        for (int i = 0; i < availableCountries.Count; i++)
        {
            GameObject newFort = new GameObject("RedFort");
            FortObject fortObject = newFort.AddComponent<FortObject>();
            fortObject.InitializeFort(availableCountries[i], Side.Red);

            Vector2Int coordinates = Util.FindCoordinates(availableCountries[i].fort);
            newFort.transform.localPosition = Util.GetPosition(coordinates);

            fortObject.hex = Util.HexExist(coordinates.x, coordinates.y);
            fortObject.hex.fort = fortObject;
            
            redForts.Add(fortObject);
        }
    }
}
