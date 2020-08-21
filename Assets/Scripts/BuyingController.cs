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

    public void BuyUnit(Hex hex, int treasury)
    {
        if (hex.unit == null && hex.fort == null && selectedBuyingUnit != null && selectedBuyingUnit.price <= treasury)
        {

        }
    }
}
