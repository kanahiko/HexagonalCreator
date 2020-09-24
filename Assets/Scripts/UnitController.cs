﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitController
{
    //public static UnitObject selectedFort;
    public static UnitObject selectedUnit;
    public static Action<UnitObject> ShowTransportButtons;
    
    public static bool SelectUnit(Hex hex)
    {
        if (hex == null || (hex.unit == null && selectedUnit == null))
        {
            return false;
        }
        if (selectedUnit != null)
        {
            if (selectedUnit.movableHexes.ContainsKey(hex))
            {
                MoveUnit(hex);
                return false;
            }
            else
            {
                if (selectedUnit.atta)
            }
        }
        
        selectedUnit = hex.unit;
        ShowTransportButtons?.Invoke(selectedUnit);
        selectedUnit.movableHexes = PathFinding.GetMovableHexes(hex, selectedUnit.moves,selectedUnit.capacity, selectedUnit.type);
        return true;
    }

    public static void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.movableHexes.Clear();
            selectedUnit = null;
            ShowTransportButtons?.Invoke(null);
            PathFinding.ClearPaths();
        }
    }

    public static void Attack()
    {

    }

    public static void SelectFort()
    {

    }
    public static void SelectUnit(int index)
    {
        if (selectedUnit != null)
        {
            if (index >= 0)
            {
                if (selectedUnit.boardedUnits.Count > 0)
                {
                    selectedUnit = selectedUnit.boardedUnits[index];
                }
            }
        }
        Hex hex = selectedUnit.hex;
        ShowTransportButtons?.Invoke(selectedUnit);
        selectedUnit.movableHexes = PathFinding.GetMovableHexes(hex, selectedUnit.moves, selectedUnit.capacity, selectedUnit.type);
    }

    public static void MoveUnit(Hex moveToHex)
    {
        selectedUnit.moveToHex = moveToHex;
        if (selectedUnit.hex.unit == selectedUnit)
        {
            selectedUnit.hex.unit = null;
        }
        else
        {
            selectedUnit.hex.unit.UnBoardTransport(selectedUnit);
        }

        if (moveToHex.unit == null)
        { 
            moveToHex.unit = selectedUnit;
        }
        else
        {
            moveToHex.unit.BoardTransport(selectedUnit);
        }

        PathFinding.ClearPaths();
        selectedUnit.Move(true);
        selectedUnit = null;
        ShowTransportButtons?.Invoke(null);
    }

    public static UnitObject CreateUnit(Unit unit,Vector2Int coordinates)
    {
        GameObject newUnit = new GameObject(unit.name);
        UnitObject unitObject = newUnit.AddComponent<UnitObject>();
        
        unitObject.Initialize(unit, coordinates);

        return unitObject;
    }
    public static void ResetController()
    {
        selectedUnit = null;
    }
}
