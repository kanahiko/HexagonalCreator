using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameController gameController;
    public Controller controller;
    public MapController mapController;

    public Side currentTurn = Side.Blue;
    public PhaseType currentPhase = PhaseType.InitialDisclosure;
    int turnCount = 0;

    bool doGuerilla;


#if UNITY_EDITOR
    /// <summary>
    /// for showing unit path
    /// </summary>
    private UnitObject selectedUnit;
#endif
    public void Start()
    {
        BuyingController.master = this;
        controller.hexClicked = HexClicked;
        controller.hexDeselected = HexDeselected;
        StartGame(mapController.currentMapTest);
    }
    public void StartGame(MapData map)
    {
        mapController.CreateMap(map);

        if (gameController == null)
        {
            gameController = new GameController();
        }
        gameController.ClearMap();
        gameController.redForts = MapCreator.redForts;
        gameController.blueForts = MapCreator.blueForts;
        currentTurn = Side.Blue;
        currentPhase = PhaseType.InitialDisclosure;

        //tests
        /*AddUnit(MapController.unitTypes[1], new Vector2Int(4,3));
        Vector2Int coords = new Vector2Int(2,3);
        AddUnit(MapController.unitTypes[0],coords);

        HexClicked(coords);
        HexClicked(new Vector2Int(3,1));*/
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
        CountryController.AddMoney(currentTurn);
    }


    public void ChangePhase()
    {
        switch (currentPhase)
        {
            case PhaseType.InitialDisclosure:
                currentPhase = PhaseType.InitialBuying;
                break;
            case PhaseType.InitialBuying:
                ChangeTurn();
                break;
            case PhaseType.Guerilla:
                currentPhase = PhaseType.Combat;
                break;
            case PhaseType.Combat:
                currentPhase = PhaseType.Recruitment;
                break;
            case PhaseType.Recruitment:
                currentPhase = PhaseType.Disclosing;
                break;
            case PhaseType.Disclosing:
                //TODO: if no disclosing or no countries change turn
                currentPhase = PhaseType.DisclosingBuying;
                break;
            case PhaseType.DisclosingBuying:
                ChangeTurn();
                break;
        }
    }

    public void HexClicked(Vector2Int offsetCoordinates)
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

    public void HexDeselected()
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (selectedUnit != null)
        {
            Gizmos.DrawSphere(selectedUnit.hex.position,0.2f);

            foreach (var hex in selectedUnit.movableHexes)
            {
                Gizmos.color = Color.Lerp(Color.green, Color.red, (hex.Value.distance / (float) selectedUnit.moves));
                Gizmos.DrawSphere(hex.Key.position,0.1f);
                Gizmos.DrawLine(hex.Key.position,hex.Value.from.position);
            }
        }
    }
#endif
}
