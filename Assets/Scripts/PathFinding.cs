using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PathFinding
{
    public static Dictionary<Hex, HexPath> GetMovableHexes(Hex startHex, int moveCount, int capacity, Unit unit)
    {
        Dictionary<Hex, HexPath> movableHexes = new Dictionary<Hex, HexPath>();
        Dictionary<Hex, HexPath> usedHexes = new Dictionary<Hex, HexPath>();
        Queue<HexPath> queue = new Queue<HexPath>();
        HexPath start = new HexPath(startHex);
        usedHexes.Add(startHex, start);
        queue.Enqueue(start);

        //MapController.sprites[startHex.x, startHex.y].color = Color.blue;

        while(queue.Count > 0)
        {
            HexPath hex = queue.Dequeue();
            //queue.RemoveAt(queue.Count - 1);
            bool isOdd = hex.to.y % 2 != 0;
            for (int i = 0; i < 6; i++)
            {
                Hex neighbour;
                if (!Util.HexExist(isOdd,hex.to.x, hex.to.y,i, out neighbour))
                {
                    continue;
                }

                int distance = hex.distance + Util.GetDistanceFromHex(neighbour, unit);

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
                        movableHexes.Add(neighbour,usedHexes[neighbour]);
                    }
                    usedHexes[neighbour].distance = distance;
                    usedHexes[neighbour].from = hex.to;
                    queue.Enqueue(usedHexes[neighbour]);
                    //MapController.sprites[neighbour.x, neighbour.y].color = Color.green;
                    //var texts = MapController.sprites[neighbour.x, neighbour.y].transform.GetComponentInChildren<Text>();
                    //texts.text = $"{distance}";

                    continue;
                }
                HexPath neighbourPath = new HexPath(hex.to, neighbour, distance);
                queue.Enqueue(neighbourPath);
                movableHexes.Add(neighbour,neighbourPath);
                //MapController.sprites[neighbour.x, neighbour.y].color = Color.green;

                //var text = MapController.sprites[neighbour.x, neighbour.y].transform.GetComponentInChildren<Text>();
                //text.text = $"{neighbourPath.distance}";
                usedHexes.Add(neighbour, neighbourPath);
            }
        }
        return movableHexes;
    }
    public static List<Hex> GetAttackableHexes(Hex startHex, Side side, int range)
    {
        List<Hex> attackableHexes = new List<Hex>();
        HashSet<Hex> usedHexes = new HashSet<Hex>();
        Queue<Hex> queue = new Queue<Hex>();
        queue.Enqueue(startHex);

        while (queue.Count > 0)
        {
            Hex hex = queue.Dequeue();
            usedHexes.Add(hex);
            bool isOdd = hex.y % 2 != 0;

            for (int i = 0; i < 6; i++)
            {
                Hex neighbour;
                if (!Util.HexExist(isOdd, hex.x, hex.y, i, out neighbour))
                {
                    continue;
                }

                if (usedHexes.Contains(neighbour))
                {
                    continue;
                }
                int distance = Util.GetDistance(startHex.x, startHex.y, neighbour.x, neighbour.y);
                if (distance > range)
                {
                    continue;
                }

                if (neighbour.unit != null && neighbour.unit.side != side)
                {
                    attackableHexes.Add(neighbour);
                }
                queue.Enqueue(neighbour);
            }
        }

        return attackableHexes;
    }

    public static void ClearPaths()
    {
        for (int i = 0; i < MapController.width; i++)
        {
            for (int j = 0; j < MapController.height; j++)
            {
                //MapController.sprites[i, j].color = Color.white;
                //var text = MapController.sprites[i, j].transform.GetComponentInChildren<Text>();
                //text.text = "";
            }
        }
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
