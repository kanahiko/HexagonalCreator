using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitObject : MonoBehaviour
{
    public Hex hex;
    public Hex moveToHex;

    public Side side;

    public int moves;
    public int hitPoints;

    public bool hasAttacked;
    public bool hasSecondaryAttacked;

    public int capacity;

    public MeshRenderer modelRenderer;

    public Unit type;

    public Dictionary<Hex, HexPath> movableHexes;

    public float animationSpeed = 10;

    public bool isAnimating = false;

    public List<UnitObject> boardedUnits;

    public Text testText;
    
    public void Initialize(Unit unit, Vector2Int coordinates)
    {
        movableHexes = new Dictionary<Hex, HexPath>();
        
        type = unit;
        hitPoints = unit.hitPoints;
        if (unit.capacity > 0)
        { 
            capacity = unit.capacity;
        }
        else
        {
            capacity = unit.bomberCapacity;
        }
        boardedUnits = new List<UnitObject>();
        hex = Util.HexExist(coordinates.x, coordinates.y);
        hex.unit = this;
        transform.position = hex.position;
        //Move(false);
        GameObject model = GameObject.Instantiate(unit.unitModel, transform);
        modelRenderer = model.GetComponent<MeshRenderer>();
        testText = model.GetComponentInChildren<Text>();
        
        ResetUnit();
        isAnimating = false;

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
    public void Attack(bool isPrimaryAttack = true)
    {
        if (isPrimaryAttack)
        { 
            hasAttacked = true;
        }
        else
        {
            hasSecondaryAttacked = true;
        }
    }

    public void Move(bool animate)
    {
        if (animate)
        {
            MoveToHex();
            if (boardedUnits != null && boardedUnits.Count > 0)
            {
                foreach (var unit in boardedUnits)
                {
                    unit.hex = hex;
                }
            }
            //Invoke(nameof(MoveToHex), 2); 
        }
        else
        {
            transform.localPosition = moveToHex.position;
            movableHexes.Clear();
            hex = moveToHex;
            moveToHex = null;
        }
    }

    List<Vector3> paths;
    public void MoveToHex()
    {
        paths = new List<Vector3>();
        List<Hex> path = new List<Hex>();

        List<Vector3> points = new List<Vector3>();
        path.Add(moveToHex);
        points.Add(moveToHex.position);
        /*
        Vector3 normal = Vector3.Normalize()
        points.Add(moveToHex.position);*/

        while (path.Count != 3)
        {
            HexPath nextHex = movableHexes[path[path.Count - 1]];
            path.Add(nextHex.from);
            points.Add(nextHex.from.position);

            if (nextHex.from == hex)
            {
                break;
            }
        }
        Vector3 normal = Vector3.Normalize(points[1] - points[0]) *2f;
        points.Insert(0, points[0] - normal);

        normal = Vector3.Normalize(points[points.Count - 1] - points[points.Count - 2]) * 2f;
        points.Add(points[points.Count-1] + normal);

        paths.AddRange(points);

        List<Vector3> movingPoints = CurveCreator.CreateCurve(points, 3, Alpha.Uniform);
        StartCoroutine(nameof(MoveAnimation),movingPoints);
        movableHexes.Clear();

        hex = moveToHex;
        moveToHex = null;
    }

    IEnumerator MoveAnimation(List<Vector3> path)
    {
        modelRenderer.enabled = true;
        testText.transform.localPosition = new Vector3(960,540);
        testText.text = $"{(hex!= null ? hex.position.ToString():"null")} ";
        //yield return new WaitForSeconds(2);
        isAnimating = true;
        int index = path.Count - 1;
        Vector3 currentHex = path[index];
        Vector3 nextHex = path[index-1];
        Vector3 currentPosition;
        float t = 0;

        Vector3 forward = transform.forward;
        Vector3 normal = Vector3.Normalize(nextHex -currentHex);

        while (transform.forward != normal)
        {
            t += Time.deltaTime * animationSpeed;
            if (!Rotate(ref t, forward, normal))
            {
                break;
            }
            yield return null;
        }
        t = 0;
        yield return null;
        while (index != 0 && isAnimating)
        {
            t += Time.deltaTime * animationSpeed;
            if (t>= 1)
            {
                currentPosition = nextHex;
                t = 0;
                index--; 
                currentHex = path[index];
                if (index != 0)
                { 
                    nextHex = path[index - 1];

                    forward = transform.forward;
                    normal = Vector3.Normalize(nextHex - currentHex);
                }
            }
            else
            {
                currentPosition = Vector3.Lerp(currentHex, nextHex, t);
            }

            if (transform.forward != normal)
            {
                Rotate(ref t, forward, normal);
            }

            transform.localPosition = currentPosition;
            yield return null;
        }
        OnAnimationStop();
    }

    void OnAnimationStop()
    {
        transform.localPosition = hex.position;
        paths.Clear();
        if (hex.unit != this)
        {
            modelRenderer.enabled = false;
            testText.transform.localPosition = new Vector3(960,540*2);
        }

        testText.text = $"{(hex!= null ? hex.position.ToString():"null")} {(hex!=null && hex.unit == this? "this":hex.unit.name)}";
    }

    bool Rotate(ref float rotationT, Vector3 forward, Vector3 normal)
    {
        if (rotationT >= 1)
        {
            rotationT = 0;
            transform.forward = normal;
            return false;
        }
        Vector3 newForward = Vector3.Slerp(forward, normal, rotationT);
        transform.forward = newForward;

        return true;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        var points = paths;
        if (paths != null)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (i == 0 || i == points.Count - 2)
                {
                    Gizmos.color = Color.green;

                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(points[i], points[i + 1]);
                }
            }
        }
    }

    public void BoardTransport(UnitObject boardingUnit)
    {
        boardedUnits.Add(boardingUnit);
        capacity--;
    }

    public void UnBoardTransport(UnitObject boardingUnit)
    {
        boardedUnits.Remove(boardingUnit);
        capacity++;
    }
}
