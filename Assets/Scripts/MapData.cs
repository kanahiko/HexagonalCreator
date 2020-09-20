using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="Map", menuName ="Map")]
public class MapData : ScriptableObject
{
    public int width = 5;
    public int height = 5;

    [EnumFlagsAttribute]
    public TileType[] types;

    public int[] elevation;

    public List<Country> countries;
}

[Serializable]
public class Hex
{
    //public Vector3Int coordinates;

    public Vector3 position;

    public int x;
    public int y;
   //public int z;

    public TileType type;

    public int elevation;

    public UnitObject unit;
    public FortObject fort;
    //public Country country;

    public Hex(int x,  int y, TileType type, int elevation)
    {
        this.x = x;
        this.y = y;
        //coordinates = new Vector3Int(x, y, z);
        this.type = type;
        this.elevation = elevation;
    }
}

[Serializable]
public class Country
{
    
    public List<int> hexes;
    public int fort;

    public FortType type;
}