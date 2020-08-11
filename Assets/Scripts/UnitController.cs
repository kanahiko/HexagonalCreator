using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitController
{
    public static UnitObject selectedUnit;
    
    public static void SelectUnit(Vector2Int hexCoordinates)
    {
        Hex hex = Util.HexExist(hexCoordinates.x, hexCoordinates.y);
        
        if (hex == null || hex.unit == null)
        {
            if (hex != null && selectedUnit != null && selectedUnit.movableHexes.ContainsKey(hex))
            {
                MoveUnit(hex);
            }
            selectedUnit = null;
            return;
        }
        
        selectedUnit = hex.unit;
        selectedUnit.movableHexes = PathFinding.GetMovableHexes(hex, selectedUnit.moves);
    }

    public static void MoveUnit(Hex moveToHex)
    {
        selectedUnit.hex.unit = null;
        selectedUnit.hex = moveToHex;
        moveToHex.unit = selectedUnit;
        
        selectedUnit.Move();
    }

    public static UnitObject CreateUnit(Unit unit,Vector2Int coordinates)
    {
        GameObject newUnit = new GameObject(unit.name);
        UnitObject unitObject = newUnit.AddComponent<UnitObject>();

        GameObject model = GameObject.Instantiate(unit.unitModel, newUnit.transform);
        
        unitObject.Initialize(unit, coordinates);

        return unitObject;
    }
}
