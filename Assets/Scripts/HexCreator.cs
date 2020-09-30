using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCreator : MonoBehaviour
{
    //public int width = 5;
    //public int height = 4;

    public MeshFilter meshFilter;
    public MeshCollider collider;
    public GameObject highlight;
    Mesh mesh;

    public MeshFilter countryMeshFilter;
    Mesh countryMesh;

    void Start()
    {
        //CreateMap(width, height);
    }

    public Hex[,] CreateMap(MapData map)
    {
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        Hex[,] hexes = new Hex[map.width,map.height];

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();
        Dictionary<string, int> vertexes = new Dictionary<string, int>();

        for (int i = 0; i < map.height; i++)
        {
            for (int j = 0; j < map.width; j++)
            {
                Vector3 offset = new Vector3(Util.horizontalOffset * j, 0, Util.verticalOffset * i);
                if (i != 0)
                {
                    offset.x += i % 2 * Util.sideOffset;
                }
                CreateHex(ref vertexes, ref vertices, ref triangles,ref colors, offset, Util.color[map.types[j+i*map.width]], map.elevation[j + i * map.width]);
                hexes[j, i] = new Hex(j, i,map.types[j + i*map.width], map.elevation[j+ i * map.width]);
                hexes[j, i].position = offset;
                
                

                var sprite = Instantiate(highlight);
                sprite.transform.position = offset + new Vector3(0, 0.25f, 0);
                sprite.name = $"{j} {i}";
                var text = sprite.transform.GetComponentInChildren<Text>();
                text.text = $"{j+i*map.width}";
                MapController.sprites[j, i] = sprite.GetComponent<SpriteRenderer>();
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.RecalculateNormals();
        mesh.SetColors(colors);
        meshFilter.sharedMesh = mesh;
        collider.sharedMesh = mesh;

        CreateCountriesOverlay(map);

        return hexes;
        Debug.Log(vertices.Count);
        //boxCollider.size = new Vector3(Util.halfRadius * 2 * width, 0.5f, Util.radius * 2 * height);
        //boxCollider.center = boxCollider.size / 2;
    }

    public void ChangeHex(int index, TileType type)
    {
        Color[] colors = mesh.colors;
        for (int i = 0, j = 0; i < colors.Length; i++)
        {
            if (colors[i] == Color.white)
            {
                continue;
            }
            else
            {
                if (j != index)
                {
                    j++;
                }
                else
                {
                    colors[i] = Util.color[type];
                    break;
                }
            }
        }

        mesh.SetColors(colors);
        collider.sharedMesh = mesh;
    }

    void CreateHex(ref Dictionary<string,int> vertexes,ref List<Vector3> vertices, ref List<int> triangles,ref List<Color> colors, Vector3 offset, Color color, int elevation)
    {
        vertices.Add(Vector3.zero + offset + new Vector3(0,elevation,0));
        vertexes.Add(vertices[vertices.Count - 1].ToString("0.0"), vertices.Count - 1);
        colors.Add(color);
        int[] vertexIndex = new int[7];
        vertexIndex[0] = vertices.Count - 1; 

        for (int i = 0; i < 6; i++)
        {
            Vector3 vertex = Util.offsets[i] + offset;
            string vertexString = vertex.ToString("0.0");
            if (vertexes.ContainsKey(vertexString))
            {
                vertexIndex[i + 1] = vertexes[vertexString];
                //Debug.Log($"old {vertex} {vertexIndex[i + 1]}");
            }
            else
            {
                vertices.Add(Util.offsets[i] + offset);
                colors.Add(Color.white);
                vertexIndex[i + 1] = vertices.Count - 1;
                vertexes.Add(vertexString, vertices.Count - 1);
                //Debug.Log($"new {vertex} { vertexes[vertex]}");
            }
        }

        for (int i = 0; i < 6; i++)
        {
            triangles.Add(vertexIndex[0]);
            triangles.Add(vertexIndex[i + 1]);
            if (i == 5)
            {
                triangles.Add(vertexIndex[1]);
            }
            else
            {
                triangles.Add(vertexIndex[i + 2]);
            }
        }
    }

    void CreateCountriesOverlay(MapData data)
    {
        if (countryMesh == null)
        {
            countryMesh = new Mesh();
        }
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> color = new List<Color>();
        List<Vector2> uvs = new List<Vector2>();
        Dictionary<string, int> vertexes = new Dictionary<string, int>();
        for(int i=0;i<data.countries.Count;i++)
        {
            CreateCountryOverlay(data.countries[i], ref vertexes, ref vertices, ref triangles, ref uvs, ref color, new Vector2(Mathf.Pow(2,i),i));
        }

        countryMesh.SetVertices(vertices);
        countryMesh.SetTriangles(triangles, 0);
        countryMesh.RecalculateNormals();
        countryMesh.SetUVs(0, uvs);
        countryMesh.SetColors(color);
        countryMeshFilter.sharedMesh = countryMesh;
    }

    void CreateCountryOverlay(Country country, ref Dictionary<string, int> vertexes, ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs, ref List<Color> color, Vector2 index)
    {
        foreach(var hex in country.hexes)
        {
            Vector2Int coordinates = Util.FindCoordinates(hex);
            byte sides = Util.GetHexSides(coordinates, country);
            Vector3 offset = new Vector3(Util.horizontalOffset * coordinates.x, 0, Util.verticalOffset * coordinates.y);
            if (coordinates.y != 0)
            {
                offset.x += coordinates.y % 2 * Util.sideOffset;
            }

            //Debug.Log(hex);
            //Debug.Log(hex);
            CreateOvelayHex(ref vertices, ref triangles, ref uvs,ref color, offset, 0.1f, sides, index);
        }
    }


    void CreateOvelayHex(ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs,ref List<Color> color, Vector3 offset, float elevation, byte sides, Vector2 index)
    {
        Vector3 elevationVector = new Vector3(0, elevation, 0);
        vertices.Add(Vector3.zero + offset + elevationVector);
        uvs.Add(index);
        color.Add(new Color(0.2f, 0.2f, 0.2f));
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
                vertices.Add(Util.overlayOffsets[i+1] + offset + elevationVector);
            }
            uvs.Add(index);
            uvs.Add(index);
            triangles.Add(zeroVertexIndex);
            triangles.Add(vertices.Count-2);
            triangles.Add(vertices.Count-1);

            int vertexIndex = vertices.Count - 2;

            if (((sides >> i) & 1) == 1)
            {

                bool hasLeftSide = false;
                if (i == 0)
                {
                    hasLeftSide = ((sides >> 5) & 1) == 1;
                }
                else
                {
                    hasLeftSide = ((sides >> (i - 1)) & 1) == 1;
                }
                bool hasRightSide = false;
                if (i == 5)
                {
                    hasRightSide = (sides & 1) == 1;
                }
                else
                {
                    hasRightSide = ((sides >> (i + 1)) & 1) == 1;
                }

                if (hasLeftSide)
                {
                    vertices.Add(Util.offsets[i] + offset + elevationVector);
                    color.Add(new Color(0.2f, 0.2f, 0.2f));
                    color.Add(new Color(0.2f, 0.2f, 0.2f));
                }
                else
                {
                    vertices.Add(Util.overlayLeftCornerOffsets[i] + offset + elevationVector);
                    color.Add(new Color(1f, 0.2f, 0.2f));
                    color.Add(new Color(1f, 1f, 1f));
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
                    color.Insert(color.Count - 2, new Color(0.2f, 0.2f, 0.2f));
                    color.Add(new Color(0.2f, 0.2f, 0.2f));
                }
                else
                {
                    vertices.Add(Util.overlayRightCornerOffsets[i] + offset + elevationVector);
                    color.Insert(color.Count - 2, new Color(1f, 0.2f, 0.2f));
                    color.Add(new Color(1f, 1f, 1f));
                }
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
                color.Add(new Color(1f, 1f, 1f));
                color.Add(new Color(1f, 1f, 1f));
            }
        }
    }
}


