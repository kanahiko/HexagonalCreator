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

    public int redTreasury;
    public int blueTreasury;
    public void Start()
    {
        controller.hexClicked = HexClicked;
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


        redTreasury = 0;
        blueTreasury = 0;

        //tests
        AddUnit(MapController.unitTypes[1], new Vector2Int(4,3));
        Vector2Int coords = new Vector2Int(2,3);
        AddUnit(MapController.unitTypes[0],coords);

        HexClicked(coords);
        HexClicked(new Vector2Int(3,1));


    }

    public void AddUnit(Unit unit, Vector2Int coordinates)
    {
        UnitObject newUnit = UnitController.CreateUnit(unit, coordinates);
        newUnit.side = currentTurn;
        if (currentTurn == Side.Blue)
        {
            gameController.blueUnits.Add(newUnit);
        }
        else
        {
            gameController.redUnits.Add(newUnit);
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
        List<FortObject> sideForts = currentTurn == Side.Blue ? gameController.blueForts : gameController.redForts;
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

    public void RemoveMoney(int money)
    {
        if (currentTurn == Side.Blue)
        {
            blueTreasury -= money;
        }
        else
        {
            redTreasury -= money;
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

    public void Disclose()
    {

    }

    public void HexClicked(Vector2Int offsetCoordinates)
    {
        Hex selectedHex = Util.HexExist(offsetCoordinates.x, offsetCoordinates.y);
        switch (currentPhase)
        {
            case PhaseType.InitialDisclosure:

                break;
            case PhaseType.Guerilla:
                break;
            case PhaseType.Combat:
                UnitController.SelectUnit(selectedHex);
                break;
            case PhaseType.Recruitment:
                break;
        }
    }
}
