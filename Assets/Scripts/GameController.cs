using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Side currentTurn = Side.Blue;
    public PhaseType currentPhase = PhaseType.InitialDisclosure;
    int turnCount = 0;

    public List<UnitObject> redUnits = new List<UnitObject>();
    public List<UnitObject> blueUnits = new List<UnitObject>();

    public List<FortObject> redForts = new List<FortObject>();
    public List<FortObject> blueForts = new List<FortObject>();

    public int redTreasury;
    public int blueTreasury;

    bool doGuerilla;

    List<FortObject> highlightableForts = new List<FortObject>();
    List<UnitObject> highlightableUnits = new List<UnitObject>();
    public void StartGame(MapData map)
    {
        ClearMap();
        CreateForts(map);
        currentTurn = Side.Blue;
        currentPhase = PhaseType.InitialDisclosure;
    }

    void CreateForts(MapData map)
    {
        int count = map.countries.Count / 2;

        foreach(var fort in map.countries)
        {
            if (Random.Range(0,100)%2 == 0)
            {
                GameObject newFort = new GameObject("BlueFort");
                FortObject fortObject = newFort.AddComponent<FortObject>();
                fortObject.InitializeFort(fort, Side.Blue);
                blueForts.Add(fortObject);
                count--;
            }
        }
    }

    public void ChangeTurn()
    {
        if (currentTurn == Side.Blue)
        {
            currentTurn = Side.Red;
            if (currentTurn == 0)
            {
                currentPhase = PhaseType.InitialDisclosure;
            }
            else
            {
                currentPhase = doGuerilla ? PhaseType.Guerilla : PhaseType.Combat;
                doGuerilla = false;
            }
        }
        else
        {
            currentTurn = Side.Blue;
            currentTurn++;
        }
        AddMoney();
    }

    void AddMoney()
    {
        List<FortObject> sideForts = currentTurn == Side.Blue ? blueForts : redForts;
        int sum = 0;

        foreach (var fort in sideForts)
        {
            if (fort.revenueTurnsLeft > 0)
            {
                sum += fort.fort.revenue;
                fort.revenueTurnsLeft--;
            }
        }

        if (currentTurn == Side.Blue)
        {
            blueTreasury += sum;
        }
        else
        {
            redTreasury += sum;
        }
    }

    public void ChangePhase()
    {
        switch (currentPhase)
        {
            case PhaseType.InitialDisclosure:
                if (doGuerilla)
                {
                    currentPhase = PhaseType.Guerilla;
                    doGuerilla = false;
                }
                else
                {
                    currentPhase = PhaseType.Combat;
                }
                break;
            case PhaseType.Guerilla:
                currentPhase = PhaseType.Combat;
                break;
            case PhaseType.Combat:
                currentPhase = PhaseType.Recruitment;
                break;
            case PhaseType.Recruitment:
                ChangeTurn();
                break;
        }
    }

    void ClearMap()
    {
        for(int i = 0; i < redUnits.Count; i++)
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

        redTreasury = 0;
        blueTreasury = 0;
    }

    public void GetDisclosableForts()
    {
        highlightableForts.Clear();
        List<FortObject> sideForts = currentTurn == Side.Blue ? blueForts : redForts;

        foreach(var fort in sideForts)
        {
            if (!fort.isDisclosed)
            {
                highlightableForts.Add(fort);
            }
        }
    }

    public void GetClickableUnits()
    {
        highlightableUnits.Clear();
        List<UnitObject> sideUnits = currentTurn == Side.Blue ? blueUnits : redUnits;

        foreach (var unit in sideUnits)
        {
            if (unit.moves > 0 )
            {
                highlightableUnits.Add(unit);
            }
        }
    }

    public void Disclose()
    {

    }
}
