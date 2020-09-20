using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitIcon : MonoBehaviour
{
    public RawImage icon;
    public TextMeshProUGUI nameLabel;

    public List<TextMeshProUGUI> stats;


    public void SetStats(string name, Texture icon, int health, int move, int attack, int attack2, int range,
        int capacity, int cost)
    {
        nameLabel.text = name;
        this.icon.texture = icon;

        SetStat(health, Stat.Health);
        SetStat(move, Stat.Move);
        SetStat(attack, Stat.Attack);
        SetStat(attack2, Stat.Attack2);
        SetStat(range, Stat.Range);
        SetStat(capacity, Stat.Capacity);
        SetStat(cost, Stat.Cost);
    }

    void SetStat(int value, Stat stat)
    {
        if (value == -1)
        {
            stats[(int)stat].transform.parent.gameObject.SetActive(false);
        }
        else
        {
            stats[(int)stat].text = value.ToString();
        }
    }

    public bool IsEnabled()
    {
        return gameObject.activeSelf;
    }
    public void Disable()
    {
        //TODO:do better
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
[Serializable]
public class StatUI
{
    public Image icon;
    public TextMeshProUGUI stat;
}

public enum Stat
{
    Health,Move,Attack, Attack2,Range,Capacity,Cost
}
