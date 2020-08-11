using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortObject : MonoBehaviour
{
    public Hex hex;
    
    public int capacity;
    public Side side;
    public Side initialSide;

    public int revenueTurnsLeft;
    public bool isDisclosed = false;
    public bool hadGuerrilla = false;
    public bool hadIndemnity = false;

    public Fort fort;
    public GameObject fortModel;
    public UnitObject[] planes;

    public void InitializeFort(Country country, Side side)
    {
        fort = MapController.fortTypes[(int)country.type];

        revenueTurnsLeft = fort.revenueTurns;
        capacity = fort.capacity;
        planes = new UnitObject[capacity];
        initialSide = side;
        this.side = Side.None;

        fortModel = Instantiate(fort.model, transform);
        fortModel.transform.localPosition = Vector3.zero;
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
            planes[i].Destruct();
            planes[i] = null;
        }
        capacity = 0;
    }
}
