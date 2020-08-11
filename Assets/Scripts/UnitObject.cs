using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public Hex hex;
    
    public Side side;

    public int moves;
    public int hitPoints;

    public bool hasAttacked;
    public bool hasSecondaryAttacked;

    public int capacity;

    public Unit type;

    public Dictionary<Hex, HexPath> movableHexes;

    public void Initialize(Unit unit, Vector2Int coordinates)
    {
        movableHexes = new Dictionary<Hex, HexPath>();
        
        type = unit;
        hitPoints = unit.hitPoints;
        capacity = unit.capacity;

        hex = Util.HexExist(coordinates.x, coordinates.y);
        hex.unit = this;
        Move();
        
        ResetUnit();
        
    }

    public void ResetUnit()
    {
        moves = type.movement;
        hasAttacked = false;
        hasSecondaryAttacked = false;
        movableHexes.Clear();
    }
    
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

    public void Move()
    {
        transform.localPosition = Util.GetPosition(hex.x, hex.y);
        movableHexes.Clear();
    }
    
    public void Move(List<Hex> path)
    {

    }
}
