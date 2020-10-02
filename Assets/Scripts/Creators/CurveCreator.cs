using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CurveCreator
{
	static float[] alphaConstant = new float[]
	{
		0,0.5f,1
	};

	public static List<Vector3> CreateCurve(List<Vector3> points, int numberOfPoints, Alpha alpha)
	{
		List<Vector3> newPoints = new List<Vector3>();
		float alphaConst = alphaConstant[(int)alpha];

		for (int i=0;i<points.Count - 3; i++)
        {
			GetPoints(ref newPoints, points[i], points[i + 1], points[i + 2], points[i + 3], numberOfPoints, alphaConst);
        }

		newPoints.Add(points[points.Count - 2]);

		return newPoints;
	}

	public static void GetPoints(ref List<Vector3> newPoints, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int numberOfPoints, float alphaConst)
    {
		float t0 = 0.0f;
		float t1 = GetT(t0, p0, p1, alphaConst);
		float t2 = GetT(t1, p1, p2, alphaConst);
		float t3 = GetT(t2, p2, p3, alphaConst);

		for (float t = t1; t < t2; t += ((t2 - t1) / (float)numberOfPoints))
		{
			Vector3 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
			Vector3 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
			Vector3 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

			Vector3 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
			Vector3 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;
			Vector3 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;


			newPoints.Add(C);
		}
	}


	static float GetT(float t, Vector3 p0, Vector3 p1, float alpha)
	{
		float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.z - p0.z), 2.0f);
		float b = Mathf.Pow(a, alpha * 0.5f);

		return (b + t);
	}
}

public enum Alpha
{
	Uniform, Centripetal, Chordal
}
