﻿using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static float radius = 1f;
    public static float halfRadius = 0.5f;
    public static float smallRadius = 0.86603f;

    public static float horizontalOffset = smallRadius * 2;
    public static float sideOffset = smallRadius;
    public static float verticalOffset = halfRadius*3;

    public static Vector3[] offsets = new Vector3[]
    {
        new Vector3(0,0,radius), new Vector3(smallRadius,0,halfRadius), new Vector3(smallRadius,0,-halfRadius),
        new Vector3(0,0,-radius), new Vector3(-smallRadius,0,-halfRadius), new Vector3(-smallRadius,0,halfRadius)
    };

    public static Dictionary<TileType, Color> color = new Dictionary<TileType, Color>
    {
        {TileType.Water, Color.blue },{TileType.River, Color.cyan },{TileType.Sand, Color.yellow },{TileType.Land, Color.green },
        {TileType.Road, Color.gray },{TileType.Mountain, Color.white },{TileType.Impassible, Color.black }
    };

    public static List<Vector2Int> neiughbourHexOdd = new List<Vector2Int>
    {
        new Vector2Int(0,-1),new Vector2Int(1,0),new Vector2Int(0,-1),
        new Vector2Int(-1,-1),new Vector2Int(-1,0),new Vector2Int(-1,1),
    };

    public static List<Vector2Int> neiughbourHexEven = new List<Vector2Int>
    {
        new Vector2Int(1,1),new Vector2Int(1,0),new Vector2Int(1,-1),
        new Vector2Int(0,-1),new Vector2Int(-1,0),new Vector2Int(0,1),
    };

    public static bool HexExist(bool isOdd, int x, int y, int direction, out Hex hex)
    {
        int newX = x + (isOdd ? neiughbourHexOdd[direction].x : neiughbourHexEven[direction].x);
        int newY = y + (isOdd ? neiughbourHexOdd[direction].y : neiughbourHexEven[direction].y);

        if (newX >= 0 && newX< MapController.height &&
            newY >= 0 && newY < MapController.width)
        {
            hex = MapController.hexes[newX,newY];
            return true;
        }

        hex = null;
        return false;
    }

    public static int GetDistance(Vector3Int pointA, Vector3Int pointB)
    {
        return Mathf.Max(Mathf.Abs(pointA.x - pointB.x), Mathf.Abs(pointA.y - pointB.y), Mathf.Abs(pointA.z - pointB.z));
    }
    public static Vector2Int GetPositionToCoordinates(Vector3 position)
    {
        float x = position.x / (smallRadius * 2f);
        float y = -x;

        float offset = position.z / (radius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new Vector2Int(iX, iZ);
    }

    public static Vector3Int GetCubicCoordinates(int x, int z)
    {
        return new Vector3Int(x, -x - z, z);
    }

    public static int GetDistanceFromType(TileType type)
    {
        switch (type)
        {
            case TileType.Water:
                return 999;
            case TileType.River:
                return 999;
            case TileType.Sand:
                return 2;
            case TileType.Land:
                return 2;
            case TileType.Road:
                return 1;
            case TileType.Mountain:
                return 999;
            case TileType.Impassible:
                return 999;
        }
        return 999;
    }
}

