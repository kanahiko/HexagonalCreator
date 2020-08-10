using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinding
{
    public static List<HexPath> GetMovableHexes(Hex startHex,int moveCount)
    {
        List<HexPath> movableHexes = new List<HexPath>();
        Dictionary<Hex, HexPath> usedHexes = new Dictionary<Hex, HexPath>();
        List<HexPath> queue = new List<HexPath>();
        HexPath start = new HexPath(startHex);
        usedHexes.Add(startHex, start);
        queue.Add(start);

        MapController.sprites[startHex.x, startHex.z].color = Color.blue;

        while(queue.Count > 0)
        {
            HexPath hex = queue[queue.Count - 1];
            queue.RemoveAt(queue.Count - 1);
            bool isOdd = hex.to.z % 2 != 0;
            for (int i = 0; i < 6; i++)
            {
                Hex neighbour;
                if (!Util.HexExist(isOdd,hex.to.z, hex.to.x,i, out neighbour))
                {
                    continue;
                }

                int distance = hex.distance + Util.GetDistanceFromType(neighbour.type);

                if (distance > moveCount)
                {
                    if (!usedHexes.ContainsKey(neighbour))
                    {
                        HexPath neighbourPathFailed = new HexPath(hex.to, neighbour, distance);
                        usedHexes.Add(neighbour, neighbourPathFailed);
                    }
                    continue;
                }
                if (usedHexes.ContainsKey(neighbour))
                {
                    if (usedHexes[neighbour].distance <= distance)
                    {
                        continue;
                    }

                    if (!usedHexes.ContainsKey(neighbour))
                    {
                        movableHexes.Add(usedHexes[neighbour]);
                    }
                    usedHexes[neighbour].distance = distance;
                    usedHexes[neighbour].from = hex.to;
                    queue.Add(usedHexes[neighbour]);

                    continue;
                }
                HexPath neighbourPath = new HexPath(hex.to, neighbour, distance);
                queue.Add(neighbourPath);
                movableHexes.Add(neighbourPath);
                MapController.sprites[neighbour.x, neighbour.z].color = Color.green;
                usedHexes.Add(neighbour, neighbourPath);
            }
        }
        return movableHexes;
    }

    static void CheckHexes()
    {

    }
}

public class HexPath
{
    public Hex from;
    public Hex to;
    public int distance;

    public HexPath(Hex start)
    {
        from = start;
        to = start;
        distance = 0;
    }

    public HexPath(Hex from, Hex to, int distance)
    {
        this.from = from;
        this.to = to;
        this.distance = distance;
    }
}

public class Queue<T> : List<T>
{
    /*public T Next()
    {
        return base[]
    }*/
}