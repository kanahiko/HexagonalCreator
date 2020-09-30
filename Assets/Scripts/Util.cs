using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static float radius = 1f;
    public static float halfRadius = 0.5f;
    public static float smallRadius = 0.86603f;

    public static float horizontalOffset = smallRadius * 2;
    public static float sideOffset = smallRadius;
    public static float verticalOffset = halfRadius * 3;

    public static Vector3[] offsets = new Vector3[]
    {
        new Vector3(0,0,radius), new Vector3(smallRadius,0,halfRadius), new Vector3(smallRadius,0,-halfRadius),
        new Vector3(0,0,-radius), new Vector3(-smallRadius,0,-halfRadius), new Vector3(-smallRadius,0,halfRadius)
    };

    public static Vector3[] overlayOffsets = new Vector3[]
    {
        new Vector3(0,0,radius*0.8f), new Vector3(smallRadius*0.8f,0,halfRadius*0.8f), new Vector3(smallRadius*0.8f,0,-halfRadius*0.8f),
        new Vector3(0,0,-radius*0.8f), new Vector3(-smallRadius*0.8f,0,-halfRadius*0.8f), new Vector3(-smallRadius*0.8f,0,halfRadius*0.8f)
    };
    public static Vector3[] overlayLeftCornerOffsets = new Vector3[]
    {
        new Vector3(0.173207f,0,.9f), new Vector3(smallRadius,0,.3f), new Vector3(.69282f,0,-.6f),
        new Vector3(-0.173207f,0,-.9f), new Vector3(-smallRadius,0,-.3f), new Vector3(-.69282f,0,.6f)
    };
    public static Vector3[] overlayRightCornerOffsets = new Vector3[]
    {
        new Vector3(.69282f,0,.6f), new Vector3(smallRadius,0,-.3f),new Vector3(0.173207f,0,-.9f),
        new Vector3(-.69282f,0,-.6f), new Vector3(-smallRadius,0,.3f),new Vector3(-0.173207f,0,.9f)
    };

    public static Dictionary<TileType, Color> color = new Dictionary<TileType, Color>
    {
        {TileType.Water, Color.blue },{TileType.River, Color.cyan },{TileType.Sand, Color.yellow },{TileType.Land, Color.green },{TileType.Forest, Color.magenta },
        {TileType.Road, Color.gray },{TileType.Mountain, Color.white },{TileType.Impassible, Color.black }
    };

    public static List<Vector2Int> neiughbourHexOdd = new List<Vector2Int>
    {
        new Vector2Int(1,1),new Vector2Int(1,0),new Vector2Int(1,-1),
        new Vector2Int(0,-1),new Vector2Int(-1,0),new Vector2Int(0,1),
    };

    public static List<Vector2Int> neiughbourHexEven = new List<Vector2Int>
    {
        new Vector2Int(0,1),new Vector2Int(1,0),new Vector2Int(0,-1),
        new Vector2Int(-1,-1),new Vector2Int(-1,0),new Vector2Int(-1,1),
    };

    public static byte GetHexSides(Vector2Int coordinates, Country country)
    {
        bool isOdd = coordinates.y % 2 != 0;
        byte sides = 0b0;
        for (int i = 0; i < 6; i++)
        {
            int newX = coordinates.x + (isOdd ? neiughbourHexOdd[i].x : neiughbourHexEven[i].x);
            int newY = coordinates.y + (isOdd ? neiughbourHexOdd[i].y : neiughbourHexEven[i].y);
            if (newX >= 0 && newX < MapController.width &&
            newY >= 0 && newY < MapController.height)
            {
                int newIndex = newX + newY* MapController.width;
                if (country.hexes.Contains(newIndex))
                {
                    sides += (byte)(1 << i);
                }
            }
        }

        return sides;
    }

    public static Hex HexExist(int index)
    {
        Vector2Int coordinates = FindCoordinates(index);
        return HexExist(coordinates.x,coordinates.y);
    }
    public static Hex HexExist(int x, int y)
    {
        if (y >= 0 && y < MapController.height &&
            x >= 0 && x < MapController.width)
        {
            return MapController.hexes[x, y];
        }

        return null;
    }
    public static bool HexExist(bool isOdd, int x, int y, int direction, out Hex hex)
    {
        int newX = x + (isOdd ? neiughbourHexOdd[direction].x : neiughbourHexEven[direction].x);
        int newY = y + (isOdd ? neiughbourHexOdd[direction].y : neiughbourHexEven[direction].y);

        if (newX >= 0 && newX< MapController.width &&
            newY >= 0 && newY < MapController.height)
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
    public static int GetDistance(Vector2Int pointA, Vector2Int pointB)
    {
        return GetDistance(pointA.x, pointA.y, pointB.x, pointB.y);
    }
    public static int GetDistance(int pointAX, int pointAY, int pointBX, int pointBY)
    {
        Vector3Int cubePointA = Util.OffsetToCube(pointAX, pointAY);
        Vector3Int cubePointB = Util.OffsetToCube(pointBX, pointBY);
        return GetDistance(cubePointA, cubePointB);
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
    public static Vector3Int OffsetToCube(Vector2Int offset)
    {
        return OffsetToCube(offset.x, offset.y);
    }
    public static Vector3Int OffsetToCube(int x, int y)
    {
        int newX = x - (y - (y & 1)) / 2;
        int newZ = y;
        return new Vector3Int(newX, -newX - newZ, newZ);
    }
    public static Vector2Int CubeToOffset(Vector2Int cube)
    {
        int column = cube.x + (cube.y - (cube.y & 1)) / 2;
        int row = cube.y;
        return new Vector2Int(column, row);
    }

    public static Vector3Int GetCubicCoordinates(int x, int z)
    {
        return new Vector3Int(x, -x - z, z);
    }

    public static UnitUIType GetPermittedUnits(bool hasNormal, bool hasNaval, bool hasSpecial)
    {
        return (UnitUIType)((hasSpecial ? ((int)UnitUIType.Special) : 0) + 
               (hasNaval ? ((int)UnitUIType.Naval) : 0) + 
               (hasNormal ? ((int)UnitUIType.Normal) : 0));
    }

    public static int GetDistanceFromHex(Hex hex, Unit unit)
    {
        if (hex.unit != null && hex.unit.capacity > 0 && (~hex.unit.type.holdableUnit & unit.unitType) != unit.unitType)
        {
            return 2;
        }

        if (hex.unit != null || hex.fort != null)
        {
            return 999;
        }
        if (!unit.traversableTerrain.ContainsKey(hex.type))
        {
            return 999;
        }

        return unit.traversableTerrain[hex.type];

    }

    public static Vector2Int FindCoordinates(int index)
    {
        int column = index % MapController.width;
        int row = index / MapController.width;
        
        return new Vector2Int(column, row);
    }
    public static Vector3 GetPosition(Vector2Int coordinates)
    {
        return GetPosition(coordinates.x, coordinates.y);
    }
    
    public static Vector3 GetPosition(int x, int y)
    {
        Vector3  position = new Vector3();
        position.z = y * (radius + halfRadius);
        position.x = x * smallRadius * 2 + (y % 2 == 0 ? 0 : smallRadius);
        return position;
    }
    
    public static Vector3 GetPosition(int index)
    {
        Vector2Int coordinates = FindCoordinates(index);
        return GetPosition(coordinates.x, coordinates.y);
    }
}

