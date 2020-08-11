using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapData currentMap;
    public HexCreator creator;
    public GameController gameController;

    public static int width;
    public static int height;

    public static Hex[,] hexes;
    public static SpriteRenderer[,] sprites;

    public List<Fort> fortPrefabs;
    public static List<Fort> fortTypes;

    public List<Unit> unitPrefabs;
    public static List<Unit> unitTypes;

    void Start()
    {
        fortTypes = fortPrefabs;
        unitTypes = unitPrefabs;
        
        width = currentMap.width;
        height = currentMap.height;
        sprites = new SpriteRenderer[width,height];
        hexes = creator.CreateMap(currentMap);
        MapCreator.CreateGameMap(currentMap);
        
        gameController.StartGame(currentMap);

        //Hex hex = hexes[width / 2, height / 2];
        //Dictionary<Hex, HexPath>  movableHexes = PathFinding.GetMovableHexes(hex, 5);
    }
}
