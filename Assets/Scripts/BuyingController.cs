using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyingController : MonoBehaviour
{
    public Unit selectedBuyingUnit;

    public void ResetController()
    {
        selectedBuyingUnit = null;
    }

    public UnitObject BuyUnit(Hex hex, int treasury)
    {
        if (hex.unit == null && hex.fort == null && selectedBuyingUnit != null && selectedBuyingUnit.price <= treasury)
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
        
            return newUnit;
        }

        return null;
    }
}
