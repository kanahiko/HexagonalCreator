using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController: MonoBehaviour
{

    public List<UnitObject> redUnits = new List<UnitObject>();
    public List<UnitObject> blueUnits = new List<UnitObject>();

    public List<FortObject> redForts = new List<FortObject>();
    public List<FortObject> blueForts = new List<FortObject>();

    List<FortObject> highlightableForts = new List<FortObject>();
    List<UnitObject> highlightableUnits = new List<UnitObject>();

    public void ClearMap()
    {
        for (int i = 0; i < redUnits.Count; i++)
        {
            redUnits[i].Destruct();
        }
        for (int i = 0; i < blueUnits.Count; i++)
        {
            blueUnits[i].Destruct();
        }


        for (int i = 0; i < redForts.Count; i++)
        {
            redForts[i].Destruct();
        }
        for (int i = 0; i < blueForts.Count; i++)
        {
            blueForts[i].Destruct();
        }

        redUnits.Clear();
        blueUnits.Clear();
        highlightableForts.Clear();
        highlightableUnits.Clear();
    }

    public void GetDisclosableForts(Side currentTurn)
    {
        highlightableForts.Clear();
        List<FortObject> sideForts = currentTurn == Side.Blue ? blueForts : redForts;

        foreach (var fort in sideForts)
        {
            if (!fort.isDisclosed)
            {
                highlightableForts.Add(fort);
            }
        }
    }

    public void GetClickableUnits(Side currentTurn)
    {
        highlightableUnits.Clear();
        List<UnitObject> sideUnits = currentTurn == Side.Blue ? blueUnits : redUnits;

        foreach (var unit in sideUnits)
        {
            if (unit.moves > 0)
            {
                highlightableUnits.Add(unit);
            }
        }
    }
}
