using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuyingController 
{
    public static GameMaster master;
    public static Unit selectedBuyingUnit;

    public static void ResetController()
    {
        selectedBuyingUnit = null;
    }

    public static UnitObject BuyUnit(Hex hex, Side side, int treasury)
    {
        if (hex.unit == null && hex.fort == null && selectedBuyingUnit != null && selectedBuyingUnit.price <= treasury)
        {

            UnitObject newUnit = UnitController.CreateUnit(selectedBuyingUnit, new Vector2Int(hex.x,hex.y));
            newUnit.side = side;
            if (side == Side.Blue)
            {
                gameController.blueUnits.Add(newUnit);
            }
            else
            {
                gameController.redUnits.Add(newUnit);
            }
        
            return newUnit;
        }

        return null;
    }
}
