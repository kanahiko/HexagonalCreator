using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "fort", menuName ="Fort")]
public class Fort : ScriptableObject
{
    
    public int guerilla;
    public int revenue;

    public int secretArmy;

    public int revenueTurns;

    public GameObject model;

    public int capacity = 3;
}
