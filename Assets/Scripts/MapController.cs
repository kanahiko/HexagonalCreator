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

    void Start()
    {
        width = currentMap.width;
        height = currentMap.height;
        sprites = new SpriteRenderer[height,width];
        hexes = creator.CreateMap(currentMap);

        Hex hex = hexes[height / 2, width / 2];
        PathFinding.GetMovableHexes(hex, 5);
    }
}
