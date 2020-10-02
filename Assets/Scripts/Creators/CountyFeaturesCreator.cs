using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CountyFeaturesCreator
{
    static Color borderColor = new Color(1, 1, 1);
    static Color centerColor = new Color(0.2f, 0.2f, 0.2f);

    public static void CreateCountriesOverlay(MapData data, ref Mesh countryMesh, ref Mesh countryBorderMesh)
    {
        CreateCountriesBorder(data, ref countryBorderMesh);
        if (countryMesh == null)
        {
            countryMesh = new Mesh();
        }
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> color = new List<Color>();
        List<Vector2> uvs = new List<Vector2>();
        Dictionary<string, int> vertexes = new Dictionary<string, int>();
        for (int i = 0; i < data.countries.Count; i++)
        {
            CreateCountryOverlay(data.countries[i], ref vertexes, ref vertices, ref triangles, ref uvs, ref color, new Vector2((i+0.5f) / data.countries.Count, 0.5f));
        }

        countryMesh.SetVertices(vertices);
        countryMesh.SetTriangles(triangles, 0);
        countryMesh.RecalculateNormals();
        countryMesh.SetUVs(1, uvs);
        countryMesh.SetColors(color);
    }

    public static void CreateCountriesBorder(MapData data, ref Mesh countryMesh)
    {
        if (countryMesh == null)
        {
            countryMesh = new Mesh();
        }
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector2> uvsIndex = new List<Vector2>();
        Dictionary<string, int> vertexes = new Dictionary<string, int>();
        for (int i = 0; i < data.countries.Count; i++)
        {
            CreateCountryBorder(data.countries[i], ref vertexes, ref vertices, ref triangles, ref uvs,ref uvsIndex, new Vector2((i + 0.5f) / data.countries.Count, 0.5f));            
        }

        countryMesh.SetVertices(vertices);
        countryMesh.SetTriangles(triangles, 0);
        countryMesh.SetUVs(0, uvs);
        countryMesh.SetUVs(1, uvsIndex);
        countryMesh.RecalculateNormals();

        MaskCreator.CreateBorderMask(0, 1, 1, data.countries.Count);
    }

    static void CreateCountryOverlay(Country country, ref Dictionary<string, int> vertexes, ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs, ref List<Color> color, Vector2 index)
    {
        foreach (var hex in country.hexes)
        {
            Vector2Int coordinates = Util.FindCoordinates(hex);
            byte sides = Util.GetHexSides(coordinates, country);
            Vector3 offset = new Vector3(Util.horizontalOffset * coordinates.x, 0, Util.verticalOffset * coordinates.y);
            if (coordinates.y != 0)
            {
                offset.x += coordinates.y % 2 * Util.sideOffset;
            }

            CreateOvelayHex(ref vertices, ref triangles, ref uvs, ref color, offset, 0.1f, sides, index);
        }
    }

    static void CreateOvelayHex(ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs, ref List<Color> color, Vector3 offset, float elevation, byte sides, Vector2 index)
    {
        Vector3 elevationVector = new Vector3(0, elevation, 0);
        vertices.Add(Vector3.zero + offset + elevationVector);
        uvs.Add(index);
        color.Add(centerColor);
        int zeroVertexIndex = vertices.Count - 1;

        for (int i = 0; i < 6; i++)
        {
            vertices.Add(Util.overlayOffsets[i] + offset + elevationVector);
            if (i + 1 >= 6)
            {
                vertices.Add(Util.overlayOffsets[0] + offset + elevationVector);
            }
            else
            {
                vertices.Add(Util.overlayOffsets[i + 1] + offset + elevationVector);
            }
            uvs.Add(index);
            uvs.Add(index);
            triangles.Add(zeroVertexIndex);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);

            int vertexIndex = vertices.Count - 2;

            if (((sides >> i) & 1) == 1)
            {

                bool hasLeftSide = (i == 0 ? ((sides >> 5) & 1): ((sides >> (i - 1)) & 1)) == 1;
                bool hasRightSide = (i == 5 ? (sides & 1): ((sides >> (i + 1)) & 1)) == 1;
                if (hasLeftSide)
                {
                    vertices.Add(Util.offsets[i] + offset + elevationVector);
                    color.Add(centerColor);
                }
                else
                {
                    vertices.Add(Util.overlayLeftCornerOffsets[i] + offset + elevationVector);
                    color.Add(borderColor);
                }
                if (hasRightSide)
                {
                    if (i + 1 >= 6)
                    {
                        vertices.Add(Util.offsets[0] + offset + elevationVector);
                    }
                    else
                    {
                        vertices.Add(Util.offsets[i + 1] + offset + elevationVector);
                    }
                    color.Add(centerColor);
                }
                else
                {
                    vertices.Add(Util.overlayRightCornerOffsets[i] + offset + elevationVector);
                    color.Add(borderColor);
                }
                color.Add(hasLeftSide ? centerColor : borderColor);
                color.Add(hasRightSide ? centerColor : borderColor);

                uvs.Add(index);
                uvs.Add(index);
                triangles.Add(vertexIndex);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 1);

            }
            else
            {
                color.Add(borderColor);
                color.Add(borderColor);
            }
        }
    }
    static float trapezeDelta = 0.0577f;
    static float trapezeCornerSize = 0.277f;
    static Vector2 trapezeUpperLeft = new Vector2(0.01f, 0.99f);
    static Vector2 trapezeLowerLeft = new Vector2(trapezeDelta, 0.01f);
    static Vector2 trapezeLowerRight = new Vector2(1-trapezeDelta, 0.01f);
    static Vector2 trapezeLowerLeftCorner = new Vector2(0 + 0.107962f, 0.01f);
    static Vector2 trapezeLowerRightCorner = new Vector2(trapezeCornerSize + 0.107962f, 0.01f);
    static Vector2 trapezeUpperLeftCorner = new Vector2(trapezeDelta + 0.107962f, 0.99f);
    static Vector2 trapezeUpperRightCorner = new Vector2(trapezeCornerSize - trapezeDelta + 0.107962f, 0.99f);
    //0.0577f
    //0.222f
    static void CreateCountryBorder(Country country, ref Dictionary<string, int> vertexes, ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs, ref List<Vector2> uvsIndex, Vector2 index)
    {
        foreach (var hex in country.hexes)
        {
            Vector2Int coordinates = Util.FindCoordinates(hex);
            byte sides = Util.GetHexSides(coordinates, country);
            Vector3 offset = new Vector3(Util.horizontalOffset * coordinates.x, 0, Util.verticalOffset * coordinates.y);
            if (coordinates.y != 0)
            {
                offset.x += coordinates.y % 2 * Util.sideOffset;
            }

            CreateBorder(ref vertices, ref triangles,ref uvs, ref uvsIndex,offset, 0.12f, sides, index);
        }
    }

    static void CreateBorder(ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs, ref List<Vector2> uvsIndex,Vector3 offset, float elevation, byte sides, Vector2 index)
    {
        Vector3 elevationVector = new Vector3(0, elevation, 0);

        for (int i = 0; i < 6; i++)
        {
            bool hasFrontSide = ((sides >> i) & 1) == 1;
            bool hasLeftSide = (i == 0 ? ((sides >> 5) & 1) : ((sides >> (i - 1)) & 1)) == 1;
            bool hasRightSide = (i == 5 ? (sides & 1) : ((sides >> (i + 1)) & 1)) == 1;

            if (hasLeftSide && hasRightSide && hasFrontSide)
            {
                continue;
            }

            if (!hasFrontSide)
            {
                vertices.Add(Util.overlayOuterOffsets[i] + offset + elevationVector);
                vertices.Add(Util.overlayOffsets[i] + offset + elevationVector);

                if (i + 1 >= 6)
                {
                    vertices.Add(Util.overlayOuterOffsets[0] + offset + elevationVector);
                    vertices.Add(Util.overlayOffsets[0] + offset + elevationVector);
                }
                else
                {
                    vertices.Add(Util.overlayOuterOffsets[i + 1] + offset + elevationVector);
                    vertices.Add(Util.overlayOffsets[i + 1] + offset + elevationVector);
                }
                uvs.Add(trapezeUpperLeft);
                uvs.Add(trapezeLowerLeft);
                uvs.Add(Vector2.one);
                uvs.Add(trapezeLowerRight);

                uvsIndex.Add(index);
                uvsIndex.Add(index);
                uvsIndex.Add(index);
                uvsIndex.Add(index);

                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 4);

                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 2);
                continue;
            }
            if (!hasLeftSide)
            {
                vertices.Add(Util.overlayOuterOffsets[i] + offset + elevationVector);
                vertices.Add(Util.overlayOffsets[i] + offset + elevationVector);
                vertices.Add(Util.overlayOuterLeftCornerOffsets[i] + offset + elevationVector);
                vertices.Add(Util.overlayLeftCornerOffsets[i] + offset + elevationVector);

                uvs.Add(trapezeUpperLeftCorner);
                uvs.Add(trapezeLowerLeftCorner);
                uvs.Add(trapezeUpperRightCorner);
                uvs.Add(trapezeLowerRightCorner);
                uvsIndex.Add(index);
                uvsIndex.Add(index);
                uvsIndex.Add(index);
                uvsIndex.Add(index);

                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 4);

                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 2);
            }
            if (!hasRightSide)
            {
                vertices.Add(Util.overlayOuterRightCornerOffsets[i] + offset + elevationVector);
                vertices.Add(Util.overlayRightCornerOffsets[i] + offset + elevationVector);
                if (i + 1 >= 6)
                {
                    vertices.Add(Util.overlayOuterOffsets[0] + offset + elevationVector);
                    vertices.Add(Util.overlayOffsets[0] + offset + elevationVector);
                }
                else
                {
                    vertices.Add(Util.overlayOuterOffsets[i + 1] + offset + elevationVector);
                    vertices.Add(Util.overlayOffsets[i + 1] + offset + elevationVector);
                }

                uvs.Add(trapezeUpperRightCorner);
                uvs.Add(trapezeLowerRightCorner);
                uvs.Add(trapezeUpperLeftCorner);
                uvs.Add(trapezeLowerLeftCorner);

                uvsIndex.Add(index);
                uvsIndex.Add(index);
                uvsIndex.Add(index);
                uvsIndex.Add(index);

                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 4);

                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 2);
            }
        }
    }
}
