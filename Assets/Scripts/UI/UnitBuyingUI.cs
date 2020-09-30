using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuyingUI : MonoBehaviour
{
    public Canvas unitBuyingCanvas;
    public RectTransform unitBuyingRect;

    public UnitCanvasDictionary blueCanvases;
    public UnitCanvasDictionary redCanvases;

    public float zeroHeight = 0;
    
    public RectTransform backgroundImage;
    public RectTransform unitSlot;
    
    private Dictionary<UnitUIType,int> blueCount;
    private Dictionary<UnitUIType,int> redCount;
    
    private Dictionary<UnitUIType,float> verticalBlueSize;
    private Dictionary<UnitUIType,float> verticalRedSize;
    private UnitUIType permitedUnits;

    private Dictionary<Unit, UnitIcon> allUnits;
    private float padding = -1;
    private float slotHeight = -1;
    private float width = -1;
    private Vector2 size;
    private void Awake()
    { 
        padding = blueCanvases[0].GetComponent<VerticalLayoutGroup>().spacing;
        slotHeight = unitSlot.rect.height;
        width = backgroundImage.rect.width;
        
        size = new Vector2(width,0);

        verticalBlueSize = new Dictionary<UnitUIType, float>();
        verticalRedSize = new Dictionary<UnitUIType, float>();
        
        
        blueCount = new Dictionary<UnitUIType, int>();
        blueCount.Add(UnitUIType.Normal,0);
        blueCount.Add(UnitUIType.Naval,0);
        blueCount.Add(UnitUIType.Special,0);
        
        redCount = new Dictionary<UnitUIType, int>();
        redCount.Add(UnitUIType.Normal,0);
        redCount.Add(UnitUIType.Naval,0);
        redCount.Add(UnitUIType.Special,0);
    }

    public void AddUnits(List<Unit> blueUnits, List<Unit> redUnits, UnitUIType permitedUnits = (UnitUIType) 7)
    {
        if (allUnits != null)
        {
            ReAddUnits(blueUnits,Side.Blue,permitedUnits);
            ReAddUnits(redUnits,Side.Red,permitedUnits);
        }
        else
        {
            allUnits = new Dictionary<Unit, UnitIcon>();
            AddUnits(blueUnits, Side.Blue, permitedUnits);
            AddUnits(redUnits, Side.Blue, permitedUnits);

        }
        this.permitedUnits = permitedUnits;
        RecalculateSize();

    }

    void RecalculateSize()
    {
        verticalBlueSize.Clear();
        foreach (var count in blueCount)
        {
            if (count.Value == 0)
            {
                verticalBlueSize.Add(count.Key,zeroHeight);
            }
            else
            {
                verticalBlueSize.Add(count.Key,count.Value * slotHeight + (count.Value + 1)*padding);
            }
        }
        verticalRedSize.Clear();
        foreach (var count in redCount)
        {if (count.Value == 0)
            {
                verticalRedSize.Add(count.Key,zeroHeight);
            }
            else
            {
                verticalRedSize.Add(count.Key, count.Value * slotHeight + (count.Value + 1) * padding);
            }
        }
    }

    void ReAddUnits(List<Unit> units, Side side, UnitUIType permitedUnits = (UnitUIType)7)
    {
        foreach (var unit in units)
        {
            if ((unit.unityUIType & permitedUnits) == permitedUnits)
            {
                if (allUnits[unit].IsEnabled())
                {
                    if (side == Side.Blue)
                    {
                        blueCount[unit.unityUIType] -= 1;
                    }else
                    {
                        redCount[unit.unityUIType] -= 1;
                    }
                }
                allUnits[unit].Disable();
                continue;
            }
            if (!allUnits[unit].IsEnabled())
            {
                if (side == Side.Blue)
                {
                    blueCount[unit.unityUIType] += 1;
                }else
                {
                    redCount[unit.unityUIType] += 1;
                }
            }
            allUnits[unit].Enable();
        }
    }
    
    private void AddUnits(List<Unit> units, Side side, UnitUIType permitedUnits = (UnitUIType)7)
    {
        foreach (var unit in units)
        {
            if ((unit.unityUIType & permitedUnits) == permitedUnits)
            {
                continue;
            }
            Transform parent = null;
            if (side == Side.Blue)
            {
                parent = blueCanvases[unit.unityUIType].transform;
                blueCount[unit.unityUIType] += 1;
            }
            else
            {
                parent = redCanvases[unit.unityUIType].transform;
                redCount[unit.unityUIType] += 1;
            }
            GameObject slot = Instantiate(unitSlot.gameObject, parent);
            UnitIcon icon = slot.GetComponent<UnitIcon>();
            icon.UnitSelected += () => BuyingController.SelectUnit(unit);
            icon.SetStats(unit.name, unit.icon,unit.hitPoints,unit.movement,unit.damage,unit.secondaryDamage,unit.range, unit.capacity,unit.price);
            allUnits.Add(unit,icon);
        }
    }

    public void ShowCanvas(UnitUIType type,Side side)
    {
        HideAll();

        if (side == Side.Blue)
        {
            blueCanvases[type].enabled = true;
            size.y = verticalBlueSize[type];
        }
        else
        {
            redCanvases[type].enabled = true;
            size.y = verticalRedSize[type];
        }

        backgroundImage.sizeDelta = size;
        unitBuyingCanvas.enabled = true;
    }

    void HideAll()
    {
        foreach (var canvas in blueCanvases)
        {
            canvas.Value.enabled = false;
        }
        foreach (var canvas in redCanvases)
        {
            canvas.Value.enabled = false;
        }
    }
}

[System.Serializable]
public class UnitCanvasDictionary : SerializableDictionaryBase<UnitUIType, Canvas> { }

//001 = 1 - only normal
//010 = 2 - only naval
//011 = 3 - normal & naval
//100 = 4 - only special
//101 = 5 - normal & special
//110 = 6 - naval & special
//111 = 7 - all