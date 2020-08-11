using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapData currentMap;
    public HexCreator creator;

    public static int width;
    public static int height;

    public static Hex[,] hexes;
    public static SpriteRenderer[,] sprites;

    public static List<Fort> fortTypes;

    void Start()
    {
        width = currentMap.width;
        height = currentMap.height;
        sprites = new SpriteRenderer[width,height];
        hexes = creator.CreateMap(currentMap);

        Hex hex = hexes[width / 2, height / 2];
        Dictionary<Hex, HexPath>  movableHexes = PathFinding.GetMovableHexes(hex, 5);
    }
}
