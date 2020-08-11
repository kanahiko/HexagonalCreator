using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public Side side;

    public int moves;
    public int hitPoints;

    public bool hasAttacked;
    public bool hasSecondaryAttacked;

    public int capacity;

    public Unit type;

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
    public void Destruct()
    {
        Destroy(gameObject);
    }
    public void Attack()
    {

    }

    public void Move(List<Hex> path)
    {

    }
}
