using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapData currentMapTest;
    public MapData currentMap;
    public HexCreator creator;
    //public GameMaster gameController;

    public static int width;
    public static int height;

    public static Hex[,] hexes;
    public static SpriteRenderer[,] sprites;

    public List<Fort> fortPrefabs;
    public static List<Fort> fortTypes;

    public List<Unit> unitPrefabs;
    public static List<Unit> unitTypes;

    public void CreateMap(MapData map)
    {
        currentMap = map;

        fortTypes = fortPrefabs;
        unitTypes = unitPrefabs;
        
        width = currentMap.width;
        height = currentMap.height;
        sprites = new SpriteRenderer[width,height];
        hexes = creator.CreateMap(currentMap);
        MapCreator.CreateGameMap(currentMap);

        //Hex hex = hexes[width / 2, height / 2];
        //Dictionary<Hex, HexPath>  movableHexes = PathFinding.GetMovableHexes(hex, 5);
    }
}
