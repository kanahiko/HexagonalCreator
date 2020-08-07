using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="Map", menuName ="Map")]
public class MapData : ScriptableObject
{
    public int width = 5;
    public int height = 5;

    public TileType[] types;

    public int[] elevation;
}

public class Hex
{
    public Vector3Int coordinates;

    public int x;
    public int y;
    public int z;

    public TileType type;

    public int elevation;

    public Hex(int x,  int z, TileType type, int elevation)
    {
        this.x = x;
        this.y = -x-z;
        this.z = z;
        coordinates = new Vector3Int(x, y, z);
        this.type = type;
        this.elevation = elevation;
    }
}


public enum TileType
{
    Water = 0, River = 1, Sand = 2, Land = 3, Road = 4, Mountain = 5, Impassible = 6
}
public enum Direction
{
    North = 0, NorthEast = 1, SouthEast = 2,
    South = 3, SouthWest = 4, NorthWest = 5
}