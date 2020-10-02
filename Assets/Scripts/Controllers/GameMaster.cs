using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Controller controller;
    public MapController mapController;

    public Side currentTurn = Side.Blue;
    public PhaseType currentPhase = PhaseType.InitialDisclosure;
    int turnCount = 0;

    bool doGuerilla;


    public void Start()
    {
        GameController.mapController = mapController;
        BuyingController.master = this;
        GameController.GoNextPhase = ChangePhase;
        controller.hexClicked = a => GameController.HexClicked(a,currentPhase,currentTurn);
        controller.hexDeselected = () => GameController.HexDeselected(currentPhase);
        StartGame(mapController.currentMapTest);
    }
    public void StartGame(MapData map)
    {
        GameController.ClearMap();
        mapController.CreateMap(map);
        currentTurn = Side.Blue;
        currentPhase = PhaseType.InitialDisclosure;
        SetupForPhase();
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
            SetupForPhase();
        }
        else
        {
            currentTurn = Side.Blue;
            currentTurn++;
        }
    }

    public void ChangePhase()
    {
        DesetupForPhase();
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
                CountryController.AddMoney(currentTurn);
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
        SetupForPhase();
        GameController.ResetControllers();
    }
    void SetupForPhase()
    {
        switch (currentPhase)
        {
            case PhaseType.InitialDisclosure:
                mapController.SetupForDisclosure(currentTurn);
                break;
            case PhaseType.InitialBuying:
                mapController.SetupForBuying(currentTurn, true);
                break;
            case PhaseType.Guerilla:
                break;
            case PhaseType.Combat:
                break;
            case PhaseType.Recruitment:
                break;
            case PhaseType.Disclosing:
                mapController.SetupForDisclosure(currentTurn);
                break;
            case PhaseType.DisclosingBuying:
                mapController.SetupForBuying(currentTurn, false);
                break;
        }
    }

    void DesetupForPhase()
    {
        switch (currentPhase)
        {
            case PhaseType.InitialDisclosure:
                mapController.DesetupForDisclosure();
                break;
            case PhaseType.InitialBuying:
                mapController.DesetupForBuying(true);
                break;
            case PhaseType.Guerilla:
                break;
            case PhaseType.Combat:
                break;
            case PhaseType.Recruitment:
                break;
            case PhaseType.Disclosing:
                mapController.DesetupForDisclosure();
                break;
            case PhaseType.DisclosingBuying:
                mapController.DesetupForBuying(false);
                break;
        }
    }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (GameController.selectedUnit != null)
            {
                Gizmos.DrawSphere(GameController.selectedUnit.hex.position, 0.2f);

                foreach (var hex in GameController.selectedUnit.movableHexes)
                {
                    Gizmos.color = Color.Lerp(Color.green, Color.red, (hex.Value.distance / (float)GameController.selectedUnit.moves));
                    Gizmos.DrawSphere(hex.Key.position, 0.1f);
                    Gizmos.DrawLine(hex.Key.position, hex.Value.from.position);
                }
            }
        }
    #endif
}
