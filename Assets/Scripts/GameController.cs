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
        redForts = MapCreator.redForts;
        blueForts = MapCreator.blueForts;
        currentTurn = Side.Blue;
        currentPhase = PhaseType.InitialDisclosure;
        
        Vector2Int coords = new Vector2Int(2,3);
        AddUnit(MapController.unitTypes[0],coords);
        UnitController.SelectUnit(coords);
        UnitController.SelectUnit(new Vector2Int(3,2));
        
    }

    public void AddUnit(Unit unit, Vector2Int coordinates)
    {
        UnitObject newUnit = UnitController.CreateUnit(unit, coordinates);
        newUnit.side = currentTurn;
        if (currentTurn == Side.Blue)
        {
            blueUnits.Add(newUnit);
        }
        else
        {
            redUnits.Add(newUnit);
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
