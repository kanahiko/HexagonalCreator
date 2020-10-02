using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GameController
{
    public static List<UnitObject> redUnits = new List<UnitObject>();
    public static List<UnitObject> blueUnits = new List<UnitObject>();

    public static List<CountryObject> redCountries = new List<CountryObject>();
    public static List<CountryObject> blueCountries = new List<CountryObject>();

    static HashSet<CountryObject> highlightableForts = new HashSet<CountryObject>();
    static HashSet<UnitObject> highlightableUnits = new HashSet<UnitObject>();

    public static MapController mapController;
    public static Action GoNextPhase;
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


        for (int i = 0; i < redCountries.Count; i++)
        {
            redCountries[i].Destruct();
        }
        for (int i = 0; i < blueCountries.Count; i++)
        {
            blueCountries[i].Destruct();
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
        mapController.ResetController();
    }


    public static void HexClicked(Vector2Int offsetCoordinates, PhaseType currentPhase, Side currentTurn)
    {
        Hex selectedHex = Util.HexExist(offsetCoordinates.x, offsetCoordinates.y);
        switch (currentPhase)
        {
            case PhaseType.InitialDisclosure:
                if (CountryController.SelectedDisclosableFort(selectedHex, currentTurn, highlightableForts))
                {
                    //TODO:check want disclose
                    CountryController.Disclose(selectedHex.fort, currentTurn);
                    GoNextPhase?.Invoke();
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

    public static int GetDisclosableCountry(Side currentTurn)
    {
        highlightableForts.Clear();
        List<CountryObject> sideCountries = currentTurn == Side.Blue ? blueCountries : redCountries;
        int countriesMask = 0;
        foreach (var country in sideCountries)
        {
            if (country.initialSide == currentTurn && country.side == Side.None)
            {
                highlightableForts.Add(country);
                countriesMask += 1 << country.id;
            }
        }

        return countriesMask;
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
