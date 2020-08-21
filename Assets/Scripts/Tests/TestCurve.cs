using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestCurve: MonoBehaviour
{
    public bool updateCurve;

    public List<Transform> positions;

    List<Vector3> newPoints = new List<Vector3>();

    public int numberOfPoint = 10;
    public Alpha alpha = Alpha.Uniform;

    // Use this for initialization
    void Start()
    {
        newPoints = CurveCreator.CreateCurve(GetPositions(), numberOfPoint, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        if (updateCurve)
        {
            updateCurve = false;
            newPoints = CurveCreator.CreateCurve(GetPositions(), numberOfPoint, alpha);
        }
    }


    List<Vector3> GetPositions()
    {
        List<Vector3> pos = new List<Vector3>();

        foreach(var point in positions)
        {
            pos.Add(point.position);
        }

        Vector3 normal = Vector3.Normalize(pos[1] - pos[0]) * 2;
        pos.Insert(0, pos[0] - normal);

        normal = Vector3.Normalize(pos[pos.Count - 1] - pos[pos.Count - 2]) * 2;
        pos.Add(pos[pos.Count - 1] + normal);


        return pos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 temp in newPoints)
        {
            Vector3 pos = new Vector3(temp.x, 0,temp.z);
            Gizmos.DrawSphere(pos, 0.1f);
        }

        Gizmos.color = Color.green;
        var points = GetPositions();
        for(int i=0;i<points.Count -1;i++)
        {
            Gizmos.DrawLine(points[i], points[i+1]);
        }
    }
}
