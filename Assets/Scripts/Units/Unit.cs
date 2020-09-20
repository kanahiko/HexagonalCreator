using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="unit", menuName ="Unit")]
public class Unit : ScriptableObject
{
    public Texture icon;
    public string name;
    [EnumUnitTypeFlagsAttribute]
    public UnitType unitType;

    [EnumUnitUITypeFlagsAttribute]
    public UnitUIType unityUIType;
    
    public int hitPoints;
    public int movement;
    public int damage;
    public int secondaryDamage;
    public int range;
    public int capacity;
    public int bomberCapacity;
    public int price;

    public TerrainDistanceDictionary traversableTerrain;
    public UnitType holdableUnit;

    public GameObject unitModel;
}

public class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }
}
public class EnumUnitTypeFlagsAttribute : PropertyAttribute
{
    public EnumUnitTypeFlagsAttribute() { }
}public class EnumUnitUITypeFlagsAttribute : PropertyAttribute
{
    public EnumUnitUITypeFlagsAttribute() { }
}


[System.Serializable]
public class TerrainDistanceDictionary : SerializableDictionaryBase<TileType, int> { }
