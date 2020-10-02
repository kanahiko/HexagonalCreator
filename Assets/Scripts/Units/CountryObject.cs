using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryObject : MonoBehaviour
{
    public int id;
    public int treasury;
    
    public Hex fortHex;
    public Hex[] hexes;
    
    public int capacity;
    public Side side;
    public Side initialSide;

    public bool canGetMoney = true;
    public int revenueTurnsLeft;
    public bool hadDisclosedMoney = false;
    public bool hadGuerrilla = false;
    public bool hadIndemnity = false;

    public Fort fort;
    public GameObject fortModel;
    public UnitObject[] planes;

    public void InitializeFort(Country country, Side side, int id)
    {
        fort = MapController.fortTypes[(int)country.type];
        hexes = new Hex[country.hexes.Count];
        revenueTurnsLeft = fort.revenueTurns;
        capacity = fort.capacity;
        planes = new UnitObject[capacity];
        initialSide = side;
        this.side = Side.None;

        fortModel = Instantiate(fort.model, transform);
        fortModel.transform.localPosition = Vector3.zero;
        canGetMoney = true;
        this.id = id;
    }

    public void Destruct()
    {
        DestroyPlanes();
        Destroy(gameObject);
    }

    public void DestroyPlanes()
    {
        for (int i = 0; i < planes.Length; i++)
        {
            if (planes[i] != null)
            {
                planes[i].Destruct();
                planes[i] = null;
            }
        }
        capacity = 0;
    }
}
