using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="unit", menuName ="Unit")]
public class Unit : ScriptableObject
{
    public int hitPoints;
    public int movement;
    public int damage;
    public int secondaryDamage;
    public int range;
    public int price;
    public int capacity;

    public GameObject unitModel;
}
