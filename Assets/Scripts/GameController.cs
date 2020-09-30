using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameController
{
    public static List<UnitObject> redUnits = new List<UnitObject>();
    public static List<UnitObject> blueUnits = new List<UnitObject>();

    public static List<CountryObject> redForts = new List<CountryObject>();
    public static List<CountryObject> blueForts = new List<CountryObject>();

    static List<CountryObject> highlightableForts = new List<CountryObject>();
    static List<UnitObject> highlightableUnits = new List<UnitObject>();

#if UNITY_EDITOR
    /// <summary>
    /// for showing unit path
    /// </summary>
    public static UnitObject selectedUnit;
#endif
    public static void ClearMap()
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

        ResetControllers();
    }

    public static void ResetControllers()
    {
        CountryController.ResetController();
        UnitController.ResetController();
        BuyingController.ResetController();
    }


    public static void HexClicked(Vector2Int offsetCoordinates, PhaseType currentPhase, Side currentTurn)
    {
        Hex selectedHex = Util.HexExist(offsetCoordinates.x, offsetCoordinates.y);
        switch (currentPhase)
        {
            case PhaseType.InitialDisclosure:
                if (CountryController.SelectDisclosableFort(selectedHex, currentTurn))
                {
                    //TODO:check can disclose
                    CountryController.Disclose(selectedHex.fort, currentTurn);
                    currentPhase = PhaseType.InitialBuying;
                    //next phase
                }
                break;
            case PhaseType.InitialBuying:
                break;
            case PhaseType.Guerilla:
            //break;
            case PhaseType.Combat:
#if UNITY_EDITOR
                if (UnitController.SelectUnit(selectedHex) && selectedHex.unit != null)
                {

                    selectedUnit = selectedHex.unit;

                }
#else
                UnitController.SelectUnit(selectedHex);
#endif
                break;
            case PhaseType.Recruitment:
                break;
            case PhaseType.Disclosing:
                break;
            case PhaseType.DisclosingBuying:
                break;
        }
    }

    public static void HexDeselected(PhaseType currentPhase)
    {
        switch (currentPhase)
        {
            case PhaseType.InitialDisclosure:
                break;
            case PhaseType.InitialBuying:
                break;
            case PhaseType.Guerilla:
            //break;
            case PhaseType.Combat:
                UnitController.DeselectUnit();
                break;
            case PhaseType.Recruitment:
                break;
            case PhaseType.Disclosing:
                break;
            case PhaseType.DisclosingBuying:
                break;
        }
    }

    public static void GetDisclosableForts(Side currentTurn)
    {
        highlightableForts.Clear();
        List<CountryObject> sideForts = currentTurn == Side.Blue ? blueForts : redForts;
        
        foreach (var fort in sideForts)
        {
            if (!fort.isDisclosed)
            {
                highlightableForts.Add(fort);
            }
        }
    }

    public static void GetClickableUnits(Side currentTurn)
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
