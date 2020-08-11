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
                var sprite = Instantiate(highlight);
                sprite.transform.position = offset + new Vector3(0, 0.25f, 0);
                sprite.name = $"{j} {i}";
                MapController.sprites[j, i] = sprite.GetComponent<SpriteRenderer>();
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles,0);
        mesh.RecalculateNormals();
        mesh.SetColors(colors);
        meshFilter.sharedMesh = mesh;
        collider.sharedMesh = mesh;

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
}


